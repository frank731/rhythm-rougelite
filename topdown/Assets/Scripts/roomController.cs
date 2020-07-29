using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Tilemaps;

public class roomController : MonoBehaviour
{
    public GameObject gate;   
    public bool isPlayerIn = false;
    public bool isDestroyed;
    public bool roomCleared = false;
    public bool startingRoom = false;
    public int distance;
    private bool needFix = false;
    public int enemyCount = 0;
    private List<int> adjacencies = new List<int>();
    private List<GameObject> enemies = new List<GameObject>();
    private roomTypes roomTypesHolder;

    void ActivateEnemies()
    {
        //activate enemies when player enters room
        foreach(GameObject enemy in enemies)
        {
            enemy.SetActive(true);
        }
    }
    public void AddEnemy(GameObject enemy)
    {
        if (!enemies.Contains(enemy)){
            enemyCount += 1;
            enemies.Add(enemy);
        }
    }
    public void EnemyDestroyed()
    {
        enemyCount -= 1;
        if (enemyCount <= 0)
        {
            gate.SetActive(false);
            roomCleared = true;
        }
    }
    private void setLayout()
    {
        int layoutType = Random.Range(0, roomTypesHolder.layouts.Length);
        GameObject layout = Instantiate(roomTypesHolder.layouts[layoutType], transform.position, transform.rotation) as GameObject;
        layout.transform.SetParent(transform);
    }
    public void AddAdjacencies(int roomType, int newdistance)
    {
        adjacencies.Add(roomType);
        if (newdistance < distance)
        {
            distance = newdistance;
            roomTypesHolder.roomDistances[gameObject] = distance;
        }
        
    }

    public void correctRooms()
    {
        //check if room needs to be have more doors
        foreach (int roomTypeNeeded in adjacencies)
        {
            if (!roomTypesHolder.numToRoom[roomTypeNeeded].Any((MyObject) => MyObject.name == gameObject.name))
            {
                needFix = true;
                break;
            }
        }

        if(needFix == true)
        {
            //get lists of rooms that have the needed doors
            List<GameObject[]> needed = new List<GameObject[]>();           
            foreach (int roomTypeNeeded in adjacencies)
            {
                needed.Add(roomTypesHolder.numToRoom[roomTypeNeeded]);
            }

            //check which rooms fufill the req of having all needed doors
            List<GameObject> compatible = needed[0].ToList();            
            for (int i = 1; i < needed.Count; i++)
            {                
                IEnumerable<GameObject> works = compatible.Intersect(needed[i].ToList());
                compatible = new List<GameObject>();
                foreach (GameObject g in works)
                {
                    compatible.Add(g);
                }
            }

            //get room that makes no new rooms by taking shortest name
            int index = 0;
            int shortestLen = 5;
            for(int i = 0; i < compatible.Count; i++)
            {
                if (compatible[i].name.Length < shortestLen)
                {
                    shortestLen = compatible[i].name.Length;
                    index = i;
                }
            }
            GameObject replace = Instantiate(compatible[index], transform.position, compatible[index].transform.rotation);
            //mark room as replaced to prevent other rooms from interacting with it and creating more rooms
            roomController replaceController = replace.GetComponent<roomController>();
            replaceController.needFix = true;
            Destroy(gameObject);
        }
            
    }
    private void Awake()
    {
        roomTypesHolder = GameObject.FindGameObjectWithTag("RoomTypeHolder").GetComponent<roomTypes>();
        if (!startingRoom)
        {
            distance = roomTypesHolder.maxRoomCount + 1;
        }
    }
    void Start()
    {
        gate.SetActive(false);
        if (!needFix){
            Invoke("correctRooms", 0.1f);
        }        
        if (!roomCleared)
        {
            setLayout();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //turns on gate and enemes when player enters room and room hasn't been cleared already
        if(collision.gameObject.tag == "Player" && !roomCleared)
        {
            isPlayerIn = true;
            gate.SetActive(true);
            Invoke("ActivateEnemies", 1f);
        }
    }
}
