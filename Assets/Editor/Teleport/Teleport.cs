using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls; // required for "AdvancedDropdown"

namespace RPG_Project.Editor
{
    [InitializeOnLoad]
    public class Teleport : EditorWindow
    {
        static public GameObject objectToTeleport; // The object to teleport
        private Vector3 customTeleportPos; // Custom teleport position

        [MenuItem("Tools/Teleport")]
        public static void ShowWindow()
        {
            GetWindow<Teleport>("Teleport");
        }

        private void OnEnable()
        {
            if(objectToTeleport == null)
            {
                objectToTeleport = GameObject.Find("Player"); // Set the Player as the default object to teleport
            }
        }

        private void OnGUI()
        {
            // CENTERED TITLE
            EditorGUILayout.LabelField("Teleport tool", EditorStyles.centeredGreyMiniLabel);

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUILayout.Space(5);

            // OBJECT TO TELEPORT
            objectToTeleport = (GameObject)EditorGUILayout.ObjectField("GameObject to teleport: ", objectToTeleport, typeof(GameObject), true);

            if (objectToTeleport == null)
            {
                EditorGUILayout.HelpBox("ERROR: No object to teleport selected!", MessageType.Warning);
            }

            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUILayout.Space(5);

            // CUSTOM POSITION TELEPORTATION
            customTeleportPos = EditorGUILayout.Vector3Field("Teleport to custom position: ", customTeleportPos);

            EditorGUILayout.HelpBox("Valid value are between : (0, 0, 0) and (1000, 0, 1000). Please note that the Y-Axis minimum value is 0, but more is recommended to avoid falling into the void.", MessageType.Info);

            if (GUILayout.Button("Press to teleport")) 
            {
                if (objectToTeleport == null)
                {
                    Debug.Log("ERROR: No object to teleport selected!");
                    return;
                }

                objectToTeleport.transform.position = customTeleportPos;
            }

            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUILayout.Space(5);

            // TELEPORTATION PRESET DROPDOWN MENU
            EditorGUILayout.LabelField("Teleport to location preset: ");

            var rect = GUILayoutUtility.GetRect(new GUIContent("Select a preset..."), EditorStyles.popup);

            if (GUI.Button(rect, new GUIContent("Select a preset..."), EditorStyles.popup))
            {
                var dropdown = new TeleportDropdownMenu(new AdvancedDropdownState()); // Instantiate a new dropdown
                dropdown.Show(rect); // Show the dropdown
            }
        }

        static public void TeleportToPresetLocation(string location)
        {
            // Check if an object to teleport is selected
            if (objectToTeleport == null)
            {
                Debug.Log("ERROR: No object to teleport selected!");
                return;
            }

            // Find the target object to teleport to
            GameObject teleportTarget = GameObject.Find("Teleport Location/" + location);

            // Check if the target object exists
            if (teleportTarget == null)
            {
                Debug.Log("ERROR: No teleport target found!");
                return;
            }

            // Teleport the object to a preset location and adjust the rotation to match the target
            objectToTeleport.transform.position = teleportTarget.transform.position;
            objectToTeleport.transform.rotation = teleportTarget.transform.rotation;
        }
    }
}
