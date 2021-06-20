using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//TODO implement use beat system so player has to choose what to do on a specific beat
public class PlayerController : MonoBehaviour
{
    public int currentGunIndex = 0;
    public List<GameObject> guns = new List<GameObject>();
    public List<Item> items = new List<Item>();
    public Ability[] abilities = new Ability[3];
    public List<int> gunIndexes;
    public List<int> itemIndexes;
    public float health = 6;
    public float iFrames = 0.1f;
    public float heartContainers = 3;
    public float luck = 0;
    public List<GameObject> hearts = new List<GameObject>();
    protected int heartEmptyIndex = 0;
    public GameObject currentGun;
    public UnityEvent loadPlayerData = new UnityEvent();
    //public ES3File playerData = new ES3File("playerData.es3");

    public bool isInvincible = false;
    public bool isDead = false;
    public bool isDashing = false;

    public Transform rightHand;
    public Transform gunsHolder;
    public RoomController currentRoom;
    public bool canShoot = false;
    public GameObject inventory;
    public ChooseAbilitySlot chooseAbilitySlot;
    public Transform itemInventoryHolder;
    public Transform gunInventoryHolder;
    public GameObject itemInventoryImageTemplate;
    public Transform heartUIGrid;
    public bool viewingMap = false;
    public GameObject heartTemplate;
    public TMPro.TextMeshProUGUI gunAmmoText;
    public Image gunUIImage;
    public UnityEvent resetStats = new UnityEvent();
    public UnityEvent playerKilled = new UnityEvent();
    public AudioClip playerHurtSFX;
    public Texture2D cursorCrosshair;
    public UnityEvent onUpdate;
    public UnityEvent onHit;

    [SerializeField]
    private GameObject[] abilitiesUI;
    private IDictionary<int, List<GameObject>> invIcons = new Dictionary<int, List<GameObject>>();
    private Animator playerAnimator;
    private AudioSource playerAudioSource;
    private PlayerMovement playerMovement;
    private FloorGlobal floorGlobal;
    private ItemDatabase itemDatabase;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Cursor.SetCursor(cursorCrosshair, new Vector2(cursorCrosshair.width / 2, cursorCrosshair.height / 2), CursorMode.ForceSoftware);
        floorGlobal = FloorGlobal.Instance;
        itemDatabase = ItemDatabase.Instance;
        //LoadPlayerData();
        GameObject playerUI = GameObject.FindGameObjectWithTag("PlayerUI");

