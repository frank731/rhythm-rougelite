using UnityEngine;

public class DoorController : MonoBehaviour
{
    public GameObject closedDoor;
    public GameObject openDoor;

    public void ChangeDoorStatus(bool open)
    {
        if (open)
        {
            openDoor.SetActive(true);
            closedDoor.SetActive(false);
        }
        else
        {
            openDoor.SetActive(false);
            closedDoor.SetActive(true);
        }
    }
}
