using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public Vector2 rawMovementInput;
    public bool isJumping;

    public void OnMove(InputAction.CallbackContext context)
    {
        rawMovementInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isJumping = true;
        }
    }

    public void Jumped()
    {
        isJumping = false;  
    }
}
