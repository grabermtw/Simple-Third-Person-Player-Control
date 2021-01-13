using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonPlayerController : MonoBehaviour
{
    public Transform characterHips;
    private CameraController cameraController;
    public float gamepadLookSensititvity = 20;
    public float mouseLookSensitivity = 15;
    
    private Movement movement;
    private Vector2 look = Vector2.zero;
    private Vector2 move = Vector2.zero;

    // --------------- INPUT EVENTS -------------
    // these are all called by the PlayerInput component based on the InputActions asset.
    // Read about the PlayerInput component here: https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/api/UnityEngine.InputSystem.PlayerInput.html
    
    public void OnMovement(InputAction.CallbackContext ctx)
    {   
        if (ctx.performed)
            move = ctx.ReadValue<Vector2>();
        else if (ctx.canceled)
            move = Vector2.zero;
    }

    public void OnLook(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            look = ctx.ReadValue<Vector2>() * (ctx.control.device is Gamepad ? Time.deltaTime * gamepadLookSensititvity : mouseLookSensitivity);
        else if (ctx.canceled)
            look = Vector2.zero;
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            movement.Jump(true);
        else if (ctx.canceled)
            movement.Jump(false);
    }

    public void OnUtility(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) // when the button is pressed down
        {
            // Do whatever you want this button to do when it is pressed
        }
        else if (ctx.canceled)
        {
            // Do whatever you want this button to do when it is released
        }
        
    }
    
    // ------------ END INPUT EVENTS -----------

    // Start is called before the first frame update
    void Start()
    {
        cameraController = GetComponentInChildren<CameraController>();
        movement = GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.AddMovement(move.x, move.y);
        movement.RotateCamera(look.x, look.y);
    }

    public Transform GetHips()
    {
        return characterHips;
    }
}
