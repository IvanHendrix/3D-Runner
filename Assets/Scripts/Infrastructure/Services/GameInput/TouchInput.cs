using UnityEngine;

namespace Infrastructure.Services.GameInput
{
    public class TouchInput : IPlayerInput
    {
        private Vector2 _startPosition;
        private bool _swipeHandled;
        private int _strafeDirection;
        private bool _jumpPressed;

        public void UpdateInput()
        {
            _strafeDirection = 0;
            _jumpPressed = false;

            if (Input.touchCount == 0) return;

            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _startPosition = touch.position;
                    _swipeHandled = false;
                    break;

                case TouchPhase.Moved:
                case TouchPhase.Ended:
                    if (_swipeHandled) return;

                    Vector2 delta = touch.position - _startPosition;

                    if (delta.magnitude < 30f)
                    {
                        _jumpPressed = true;
                    }
                    else if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                    {
                        _strafeDirection = delta.x > 0 ? 1 : -1;
                    }

                    _swipeHandled = true;
                    break;
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