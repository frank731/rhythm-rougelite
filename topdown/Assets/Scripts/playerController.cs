using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public float startHealth = 6;
    public float health = 0;
    public float iFrames = 1f;
    public bool isInvincible = false;
    public bool isDead = false;
    public GameObject currentGun;
    public Transform rightHand;
    public Transform gunsHolder;
    public int currentGunIndex = 0;
    public List<GameObject> guns = new List<GameObject>();
    public List<Item> items = new List<Item>();
    public List<int> gunIndexes;
    public List<int> itemIndexes;
    public RoomController currentRoom;
    public ItemDatabase itemDatabase;
    public FloorGlobal floorGlobal;
    public bool canShoot = false;
    public GameObject inventory;
    public Transform itemInventoryHolder;
    public Transform gunInventoryHolder;
    public GameObject itemInventoryImageTemplate;
    public OnBeatRange onBeatRange;
    public Transform heartUIGrid;
    public bool viewingMap = false;
    public GameObject heartTemplate;
    private List<GameObject> hearts = new List<GameObject>();
    public float heartContainers = 3;
    public TMPro.TextMeshProUGUI gunAmmoText;
    public Image gunUIImage;
    public UnityEvent resetStats = new UnityEvent();
    private int heartEmptyIndex = 0;
    private Animator playerAnimator;
    
    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        floorGlobal.pausableScripts.Add(this);
        AddHealth(startHealth, heartContainers);
        inventory.SetActive(false); //inventory canvas must be enabled at start to place images on it correctly
        SwitchToNewWeapon(currentGun);
    }

    public void OnBeatAction()
    {
        onBeatRange.BeatAction();
    }

    public void AddItem(int itemId, string itemType)
    {
        switch (itemType)
        {
            case "gun":
                gunIndexes.Add(itemId);
                Gun gun = itemDatabase.guns[itemId];
                GameObject newGun = Instantiate(gun.gunObject, rightHand.position, rightHand.rotation);
                newGun.transform.SetParent(gunsHolder);
                //create icon for inventory
                GameObject gunSprite = Instantiate(itemInventoryImageTemplate, itemInventoryImageTemplate.transform.position, itemInventoryImageTemplate.transform.rotation);
                gunSprite.GetComponent<Image>().sprite = gun.gunSprite;
                gunSprite.GetComponent<ShowDescription>().description = gun.description;
                gunSprite.transform.SetParent(gunInventoryHolder);
                guns.Add(newGun);
                SwitchToNewWeapon(newGun);
                break;

            case "item":
                itemIndexes.Add(itemId);
                Item databaseItem = itemDatabase.items[itemId];
                Item newItem = new Item(databaseItem.itemId, databaseItem.displayName, databaseItem.description, databaseItem.itemSprite, databaseItem.function); //create copy of item instead of pointer to databases item
                //create icon for inventory
                GameObject itemSprite = Instantiate(itemInventoryImageTemplate, itemInventoryImageTemplate.transform.position, itemInventoryImageTemplate.transform.rotation);
                itemSprite.GetComponent<Image>().sprite = newItem.itemSprite;
                itemSprite.GetComponent<ShowDescription>().description = newItem.description;
                itemSprite.transform.SetParent(itemInventoryHolder);
                Debug.Log(itemSprite.GetInstanceID() + "new");
                newItem.inventoryIcon = itemSprite;
                items.Add(newItem);
                foreach (Item item in items)
                {
                    item.function();
                    Debug.Log(item.inventoryIcon.GetInstanceID());
                }
                break;
        }
        
    }

    public void RemoveItem(int removeItemId)
    {
        foreach(Item i in items)
        {
            Debug.Log(i.inventoryIcon.GetInstanceID());
        }
        Item toDelete = items.Find(i => i.itemId == removeItemId); //finds first item with needed id
        itemIndexes.Remove(removeItemId);
        //Debug.Log(toDelete.inventoryIcon.transform.position);
        Destroy(toDelete.inventoryIcon);
        items.Remove(toDelete);
        resetStats.Invoke();
        foreach (Item item in items)
        {
            item.function();
        }
        //TODO make item effects reversible by keeping list of all current effects and calling when item is added or removed
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
        floorGlobal.OnPause();
    }
    public void OnViewMap()
    {
        //calls when tab is pressed and when tab is released
        viewingMap = !viewingMap;
        floorGlobal.OnViewMap(viewingMap);
    }

    public void AddHealth(float healthAdded, float containerCount = 0)
    {
        //create new heart containers
        for (int i = 0; i < containerCount; i++)
        {
            GameObject newHeartContainer = Instantiate(heartTemplate, heartTemplate.transform.position, heartTemplate.transform.rotation);
            newHeartContainer.transform.SetParent(heartUIGrid);
            hearts.Add(newHeartContainer);
            heartContainers++;
        }
        //fill heart containers
        if (hearts[heartEmptyIndex].GetComponent<Image>().sprite.name == "hearts_half") //if the current "empty" heart is a half heart add +1 to health so it will call the next two loops properly
        {
            healthAdded++;
            health -= 1;
        }

        for (int i = 0; i < (healthAdded / 2) - (healthAdded % 2); i++) //fill full hearts
        {
            //changes last empty heart to full
            hearts[heartEmptyIndex].GetComponent<Image>().sprite = floorGlobal.heartSprites[1];
            health += 2;
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
                health++;
            }
        }

    }

    public void RemoveHealth(float damage)
    {
        if (!isInvincible)
        {
            StartCoroutine(IFrameDelay());

            health -= damage;

            playerAnimator.SetTrigger("Player Damaged");

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
                isDead = true;
                playerAnimator.SetTrigger("Player Killed");
                Debug.Log("dead");
                //Destroy(gameObject);
            }
        }
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            RemoveItem(1);
        }
    }
    public IEnumerator IFrameDelay()
    {
        isInvincible = true;
        yield return new WaitForSeconds(iFrames);
        isInvincible = false; 
    }
}
