using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomChangeController : MonoBehaviour
{
    public PlayerController playerController;
    private Camera mainCamera;
    private Transform minimapCanvas;
    //speed in seconds
    private float cameraPanSpeed = 0.15f;
    void MoveRoom(int px, int py)
    {
        Debug.Log("move");
        //disable camera follow player script just in case
        FloorGlobal.Instance.cameraFollowPlayer.enabled = false;

        //disable the room that you left as a optimization
        playerController.currentRoom.Invoke("DisableRoom", 0.15f);

        transform.position += new Vector3(px, py, 0);

        StartCoroutine(MoveMinimapCamera()); //put on delay so player current room can update
    }

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        mainCamera = Camera.main;
        minimapCanvas = FloorGlobal.Instance.minimapCanvas.transform;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "DownRoomCheck":
                MoveRoom(0, -7);
                break;
            case "TopRoomCheck":
                MoveRoom(0, 7);
                break;
            case "RightRoomCheck":
                MoveRoom(7, 0);
                break;
            case "LeftRoomCheck":
                MoveRoom(-7, 0);
                break;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        mainCamera = Camera.main;
    }

    private IEnumerator MoveMinimapCamera()
    {
        yield return new WaitForSeconds(0.02f);
        if (!playerController.viewingMap)
        {
            Vector3 canvasPos = minimapCanvas.localPosition;
            Vector3 neededCanvasPos = playerController.currentRoom.mapIcon.transform.parent.localPosition * -1;
            StartCoroutine(KinematicFunctions.MoveObject(minimapCanvas, canvasPos, neededCanvasPos, cameraPanSpeed));
        }
        Vector3 cameraPos = mainCamera.transform.position;
        Vector3 neededPos = playerController.currentRoom.transform.position;
        neededPos.z = -100; //make sure camera isnt on the scene 
        IEnumerator cameraMove = KinematicFunctions.MoveObject(mainCamera.transform, cameraPos, neededPos, cameraPanSpeed);
        StartCoroutine(cameraMove);
        if (playerController.currentRoom.roomShape == 1)
        {
            //enable camera following
            FloorGlobal.Instance.cameraFollowPlayer.enabled = true;
            StopCoroutine(cameraMove); //stops camera from trying to move back to room spawnpoint instead of player
        }

    }
}
