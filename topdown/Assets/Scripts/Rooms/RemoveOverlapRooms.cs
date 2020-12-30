using UnityEngine;

public class RemoveOverlapRooms : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("RoomSpawnPoint"))
        {
            if (!collision.transform.parent.gameObject.GetComponent<RoomController>().isDestroyed)
            {
                Debug.Log("destroyed " + transform.parent.gameObject);
                transform.parent.gameObject.GetComponent<RoomController>().isDestroyed = true;
                Destroy(transform.parent.gameObject);
            }
        }
    }
    void Start()
    {
        Destroy(gameObject, 1f);
    }
}
