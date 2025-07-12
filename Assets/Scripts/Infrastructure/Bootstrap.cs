using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
    public class Bootstrap : MonoBehaviour
    {
        private const string SceneName = "Main";

        private void Start()
        {
            SceneManager.LoadScene(SceneName);
        }
    }
}