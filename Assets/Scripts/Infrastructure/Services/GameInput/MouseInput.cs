using UnityEngine;

namespace Infrastructure.Services.GameInput
{
    public class MouseInput : IPlayerInput
    {
        private Vector2 _startPosition;
        private bool _swipeHandled;
        private int _strafeDirection;
        private bool _jumpPressed;

        public void UpdateInput()
        {
            _strafeDirection = 0;
            _jumpPressed = false;

            if (Input.GetMouseButtonDown(0))
            {
                _startPosition = Input.mousePosition;
                _swipeHandled = false;
            }
            else if (Input.GetMouseButtonUp(0) && !_swipeHandled)
            {
                Vector2 delta = (Vector2)Input.mousePosition - _startPosition;

                if (delta.magnitude < 30f)
                {
                    _jumpPressed = true;
                }
                else if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                {
                    _strafeDirection = delta.x > 0 ? 1 : -1;
                }

                _swipeHandled = true;
            }
        }

        public bool TryGetStrafe(out int direction)
        {
            direction = _strafeDirection;
            return direction != 0;
        }

        public bool TryGetJump()
        {
            return _jumpPressed;
        }
    }
}