using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionButton : MonoBehaviour
{
    public Animator transitionAnimator;
    private int nextLevelIndex = 1;
    public void LoadLevel(int levelIndex)
    {
        transitionAnimator.SetTrigger("startPressed");
        nextLevelIndex = levelIndex;
    }
    public void Switch()
    {
        SceneManager.LoadScene(nextLevelIndex);
    }
}
