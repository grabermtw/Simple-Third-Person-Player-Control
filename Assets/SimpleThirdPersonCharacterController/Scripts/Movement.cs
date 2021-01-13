using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class Movement : MonoBehaviour
{
    public float Direction { get => actualVelocity == Vector3.zero ? 0f : Mathf.Abs(Quaternion.LookRotation(actualVelocity, Vector3.up).eulerAngles.y - characterMesh.transform.rotation.eulerAngles.y); }

    public float jumpForce = 300f;
    public float runSpeed = 5f;
    public float rotationSpeed = 10f;

    private CameraController cameraController;
    private Animator anim;
    private Rigidbody rb;
    private Transform characterMesh;

    private bool preventingJumpLock = false;

    private float acceleration;
    private Vector3 velocity = Vector3.zero;
    private Vector3 actualVelocity; // accounts for walking into walls
    private Vector3 playerPosition; // position in previous frame

    // used to smooth out speed transition an animation
    private float speed = 0;
    private float bonusSpeed = 1f;
    private float smoothSpeed = 0;
    private const float dampTime = 0.05f; // reduce jittering in animator by providing dampening
    
    // used to determine when jumping can occur
    private bool grounded = true;
    private bool falling = false;
    public bool Falling { get => falling; set => falling = value; } 
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        cameraController = GetComponentInChildren<CameraController>();
        rb = GetComponent<Rigidbody>();
        characterMesh = GetComponent<ThirdPersonPlayerController>().GetHips().parent;
    }

    /*  called by an animation event when the player reaches the peak of their jump
        to enable them to land prevents triggering landing at the start of the jump */
    public void StartFalling()
    {
        falling = true;
    }

    /*  rotate the camera around the player */
    public void RotateCamera(float x, float y)
    {
        cameraController.Rotate(x, y);
    }

    /*  moves the player rigidbody */
    public void AddMovement(float forward, float right)
    {
        actualVelocity = Vector3.Lerp(actualVelocity, (transform.position - playerPosition) / Time.deltaTime, Time.deltaTime * 10);
        playerPosition = transform.position;

        Vector3 translation = Vector3.zero;
        translation += right * cameraController.transform.forward;
        translation += forward * cameraController.transform.right;
    
        translation.y = 0;
        if (translation.magnitude > 0)
        {
            velocity = translation;
        }
        else
        {
            velocity = Vector3.zero;
        }

        // moved from update
        if (velocity.magnitude > 0)
        {
            rb.velocity = new Vector3(velocity.normalized.x * smoothSpeed, rb.velocity.y, velocity.normalized.z * smoothSpeed);
            smoothSpeed = Mathf.Lerp(smoothSpeed, runSpeed * bonusSpeed, Time.deltaTime);
            // rotate the character mesh if enabled
            
            characterMesh.rotation = Quaternion.Lerp(characterMesh.rotation, Quaternion.LookRotation(velocity), Time.deltaTime * rotationSpeed);
            
        }
        else
        {
            smoothSpeed = Mathf.Lerp(smoothSpeed, 0, Time.deltaTime*8);
        }
    
        // if the player landed, enable another jump
        if (!grounded)
        {
            RaycastHit hit;
            if (falling && Physics.Linecast(transform.position + new Vector3(0, 0.1f, 0), transform.position + new Vector3(0, -0.2f, 0), out hit))
            {
                falling = false;
                Land();
            }
            else if (Physics.Linecast(transform.position + new Vector3(0, 0.1f, 0), transform.position + new Vector3(0, -0.2f, 0), out hit))
            {
                StartCoroutine(PreventJumpLock());
            }
        }
        // blend speed in animator to match pace of footsteps
        // normal movement (character moves independent of camera)
        
        speed = Mathf.SmoothStep(speed, actualVelocity.magnitude, Time.deltaTime * 20);
    
        anim.SetFloat("speed", speed, dampTime, Time.deltaTime);
        //Debug.Log("velocity" + velocity);
    }


    // this is a failsafe in case the player presses jump at the instant
    // that somehow causes them to land without land being called.
    // Basically if we haven't landed after 4 seconds, we're landing
    private IEnumerator PreventJumpLock()
    {
        if (preventingJumpLock)
            yield break;
        else
        {
            preventingJumpLock = true;
            float maxJumpFixTime = 4;
            float jumpFixTimer = 0;
            while (jumpFixTimer < maxJumpFixTime && !grounded)
            {
                jumpFixTimer += Time.deltaTime;
                yield return null;
            }
            if (jumpFixTimer >= maxJumpFixTime && rb.velocity.y > -10)
            {
                Debug.Log("Fixing jump!");
                falling = false;
                grounded = true;
            }
            preventingJumpLock = false;
        }
    }


    /* causes the player to jump */
    public void Jump(bool hold)
    {
        if (grounded)
        {
            if (Physics.Linecast(transform.position + new Vector3(0, 0.1f, 0), transform.position + new Vector3(0, -0.1f, 0)))
            {
                anim.ResetTrigger("land");
                rb.AddForce(Vector3.up * jumpForce);
                grounded = false;
                anim.SetTrigger("jump");
            }
        }
    }

    /*  grounds the player after a jump is complete */
    public void Land()
    {
        grounded = true;
        anim.SetTrigger("land");
    }

    private void LateUpdate()
    {
        //Debug.Log(falling);
        // Prevent short jumps from happening twice due to a lingering trigger
        anim.ResetTrigger("jump");
    }
}
