using UnityEngine;
using UnityEngine.AI;
using RPG_Project.AI.BehaviourTree;
using RPG_Project.AI.Manager;

namespace RPG_Project.AI.Tasks
{
    public class Follow : Node
    {
        private Transform _player;
        private Transform _AI;
        private NavMeshAgent _agent;
        private Animator _animator;
        private DataManager _dataManager;

        private float defaultSpeed;
        private float runSpeed;

        private bool isRunning;
        private bool isWalking;
        private bool isWalkingBack;

        public Follow(Transform transform)
        {
            _player = GameObject.Find("Player").transform;
            _AI = transform; // Get the transform of the AI
            _agent = _AI.GetComponent<NavMeshAgent>();
            _animator = transform.GetComponent<Animator>();
            _dataManager = transform.GetComponent<DataManager>();

            defaultSpeed = _agent.speed;
            runSpeed = _agent.speed * 2;
        }

        public override NodeState Evaluate()
        {
            // Get the running state of the player
            isRunning = _player.GetComponent<Animator>().GetBool("isRunning");
            isWalking = _player.GetComponent<Animator>().GetBool("isWalking");
            isWalkingBack = _player.GetComponent<Animator>().GetBool("isWalkingBack");

            if(isRunning && (isWalking || isWalkingBack))
            {
                _agent.speed = runSpeed; // Increase speed (run)
            }
            else
            {
                _agent.speed = defaultSpeed; // Reset speed
            }

            _animator.SetFloat("movementSpeed", _agent.velocity.magnitude); // Set animation speed to agent speed

            if (Vector3.Distance(_AI.position, _player.position) > _dataManager.distanceOffset) // Check if target is in range
            {
                if(_agent.isStopped == true)
                {
                    _agent.isStopped = false; // Resume the agent if it was stopped
                }

                _agent.SetDestination(_player.position - _AI.forward * (_dataManager.distanceOffset - 0.1f)); // Move toward the target while keeping an offset

                return NodeState.RUNNING;
            }

            return NodeState.SUCCESS;
        }
    }
}
