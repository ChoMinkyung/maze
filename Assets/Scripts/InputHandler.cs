using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public delegate void PlayerInputHandle();
    public event PlayerInputHandle OnPlayerWalkInput;
    public event PlayerInputHandle OnPlayerRunInput;
    public event PlayerInputHandle OnPlayerIdle;
    public event PlayerInputHandle OnPlayerJumpInput;

    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (horizontalInput != 0 || verticalInput != 0)
        {
            OnPlayerWalkInput?.Invoke();

            if(verticalInput > 0)
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    OnPlayerRunInput?.Invoke();
                }
            }
        }
        else
        {
            OnPlayerIdle?.Invoke();
        }

        if(Input.GetKey(KeyCode.Space))
        {
            OnPlayerJumpInput?.Invoke();
        }
       


    }
}
