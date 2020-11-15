using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Cryptography;
using UnityEngine.UI;
using UnityEngine;

public class RoomCreator : MonoBehaviour
{
    public int openingDirection;
    /*
     1 is need down opening
     2 is need top opening
     3 is need right opening
     4 is need left opening
      */
    public RoomController roomController;
    private Transform mapLocation;
    private FloorGlobal floorGlobal;
    private int randRoom;
    private bool alreadySpawned = false;
    private float roomCreateTime;
    public bool test = false;

    private void Awake()
    {
        //randomly choose whether or not there will be a room created here
        if (!roomController.startingRoom && Random.Range(0, 3) == 0)
        {
            roomController.SpawnPointDestroyed();
            Destroy(gameObject);
        }
    }
    void Start()
    {
        //get its minimaps spawn location for next room
        //Debug.Log(roomController.mapIcon.transform.parent.GetChild(openingDirection).name + roomController.mapIcon.transform.parent.GetChild(openingDirection).position);
        mapLocation = roomController.mapIcon.transform.parent.GetChild(openingDirection); 

        //offset the room creation time to prevent stacking of rooms
        roomCreateTime = Random.Range(0.02f, 0.03f);

        //get the arrays of rooms, then call the creating room method. delete spawner to clear space later
        floorGlobal = GameObject.FindGameObjectWithTag("FloorGlobalHolder").GetComponent<FloorGlobal>();
        Invoke("CreateRoom", roomCreateTime);     
        Destroy(gameObject, 1f);
    }
    void CreateMinimapIcon(GameObject room, Transform createPos, int roomShapeIndex, RoomController roomController)
    {
        GameObject minimapRoom = Instantiate(floorGlobal.minimapRoomPrefabs[roomShapeIndex], createPos.position, createPos.rotation);
        minimapRoom.transform.SetParent(floorGlobal.minimapCanvas.transform);
        minimapRoom.transform.position = createPos.position;
        roomController.mapIcon = minimapRoom.transform.GetChild(0).gameObject;
    }
    void CreateRoom()
    {
        
        if (alreadySpawned == false)
        {
            
            randRoom = Random.Range(0, floorGlobal.roomArrSize);
            int roomCount = floorGlobal.roomCount;
            int maxRoomCount = floorGlobal.maxRoomCount;
            //create rooms based on opening type 
            if(roomCount < maxRoomCount)
            {
                GameObject newRoom;
                if ((randRoom == 1 && openingDirection == 2) || (randRoom == 1 && openingDirection == 3))
                {
                    newRoom = Instantiate(floorGlobal.roomShapes[randRoom], transform.position - new Vector3(23, 15, 0), transform.rotation);
                }
                else
                {
                    newRoom = Instantiate(floorGlobal.roomShapes[randRoom], transform.position, transform.rotation);
                }
                RoomController newRoomController = newRoom.GetComponent<RoomController>();
                roomCount += newRoomController.spawnHolder.childCount; //adds the number of new rooms that will be spawned by this new room
                if(roomCount > maxRoomCount)
                {
                    //shrinks the amount of new rooms spawned by the new room
                    for (int i = 0; i < roomCount - maxRoomCount; i++)
                    {
                        Destroy(newRoomController.spawnHolder.GetChild(Random.Range(0, newRoomController.spawnHolder.childCount)).gameObject);
                    }
                    roomCount = maxRoomCount;
                }
                floorGlobal.roomCount = roomCount;
                floorGlobal.maxRoomCount = maxRoomCount;
                newRoom.name = newRoom.name.Replace("(Clone)", "");
                CreateMinimapIcon(newRoom, mapLocation, newRoom.GetComponent<RoomController>().roomShape, newRoomController);
                if (test)
                {

                    Debug.Log(newRoom.transform.position);
                }
            }

            //create only ending rooms after max cycles of room creation
            else
            {
                GameObject newRoom = Instantiate(floorGlobal.roomShapes[0], transform.position, transform.rotation);
                RoomController newRoomController = newRoom.GetComponent<RoomController>();
                Destroy(newRoomController.spawnHolder.gameObject); //stop room from spawning more
                newRoom.name = newRoom.name.Replace("(Clone)", "");
                newRoomController.endRoom = true;
                CreateMinimapIcon(newRoom, mapLocation, newRoom.GetComponent<RoomController>().roomShape, newRoomController);
            }
            //stop infinite spawning of rooms
            alreadySpawned = true;
        }    
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        //prevent rooms spawning on rooms
        if(collision.tag == "RoomSpawnPoint" || collision.tag == "RoomSpawner")
        {
            alreadySpawned = true;
            Destroy(gameObject);
        }
        
    }
}
