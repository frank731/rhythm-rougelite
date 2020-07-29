using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class removeOverlapRooms : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "RoomSpawnPoint" && transform.tag == "RoomSpawnPoint")
        {
            if (!collision.transform.parent.gameObject.GetComponent<roomController>().isDestroyed)
            {
                transform.parent.gameObject.GetComponent<roomController>().isDestroyed = true;
                Destroy(transform.parent.gameObject);
                transform.parent.gameObject.GetComponent<roomController>().correctRooms();
            }

        }
    }
}
