using UnityEngine;
using UnityEditor; // used for "Handles"
using UnityEngine.AI; // used for "NavMesh"

namespace RPG_Project.AI.Tools
{
    public class PatrolNodesGizmo : MonoBehaviour
    {
        [Header("Parameter")]
        [SerializeField] private bool showGizmos = true; // Determine if the Gizmos are shown or not
        [SerializeField] private bool showPath = true; // Determine if the path between nodes is shown or not

        // Gizmos data
        private NavMeshHit hit; // used to store the hit information
        private Vector3 offset = Vector3.up * 10; // offset by 10 on the Y axis
        private float radius = 1.0f; // radius of the sphere
        private string icon; // icon to use for the gizmos

        private void OnDrawGizmos()
        {
            if(NavMesh.SamplePosition(transform.position, out hit, Mathf.Infinity, NavMesh.AllAreas))
            {
                Gizmos.color = Color.green; // change the color of the gizmos
                Handles.color = new Color(0, 1, 0, 0.25f); // change the color of the handles
                icon = "Target_green.png";
            }
            
            if(showGizmos)
            {
                Gizmos.DrawLine(transform.position, hit.position); // draw a line between the object and the ground
            }

            if(hit.distance > 0.1f) // if the object is not touching the ground/NavMesh
            {
                Gizmos.color = Color.red; // change the color of the gizmos
                Handles.color = new Color(1, 0, 0, 0.25f); // change the color of the handles
                icon = "Target_red.png";
            }

            if(showGizmos)
            {
                Handles.Label(transform.position + offset * 1.15f, transform.name, new GUIStyle() { alignment = TextAnchor.MiddleCenter, fontSize = 12, fontStyle = FontStyle.Bold, normal = new GUIStyleState() { textColor = Gizmos.color } }); // draw the name of the node centered above the node
                Gizmos.DrawSphere(transform.position + offset, radius); // draw a sphere at an offset above the object
                Gizmos.DrawLine(transform.position + offset, transform.position); // draw a line between at an offset above the object
                Gizmos.DrawIcon(transform.position, icon); // draw an icon above the object
                Handles.DrawSolidDisc(hit.position, Vector3.up, hit.distance); // draw a solid disc on the ground
            }

            if(showPath) // draw the path between nodes
            {
                if(transform.GetSiblingIndex() > 0) // if the node is not the first one
                {
                    Gizmos.DrawLine(transform.position, transform.parent.GetChild(transform.GetSiblingIndex() - 1).position); // draw a line between this node and the previous node
                }
                else // if this is the first node
                {
                    Gizmos.DrawLine(transform.position, transform.parent.GetChild(transform.parent.childCount - 1).position); // draw a line between this node and the last node
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Handles.color = Color.black; // change the color of the handles

            if(showGizmos)
            {
                Handles.DrawWireDisc(hit.position, Vector3.up, hit.distance); // draw a wire disc on the ground
            }
        }
    }
}
