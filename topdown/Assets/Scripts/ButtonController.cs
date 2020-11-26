using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : LevelChange
{
    public Animator transitionAnimator;
    public void LoadFirstLevel(int levelIndex)
    {
        transitionAnimator.SetTrigger("startPressed");
    }
    public void RestartGame()
    {
        FloorGlobal.Instance.restarted = true;
        SceneManager.LoadScene(1);
    }

    public void ResumeGame()
    {
        FloorGlobal.Instance.OnPause(FloorGlobal.Instance.pauseCanvas);
    }
}
