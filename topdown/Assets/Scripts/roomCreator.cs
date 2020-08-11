using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Cryptography;
using UnityEngine;

public class roomCreator : MonoBehaviour
{
    public int openingDirection;
    /*
     1 is need down opening
     2 is need top opening
     3 is need right opening
     4 is need left opening
      */
    private roomTypes roomTypesHolder;
    private int randRoom;
    private bool alreadySpawned = false;
    private float roomCreateTime;
    void Start()
    {
        //offset the room creation time to prevent stacking of rooms
        roomCreateTime = Random.Range(0.01f, 0.02f);
        //get the arrays of rooms, then call the creating room method. delete spawner to clear space later
        roomTypesHolder = GameObject.FindGameObjectWithTag("RoomTypeHolder").GetComponent<roomTypes>();
        Invoke("CreateRoom", roomCreateTime);     
        Destroy(gameObject, 1f);
    }
    void CreateRoom()
    {
        if(alreadySpawned == false)
        {
            randRoom = Random.Range(0, roomTypesHolder.roomArrSize);
            int roomCount = roomTypesHolder.roomCount;
            int maxRoomCount = roomTypesHolder.maxRoomCount;
            //create rooms based on opening type 
            if(roomCount < maxRoomCount)
            {
                roomCount += 1;
                GameObject[] roomType = roomTypesHolder.numToRoom[openingDirection];
                GameObject newroom = roomType[randRoom];
                roomCount += newroom.name.Length - 1;
                if(roomCount > maxRoomCount)
                {
                    while (roomCount != maxRoomCount)
                    {
                        roomCount -= newroom.name.Length - 1;
                        randRoom = Random.Range(0, roomTypesHolder.roomArrSize);
                        newroom = roomType[randRoom];
                        roomCount += newroom.name.Length - 1;
                    }
                    
                }
                roomTypesHolder.roomCount = roomCount;
                roomTypesHolder.maxRoomCount = maxRoomCount;
                GameObject room = Instantiate(newroom, transform.position, newroom.transform.rotation);
                //room.GetComponent<roomController>().distance = transform.parent.parent.GetComponent<roomController>().distance + 1;
                room.name = room.name.Replace("(Clone)", "");
            }

            //create only ending rooms after max cycles of room creation
            else
            {
                GameObject room = Instantiate(roomTypesHolder.endRooms[openingDirection - 1], transform.position, roomTypesHolder.endRooms[openingDirection - 1].transform.rotation);
                room.name = room.name.Replace("(Clone)", "");
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
