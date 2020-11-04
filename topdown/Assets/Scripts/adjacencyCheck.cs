using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjacencyCheck : MonoBehaviour
{
    public int needOpening;
    public Transform doorSpawn;
    /*
     1 is need down opening
     2 is need top opening
     3 is need right opening
     4 is need left opening
      */
    void Start()
    {
        Destroy(gameObject, 1f);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "RoomSpawnPoint")
        {
            RoomController adjRoom = collision.transform.parent.gameObject.GetComponent<RoomController>();
            adjRoom.AddAdjacencies(needOpening, transform.parent.GetComponent<RoomController>().distance + 1);
            RoomController room = transform.parent.gameObject.GetComponent<RoomController>();
            room.adjacentRooms.Add(collision.transform.parent.gameObject);
            room.AddDoor(needOpening, doorSpawn);
            Destroy(gameObject);
        }

    }
}
