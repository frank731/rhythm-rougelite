using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class RoomChangeController : KinematicFunctions
{
    public PlayerController playerController;
    public FloorGlobal floorGlobal;
    private Camera mainCamera;
    private Transform minimapCanvas;
    //speed in seconds
    private float cameraPanSpeed = 0.15f;
    void MoveRoom(int px, int py, int cx, int cy)
    {
        //disable the room that you left as a optimization
        playerController.currentRoom.Invoke("DisableRoom", 0.15f);

        transform.position += new Vector3(px, py, 0);
        Vector3 cameraPos = mainCamera.transform.position;
        Vector3 neededPos = cameraPos + new Vector3(cx, cy, 0);
        StartCoroutine(MoveObject(mainCamera.transform, cameraPos, neededPos, cameraPanSpeed));
        StartCoroutine(MoveMinimap()); //put on delay so player current room can update
    }
    
    private void Awake()
    {
        mainCamera = Camera.main;
        minimapCanvas = floorGlobal.minimapCanvas.transform;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "DownRoomCheck":
                MoveRoom(0, -7, 0, -15);
                break;
            case "TopRoomCheck":
                MoveRoom(0, 7, 0, 15);
                break;
            case "RightRoomCheck":
                MoveRoom(7, 0, 23, 0);
                break;
            case "LeftRoomCheck":
                MoveRoom(-7, 0, -23, 0);
                break;
        }
    }

    private IEnumerator MoveMinimap()
    {
        yield return new WaitForSeconds(0.02f);
        Vector3 canvasPos = minimapCanvas.localPosition;
        Vector3 neededCanvasPos = playerController.currentRoom.mapIcon.transform.parent.localPosition * -1;
        StartCoroutine(MoveObject(minimapCanvas, canvasPos, neededCanvasPos, cameraPanSpeed));
    }
}
