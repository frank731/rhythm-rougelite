using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChange : MonoBehaviour
{
    private FloorGlobal floorGlobal;

    private void Start()
    {
        floorGlobal = FloorGlobal.Instance;    
    }

    public void GoToLevel(AnimationEvent TypeAndIndex)
    {
        try
        {
            FloorGlobal.Instance.levelChanged.Invoke();
        }
        catch
        {
            
        }
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
