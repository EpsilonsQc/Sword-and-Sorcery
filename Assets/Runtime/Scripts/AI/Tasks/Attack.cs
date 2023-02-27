using UnityEngine;
using RPG_Project.AI.BehaviourTree;
using RPG_Project.AI.Manager;

namespace RPG_Project.AI.Tasks
{
    public class Attack : Node
    {
        private Transform _player;
        private Transform _AI;
        private Animator _animator;
        private DataManager _dataManager;

        private float _attackSpeed;
        private float _attackTimer;

        public Attack(Transform transform)
        {
            _player = GameObject.Find("Player").transform;
            _AI = transform; // Get the transform of the AI
            _animator = transform.GetComponent<Animator>();
            _dataManager = transform.GetComponent<DataManager>();

            _attackSpeed = _dataManager.attackSpeed;
        }

        public override NodeState Evaluate()
        {
            if (Vector3.Distance(_AI.position, _player.position) <= _dataManager.attackRange) // Check if player is in range
            {
                // Check if an attack is already in progress
                if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack Light") || _animator.GetCurrentAnimatorStateInfo(0).IsName("Attack Heavy"))
                {
                    _animator.SetBool("isAttackingLight", false); // Stop attacking
                    _animator.SetBool("isAttackingHeavy", false); // Stop attacking
                    return NodeState.RUNNING; // Attack already in progress
                }

                _attackTimer += Time.deltaTime; // Count the time since the last attack
                if(_attackTimer >= _attackSpeed) // Check if the timer is greater than the attack speed
                {
                    int attackType = Random.Range(0, 2); // 0 = light attack, 1 = heavy attack

                    if (attackType == 0) // Light attack
                    {
                        _animator.SetBool("isAttackingHeavy", false); // Stop heavy attack animation
                        _animator.SetBool("isAttackingLight", true); // Play light attack animation
                    }
                    else if (attackType == 1) // Heavy attack
                    {
                        _animator.SetBool("isAttackingLight", false); // Stop light attack animation
                        _animator.SetBool("isAttackingHeavy", true); // Play heavy attack animation
                    }

                    _attackTimer = 0; // Reset the timer for the next attack
                }

                return NodeState.RUNNING; // Enemy attacked
            }

            _animator.SetBool("isAttackingLight", false); // Stop attacking
            _animator.SetBool("isAttackingHeavy", false); // Stop attacking
            return NodeState.FAILURE; // Target is out of range
        }
    }
}
