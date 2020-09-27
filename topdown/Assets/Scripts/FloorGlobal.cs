using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class FloorGlobal : MonoBehaviour
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
    public IDictionary<int, Vector3> numToMap;
    public IDictionary<int, List<GameObject>> roomDistances = new Dictionary<int, List<GameObject>>() {};
    public GameObject emptyLayout;
    public GameObject itemLayout;
    public GameObject minimapCanvas;
    public GameObject minimapRoomPrefab;
    public GameObject bossIcon;
    public GameObject itemIcon;
    public GameObject pauseCanvas;
    public bool isPaused = false;
    public List<MonoBehaviour> pausableScripts;

    private void Awake()
    {
        roomArrSize = topRooms.Length;
        normalLayouts = Resources.LoadAll("Prefabs/Layouts/Room Layouts", typeof(GameObject));
        bossLayouts = Resources.LoadAll("Prefabs/Layouts/Boss Layouts", typeof(GameObject));
        float offset = minimapRoomPrefab.GetComponent<RectTransform>().rect.width;
        offset += (offset / 5);
        numToMap = new Dictionary<int, Vector3>() { { 1, new Vector3(0, offset) }, { 2, new Vector3(0, -offset) }, { 3, new Vector3(-offset, 0) }, { 4, new Vector3(offset, 0) } };
    }
    private void Start()
    {
        numToRoom.Add(1, downRooms);
        numToRoom.Add(2, topRooms);
        numToRoom.Add(3, rightRooms);
        numToRoom.Add(4, leftRooms);

        Invoke("ChooseSpecialRooms", 0.5f);
    }

    private void CreateSpecialRooms(int dist1, int dist2, int index1, int index2)
    {
        //set boss room
        GameObject farthestRoom = roomDistances[dist1][index1];
        RoomController farthestRoomController = farthestRoom.GetComponent<RoomController>();
        while (!farthestRoomController.endRoom)
        {
            index1 += 1;
            farthestRoom = roomDistances[dist1][index1];
            farthestRoomController = farthestRoom.GetComponent<RoomController>();
        }
        int roomType = Random.Range(0, bossLayouts.Length);
        farthestRoomController.ChangeLayout(bossLayouts[roomType]);
        farthestRoomController.bossRoom = true;
        farthestRoomController.AddMapDetail(bossIcon);
        
        //set item room
        farthestRoom = roomDistances[dist2][index2];
        farthestRoomController = farthestRoom.GetComponent<RoomController>();
        while (!farthestRoomController.endRoom)
        {
            index2 += 1;
            farthestRoom = roomDistances[dist2][index2];
            farthestRoomController = farthestRoom.GetComponent<RoomController>();
        }
        farthestRoomController.ChangeLayout(itemLayout);
        farthestRoomController.roomCleared = true;
        farthestRoomController.AddMapDetail(itemIcon);
    }
    private void ChooseSpecialRooms()
    {
        //add room distances to array
        rooms.RemoveAll(item => item == null);
        foreach (GameObject room in rooms)
        {
            int roomDist = room.GetComponent<RoomController>().distance;
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
            CreateSpecialRooms(farthestDistance, farthestDistance, 0, 1);
        }
        else
        {
            CreateSpecialRooms(farthestDistance, farthestDistance - 1, 0, 0);
        }
    }
    public void OnPause()
    {
        if (!isPaused)
        {
            isPaused = true;
            Time.timeScale = 0;
            foreach (MonoBehaviour script in pausableScripts)
            {
                script.enabled = false;
            }
            pauseCanvas.SetActive(true);
        }
        else
        {
            isPaused = false;
            Time.timeScale = 1;
            foreach (MonoBehaviour script in pausableScripts)
            {
                script.enabled = true;
            }
            pauseCanvas.SetActive(false);
        }
    }
}
