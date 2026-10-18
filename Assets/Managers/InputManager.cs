using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public PlayerInput playerInput;
        
    public static Vector2 moveInput;
    
    public static bool isJumpPressed;
    public static bool isJumpReleased;
    
    

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started || context.performed)
        {
            isJumpPressed = true;
            isJumpReleased = false;
        }
        else if (context.canceled)
        {
            isJumpPressed = false;
            isJumpReleased = true;
        }
    }
        
    public void Dash(InputAction.CallbackContext context)
    {
        if (context.started || context.performed)
        {
                
        }
    }
    

}
