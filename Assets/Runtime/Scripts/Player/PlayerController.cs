using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG_Project.Player
{
    public class PlayerController : MonoBehaviour
    {
        // References
        private Animator animator;
        private UserInput userInput;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            userInput = new UserInput();

            userInput.PlayerMovements.Walk.performed += OnWalking;
            userInput.PlayerMovements.Walk.canceled += OnStopWalking;
            userInput.PlayerMovements.Run.performed += OnRunning;
            userInput.PlayerMovements.Run.canceled += OnStopRunning;
            userInput.PlayerMovements.TurnLeft.performed += OnTurnLeft;
            userInput.PlayerMovements.TurnLeft.canceled += OnStopTurningLeft;
            userInput.PlayerMovements.TurnRight.performed += OnTurnRight;
            userInput.PlayerMovements.TurnRight.canceled += OnStopTurningRight;
            userInput.PlayerMovements.WalkBack.performed += OnWalkBack;
            userInput.PlayerMovements.WalkBack.canceled += OnStopWalkBack;
            userInput.PlayerMovements.Jump.performed += OnJumping;
            userInput.PlayerMovements.Jump.canceled += OnStopJumping;
            userInput.PlayerMovements.MouseLook.performed += OnMouseLook;
            userInput.PlayerMovements.MouseLook.canceled += OnStopMouseLook;

            userInput.PlayerActions.Attack.performed += OnAttacking;
            userInput.PlayerActions.Attack.canceled += OnStopAttacking;
            userInput.PlayerActions.Block.performed += OnBlocking;
            userInput.PlayerActions.Block.canceled += OnStopBlocking;
        }

        private void OnEnable()
        {
            userInput.PlayerMovements.Enable();
            userInput.PlayerActions.Enable();
        }

        private void OnDisable()
        {
            userInput.PlayerMovements.Disable();
            userInput.PlayerActions.Disable();
        }

        private void OnWalking(InputAction.CallbackContext context)
        {
        animator.SetBool("isWalking", true);
        }
        private void OnStopWalking(InputAction.CallbackContext context)
        {
        animator.SetBool("isWalking", false);
        }

        private void OnRunning(InputAction.CallbackContext context)
        {
            animator.SetBool("isRunning", true);
        }

        private void OnStopRunning(InputAction.CallbackContext context)
        {
            animator.SetBool("isRunning", false);
        }

        private void OnTurnLeft(InputAction.CallbackContext context)
        {
            animator.SetBool("isTurningLeft", true);
        }

        private void OnStopTurningLeft(InputAction.CallbackContext context)
        {
            animator.SetBool("isTurningLeft", false);
        }

        private void OnTurnRight(InputAction.CallbackContext context)
        {
            animator.SetBool("isTurningRight", true);
        }

        private void OnStopTurningRight(InputAction.CallbackContext context)
        {
            animator.SetBool("isTurningRight", false);
        }

        private void OnWalkBack(InputAction.CallbackContext context)
        {
            animator.SetBool("isWalkingBack", true);
        }

        private void OnStopWalkBack(InputAction.CallbackContext context)
        {
            animator.SetBool("isWalkingBack", false);
        }

        private void OnJumping(InputAction.CallbackContext context)
        {
            animator.SetTrigger("isJumping");
        }

        private void OnStopJumping(InputAction.CallbackContext context)
        {
            animator.ResetTrigger("isJumping");
        }

        private void OnMouseLook(InputAction.CallbackContext context)
        {
            if(!Cursor.visible) // If cursor not visible
            {
                // If mouse is moving to the right
                if(context.ReadValue<float>() > 0.00f)
                {
                    animator.SetBool("isTurningRight", true);
                }

                // If mouse is moving to the left
                if(context.ReadValue<float>() < 0.00f)
                {
                    animator.SetBool("isTurningLeft", true);
                }
            }
        }

        private void OnStopMouseLook(InputAction.CallbackContext context)
        {
            // If "A" is not pressed
            if(userInput.PlayerMovements.TurnLeft.ReadValue<float>().Equals(0.00f))
            {
                animator.SetBool("isTurningLeft", false);
            }

            // If "D" is not pressed
            if(userInput.PlayerMovements.TurnRight.ReadValue<float>().Equals(0.00f))
            {
                animator.SetBool("isTurningRight", false);
            }
        }

        private void OnAttacking(InputAction.CallbackContext context)
        {
            animator.SetBool("isAttacking", true);
        }

        private void OnStopAttacking(InputAction.CallbackContext context)
        {
            animator.SetBool("isAttacking", false);
        }

        private void OnBlocking(InputAction.CallbackContext context)
        {
            animator.SetBool("isBlocking", true);
        }

        private void OnStopBlocking(InputAction.CallbackContext context)
        {
            animator.SetBool("isBlocking", false);
        }
    }
}
