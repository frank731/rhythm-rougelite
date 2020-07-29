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
    public Object[] layouts;
    public int maxRoomCount = 10;
    public int roomCount = 0;
    public int roomArrSize;
    public IDictionary<int, GameObject[]> numToRoom = new Dictionary<int, GameObject[]>() {};
    public IDictionary<GameObject, int> roomDistances = new Dictionary<GameObject, int>() {};

    private void Awake()
    {
        roomArrSize = topRooms.Length;
        layouts = Resources.LoadAll("Prefabs/Layouts", typeof(GameObject));
    }
    private void Start()
    {
        numToRoom.Add(1, downRooms);
        numToRoom.Add(2, topRooms);
        numToRoom.Add(3, rightRooms);
        numToRoom.Add(4, leftRooms);

        Invoke("print", 3f);
    }

    private void print()
    {
        foreach (KeyValuePair<GameObject, int> distance in roomDistances)
        {
            Debug.Log(distance.Key + " " + distance.Value);
        }
    }
}
