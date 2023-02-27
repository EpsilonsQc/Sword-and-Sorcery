using UnityEngine;
using UnityEngine.AI;
using RPG_Project.AI.BehaviourTree;
using RPG_Project.AI.Tools;

namespace RPG_Project.AI.Tasks
{
    public class Patrol : Node
    {
        private Transform _AI;
        private NavMeshAgent _agent;
        private Animator _animator;
        private PatrolNodes _patrolNodes;

        public Patrol(Transform transform)
        {
            _AI = transform; // Get the transform of the AI
            _agent = _AI.GetComponent<NavMeshAgent>();
            _animator = transform.GetComponent<Animator>();
            _patrolNodes = transform.GetComponent<PatrolNodes>();
        }

        public override NodeState Evaluate()
        {
            if (_patrolNodes.PausePatrol == true)
            {
                _agent.isStopped = false; // Resume the agent if it was stopped
                _patrolNodes.PausePatrol = false; // Resume patrol route if it was paused
            }

            _animator.SetFloat("movementSpeed", _agent.velocity.magnitude); // Set animation speed to agent speed

            return NodeState.RUNNING;
        }
    }
}
