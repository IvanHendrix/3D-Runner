using System;
using Infrastructure.Services.Audio;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Logic
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }

        private const string SceneName = "Main";
        
        [SerializeField] private GameUI _ui;
        [SerializeField] private GameTimer _timer;
        [SerializeField] private int _startHP = 3;

        private int _coins;
        private int _hp;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _ui.OnResetClick += ReloadScene;
            _timer.OnTimeEnd += WinGame;
            
            ResetGame();
        }

        public void AddCoin()
        {
            _coins++;
            _ui.UpdateCoins(_coins);
        }

        public void LoseHP()
        {
            _hp--;
            _ui.UpdateHP(_hp);
            if (_hp <= 0)
            {
                GameOver();
            }
        }

        private void GameOver()
        {
            _ui.ShowLoseScreen();
            AudioManager.Instance.PlaySFX("Lose");
        }

        private void WinGame()
        {
            _ui.ShowWinScreen();
            AudioManager.Instance.PlaySFX("Win");
        }

        private void ReloadScene()
        {
            SceneManager.LoadScene(SceneName);
        }
        
        private void ResetGame()
        {
            _coins = 0;
            _hp = _startHP;
            _ui.UpdateCoins(_coins);
            _ui.UpdateHP(_hp);
        }

        private void OnDestroy()
        {
            _ui.OnResetClick -= ReloadScene;
            _timer.OnTimeEnd -= WinGame;
        }
    }
}