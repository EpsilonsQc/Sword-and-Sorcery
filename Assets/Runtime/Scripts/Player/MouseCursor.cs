using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG_Project.Player
{
    public class MouseCursor : MonoBehaviour
    {
        [Header("Mouse Cursor")]
        [SerializeField] private Texture2D defaultCursor;

        // Input
        private UserInput userInput;

        private void Awake()
        {
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto); // Set custom cursor

            userInput = new UserInput();
            userInput.PlayerMovements.MouseCursor.performed += OnMouseCursor;
        }

        private void OnEnable()
        {
            userInput.PlayerMovements.Enable();
        }

        private void OnDisable()
        {
            userInput.PlayerMovements.Disable();
        }

        private void OnMouseCursor(InputAction.CallbackContext context)
        {
            ShowHideCursor(); // Show or hide the cursor
        }

        private void ShowHideCursor()
        {
            if (Cursor.visible)
            {
                Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
                Cursor.visible = false; // Make the cursor invisibler
            }
            else // If cursor not visible
            {
                Cursor.lockState = CursorLockMode.None; // Unlock the cursor
                Cursor.visible = true; // Make the cursor visible
            }
        }
    }
}
