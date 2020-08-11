using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class roomTypes : MonoBehaviour
{
    public GameObject[] topRooms;
    public GameObject[] downRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;
    public GameObject[] endRooms;
    public Object[] normalLayouts;
    public Object[] bossLayouts;
    public int maxRoomCount = 10;
    public int roomCount = 0;
    public int roomArrSize;
    public List<GameObject> rooms = new List<GameObject>();
    public IDictionary<int, GameObject[]> numToRoom = new Dictionary<int, GameObject[]>() {};
    public IDictionary<int, Vector2> numToMap = new Dictionary<int, Vector2>() { {1, new Vector2(0, 30)}, {2, new Vector2(0, -30)}, {3, new Vector2(-30, 0)}, {4, new Vector2(30, 0)} };
    public IDictionary<int, List<GameObject>> roomDistances = new Dictionary<int, List<GameObject>>() {};
    public GameObject emptyLayout;
    public GameObject itemLayout;
    public GameObject minimapCanvas;

    private void Awake()
    {
        roomArrSize = topRooms.Length;
        normalLayouts = Resources.LoadAll("Prefabs/Layouts/Room Layouts", typeof(GameObject));
        bossLayouts = Resources.LoadAll("Prefabs/Layouts/Boss Layouts", typeof(GameObject));
    }
    private void Start()
    {
        numToRoom.Add(1, downRooms);
        numToRoom.Add(2, topRooms);
        numToRoom.Add(3, rightRooms);
        numToRoom.Add(4, leftRooms);

        Invoke("ChooseSpecialRooms", 0.5f);
    }

    private void ChooseSpecialRooms()
    {
        //add room distances to array
        rooms.RemoveAll(item => item == null);
        foreach (GameObject room in rooms)
        {
            int roomDist = room.GetComponent<roomController>().distance;
            if (roomDistances.ContainsKey(roomDist))
            {
                roomDistances[roomDist].Add(room);
            }
            else
            {
                roomDistances[roomDist] = new List<GameObject> { room };
            }
        }
        int farthestDistance = roomDistances.Keys.Max();
        if (roomDistances[farthestDistance].Count >= 2)
        {
            //set boss room
            GameObject farthestRoom = roomDistances[farthestDistance][0];
            roomController farthestRoomController = farthestRoom.GetComponent<roomController>();
            int roomType = Random.Range(0, bossLayouts.Length);
            farthestRoomController.changeLayout(bossLayouts[roomType]);
            farthestRoomController.bossRoom = true;
            //set item room
            farthestRoom = roomDistances[farthestDistance][1];
            farthestRoomController = farthestRoom.GetComponent<roomController>();
            farthestRoomController.changeLayout(itemLayout);
            farthestRoomController.roomCleared = true;
        }
        else
        {
            GameObject farthestRoom = roomDistances[farthestDistance][0];
            roomController farthestRoomController = farthestRoom.GetComponent<roomController>();
            int roomType = Random.Range(0, bossLayouts.Length);
            farthestRoomController.changeLayout(bossLayouts[roomType]);
            farthestRoomController.bossRoom = true;
            //set item room
            farthestRoom = roomDistances[farthestDistance - 1][0];
            farthestRoomController = farthestRoom.GetComponent<roomController>();
            farthestRoomController.changeLayout(itemLayout);
            farthestRoomController.roomCleared = true;
        }
    }
}
