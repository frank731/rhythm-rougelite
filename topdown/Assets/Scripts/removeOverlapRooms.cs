using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveOverlapRooms : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "RoomSpawnPoint" && transform.tag == "RoomSpawnPoint")
        {
            if (!collision.transform.parent.gameObject.GetComponent<RoomController>().isDestroyed)
            {
                transform.parent.gameObject.GetComponent<RoomController>().isDestroyed = true;
                Destroy(transform.parent.gameObject);
                //on a delay to give room time to find adjencencies
                transform.parent.gameObject.GetComponent<RoomController>().Invoke("CorrectRooms", 0.1f);
            }
        }
    }
    void Start()
    {
        Destroy(gameObject, 1f);
    }
}
