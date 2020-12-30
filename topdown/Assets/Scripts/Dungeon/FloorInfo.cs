using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorInfo : Singleton<FloorInfo> {
    [SerializeField]
    private int floorID;
    [SerializeField]
    private Layouts layouts;
    private GameObject[] rooms;
    [SerializeField]
    private GameObject[] doors;

    protected override void Awake()
    {
        base.Awake();
        rooms = Resources.LoadAll<GameObject>("Prefabs/Basic Rooms/" + floorID.ToString());
    }

    public int GetFloorID()
    {
        return floorID;
    }

    public Layouts GetLayouts()
    {
        return layouts;
    }

    public GameObject[] GetRooms()
    {
        return rooms;
    }

    public GameObject[] GetDoors()
    {
        return doors;
    }
}
