using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : LevelChange
{
    public Animator transitionAnimator;
    public void LoadFirstLevel(int levelIndex)
    {
        transitionAnimator.SetTrigger("startPressed");
    }
}
