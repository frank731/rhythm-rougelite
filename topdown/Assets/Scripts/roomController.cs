using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomController : MonoBehaviour
{
    public List<GameObject> doors;
    public GameObject layout = null;
    public GameObject mapIcon;
    public Transform spawnHolder;
    public Tilemap walls;
    public int roomShape;
    public Color mapColor;
    public bool isPlayerIn = false;
    public bool isDestroyed;
    public bool roomCleared = false;
    public bool inPlayerRange = false;
    public bool startingRoom = false;
    public bool endRoom = false;
    public bool bossRoom = false;
    public bool itemRoom = false;
    public bool createdLargeRoom = false;
    public int distance;
    public int enemyCount = 0;
    public List<GameObject> adjacentRooms = new List<GameObject>();
    private List<int> adjacencies = new List<int>();
    public List<GameObject> enemies = new List<GameObject>();
    private PlayerController playerController = null;

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
        foreach (Transform child in transform)
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
        if (!enemies.Contains(enemy))
        {
            enemyCount += 1;
            enemies.Add(enemy);
        }
    }

    public void ClearRoom()
    {
        StartCoroutine(ChangeDoors(true));
        roomCleared = true;
        if (bossRoom)
        {
            foreach (Transform child in layout.transform)
            {
                if (child.CompareTag("Exit"))
                {
                    child.gameObject.SetActive(true);
                    break;
                }

            }
        }
        else
        {
            for(int i = 0; i < playerController.luck + 1; i++)
            {
                if (Random.Range(0, 10) == 0)
                {
                    Instantiate(FloorGlobal.Instance.pickups[Random.Range(0, FloorGlobal.Instance.pickups.Length)], transform.position, transform.rotation);
                    break;
                }
            }
        }
    }
    public void EnemyDestroyed()
    {
        enemyCount -= 1;
        if (enemyCount <= 0)
        {
            ClearRoom();
        }
    }
    public void SpawnPointDestroyed()
    {
        if (spawnHolder.childCount == 0)
        {
            endRoom = true;
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
    public void AddDoor(int roomType, Transform doorPos)
    {
        GameObject door = Instantiate(FloorGlobal.Instance.doors[roomType - 1], doorPos.position, FloorGlobal.Instance.doors[roomType - 1].transform.rotation);
        Destroy(doorPos.gameObject);
        door.transform.SetParent(transform);
        door.transform.position = doorPos.position;
        door.SetActive(true);
        doors.Add(door);
    }

    private void Awake()
    {
        FloorGlobal.Instance.rooms.Add(gameObject);
        if (!startingRoom)
        {
            distance = FloorGlobal.Instance.maxRoomCount + 1;
        }
    }
    void Start()
    {
        
        //sets room layout
        if (!startingRoom)
        {
            int layoutType = Random.Range(0, FloorGlobal.Instance.layouts[roomShape].Length);
            ChangeLayout(FloorGlobal.Instance.layouts[roomShape][layoutType]);
            //disable room as optimization
            //Invoke("DisableRoom", 0.2f);
            StartCoroutine(ChangeDoors(false));
        }
        else
        {
            //create first map icon
            GameObject minimapRoom = Instantiate(FloorGlobal.Instance.minimapRoomPrefabs[roomShape], FloorGlobal.Instance.minimapCanvas.transform.position, transform.rotation);
            minimapRoom.transform.SetParent(FloorGlobal.Instance.minimapCanvas.transform);
            mapIcon = minimapRoom.transform.GetChild(0).gameObject;
            mapIcon.SetActive(true);

            ChangeLayout(FloorGlobal.Instance.emptyLayout);
            layout.SetActive(true);
            inPlayerRange = true;
            Invoke("RevealMap", 0.2f);
            StartCoroutine(ChangeDoors(true));
        }
    }

    public void OnDestroy()
    {
        //destroy its corresponding map icon on destroy
        try {
            Destroy(mapIcon.transform.parent.gameObject);
        }
        catch
        {
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //turns on gate and enemes when player enters room and room hasn't been cleared already
        if (collision.CompareTag("Player"))
        {
            if (!playerController)
            {
                playerController = collision.gameObject.GetComponent<PlayerController>();
            }
            playerController.currentRoom = this;
            //re enable room on enter
            isPlayerIn = true;
            inPlayerRange = true;
            EnableRoom();
            ChangeMapIconTransparency(1f);
            RevealMap();
            if (!roomCleared)
            {
                StartCoroutine(ChangeDoors(false));
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerIn = false;
            //delete enemy corpses
            if (roomCleared)
            {
                foreach (GameObject enemy in enemies)
                {
                    Destroy(enemy);
                }
                enemies.Clear();
            }
        }
    }

    public IEnumerator ChangeDoors(bool open)
    {
        yield return new WaitForSeconds(0.1f); //waits until all the doors have been created
        if (open)
        {
            
            foreach (GameObject door in doors)
            {
                door.GetComponent<DoorController>().OpenDoor();
            }
        }
        else
        {
            foreach (GameObject door in doors)
            {
                door.GetComponent<DoorController>().CloseDoor();
            }
        }
    }
}
