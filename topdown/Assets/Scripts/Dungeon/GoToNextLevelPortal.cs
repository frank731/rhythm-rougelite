using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChange : MonoBehaviour
{
    public void GoToLevel(AnimationEvent TypeAndIndex)
    {
        switch (TypeAndIndex.stringParameter)
        {
            case "nextLevel":
                FloorGlobal.Instance.levelChanged.Invoke();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                break;
            case "selectLevel":
                FloorGlobal.Instance.levelChanged.Invoke();
                SceneManager.LoadScene(TypeAndIndex.intParameter);
                break;
        }

    }
}

public class GoToNextLevelPortal : LevelChange
{
    public Animator transitionAnimatorChangeScene;
    private Animator transitionAnimatorVisual;
    private void Start()
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
