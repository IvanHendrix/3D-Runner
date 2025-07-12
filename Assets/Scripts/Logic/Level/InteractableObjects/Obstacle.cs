using System;
using Infrastructure.Services.Audio;
using Infrastructure.Services.Pool;
using UnityEngine;

namespace Logic.Level.InteractableObjects
{
    public abstract class Obstacle : MonoBehaviour, IPoolable, IPlayerInteractable
    {
        public event Action OnPlayerInteracted;
        [SerializeField] protected Renderer _renderer;

        public abstract ObstacleType Type { get; }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _renderer.material.color = Color.red;
                OnPlayerInteracted?.Invoke();
                GameController.Instance.LoseHP();
                AudioManager.Instance.PlaySFX("Hit");
            }
        }

        public virtual void ResetObject()
        {
            _renderer.material.color = Color.blue;
        }
    }
}