        foreach (int gunIndex in gunIndexes)
        {
            AddItem(gunIndex, "gun", true);
        }
        foreach (int itemIndex in itemIndexes)
        {
            AddItem(itemIndex, "item", true);
        }
        playerAnimator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAudioSource = GetComponent<AudioSource>();
        floorGlobal.pausableScripts.Add(this);
        AddHealth(health, heartContainers, true);
        //Debug.Log(heartContainers);
        SwitchToNewWeapon(currentGun);
        floorGlobal.levelChanged.AddListener(LevelChanged);
        floorGlobal.dontDestroys.Add(gameObject);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            LevelChanged();
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            LoadPlayerData();
        }
        onUpdate.Invoke();
    }

    public void OnBeatAction()
    {
        floorGlobal.onBeatRange.BeatAction();
    }
    private void AddEffect(Item item)
    {
        if (item.useUpdate) onUpdate.AddListener(item.itemEffect.OnUpdate);
        if (item.usePickup) item.itemEffect.OnPickup();
        if (item.useHit) onHit.AddListener(item.itemEffect.OnHit);
    }
    private void RemoveEffect(Item item)
    {
        if (item.useUpdate) onUpdate.RemoveListener(item.itemEffect.OnUpdate);
        if (item.usePickup) item.itemEffect.OnRemove();
        if (item.useHit) onHit.RemoveListener(item.itemEffect.OnHit);
    }

    public void AddItem(int itemId, string itemType, bool initializing = false, GameObject abilityPickup = null)
    {
        switch (itemType)
        {
            case "gun":
                if (!initializing)
                {
                    gunIndexes.Add(itemId);
                }
                Gun gun = (Gun)itemDatabase.items[itemId];
                GameObject newGun = Instantiate(gun.gunObject, rightHand.position, rightHand.rotation);
                newGun.transform.SetParent(gunsHolder);
                //create icon for inventory
                GameObject gunSprite = Instantiate(itemInventoryImageTemplate, itemInventoryImageTemplate.transform.position, itemInventoryImageTemplate.transform.rotation);
                gunSprite.GetComponent<Image>().sprite = gun.itemSprite;
                gunSprite.GetComponent<ShowDescription>().description = gun.description;
                gunSprite.transform.SetParent(gunInventoryHolder);
                guns.Add(newGun);
                SwitchToNewWeapon(newGun);
                break;

            case "item":
                if (!initializing)
                {
                    itemIndexes.Add(itemId);
                }
                //Item newItem = Instantiate(itemDatabase.items[itemId]);
                Item newItem = itemDatabase.items[itemId];
                newItem.playerController = this;
                newItem.playerMovement = playerMovement;
                AddEffect(newItem);
                //Item newItem = new Item(databaseItem.itemId, databaseItem.displayName, databaseItem.description, databaseItem.itemSprite, databaseItem.function); //create copy of item instead of pointer to databases item
                //create icon for inventory
                GameObject itemSprite = Instantiate(itemInventoryImageTemplate, itemInventoryImageTemplate.transform.position, itemInventoryImageTemplate.transform.rotation);
                itemSprite.GetComponent<Image>().sprite = newItem.itemSprite;
                itemSprite.GetComponent<ShowDescription>().description = newItem.description;
                itemSprite.transform.SetParent(itemInventoryHolder);
                if(!invIcons.ContainsKey(newItem.itemId)) invIcons[newItem.itemId] = new List<GameObject>();
                invIcons[newItem.itemId].Add(itemSprite);
                items.Add(newItem);
                break;
            case "ability":
                Ability newAbility = Instantiate((Ability)itemDatabase.items[itemId]);
                newAbility.floorGlobal = floorGlobal;
                newAbility.playerController = this;
                newAbility.playerMovement = playerMovement;
                AddEffect(newAbility);
                chooseAbilitySlot.SetNewAbility(newAbility);
                chooseAbilitySlot.gameObject.SetActive(true);
                Time.timeScale = 0;
                break;
        }

    }

    public void RemoveItem(int removeItemId)
    {
        foreach (Item i in items)
        {
            Debug.Log(i.itemSprite.GetInstanceID());
        }
        try
        {
            Item toDelete = items.Find(i => i.itemId == removeItemId); //finds first item with needed id
            itemIndexes.Remove(removeItemId);
            //Debug.Log(toDelete.inventoryIcon.transform.position);
            Destroy(invIcons[removeItemId][0]);
            invIcons[removeItemId].RemoveAt(0);
            items.Remove(toDelete);
            //resetStats.Invoke();
            RemoveEffect(toDelete);
        }
        catch
        {
            Debug.Log("ID not in items");
        }
        //TODO make item effects reversible by keeping list of all current effects and calling when item is added or removed
    }
    public void AddAbility(int slot, Ability newAbility)
    {
        if(abilities[slot] != null)
        {
            RemoveEffect(newAbility);
        }
        abilities[slot] = newAbility;
        //abilitiesUI[1].GetComponent<Image>().sprite = newAbility.abilitySprite;
        abilities[slot].SetUI(abilitiesUI[slot]);
    }

    public void OnSwitchWeapon()
    {
        //switches to next gun in rotation
        currentGun.SetActive(false);
        currentGunIndex += 1;
        if (currentGunIndex >= guns.Count)
        {
            currentGunIndex = 0;
        }
        currentGun = guns[currentGunIndex];
        currentGun.SetActive(true);
        UpdateGunUI();
    }
    public void SwitchToNewWeapon(GameObject newWeapon)
    {
        currentGun.SetActive(false);
        currentGun = newWeapon;
        newWeapon.SetActive(true);
        currentGunIndex = guns.Count - 1;
        UpdateGunUI();
    }
    public void UpdateGunUI()
    {
        gunUIImage.sprite = currentGun.GetComponent<SpriteRenderer>().sprite;
        gunAmmoText.text = currentGun.GetComponent<PlayerShoot>().currentAmmo.ToString() + "/" + currentGun.GetComponent<PlayerShoot>().maxAmmo.ToString();
    }

    public void OnPause()
    {
        floorGlobal.Pause(floorGlobal.pauseCanvas);
    }

    public void OnViewInventory()
    {
        floorGlobal.Pause(inventory);
    }

    public void OnViewMap()
    {
        //calls when tab is pressed and when tab is released
        viewingMap = !viewingMap;
        floorGlobal.OnViewMap(viewingMap);
    }

    public void AddHealth(float healthAdded, float containerCount = 0, bool initializing = false)
    {
        //create new heart containers
        for (int i = 0; i < containerCount; i++)
        {
            GameObject newHeartContainer = Instantiate(heartTemplate, heartTemplate.transform.position, heartTemplate.transform.rotation);
            newHeartContainer.transform.SetParent(heartUIGrid);
            hearts.Add(newHeartContainer);
            if (!initializing)
            {
                heartContainers++;
            }
        }
        //fill heart containers
        if (hearts[heartEmptyIndex].GetComponent<Image>().sprite.name == "hearts_half") //if the current "empty" heart is a half heart add +1 to health so it will call the next two loops properly
        {
            healthAdded++;
            if (!initializing)
            {
                health -= 1;
            }
        }

        for (int i = 0; i < (healthAdded / 2) - (healthAdded % 2); i++) //fill full hearts
        {
            //changes last empty heart to full
            hearts[heartEmptyIndex].GetComponent<Image>().sprite = floorGlobal.heartSprites[1];
            if (!initializing)
            {
                health += 2;
            }
            heartEmptyIndex++;
            if (heartEmptyIndex == hearts.Count)
            {
                heartEmptyIndex--;
                break;
            }
        }
        for (int i = 0; i < (healthAdded % 2); i++)
        {
            //changes empty hearts to half hearts
            if (heartEmptyIndex != hearts.Count && hearts[heartEmptyIndex].GetComponent<Image>().sprite.name == "hearts_empty")
            {
                hearts[heartEmptyIndex].GetComponent<Image>().sprite = floorGlobal.heartSprites[2];
                if (!initializing)
                {
                    health++;
                }
            }
        }

    }

    public void OnDebugClear()
    {
        currentRoom.ClearRoom();
        foreach(GameObject enemy in currentRoom.enemies)
        {
            Destroy(enemy);
        }
    }
    
    public void KillPlayer()
    {
        health = 0;
        isDead = true;
        playerAnimator.SetTrigger("Player Killed");
        playerMovement.rb.bodyType = RigidbodyType2D.Static;
        playerMovement.enabled = false;
        gunsHolder.gameObject.SetActive(false);
        GetComponent<Collider2D>().enabled = false;
        playerKilled.Invoke();
    }

    public void RemoveHealth(float damage)
    {
        if (!isInvincible && !isDead)
        {
            StartCoroutine(IFrameDelay());

            health -= damage;

            playerAnimator.SetTrigger("Player Damaged");
            playerAudioSource.PlayOneShot(playerHurtSFX, 0.3f);

            //remove health from ui
            if (hearts[heartEmptyIndex].GetComponent<Image>().sprite.name == "hearts_half") //if the current "empty" heart is a half heart add +1 to health so it will call the next two loops properly
            {
                damage++;
            }
            for (int i = 0; i < (damage / 2) - (damage % 2); i++) //destroy full hearts
            {
                //changes last full heart to empty
                hearts[heartEmptyIndex].GetComponent<Image>().sprite = floorGlobal.heartSprites[0];
                heartEmptyIndex--;
                if (heartEmptyIndex == -1)
                {
                    heartEmptyIndex++;
                    break;
                }
            }
            for (int i = 0; i < (damage % 2); i++)
            {
                //changes full hearts to half hearts
                if (heartEmptyIndex > -1 && hearts[heartEmptyIndex].GetComponent<Image>().sprite.name == "hearts_full")
                {
                    hearts[heartEmptyIndex].GetComponent<Image>().sprite = floorGlobal.heartSprites[2];
                }
            }
            
            //destroys player once health is lower than or equal to zero
            if (health <= 0)
            {
                KillPlayer();
            }
            onHit.Invoke();
        }

    }

    private void LevelChanged()
    {
        if (isDead)
        {
            Destroy(gameObject);
        }
        Debug.Log("Saving");
        ES3.Save("currentHealth", health);
        ES3.Save("heartContainers", heartContainers);
        ES3.Save("heartEmptyIndex", heartEmptyIndex);
        ES3.Save("itemIndexes", itemIndexes);
        ES3.Save("gunIndexes", gunIndexes);
        ES3.Save("currentGun", currentGun);

        transform.position = new Vector3(0, 0, 0); //reset the players position to the starting room when the new level is loaded
    }

    public void LoadPlayerData()
    {
        Debug.Log("loading");
        health = ES3.Load<float>("currentHealth", 6);
        heartContainers = ES3.Load<float>("heartContainers", 3);
        heartEmptyIndex = ES3.Load("heartEmptyIndex", 0);
        gunIndexes = ES3.Load("gunIndexes", new List<int>());
        itemIndexes = ES3.Load("itemIndexes", new List<int>());
        gunIndexes = ES3.Load("gunIndexes", new List<int>());
        currentGun = ES3.Load("currentGun", guns[0]);
        loadPlayerData.Invoke();
    }

    public IEnumerator IFrameDelay()
    {
        isInvincible = true;
        yield return new WaitForSeconds(iFrames);
        isInvincible = false;
    }

    private void OnDestroy()
    {
        foreach(GameObject heart in hearts){
            Destroy(heart);
        }
        hearts.Clear();
    }
}
