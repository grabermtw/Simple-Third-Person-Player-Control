using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*  called when the player reaches the peak of their jump to enable them to land
        prevents triggering landing at the start of the jump */
    public void StartFalling()
    {
        transform.parent.GetComponent<Movement>().Falling = true;
    }
}
