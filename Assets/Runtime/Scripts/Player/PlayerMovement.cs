using UnityEngine;

namespace RPG_Project.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Player Movements")]
        [SerializeField] private int _walkSpeed;
        [SerializeField] private int _runSpeed;
        [SerializeField] private int _rotationSpeed;

        // References
        private Animator _animator;
        private Rigidbody _rigidbody;
        private Transform _transform;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody>();
            _transform = this.transform;
        }

        private void FixedUpdate()
        {
            MovementSpeed();
            RotationSpeed();
        }

        // Fine-control over the movement speed of the player (root motion override)
        private void MovementSpeed()
        {
            // WALKING OR RUNNING
            if (_animator.GetBool("isWalking"))
            {
                if (_animator.GetBool("isAttacking"))
                {
                    _rigidbody.velocity = Vector3.zero; // don't move while attacking
                    return;
                }
                else if(_animator.GetBool("isRunning") && (!_animator.GetBool("isWalkingBack")))
                {
                    AddForce(_runSpeed);
                    return;
                }
                else if (!_animator.GetBool("isWalkingBack"))
                {
                    AddForce(_walkSpeed);
                    return;
                }
            }

            // WALKING BACK OR RUNNING BACK
            if (_animator.GetBool("isWalkingBack"))
            {
                if (_animator.GetBool("isAttacking"))
                {
                    _rigidbody.velocity = Vector3.zero; // don't move while attacking
                    return;
                }
                else if(_animator.GetBool("isRunning") && (!_animator.GetBool("isWalking")))
                {
                    AddForce(-_runSpeed);
                    return;
                }
                else if (!_animator.GetBool("isWalking"))
                {
                    AddForce(-_walkSpeed);
                    return;
                }
            }
        }

        // Fine-control over the rotation speed of the player (root motion override)
        private void RotationSpeed()
        {
            if (_animator.GetBool("isWalking") || _animator.GetBool("isWalkingBack") || _animator.GetBool("isRunning"))
            {
                _rotationSpeed = 100; // Default rotation speed
            }
            else // Idle
            {
                _rotationSpeed = 20; // Reduce rotation speed (rotation speed when idle partially rely on root motion)
            }

            if (_animator.GetBool("isTurningLeft"))
            {
                MoveRotation(-_rotationSpeed);
            }
            else if (_animator.GetBool("isTurningRight"))
            {
                MoveRotation(_rotationSpeed);
            }
        }

        // Move the player forward or backward
        private void AddForce(float movementSpeed)
        {
            _rigidbody.AddForce(_transform.forward * movementSpeed, ForceMode.Acceleration);
        }

        // Rotate (turn left or right)
        private void MoveRotation(float rotationSpeed)
        {
            _rigidbody.MoveRotation(_rigidbody.rotation * Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0));
        }
    }
}
