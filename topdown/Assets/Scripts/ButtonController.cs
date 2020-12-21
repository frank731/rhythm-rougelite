using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : LevelChange
{
    public Animator transitionAnimator;
    public GameObject settingsScreen;
    public void LoadFirstLevel(int levelIndex)
    {
        transitionAnimator.SetTrigger("startPressed");
    }
    public void RestartGame()
    {
        FloorGlobal.Instance.deathCanvas.SetActive(false);
        FloorGlobal.Instance.levelChanged.Invoke();
        SceneManager.LoadScene(1);
    }

    public void ResumeGame()
    {
        FloorGlobal.Instance.Pause(FloorGlobal.Instance.pauseCanvas);
    }
    public void OpenSettings()
    {
        FloorGlobal.Instance.Pause(settingsScreen);
    }
}
