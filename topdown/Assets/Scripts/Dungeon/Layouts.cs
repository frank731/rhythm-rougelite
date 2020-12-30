using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Layouts", menuName = "Layouts", order = 1)]
public class Layouts : ScriptableObject
{
    public int floorID;
    public GameObject[] normalLayouts;
    public GameObject[] largeLayouts;
    public GameObject[] bossLayouts;
    public GameObject[] itemLayouts;

    public void LoadLayouts()
    {
        normalLayouts = Resources.LoadAll<GameObject>("Prefabs/Layouts/Room Layouts/" + floorID.ToString());
        largeLayouts = Resources.LoadAll<GameObject>("Prefabs/Layouts/Large Room Layouts/" + floorID.ToString());
        bossLayouts = Resources.LoadAll<GameObject>("Prefabs/Layouts/Boss Layouts/" + floorID.ToString());
        itemLayouts = Resources.LoadAll<GameObject>("Prefabs/Layouts/Item Layouts/" + floorID.ToString());
    }
}
