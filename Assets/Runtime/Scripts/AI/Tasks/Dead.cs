using UnityEngine;
using UnityEngine.AI;
using RPG_Project.AI.BehaviourTree;
using RPG_Project.AI.Manager;
using RPG_Project.AI.Tools;

namespace RPG_Project.AI.Tasks
{
    public class Dead : Node
    {
        private Transform _AI;
        private NavMeshAgent _agent;
        private Animator _animator;
        private DataManager _dataManager;
        private PatrolNodes _patrolNodes;

        public Dead(Transform transform)
        {
            _AI = transform; // Get the transform of the AI
            _agent = _AI.GetComponent<NavMeshAgent>();
            _animator = transform.GetComponent<Animator>();
            _dataManager = transform.GetComponent<DataManager>();
            _patrolNodes = transform.GetComponent<PatrolNodes>();
        }

        public override NodeState Evaluate()
        {
            if (_dataManager.isDead)
            {
                _animator.SetBool("isDead", true);
                _agent.isStopped = true; // Stop the agent if it was moving
                _patrolNodes.PausePatrol = true; // Pause patrol route if it was active
                return NodeState.FAILURE; // Enemy is dead
            }
            else
            {
                return NodeState.SUCCESS; // Enemy is alive
            }

        }
    }
}
