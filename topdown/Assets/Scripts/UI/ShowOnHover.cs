using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOnHover : MonoBehaviour
{
    public GameObject toEnable;

    public void OnMouseEnter()
    {
        toEnable.SetActive(true);
    }

    public void OnMouseExit()
    {
        toEnable.SetActive(false);
    }
}
