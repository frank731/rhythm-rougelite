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
    private FloorGlobal floorGlobal;
    private int randRoom;
    private bool alreadySpawned = false;
    private float roomCreateTime;
    void Start()
    {
        //offset the room creation time to prevent stacking of rooms
        roomCreateTime = Random.Range(0.01f, 0.02f);
        //get the arrays of rooms, then call the creating room method. delete spawner to clear space later
        floorGlobal = GameObject.FindGameObjectWithTag("FloorGlobalHolder").GetComponent<FloorGlobal>();
        Invoke("CreateRoom", roomCreateTime);     
        Destroy(gameObject, 1f);
    }
    void CreateMinimapIcon(GameObject room)
    {
        GameObject minimapRoom = Instantiate(floorGlobal.minimapRoomPrefab, transform.parent.parent.GetComponent<RoomController>().mapIcon.transform.position + floorGlobal.numToMap[openingDirection], room.transform.rotation);
        minimapRoom.transform.SetParent(floorGlobal.minimapCanvas.transform);
        RoomController newroomController = room.GetComponent<RoomController>();
        newroomController.mapIcon = minimapRoom;
    }
    void CreateRoom()
    {
        if(alreadySpawned == false)
        {
            randRoom = Random.Range(0, floorGlobal.roomArrSize);
            int roomCount = floorGlobal.roomCount;
            int maxRoomCount = floorGlobal.maxRoomCount;
            //create rooms based on opening type 
            if(roomCount < maxRoomCount)
            {
                roomCount += 1;
                GameObject[] roomType = floorGlobal.numToRoom[openingDirection];
                GameObject newroom = roomType[randRoom];
                roomCount += newroom.name.Length - 1;
                if(roomCount > maxRoomCount)
                {
                    while (roomCount != maxRoomCount)
                    {
                        roomCount -= newroom.name.Length - 1;
                        randRoom = Random.Range(0, floorGlobal.roomArrSize);
                        newroom = roomType[randRoom];
                        roomCount += newroom.name.Length - 1;
                    }
                    
                }
                floorGlobal.roomCount = roomCount;
                floorGlobal.maxRoomCount = maxRoomCount;
                GameObject room = Instantiate(newroom, transform.position, newroom.transform.rotation);
                room.name = room.name.Replace("(Clone)", "");
                CreateMinimapIcon(room);
            }

            //create only ending rooms after max cycles of room creation
            else
            {
                GameObject room = Instantiate(floorGlobal.endRooms[openingDirection - 1], transform.position, floorGlobal.endRooms[openingDirection - 1].transform.rotation);
                room.name = room.name.Replace("(Clone)", "");
                room.GetComponent<RoomController>().endRoom = true;
                CreateMinimapIcon(room);
            }
            //stop infinite spawning of rooms
            alreadySpawned = true;
        }    
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        //prevent rooms spawning on rooms
        if(collision.tag == "RoomSpawnPoint" && collision.name != "replaced")
        {
            alreadySpawned = true;
            Destroy(gameObject);
        }
        
    }
}
