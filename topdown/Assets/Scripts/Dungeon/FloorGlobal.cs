using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FloorGlobal : Singleton<FloorGlobal>
{
    private Layouts floorLayouts;
    public GameObject[] roomShapes;
    public GameObject[] normalLayouts;
    public GameObject[] largeLayouts;
    public GameObject[] bossLayouts;
    public GameObject[] itemLayouts;
    public GameObject[] doors;
    public GameObject[][] layouts;
    public GameObject[] pickups;

    public List<GameObject> characters;
    public CharacterChoicesObject characterChoices;

    public int maxRoomCount = 10;
    public int roomCount = 0;
    public int roomArrSize;
    public int floorID;

    public long beatNumber = 0;
    public float inputOffset;
    public UnityEvent onBeat = new UnityEvent();
    public BeatVisualiser bpmVisualiser;
    public OnBeatRange onBeatRange;

    public List<GameObject> rooms = new List<GameObject>();
    public IDictionary<int, List<GameObject>> roomDistances = new Dictionary<int, List<GameObject>>() { };
    public GameObject emptyLayout;
    public GameObject minimapCanvas;
    public GameObject[] minimapRoomPrefabs;
    public RectMask2D minimapMask;

    public GameObject bossIcon;
    public GameObject itemIcon;

    public GameObject pauseCanvas;
    public GameObject deathCanvas;
    public GameObject winScreen;
    public List<GameObject> openUIScreens = new List<GameObject>();


    public GameObject baseCollectable;
    public List<GameObject> dontDestroys = new List<GameObject>();
    public Sprite[] heartSprites;

    public bool isPaused = false;
    public UnityEvent levelChanged = new UnityEvent();
    public List<MonoBehaviour> pausableScripts;
    
    public CameraFollowPlayer cameraFollowPlayer;
    public PlayerController playerController;
    public Transform playerSpawnPoint;
    
    public PlayTimer playTimer;
    public int kills;

    private FloorInfo floorInfo;
    private ItemDatabase itemDatabase;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        Layouts[] loadLayouts = Resources.LoadAll<Layouts>("Prefabs/Layouts");

        heartSprites = Resources.LoadAll<Sprite>("Sprites/HeartsUI");
        pickups = Resources.LoadAll<GameObject>("Prefabs/Pickups");

        levelChanged.AddListener(OnLevelChanged);
        SceneManager.sceneLoaded += OnSceneLoaded;

        characters = characterChoices.characters;
        playerController = Instantiate(characters[ES3.Load("charChoice", 0)], playerSpawnPoint.position, playerSpawnPoint.rotation).GetComponent<PlayerController>();

        inputOffset = ES3.Load<float>("inputOffset", 0);
    }

    private void Start()
    {
        floorInfo = FloorInfo.Instance;
        itemDatabase = ItemDatabase.Instance;
        playerController.playerKilled.AddListener(OnKilled);
    }

    public void CreateItem(int itemID, Vector2 pos, Quaternion rot)
    {
        GameObject newCollectable = Instantiate(baseCollectable, pos, rot);
        Item newItem = itemDatabase.items[itemID];
        newCollectable.GetComponent<SpriteRenderer>().sprite = newItem.itemSprite;
        newCollectable.GetComponent<AddItem>().itemId = newItem.itemId;
        newCollectable.GetComponent<AddItem>().itemType = newItem.type;
    }

    void CreateSpecialRoom(RoomController specialRoomController, GameObject[] layouts, ref bool roomType, GameObject icon, bool isOpen)
    {
        int roomLayout = Random.Range(0, layouts.Length);
        specialRoomController.ChangeLayout(layouts[roomLayout]);
        roomType = true;
        specialRoomController.AddMapDetail(icon);
        if (isOpen)
        {
            specialRoomController.roomCleared = true;
            StartCoroutine(specialRoomController.ChangeDoors(true));
        }
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
        CreateSpecialRoom(bossRoomController, bossLayouts, ref bossRoomController.bossRoom, bossIcon, false);

        index++;
        while (index >= roomDistances[maxDist].Count)
        {
            maxDist -= 1;
            index = 0;
        }

        GameObject itemRoom = roomDistances[maxDist][index];
        RoomController itemRoomController = itemRoom.GetComponent<RoomController>();
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
            itemRoomController = itemRoom.GetComponent<RoomController>();
        }
        //set item room
        CreateSpecialRoom(itemRoomController, itemLayouts, ref itemRoomController.itemRoom, itemIcon, true);
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
    public void Pause(GameObject newPauseCanvas)
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
            newPauseCanvas.SetActive(true);
            openUIScreens.Add(newPauseCanvas);
        }
        else
        {

            if (newPauseCanvas == pauseCanvas)
            {
                openUIScreens.ElementAt(openUIScreens.Count - 1).SetActive(false);
                openUIScreens.RemoveAt(openUIScreens.Count - 1);
            }
            else if (openUIScreens.Contains(newPauseCanvas))
            {
                openUIScreens.Remove(newPauseCanvas);
                newPauseCanvas.SetActive(false);
            }
            else
            {
                newPauseCanvas.SetActive(true);
                openUIScreens.Add(newPauseCanvas);
            }

            if (openUIScreens.Count == 0)
            {
                isPaused = false;
                Time.timeScale = 1;
                foreach (MonoBehaviour script in pausableScripts)
                {
                    script.enabled = true;
                }
            }
        }
    }

    public void OnKilled()
    {
        deathCanvas.SetActive(true);
    }

    private void OnLevelChanged() //before scene change
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

    public float IsOnBeat()
    {
        //Debug.Log(bpmVisualiser.pastBeatTime);
        //Debug.Log(bpmVisualiser.nextBeatTime);
        //Debug.Log(bpmVisualiser.songPos.ToString() + " " + (Mathf.Abs(bpmVisualiser.nextBeatTime - bpmVisualiser.songPos) < bpmVisualiser.beatHangTime || Mathf.Abs(bpmVisualiser.pastBeatTime - bpmVisualiser.songPos) < bpmVisualiser.beatHangTime) + " " + bpmVisualiser.pastBeatTime.ToString() + " " + bpmVisualiser.nextBeatTime);
        //Debug.Log(Mathf.Abs(bpmVisualiser.nextBeatTime - bpmVisualiser.audioSource.time));
        float first = Mathf.Abs(bpmVisualiser.nextBeatTime - (bpmVisualiser.songPos - bpmVisualiser.inputOffset)), second = Mathf.Abs(bpmVisualiser.pastBeatTime - (bpmVisualiser.songPos - bpmVisualiser.inputOffset));
        if (first < bpmVisualiser.beatHangTime)
        {
            return 1 - (first / bpmVisualiser.beatHangTime);
        }
        else if(second < bpmVisualiser.beatHangTime)
        {
            return 1 - (second / bpmVisualiser.beatHangTime);
        }
        else
        {
            return 0;
        }

    }

    public void Win()
    {
        Instantiate(winScreen, transform.position, transform.rotation);
    }

    public void DestroyAllDontDestroyOnLoad()
    {
        foreach(GameObject g in dontDestroys)
        {
            Destroy(g);
        }
        Destroy(gameObject);
    }

    private void NewScene() //callable without needed to use scene parameter
    {
        floorInfo = FloorInfo.Instance;
        floorID = floorInfo.GetFloorID();
        roomShapes = floorInfo.GetRooms();
        floorLayouts = floorInfo.GetLayouts();
        doors = floorInfo.GetDoors();
        floorLayouts.LoadLayouts();
        cameraFollowPlayer = Camera.main.GetComponent<CameraFollowPlayer>();
        normalLayouts = floorLayouts.normalLayouts;
        largeLayouts = floorLayouts.largeLayouts;
        itemLayouts = floorLayouts.itemLayouts;
        bossLayouts = floorLayouts.bossLayouts;
        emptyLayout = floorLayouts.emptyLayout;
        roomArrSize = roomShapes.Length;
        rooms.Clear();
        roomCount = 0;
        beatNumber = 0;
        layouts = new GameObject[][] {normalLayouts, largeLayouts};
        Invoke("GetMaxDistance", 0.5f);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) //turns out this gets called in awake
    {
        NewScene();
    }

    protected override void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        base.OnDestroy();
    }
}
