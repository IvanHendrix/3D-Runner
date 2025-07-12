using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.Services.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _sfxSource;
        [SerializeField] private AudioClip _backgroundMusic;
        [SerializeField] private List<AudioClip> _sfxClips;

        private Dictionary<string, AudioClip> _sfxMap;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            _sfxMap = new Dictionary<string, AudioClip>();
            foreach (var clip in _sfxClips)
            {
                _sfxMap[clip.name] = clip;
            }
        }

        private void Start()
        {
            _musicSource.clip = _backgroundMusic;
            _musicSource.loop = true;
            _musicSource.Play();
        }

        public void PlaySFX(string key)
        {
            if (_sfxMap.TryGetValue(key, out var clip))
            {
                _sfxSource.PlayOneShot(clip);
            }
        }
    }
}