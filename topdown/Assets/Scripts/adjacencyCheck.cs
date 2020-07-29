﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class adjacencyCheck : MonoBehaviour
{
    public int needOpening;
    /*
     1 is need down opening
     2 is need top opening
     3 is need right opening
     4 is need left opening
      */
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "RoomSpawnPoint" && collision.name != "replaced")
        {
            roomController room = collision.transform.parent.gameObject.GetComponent<roomController>();
            room.AddAdjacencies(needOpening, transform.parent.GetComponent<roomController>().distance + 1);
            Destroy(gameObject);
        }

    }
}