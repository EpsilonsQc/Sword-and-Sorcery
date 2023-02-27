using UnityEngine;
using UnityEngine.AI;
using RPG_Project.AI.BehaviourTree;
using RPG_Project.AI.Tools;

namespace RPG_Project.AI.Tasks
{
    public class StopPatrol : Node
    {
        private Transform _AI;
        private NavMeshAgent _agent;
        private Animator _animator;
        private PatrolNodes _patrolNodes;

        public StopPatrol(Transform transform)
        {
            _AI = transform; // Get the transform of the AI
            _agent = _AI.GetComponent<NavMeshAgent>();
            _animator = transform.GetComponent<Animator>();
            _patrolNodes = transform.GetComponent<PatrolNodes>();
        }

        public override NodeState Evaluate()
        {
            if (_patrolNodes.PausePatrol == false)
            {
                _agent.isStopped = true; // Stop the agent
                _patrolNodes.PausePatrol = true; // Pause patrol if it was not paused
            }

            _animator.SetFloat("movementSpeed", _agent.velocity.magnitude);

            return NodeState.SUCCESS;
        }
    }
}
