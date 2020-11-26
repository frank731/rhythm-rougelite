using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScreen : MonoBehaviour
{
    private bool restarted = false;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)
        {
            if (restarted)
            {
                Destroy(gameObject);
            }
            else
            {
                restarted = true;
            }
        }
    }
}
