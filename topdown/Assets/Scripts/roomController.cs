﻿using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UI;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomController : MonoBehaviour
{
    public List<GameObject> doors;
    public GameObject layout = null;
    public GameObject mapIcon;
    public Transform spawnHolder;
    public Tilemap walls;
    public int roomShape;
    public Color mapColor;
    public bool isPlayerIn = false;
    public bool isDestroyed;
    public bool roomCleared = false;
    public bool inPlayerRange = false;
    public bool startingRoom = false;
    public bool endRoom = false;
    public bool bossRoom = false;
    public bool itemRoom = false;
    public int distance;
    public int enemyCount = 0;
    public List<GameObject> adjacentRooms = new List<GameObject>();
    private List<int> adjacencies = new List<int>();
    private List<GameObject> enemies = new List<GameObject>();
    private FloorGlobal floorGlobal;
    private PlayerController PlayerController = null;
    
    public void RevealMap()
    {
        adjacentRooms.RemoveAll(item => item == null);
        foreach (GameObject room in adjacentRooms)
        {
            RoomController controller = room.GetComponent<RoomController>();
            if (!controller.inPlayerRange)
            {
                controller.mapIcon.SetActive(true);
                controller.ChangeMapIconTransparency(0.2f);
                controller.inPlayerRange = true;
            }
        }
    }
    public void ChangeMapIconTransparency(float t)
    {
        UnityEngine.UI.Image i = mapIcon.GetComponent<UnityEngine.UI.Image>();
        Color c = i.color;
        c.a = t;
        i.color = c;
    }

    public void AddMapDetail(GameObject detail)
    {
        GameObject mapDetail = Instantiate(detail, mapIcon.transform.position, mapIcon.transform.rotation);
        mapDetail.transform.SetParent(mapIcon.transform);
    }

    public void DisableRoom()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        //make sure unseen rooms are 0.2 transparent
        if (roomCleared)
        {
            ChangeMapIconTransparency(0.6f);
        }
        else
        {
            ChangeMapIconTransparency(0.2f);
        }
        
    }
    public void EnableRoom()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
    public void AddEnemy(GameObject enemy)
    {
        if (!enemies.Contains(enemy)){
            enemyCount += 1;
            enemies.Add(enemy);
        }
    }
    public void EnemyDestroyed()
    {
        enemyCount -= 1;
        if (enemyCount <= 0)
        {
            StartCoroutine(ChangeDoors(true));
            roomCleared = true;
            if (bossRoom)
            {
                foreach (Transform child in layout.transform)
                { 
                    if (child.tag == "Exit")
                    {
                        child.gameObject.SetActive(true);
                        break;
                    }
                        
                }
            }
        }
    }
    public void SpawnPointDestroyed()
    {
        if(spawnHolder.childCount == 0)
        {
            endRoom = true;
        }
    }

    public void ChangeLayout(Object newlayout)
    {
        if (layout != null)
        {
            Destroy(layout);
            enemyCount = 0;
        }
        layout = Instantiate(newlayout, transform.position, transform.rotation) as GameObject;
        layout.transform.SetParent(transform);
        layout.SetActive(false);
        enemies.Clear();
    }
    public void AddAdjacencies(int roomType, int newdistance)
    {
        adjacencies.Add(roomType);
        if (newdistance < distance)
        {
            distance = newdistance;
        }
    }
    public void AddDoor(int roomType, Transform doorPos)
    {
        GameObject door = Instantiate(floorGlobal.doors[roomType - 1], doorPos.position, floorGlobal.doors[roomType - 1].transform.rotation);
        door.transform.SetParent(transform);
        door.transform.position = doorPos.position;
        door.SetActive(true);
        doors.Add(door);
    }

    private void Awake()
    {
        floorGlobal = GameObject.FindGameObjectWithTag("FloorGlobalHolder").GetComponent<FloorGlobal>();
        floorGlobal.rooms.Add(gameObject);
        if (!startingRoom)
        {
            distance = floorGlobal.maxRoomCount + 1;
        }
        else
        {
            //create first map icon
            GameObject minimapRoom = Instantiate(floorGlobal.minimapRoomPrefabs[roomShape], floorGlobal.minimapCanvas.transform.position, transform.rotation);
            minimapRoom.transform.SetParent(floorGlobal.minimapCanvas.transform);
            mapIcon = minimapRoom.transform.GetChild(0).gameObject;
            mapIcon.SetActive(true);
        }
    }

    void Start()
    {
        //sets room layout
        if (!startingRoom)
        {
            int layoutType = Random.Range(0, floorGlobal.normalLayouts.Length);
            ChangeLayout(floorGlobal.normalLayouts[layoutType]);
            //disable room as optimization
            Invoke("DisableRoom", 0.2f);
            StartCoroutine(ChangeDoors(false));
        }
        else
        {
            //other
            ChangeLayout(floorGlobal.emptyLayout);
            layout.SetActive(true);
            inPlayerRange = true;
            Invoke("RevealMap", 0.2f);
            StartCoroutine(ChangeDoors(true));
        }
    }

    public void OnDestroy()
    {
        //destroy its corresponding map icon on destroy
        Destroy(mapIcon.transform.parent.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //turns on gate and enemes when player enters room and room hasn't been cleared already
        if(collision.gameObject.tag == "Player")
        { 
            if (!PlayerController)
            {
                PlayerController = collision.gameObject.GetComponent<PlayerController>();
            }
            PlayerController.currentRoom = this;
            //re enable room on enter
            isPlayerIn = true;
            inPlayerRange = true;
            EnableRoom();
            ChangeMapIconTransparency(1f);
            RevealMap();
            if (!roomCleared)
            {
                StartCoroutine(ChangeDoors(false));
                //Invoke("ActivateEnemies", 0.5f);
            }
        }
    }

    public IEnumerator ChangeDoors(bool open)
    {
        yield return new WaitForSeconds(0.1f); //waits until all the doors have been created
        foreach (GameObject door in doors)
        {
            door.GetComponent<DoorController>().ChangeDoorStatus(open);
        }
    }
}
