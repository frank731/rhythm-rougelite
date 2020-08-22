using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class roomChangeController : KinematicFunctions
{
    public playerController PlayerController;
    public roomTypes RoomTypes;
    private Camera mainCamera;
    private Transform minimapCanvas;
    //speed in seconds
    private float cameraPanSpeed = 0.15f;
    void CameraMove(int px, int py, int cx, int cy, int direction)
    {
        transform.position += new Vector3(px, py, 0);
        Vector3 cameraPos = mainCamera.transform.position;
        Vector3 neededPos = cameraPos + new Vector3(cx, cy, 0);
        StartCoroutine(moveObject(mainCamera.transform, cameraPos, neededPos, cameraPanSpeed));
        Vector3 canvasPos = minimapCanvas.localPosition;
        Vector3 neededCanvasPos = canvasPos + RoomTypes.numToMap[direction];
        StartCoroutine(moveObject(minimapCanvas, canvasPos, neededCanvasPos, cameraPanSpeed));
        //disable the room that you left as a optimization
        PlayerController.currentRoom.Invoke("DisableRoom", 0.15f);
    }
    
    private void Awake()
    {
        mainCamera = Camera.main;
        minimapCanvas = RoomTypes.minimapCanvas.transform;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "DownRoomCheck":
                CameraMove(0, -2, 0, -9, 1);
                break;
            case "TopRoomCheck":
                CameraMove(0, 2, 0, 9, 2);
                break;
            case "RightRoomCheck":
                CameraMove(2, 0, 16, 0, 3);
                break;
            case "LeftRoomCheck":
                CameraMove(-2, 0, -16, 0, 4);
                break;
        }
    }
}
