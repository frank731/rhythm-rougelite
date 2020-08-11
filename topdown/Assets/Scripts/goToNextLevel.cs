using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class goToNextLevel : MonoBehaviour
{
    public Animator transitionAnimatorChangeScene;
    private Animator transitionAnimatorVisual;
    int nextLevelIndex;
    void Start()
    {
        transitionAnimatorVisual = GameObject.FindGameObjectWithTag("FadeCanvas").GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //fades to white and then loads next level
        nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;
        transitionAnimatorVisual.SetTrigger("floorComplete");
        transitionAnimatorChangeScene.SetTrigger("floorComplete");
    }
    public void GoToLevel()
    {
        SceneManager.LoadScene(nextLevelIndex);
    }
}
