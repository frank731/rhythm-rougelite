using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAutoDestroy : MonoBehaviour
{
    public void DisableSelf()
    {
        gameObject.SetActive(false);
    }
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
