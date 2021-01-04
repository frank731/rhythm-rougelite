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
    public GameObject emptyLayout;

    private string floorIDString;

    public void LoadLayouts()
    {
        floorIDString = floorID.ToString();
        normalLayouts = Resources.LoadAll<GameObject>("Prefabs/Layouts/Room Layouts/" + floorIDString);
        largeLayouts = Resources.LoadAll<GameObject>("Prefabs/Layouts/Large Room Layouts/" + floorIDString);
        bossLayouts = Resources.LoadAll<GameObject>("Prefabs/Layouts/Boss Layouts/" + floorIDString);
        itemLayouts = Resources.LoadAll<GameObject>("Prefabs/Layouts/Item Layouts/" + floorIDString);
        emptyLayout = Resources.Load<GameObject>("Prefabs/Layouts/Miscellaneous Layouts/Empty Layout " + floorIDString);
    }
}
