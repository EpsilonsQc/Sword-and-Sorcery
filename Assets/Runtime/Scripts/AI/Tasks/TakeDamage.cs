using UnityEngine;
using RPG_Project.AI.BehaviourTree;
using RPG_Project.AI.Manager;

namespace RPG_Project.AI.Tasks
{
    public class TakeDamage : Node
    {
        private Transform _player;
        private Transform _AI;
        private Animator _animator;
        private DataManager _dataManager;
        private DamageManager _damageManager;

        private float _attackSpeed;
        private float _attackTimer;

        public TakeDamage(Transform transform)
        {
            _player = GameObject.Find("Player").transform;
            _AI = transform; // Get the transform of the AI
            _animator = transform.GetComponent<Animator>();
            _dataManager = transform.GetComponent<DataManager>();
            _damageManager = transform.GetComponent<DamageManager>();

            _attackSpeed = _dataManager.attackSpeed; // TO DO: Get attack speed from the player (or weapon) instead of the AI data
        }

        public override NodeState Evaluate()
        {
            if (Vector3.Distance(_AI.position, _player.position) <= _dataManager.attackRange) // Check if player is in range [TO DO: Enemy is in range]
            {
                if (_player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(2).IsName("Attack"))
                {
                    _attackTimer += Time.deltaTime; // Count the time since the last attack
                    if (_attackTimer >= _attackSpeed) // Check if the timer is greater than the attack speed
                    {
                        _damageManager.TakeHit(); // AI take damage from the player
                        _animator.SetTrigger("isTakingDamage"); // Set the animation to "taking damage"
                        _attackTimer = 0; // Reset the timer for the next attack
                    }
                    return NodeState.RUNNING; // Enemy take damage
                }
                else
                {
                    _attackTimer = 0; // Reset the timer for the next attack
                    return NodeState.RUNNING; // Enemy take damage
                }
            }

            return NodeState.FAILURE; // Player is out of range [TO DO: Enemy is out of range]
        }
    }
}
