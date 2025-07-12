using UnityEngine;

namespace Infrastructure.Services.GameInput
{
    public class InputProviderResolver : MonoBehaviour
    {
        public static InputProviderResolver Instance { get; private set; }

        public static IPlayerInput Input => Instance._input;
        
        private IPlayerInput _input;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                ResolveInput();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void ResolveInput()
        {
#if UNITY_EDITOR
            _input = new MouseInput();
#elif UNITY_ANDROID || UNITY_IOS
            _input = new TouchInput();
#else
            _input = new MouseInput();
#endif
        }
    }
}