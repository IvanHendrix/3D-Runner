using System;
using Infrastructure.Services.Audio;
using Infrastructure.Services.Pool;
using UnityEngine;

namespace Logic.Level.InteractableObjects
{
    public class Coin : MonoBehaviour, IPoolable, IPlayerInteractable
    {
        public event Action OnPlayerInteracted;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                OnPlayerInteracted?.Invoke();
                PoolService.Instance.PlayVFX(transform.position);
                PoolService.Instance.ReturnCoin(this);
                GameController.Instance.AddCoin();
                AudioManager.Instance.PlaySFX("Coin");
            }
        }

        public void ResetObject() { }
    }
}