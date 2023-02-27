using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;
using UnityEditor.IMGUI.Controls; // required for "AdvancedDropdown"
using RPG_Project.Editor; // required for "Editor tools" dependency

#if UNITY_EDITOR // Only compile this script in the editor
namespace RPG_Project.IMGUI
{
    public class DebugTool : MonoBehaviour
    {
        [Header("GUI Skin")]
        [SerializeField] private GUISkin customSkin; // Define the custom skin to use in the inspector

        [Header("Parameter")]
        [SerializeField] private bool showDebugToolGUI; // Define if the GUI is shown or not

        // User Input
        private UserInput userInput;

        private void Awake()
        {
            userInput = new UserInput();
            userInput.DebugOptions.ShowDebugTool.performed += OnShowDebugTool;
        }

        private void OnEnable()
        {
            userInput.DebugOptions.Enable();
        }

        private void OnDisable()
        {
            userInput.DebugOptions.Disable();
        }

        // Press "F1" to show/hide the debug tool
        private void OnShowDebugTool(InputAction.CallbackContext context)
        {
            if (showDebugToolGUI)
            {
                showDebugToolGUI = false;
            }
            else
            {
                showDebugToolGUI = true;
            }
        }

        private void OnGUI()
        {
            if(showDebugToolGUI)
            {
                GUILayout.BeginArea(new Rect(10, 10, 250, 165));

                // BOX AROUND THE GUI ELEMENTS WITH CENTERED TITLE
                GUI.Box (new Rect (0, 0, 250, 165), "Debug tool");

                GUILayout.Space(20);
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                // OPEN AUTOSAVE EDITOR WINDOW
                if(GUILayout.Button ("Open AutoSave Tool", EditorStyles.toolbarButton))
                {
                    AutoSave.ShowWindow(); // Open the autosave tool
                }

                EditorGUILayout.Space(5);
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                // OPEN TELEPORT EDITOR WINDOW
                if(GUILayout.Button ("Open Teleport Tool", EditorStyles.toolbarButton))
                {
                    Teleport.ShowWindow(); // Open the teleport tool
                }

                // OPEN TELEPORT ADVANCED DROPDOWN
                if (Teleport.objectToTeleport != null)
                {
                    GUILayout.Label ("Teleport to location preset: ");

                    if(GUILayout.Button ("Select a preset...", EditorStyles.popup))
                    {
                        GUI.skin = customSkin; // Set a custom skin to the GUI
                        var dropdown = new TeleportDropdownMenu(new AdvancedDropdownState()); // Instantiate a new dropdown
                        dropdown.Show(new Rect(Event.current.mousePosition.x + 300, Event.current.mousePosition.y + 75, 200, 30));
                        GUI.skin = null; // Revert to the default Unity skin
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("ERROR: No object to teleport selected!", MessageType.Warning);
                }
                
                GUILayout.EndArea();
            }
        }
    }
}
#endif
