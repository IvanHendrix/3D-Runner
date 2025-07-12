using System;
using UnityEngine;

namespace Logic
{
    public class GameTimer : MonoBehaviour
    {
        public event Action OnTimeEnd;
        
        public float TimeLimit = 60f;
        
        private float _timeLeft;

        private bool _isTimeout;

        private void Start()
        {
            _timeLeft = TimeLimit;
        }

        private void Update()
        {
            _timeLeft -= Time.deltaTime;

            if (_timeLeft <= 0f && !_isTimeout)
            {
                _isTimeout = true;
                OnTimeEnd?.Invoke();
            }
        }
    }
}