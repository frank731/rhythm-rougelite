using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameButtonController : MonoBehaviour
{
    public GameObject settingsScreen;
    private FloorGlobal floorGlobal;

    private void Start()
    {
        floorGlobal = FloorGlobal.Instance;
    }

    public void RestartGame()
    {
        floorGlobal.deathCanvas.SetActive(false);
        floorGlobal.levelChanged.Invoke();
        SceneManager.LoadScene(1);
    }

    public void ResumeGame()
    {
        floorGlobal.Pause(floorGlobal.pauseCanvas);
    }
    public void OpenSettings()
    {
        floorGlobal.Pause(settingsScreen);
    }
}
