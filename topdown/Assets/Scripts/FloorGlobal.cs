using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FloorGlobal : Singleton<FloorGlobal>
{
    public GameObject[] roomShapes;
    public GameObject[] normalLayouts;
    public GameObject[] largeLayouts;
    public GameObject[] bossLayouts;
    public GameObject[] itemLayouts;
    public GameObject[] doors;
    public GameObject[][] layouts;
    public int maxRoomCount = 10;
    public int roomCount = 0;
    public int roomArrSize;
    public List<GameObject> rooms = new List<GameObject>();
    public IDictionary<int, List<GameObject>> roomDistances = new Dictionary<int, List<GameObject>>() { };
    public GameObject emptyLayout;
    public GameObject minimapCanvas;
    public GameObject[] minimapRoomPrefabs;
    public GameObject bossIcon;
    public GameObject itemIcon;
    public GameObject pauseCanvas;
    public GameObject deathCanvas;
    public Sprite[] heartSprites;
    public bool isPaused = false;
    public bool isOnBeat = false;
    public bool restarted = false;
    public UnityEvent onBeat = new UnityEvent();
    public UnityEvent levelChanged = new UnityEvent();
    public List<MonoBehaviour> pausableScripts;
    public RectMask2D minimapMask;
    public CameraFollowPlayer cameraFollowPlayer;
    public PlayerController playerController;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        //Debug.Log(Instance.transform.position);

        roomArrSize = roomShapes.Length;
        normalLayouts = Resources.LoadAll<GameObject>("Prefabs/Layouts/Room Layouts");
        largeLayouts = Resources.LoadAll<GameObject>("Prefabs/Layouts/Large Room Layouts");
        bossLayouts = Resources.LoadAll<GameObject>("Prefabs/Layouts/Boss Layouts");
        itemLayouts = Resources.LoadAll<GameObject>("Prefabs/Layouts/Item Layouts");
        heartSprites = Resources.LoadAll<Sprite>("Sprites/HeartsUI");

        layouts = new GameObject[][] { normalLayouts, largeLayouts };

        levelChanged.AddListener(OnLevelChanged);
        SceneManager.sceneLoaded += OnSceneLoaded;
        
    }

    private void Start()
    {
        Invoke("GetMaxDistance", 0.5f);
        playerController.playerKilled.AddListener(OnKilled);
    }

    void CreateSpecialRoom(GameObject room, GameObject[] layouts, ref bool roomType, GameObject icon)
    {
        RoomController farthestRoomController = room.GetComponent<RoomController>();
        int roomLayout = Random.Range(0, layouts.Length);
        farthestRoomController.ChangeLayout(layouts[roomLayout]);
        roomType = true;
        farthestRoomController.AddMapDetail(icon);
        farthestRoomController.roomCleared = true;
        StartCoroutine(farthestRoomController.ChangeDoors(true));
    }
    private void CreateSpecialRooms(int maxDist)
    {
        int index = 0;

        GameObject bossRoom = roomDistances[maxDist][index];
        RoomController bossRoomController = bossRoom.GetComponent<RoomController>();
        //ensures room is an ending room
        while (!bossRoomController.endRoom)
        {
            if (index < roomDistances[maxDist].Count)
            {
                index++;
            }
            else
            {
                maxDist -= 1;
            }
            bossRoom = roomDistances[maxDist][index];
            bossRoomController = bossRoom.GetComponent<RoomController>();
        }
        //set boss room
        CreateSpecialRoom(bossRoom, bossLayouts, ref bossRoom.GetComponent<RoomController>().bossRoom, bossIcon);

        index++;
        while (index >= roomDistances[maxDist].Count)
        {
            maxDist -= 1;
            index = 0;
        }

        GameObject itemRoom = roomDistances[maxDist][index];
        RoomController itemRoomController = bossRoom.GetComponent<RoomController>();
        //ensures room is an ending room
        while (!itemRoomController.endRoom)
        {
            if (index < roomDistances[maxDist].Count)
            {
                index++;
            }
            else
            {
                maxDist -= 1;
            }
            itemRoom = roomDistances[maxDist][index];
            itemRoomController = bossRoom.GetComponent<RoomController>();
        }
        //set item room
        CreateSpecialRoom(itemRoom, itemLayouts, ref itemRoom.GetComponent<RoomController>().itemRoom, itemIcon);

    }
    private void GetMaxDistance()
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
        CreateSpecialRooms(farthestDistance);


    }
    public void OnPause(GameObject pauseCanvas)
    {
        //removes any scripts that have been deleted
        pausableScripts.RemoveAll(script => script == null);
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

    public void OnKilled()
    {
        deathCanvas.SetActive(true);
    }

    private void OnLevelChanged()
    {
        roomDistances.Clear();
        rooms.Clear();
    }

    public void OnViewMap(bool viewingMap)
    {
        minimapMask.enabled = !minimapMask.enabled;
        if (viewingMap)
        {
            minimapCanvas.transform.localScale.Set(1.5f, 1.5f, 1);
            minimapCanvas.transform.position -= new Vector3(100f, 100f, 1);
        }
        else
        {
            minimapCanvas.transform.localScale.Set(1, 1, 1);
            minimapCanvas.transform.position += new Vector3(100f, 100f, 1);
        }

    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        cameraFollowPlayer = Camera.main.GetComponent<CameraFollowPlayer>();
    }

    protected override void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        base.OnDestroy();
    }
}
