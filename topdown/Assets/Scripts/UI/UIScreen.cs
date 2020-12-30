using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScreen : MonoBehaviour
{
    public bool startDisabled = false;
    private bool restarted = false;
    private void Awake()
    {
        if (GameObject.FindGameObjectsWithTag(tag).Length > 1)
        {
            Destroy(gameObject); //ensure same screen not duplicated
        }
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (startDisabled)
        {
            gameObject.SetActive(false);
        }
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
        }
    }
}
