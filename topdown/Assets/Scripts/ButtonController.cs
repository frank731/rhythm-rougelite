using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : LevelChange
{
    public Animator transitionAnimator;
    public FloorGlobal floorGlobal;
    public void LoadFirstLevel(int levelIndex)
    {
        transitionAnimator.SetTrigger("startPressed");
    }
    public void ResumeGame()
    {
        floorGlobal.OnPause();
    }
}
