using UnityEngine;

public class DoorController : MonoBehaviour
{
    public GameObject closedDoor;
    public GameObject openDoor;
    public Animator openDoorAnimator;
    public Animator closedDoorAnimator;

    public void OpenDoor()
    {
        closedDoorAnimator.SetTrigger("Open Door");
    }

    public void CloseDoor()
    {
        openDoorAnimator.SetTrigger("Close Door");
    }

    public void DoorOpened()
    {
        closedDoor.SetActive(false);
        openDoor.SetActive(true);
    }
    public void DoorClosed()
    {
        closedDoor.SetActive(true);
        openDoor.SetActive(false);
    }
}
