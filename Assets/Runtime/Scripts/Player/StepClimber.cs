using UnityEngine;

namespace RPG_Project.Player
{
    public class StepClimber : MonoBehaviour
    {
        [Header("Step Climbing")]
        [SerializeField] private float climbForce; // The force applied when the player collide with a step

        [Header("Raycast")]
        [SerializeField] private float rayLength; // Length of the Raycast

        [Header("Gizmos")]
        [SerializeField] private bool showGizmos; // Show the raycast in the scene view

        // References
        private GameObject upperRaycast; // Upper Raycast (max step height)
        private GameObject lowerRaycast; // Lower Raycast (min step height)
        private Rigidbody _rigidbody;

        private void Awake()
        {
            upperRaycast = GameObject.Find("Player/StepRayUpper");
            lowerRaycast = GameObject.Find("Player/StepRayLower");
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            DetectCollision(); // Check if the player is colliding with a step
        }

        private void DetectCollision()
        {
            #region FRONT RAYCAST
            // FORWARD
            RaycastHit hitLowerForward;
            if(Physics.Raycast(lowerRaycast.transform.position, transform.TransformDirection(Vector3.forward), out hitLowerForward, rayLength))
            {
                RaycastHit hitUpperForward;
                if(!Physics.Raycast(upperRaycast.transform.position, transform.TransformDirection(Vector3.forward), out hitUpperForward, rayLength))
                {
                    ClimbStep();
                }
            }

            // 45 DEGREES RIGHT
            RaycastHit hitLower45Right;
            if(Physics.Raycast(lowerRaycast.transform.position, transform.TransformDirection(1.5f, 0.0f, 1.0f), out hitLower45Right, rayLength))
            {
                RaycastHit hitUpper45Right;
                if(!Physics.Raycast(upperRaycast.transform.position, transform.TransformDirection(1.5f, 0.0f, 1.0f), out hitUpper45Right, rayLength))
                {
                    ClimbStep();
                }
            }

            // 45 DEGREES LEFT
            RaycastHit hitLower45Left;
            if(Physics.Raycast(lowerRaycast.transform.position, transform.TransformDirection(-1.5f, 0.0f, 1.0f), out hitLower45Left, rayLength))
            {
                RaycastHit hitUpper45Left;
                if(!Physics.Raycast(upperRaycast.transform.position, transform.TransformDirection(-1.5f, 0.0f, 1.0f), out hitUpper45Left, rayLength))
                {
                    ClimbStep();
                }
            }
            #endregion

            #region BACK RAYCAST
            // BACKWARD
            RaycastHit hitLowerBackward;
            if(Physics.Raycast(lowerRaycast.transform.position, transform.TransformDirection(Vector3.back), out hitLowerBackward, rayLength))
            {
                RaycastHit hitUpperBackward;
                if(!Physics.Raycast(upperRaycast.transform.position, transform.TransformDirection(Vector3.back), out hitUpperBackward, rayLength))
                {
                    ClimbStep();
                }
            }

            // 45 DEGREES RIGHT
            RaycastHit hitLowerBack45Right;
            if(Physics.Raycast(lowerRaycast.transform.position, transform.TransformDirection(1.5f, 0.0f, 1.0f), out hitLowerBack45Right, rayLength))
            {
                RaycastHit hitUpperBack45Right;
                if(!Physics.Raycast(upperRaycast.transform.position, transform.TransformDirection(1.5f, 0.0f, 1.0f), out hitUpperBack45Right, rayLength))
                {
                    ClimbStep();
                }
            }

            // 45 DEGREES LEFT
            RaycastHit hitLowerBack45Left;
            if(Physics.Raycast(lowerRaycast.transform.position, transform.TransformDirection(-1.5f, 0.0f, 1.0f), out hitLowerBack45Left, rayLength))
            {
                RaycastHit hitUpperBack45Left;
                if(!Physics.Raycast(upperRaycast.transform.position, transform.TransformDirection(-1.5f, 0.0f, 1.0f), out hitUpperBack45Left, rayLength))
                {
                    ClimbStep();
                }
            }
            #endregion
        }

        // Climb the step
        private void ClimbStep()
        {
            _rigidbody.position += new Vector3(0, climbForce, 0); // Apply force on the Y-axis (climb the step)
        }
        
        #if UNITY_EDITOR // Only compile this part in the editor
        private void OnDrawGizmos()
        {
            if(Application.isPlaying && showGizmos)
            {
                Gizmos.color = Color.red;

                #region FRONT RAYCAST
                // FRONT
                Gizmos.DrawRay(lowerRaycast.transform.position, transform.TransformDirection(Vector3.forward) * rayLength);
                Gizmos.DrawRay(upperRaycast.transform.position, transform.TransformDirection(Vector3.forward) * rayLength);

                // 45 DEGREES RIGHT
                Gizmos.DrawRay(lowerRaycast.transform.position, transform.TransformDirection(1.5f, 0, 1) * rayLength);
                Gizmos.DrawRay(upperRaycast.transform.position, transform.TransformDirection(1.5f, 0, 1) * rayLength);

                // 45 DEGREES LEFT
                Gizmos.DrawRay(lowerRaycast.transform.position, transform.TransformDirection(-1.5f, 0, 1) * rayLength);
                Gizmos.DrawRay(upperRaycast.transform.position, transform.TransformDirection(-1.5f, 0, 1f) * rayLength);
                #endregion

                #region BACK RAYCAST
                // BACKWARD
                Gizmos.DrawRay(lowerRaycast.transform.position, transform.TransformDirection(Vector3.back) * rayLength);
                Gizmos.DrawRay(upperRaycast.transform.position, transform.TransformDirection(Vector3.back) * rayLength);

                // 45 DEGREES RIGHT
                Gizmos.DrawRay(lowerRaycast.transform.position, transform.TransformDirection(1.5f, 0, -1) * rayLength);
                Gizmos.DrawRay(upperRaycast.transform.position, transform.TransformDirection(1.5f, 0, -1) * rayLength);

                // 45 DEGREES LEFT
                Gizmos.DrawRay(lowerRaycast.transform.position, transform.TransformDirection(-1.5f, 0, -1) * rayLength);
                Gizmos.DrawRay(upperRaycast.transform.position, transform.TransformDirection(-1.5f, 0, -1) * rayLength);
                #endregion
            }
        }
        #endif
    }
}
