using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelChange : MonoBehaviour
{
    public void GoToLevel(AnimationEvent TypeAndIndex)
    {
        switch (TypeAndIndex.stringParameter)
        {
            case "nextLevel":
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                break;
            case "selectLevel":
                SceneManager.LoadScene(TypeAndIndex.intParameter);
                break;
        }
        
    }
}

public class GoToNextLevel : LevelChange
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
        transitionAnimatorVisual.SetTrigger("floorComplete");
        transitionAnimatorChangeScene.SetTrigger("floorComplete");
    }
    
}
