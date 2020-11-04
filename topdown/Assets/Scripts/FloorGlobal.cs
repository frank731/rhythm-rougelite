﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FloorGlobal : MonoBehaviour
{
    public GameObject[] topRooms;
    public GameObject[] downRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;
    public GameObject[] endRooms;
    public GameObject[] roomShapes;
    public GameObject[] normalLayouts;
    public GameObject[] bossLayouts;
    public GameObject[] itemLayouts;
    public GameObject[] doors;
    public int maxRoomCount = 10;
    public int roomCount = 0;
    public int roomArrSize;
    public List<GameObject> rooms = new List<GameObject>();
    public IDictionary<int, GameObject[]> numToRoom = new Dictionary<int, GameObject[]>() {};
    public IDictionary<int, Vector3> numToMap;
    public IDictionary<int, List<GameObject>> roomDistances = new Dictionary<int, List<GameObject>>() {};
    public GameObject emptyLayout;
    public GameObject minimapCanvas;
    public GameObject[] minimapRoomPrefabs;
    public GameObject bossIcon;
    public GameObject itemIcon;
    public GameObject pauseCanvas;
    public Sprite[] heartSprites;
    public bool isPaused = false;
    public bool isOnBeat = false;
    public UnityEvent onBeat = new UnityEvent();
    public List<MonoBehaviour> pausableScripts;
    public RectMask2D minimapMask;

    private void Awake()
    {
        roomArrSize = roomShapes.Length;
        normalLayouts = Resources.LoadAll<GameObject>("Prefabs/Layouts/Room Layouts");
        bossLayouts = Resources.LoadAll<GameObject>("Prefabs/Layouts/Boss Layouts");
        itemLayouts = Resources.LoadAll<GameObject>("Prefabs/Layouts/Item Layouts");
        heartSprites = Resources.LoadAll<Sprite>("Sprites/hearts");
    }
    private void Start()
    {
        numToRoom.Add(1, downRooms);
        numToRoom.Add(2, topRooms);
        numToRoom.Add(3, rightRooms);
        numToRoom.Add(4, leftRooms);

        Invoke("GetMaxDistance", 0.5f);
    }

    void CreateSpecialRoom(GameObject room, GameObject[] layouts, ref bool roomType, GameObject icon)
    {
        RoomController farthestRoomController = room.GetComponent<RoomController>();
        int roomLayout = Random.Range(0, layouts.Length);
        farthestRoomController.ChangeLayout(layouts[roomLayout]);
        roomType = true;
        farthestRoomController.AddMapDetail(icon);
        farthestRoomController.roomCleared = true;
        StartCoroutine(farthestRoomController.ChangeDoors(true));
    }
    private void CreateSpecialRooms(int maxDist)
    {
        int index = 0;

        GameObject bossRoom = roomDistances[maxDist][index];
        RoomController bossRoomController = bossRoom.GetComponent<RoomController>();
        //ensures room is an ending room
        while (!bossRoomController.endRoom)
        {
            if (index < roomDistances[maxDist].Count)
            {
                index++;
            }
            else
            {
                maxDist -= 1;
            }
            bossRoom = roomDistances[maxDist][index];
            bossRoomController = bossRoom.GetComponent<RoomController>();
        }
        //set boss room
        CreateSpecialRoom(bossRoom, bossLayouts, ref bossRoom.GetComponent<RoomController>().bossRoom, bossIcon);

        index++;
        if(index == roomDistances[maxDist].Count)
        {
            index = 0;
            maxDist -= 1;
        }

        GameObject itemRoom = roomDistances[maxDist][index];
        RoomController itemRoomController = bossRoom.GetComponent<RoomController>();
        //ensures room is an ending room
        while (!itemRoomController.endRoom)
        {
            if (index < roomDistances[maxDist].Count)
            {
                index++;
            }
            else
            {
                maxDist -= 1;
            }
            itemRoom = roomDistances[maxDist][index];
            itemRoomController = bossRoom.GetComponent<RoomController>();
        }
        //set item room
        CreateSpecialRoom(itemRoom, itemLayouts, ref itemRoom.GetComponent<RoomController>().itemRoom, itemIcon);

    }
    private void GetMaxDistance()
    {
        //add room distances to array
        rooms.RemoveAll(item => item == null);
        foreach (GameObject room in rooms)
        {
            int roomDist = room.GetComponent<RoomController>().distance;
            if (roomDistances.ContainsKey(roomDist))
            {
                roomDistances[roomDist].Add(room);
            }
            else
            {
                roomDistances[roomDist] = new List<GameObject> { room };
            }
        }
        int farthestDistance = roomDistances.Keys.Max();
        CreateSpecialRooms(farthestDistance);
        
    }
    public void OnPause()
    {
        //removes any scripts that have been deleted
        pausableScripts.RemoveAll(script => script == null);
        if (!isPaused)
        {
            isPaused = true;
            Time.timeScale = 0;
            foreach (MonoBehaviour script in pausableScripts)
            {
                script.enabled = false;
            }
            pauseCanvas.SetActive(true);
        }
        else
        {
            isPaused = false;
            Time.timeScale = 1;
            foreach (MonoBehaviour script in pausableScripts)
            {
                script.enabled = true;
            }
            pauseCanvas.SetActive(false);
        }
    }

    public void OnViewMap(bool viewingMap)
    {
        minimapMask.enabled = !minimapMask.enabled;
        if (viewingMap)
        {
            minimapCanvas.transform.localScale = new Vector3(1.5f, 1.5f, 1);
            minimapCanvas.transform.position -= new Vector3(100f, 100f, 1);
        }
        else
        {
            minimapCanvas.transform.localScale = new Vector3(1, 1, 1);
            minimapCanvas.transform.position += new Vector3(100f, 100f, 1);
        }
       
        Debug.Log("br");
    }
}
