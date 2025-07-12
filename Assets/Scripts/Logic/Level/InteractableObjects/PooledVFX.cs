using System;
using System.Collections;
using Infrastructure.Services.Pool;
using UnityEngine;

namespace Logic.Level.InteractableObjects
{
    public class PooledVFX : MonoBehaviour, IPoolable
    {
        [SerializeField] private ParticleSystem _particleSystem;

        private Action _onComplete;

        public void Play(Action onComplete)
        {
            _onComplete = onComplete;
            _particleSystem.Play();
            StartCoroutine(ReturnAfterDuration());
        }

        private IEnumerator ReturnAfterDuration()
        {
            yield return new WaitForSeconds(_particleSystem.main.duration);
            _onComplete?.Invoke();
        }

        public void ResetObject()
        {
            _onComplete = null;
            _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }
}