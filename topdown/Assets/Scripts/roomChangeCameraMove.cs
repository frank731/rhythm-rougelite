using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class roomChangeCameraMove : KinematicFunctions
{
    private Camera mainCamera;
    //speed in seconds
    private float cameraPanSpeed = 0.15f;
    void CameraMove(int px, int py, int cx, int cy)
    {
        transform.position += new Vector3(px, py, 0);
        Vector3 cameraPos = mainCamera.transform.position;
        Vector3 neededPos = cameraPos + new Vector3(cx, cy, 0);
        StartCoroutine(moveObject(mainCamera.transform, cameraPos, neededPos, cameraPanSpeed));
    }
    
    private void Awake()
    {
        mainCamera = Camera.main;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "DownRoomCheck":
                CameraMove(0, -2, 0, -9);
                break;
            case "TopRoomCheck":
                CameraMove(0, 2, 0, 9);
                break;
            case "LeftRoomCheck":
                CameraMove(-2, 0, -16, 0);
                break;
            case "RightRoomCheck":
                CameraMove(2, 0, 16, 0);
                break;
            
        }
    }
}
