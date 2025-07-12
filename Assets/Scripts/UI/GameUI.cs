using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameUI : MonoBehaviour
    {
        public event Action OnResetClick;
        
        [SerializeField] private TMP_Text _coinText;
        [SerializeField] private TMP_Text _hpText;
        [SerializeField] private GameObject _winScreen;
        [SerializeField] private GameObject _gameOverScreen;
        [SerializeField] private Button _reloadButton;
        
        private void Start()
        {
            _reloadButton.onClick.AddListener(OnReloadButtonClick);
        }

        private void OnReloadButtonClick()
        {
            _winScreen.SetActive(false);
            _gameOverScreen.SetActive(false);
            
            OnResetClick?.Invoke();
        }

        public void UpdateCoins(int coins)
        {
            _coinText.text = $"Coins: {coins}";
        }

        public void UpdateHP(int hp)
        {
            _hpText.text = $"HP: {hp}";
        }

        public void ShowWinScreen()
        {
            _reloadButton.gameObject.SetActive(true);
            _winScreen.SetActive(true);
        }

        public void ShowLoseScreen()
        {
            _reloadButton.gameObject.SetActive(true);
            _gameOverScreen.SetActive(true);
        }

        private void OnDestroy()
        {
            _reloadButton.onClick.RemoveListener(OnReloadButtonClick);
        }
    }
}