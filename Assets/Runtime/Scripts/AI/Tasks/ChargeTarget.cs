using UnityEngine;
using UnityEngine.AI;
using RPG_Project.AI.BehaviourTree;
using RPG_Project.AI.Manager;

namespace RPG_Project.AI.Tasks
{
    public class ChargeTarget : Node
    {
        private Transform _player;
        private Transform _AI;
        private NavMeshAgent _agent;
        private Animator _animator;
        private DataManager _dataManager;

        private float defaultSpeed;
        private float chargeSpeed;

        public ChargeTarget(Transform transform)
        {
            _player = GameObject.Find("Player").transform;
            _AI = transform; // Get the transform of the AI
            _agent = _AI.GetComponent<NavMeshAgent>();
            _animator = transform.GetComponent<Animator>();
            _dataManager = transform.GetComponent<DataManager>();

            defaultSpeed = _agent.speed;
            chargeSpeed = _agent.speed * 2f;
        }

        public override NodeState Evaluate()
        {
            _animator.SetFloat("movementSpeed", _agent.velocity.magnitude); // Set animation speed to agent speed

            if (Vector3.Distance(_AI.position, _player.position) >= _dataManager.distanceOffset) // If the target is too far away
            {
                if(_agent.isStopped == true)
                {
                    _agent.isStopped = false; // Resume the agent if it was stopped
                }

                _agent.speed = chargeSpeed; // Increase speed
                _agent.SetDestination(_player.position - _AI.forward * (_dataManager.distanceOffset - 0.1f)); // Move toward the target while keeping an offset
                return NodeState.RUNNING;
            }

            _agent.speed = defaultSpeed; // Reset speed
            return NodeState.RUNNING;
        }
    }
}
