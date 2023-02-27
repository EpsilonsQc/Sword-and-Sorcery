using UnityEngine;
using UnityEngine.AI;
using RPG_Project.AI.BehaviourTree;

namespace RPG_Project.AI.Tasks
{
    public class FaceTarget : Node
    {
        private Transform _player;
        private Transform _AI;
        private NavMeshAgent _agent;

        public FaceTarget(Transform transform)
        {
            _player = GameObject.Find("Player").transform;
            _AI = transform; // Get the transform of the AI
            _agent = _AI.GetComponent<NavMeshAgent>();
        }

        public override NodeState Evaluate()
        {
            Vector3 direction = (_player.position - _AI.position).normalized; // Get direction to target
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); // Create rotation to look at target
            _AI.rotation = Quaternion.Slerp(_AI.rotation, lookRotation, Time.deltaTime * (_agent.angularSpeed * Mathf.Deg2Rad)); // Rotate toward target smoothly

            return NodeState.RUNNING;
        }
    }
}
