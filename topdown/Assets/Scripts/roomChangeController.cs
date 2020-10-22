using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class RoomChangeController : KinematicFunctions
{
    public PlayerController PlayerController;
    public FloorGlobal floorGlobal;
    private Camera mainCamera;
    private Transform minimapCanvas;
    //speed in seconds
    private float cameraPanSpeed = 0.15f;
    void MoveRoom(int px, int py, int cx, int cy, int direction)
    {
        transform.position += new Vector3(px, py, 0);
        Vector3 cameraPos = mainCamera.transform.position;
        Vector3 neededPos = cameraPos + new Vector3(cx, cy, 0);
        StartCoroutine(MoveObject(mainCamera.transform, cameraPos, neededPos, cameraPanSpeed));
        Vector3 canvasPos = minimapCanvas.localPosition;
        Vector3 neededCanvasPos = canvasPos + floorGlobal.numToMap[direction];
        StartCoroutine(MoveObject(minimapCanvas, canvasPos, neededCanvasPos, cameraPanSpeed));
        //disable the room that you left as a optimization
        PlayerController.currentRoom.Invoke("DisableRoom", 0.15f);
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
                MoveRoom(0, -2, 0, -9, 1);
                break;
            case "TopRoomCheck":
                MoveRoom(0, 2, 0, 9, 2);
                break;
            case "RightRoomCheck":
                MoveRoom(2, 0, 16, 0, 3);
                break;
            case "LeftRoomCheck":
                MoveRoom(-2, 0, -16, 0, 4);
                break;
        }
    }
}
