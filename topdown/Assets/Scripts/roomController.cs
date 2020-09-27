using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UI;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomController : MonoBehaviour
{
    public GameObject gate;
    public GameObject layout = null;
    public GameObject mapIcon;
    public Color mapColor;
    public bool isPlayerIn = false;
    public bool isDestroyed;
    public bool roomCleared = false;
    public bool inPlayerRange = false;
    public bool startingRoom = false;
    public bool endRoom = false;
    public bool bossRoom = false;
    public int distance;
    private bool needFix = false;
    public int enemyCount = 0;
    public List<GameObject> adjacentRooms = new List<GameObject>();
    private List<int> adjacencies = new List<int>();
    private List<GameObject> enemies = new List<GameObject>();
    private FloorGlobal floorGlobal;
    private PlayerController PlayerController = null;
    
    /*
    void ActivateEnemies()
    {
        //activate enemies when player enters room
        foreach(GameObject enemy in enemies)
        {
            enemy.SetActive(true);
        }
    }
    */
    public void RevealMap()
    {
        adjacentRooms.RemoveAll(item => item == null);
        foreach (GameObject room in adjacentRooms)
        {
            RoomController controller = room.GetComponent<RoomController>();
            if (!controller.inPlayerRange)
            {
                controller.mapIcon.SetActive(true);
                controller.ChangeMapIconTransparency(0.2f);
                controller.inPlayerRange = true;
            }
        }
    }
    public void ChangeMapIconTransparency(float t)
    {
        UnityEngine.UI.Image i = mapIcon.GetComponent<UnityEngine.UI.Image>();
        Color c = i.color;
        c.a = t;
        i.color = c;
    }

    public void AddMapDetail(GameObject detail)
    {
        GameObject mapDetail = Instantiate(detail, mapIcon.transform.position, mapIcon.transform.rotation);
        mapDetail.transform.SetParent(mapIcon.transform);
    }

    public void DisableRoom()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        //make sure unseen rooms are 0.2 transparent
        if (roomCleared)
        {
            ChangeMapIconTransparency(0.6f);
        }
        else
        {
            ChangeMapIconTransparency(0.2f);
        }
        
    }
    public void EnableRoom()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
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
            if (bossRoom)
            {
                foreach (Transform child in layout.transform)
                { 
                    if (child.tag == "Exit")
                    {
                        child.gameObject.SetActive(true);
                        break;
                    }
                        
                }
            }
        }
    }
    public void ChangeLayout(Object newlayout)
    {
        if (layout != null)
        {
            Destroy(layout);
            enemyCount = 0;
        }
        layout = Instantiate(newlayout, transform.position, transform.rotation) as GameObject;
        layout.transform.SetParent(transform);
        layout.SetActive(false);
        enemies.Clear();
    }
    public void AddAdjacencies(int roomType, int newdistance)
    {
        adjacencies.Add(roomType);
        if (newdistance < distance)
        {
            distance = newdistance;
        }
        
    }

    public void CorrectRooms()
    {
        //check if room needs to be have more doors
        foreach (int roomTypeNeeded in adjacencies)
        {
            if (!floorGlobal.numToRoom[roomTypeNeeded].Any((MyObject) => MyObject.name == gameObject.name))
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
                needed.Add(floorGlobal.numToRoom[roomTypeNeeded]);
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
            RoomController replaceController = replace.GetComponent<RoomController>();
            GameObject minimapRoom = Instantiate(floorGlobal.minimapRoomPrefab, mapIcon.transform.position, transform.rotation);
            minimapRoom.transform.SetParent(floorGlobal.minimapCanvas.transform);
            replaceController.mapIcon = minimapRoom;
            replaceController.needFix = true;
            replaceController.distance = distance;
            Destroy(gameObject);
        }
            
    }
    private void Awake()
    {
        floorGlobal = GameObject.FindGameObjectWithTag("FloorGlobalHolder").GetComponent<FloorGlobal>();
        floorGlobal.rooms.Add(gameObject);
        if (!startingRoom)
        {
            distance = floorGlobal.maxRoomCount + 1;
        }
    }
    void Start()
    {
        gate.SetActive(false);
        if (!needFix){
            Invoke("CorrectRooms", 0.1f);
        }        
        //sets room layout
        if (!startingRoom)
        {
            int layoutType = Random.Range(0, floorGlobal.normalLayouts.Length);
            ChangeLayout(floorGlobal.normalLayouts[layoutType]);
            //disable room as optimization
            Invoke("DisableRoom", 0.2f);
        }
        else
        {
            //create first map icon
            GameObject minimapRoom = Instantiate(floorGlobal.minimapRoomPrefab, floorGlobal.minimapCanvas.transform.position, transform.rotation);
            minimapRoom.transform.SetParent(floorGlobal.minimapCanvas.transform);
            minimapRoom.SetActive(true);
            mapIcon = minimapRoom;
            //other
            ChangeLayout(floorGlobal.emptyLayout);
            inPlayerRange = true;
            Invoke("RevealMap", 0.2f);
        }
    }

    public void OnDestroy()
    {
        //destroy its corresponding map icon on destroy
        Destroy(mapIcon);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //turns on gate and enemes when player enters room and room hasn't been cleared already
        if(collision.gameObject.tag == "Player")
        {
            //re enable room on enter
            isPlayerIn = true;
            inPlayerRange = true;
            EnableRoom();
            ChangeMapIconTransparency(1f);
            RevealMap();
            if (!roomCleared)
            {
                gate.SetActive(true);
                //Invoke("ActivateEnemies", 0.5f);
            }
            if (!PlayerController)
            {
                PlayerController = collision.gameObject.GetComponent<PlayerController>();
            }
            PlayerController.currentRoom = this;
        }
    }
}
