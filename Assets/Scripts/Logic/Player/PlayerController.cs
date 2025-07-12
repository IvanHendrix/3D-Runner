using Infrastructure.Services.Audio;
using Infrastructure.Services.GameInput;
using UnityEngine;

namespace Logic.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        private const string IsRunningKey = "isRunning";
        private const string IsJumpingKey = "isJumping";

        [Header("Movement Settings")]
        [SerializeField] private float _strafeSpeed = 5f;
        [SerializeField] private float _jumpForce = 8f;
        [SerializeField] private float _gravity = 20f;
        [SerializeField] private float _laneDistance = 2f;

        [Header("References")] 
        [SerializeField] private Animator _animator;
        [SerializeField] private CharacterController _controller;

        private IPlayerInput _input;

        private Vector3 _moveVector;
        private float _verticalVelocity;
        private int _currentLane = 1; // 0 = left, 1 = center, 2 = right

        private void Start()
        {
            _input = InputProviderResolver.Input;
            _animator.SetBool(IsRunningKey, true);
        }

        private void Update()
        {
            _input.UpdateInput();

            HandleStrafe();
            HandleJump();
            ApplyGravity();
            MoveCharacter();
        }

        private void HandleStrafe()
        {
            if (!_controller.isGrounded)
            {
                return;
            }

            if (_input.TryGetStrafe(out int direction))
            {
                MoveLane(direction > 0); // true = right, false = left
                AudioManager.Instance.PlaySFX("Strafe");
            }
        }

        private void HandleJump()
        {
            if (!_controller.isGrounded)
            {
                return;
            }

            if (_input.TryGetJump())
            {
                _verticalVelocity = _jumpForce;
                _animator.SetTrigger(IsJumpingKey);
            }
        }

        private void ApplyGravity()
        {
            if (_controller.isGrounded)
            {
                if (_verticalVelocity < 0f)
                    _verticalVelocity = -1f;
                
                if (!_animator.GetBool(IsRunningKey))
                {
                    _animator.SetBool(IsRunningKey, true);
                }
            }
            else
            {
                _verticalVelocity -= _gravity * Time.deltaTime;
            }
        }

        private void MoveCharacter()
        {
            Vector3 targetPosition = transform.position.z * Vector3.forward;

            if (_currentLane == 0)
            {
                targetPosition += Vector3.left * _laneDistance;
            }
            else if (_currentLane == 2)
            {
                targetPosition += Vector3.right * _laneDistance;
            }

            float diffX = targetPosition.x - transform.position.x;
            float moveX = diffX * _strafeSpeed;

            _moveVector = new Vector3(moveX, _verticalVelocity, 0f);
            _controller.Move(_moveVector * Time.deltaTime);
        }

        private void MoveLane(bool moveRight)
        {
            _currentLane += moveRight ? 1 : -1;
            _currentLane = Mathf.Clamp(_currentLane, 0, 2);
        }
    }
}