using UnityEngine;

public class RoomCreator : MonoBehaviour
{
    public int openingDirection;
    /*
     1 is need down opening
     2 is need top opening
     3 is need right opening
     4 is need left opening
      */
    public RoomController roomController;
    private Vector3 mapLocation;
    private int randRoom;
    private bool alreadySpawned = false;
    private float roomCreateTime;

    private void Awake()
    {
        //randomly choose whether or not there will be a room created here
        if (!roomController.startingRoom && Random.Range(0, 3) == 0)
        {
            roomController.SpawnPointDestroyed();
            Destroy(gameObject);
        }
    }
    void Start()
    {
        //get its minimaps spawn location for next room
        //Debug.Log(roomController.mapIcon.transform.parent.GetChild(openingDirection).name + roomController.mapIcon.transform.parent.GetChild(openingDirection).position);
        mapLocation = roomController.transform.position - transform.position;

        //offset the room creation time to prevent stacking of rooms
        roomCreateTime = Random.Range(0.02f, 0.03f);

        //get the arrays of rooms, then call the creating room method. delete spawner to clear space later
        Invoke("CreateRoom", roomCreateTime);
        Destroy(gameObject, 1f);
    }
    void CreateMinimapIcon(GameObject room, Vector3 createPos, int roomShapeIndex, RoomController newRoomController)
    {
        createPos.x *= 1.3f;
        createPos.y *= 2f;
        GameObject minimapRoom = Instantiate(FloorGlobal.Instance.minimapRoomPrefabs[roomShapeIndex], roomController.mapIcon.transform.position - createPos, room.transform.rotation);
        minimapRoom.transform.SetParent(FloorGlobal.Instance.minimapCanvas.transform);
        //minimapRoom.transform.position = createPos;
        newRoomController.mapIcon = minimapRoom.transform.GetChild(0).gameObject;
    }
    void CreateRoom()
    {

        if (alreadySpawned == false)
        {

            randRoom = Random.Range(0, FloorGlobal.Instance.roomArrSize);
            int roomCount = FloorGlobal.Instance.roomCount;
            int maxRoomCount = FloorGlobal.Instance.maxRoomCount;
            //create rooms based on opening type 
            if (roomCount < maxRoomCount)
            {
                GameObject newRoom;
                if (randRoom > 0) //check if the room shape is a larger room
                {
                    if (roomController.createdLargeRoom)
                    {
                        randRoom = 0;
                    }
                    else
                    {
                        roomController.createdLargeRoom = true;
                    }
                }

                if ((randRoom == 1 && openingDirection == 2) || (randRoom == 1 && openingDirection == 3))
                {
                    newRoom = Instantiate(FloorGlobal.Instance.roomShapes[randRoom], transform.position - new Vector3(23, 15, 0), transform.rotation);
                }
                else
                {
                    newRoom = Instantiate(FloorGlobal.Instance.roomShapes[randRoom], transform.position, transform.rotation);
                }

                RoomController newRoomController = newRoom.GetComponent<RoomController>();
                roomCount += newRoomController.spawnHolder.childCount; //adds the number of new rooms that will be spawned by this new room

                if (roomCount > maxRoomCount)
                {
                    //shrinks the amount of new rooms spawned by the new room
                    for (int i = 0; i < roomCount - maxRoomCount; i++)
                    {
                        Destroy(newRoomController.spawnHolder.GetChild(Random.Range(0, newRoomController.spawnHolder.childCount)).gameObject);
                    }
                    roomCount = maxRoomCount;
                }
                FloorGlobal.Instance.roomCount = roomCount;
                FloorGlobal.Instance.maxRoomCount = maxRoomCount;
                newRoom.name = newRoom.name.Replace("(Clone)", "");
                mapLocation = roomController.transform.position - newRoom.transform.position;
                CreateMinimapIcon(newRoom, mapLocation, newRoom.GetComponent<RoomController>().roomShape, newRoomController);
            }

            //create only ending rooms after max cycles of room creation
            else
            {
                GameObject newRoom = Instantiate(FloorGlobal.Instance.roomShapes[0], transform.position, transform.rotation);
                RoomController newRoomController = newRoom.GetComponent<RoomController>();
                Destroy(newRoomController.spawnHolder.gameObject); //stop room from spawning more
                newRoom.name = newRoom.name.Replace("(Clone)", "");
                newRoomController.endRoom = true;
                mapLocation = roomController.transform.position - newRoom.transform.position;
                CreateMinimapIcon(newRoom, mapLocation, newRoom.GetComponent<RoomController>().roomShape, newRoomController);
            }
            //stop infinite spawning of rooms
            alreadySpawned = true;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        //prevent rooms spawning on rooms
        if (collision.CompareTag("RoomSpawnPoint") || collision.CompareTag("RoomSpawner"))
        {
            alreadySpawned = true;
            Destroy(gameObject);
        }
    }
}
