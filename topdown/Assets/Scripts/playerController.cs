using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float health = 3;
    public bool isDead = false;
    public GameObject currentGun;
    public Transform rightHand;
    public Transform gunsHolder;
    public int currentGunIndex = 0;
    public List<GameObject> guns;
    public List<int> gunIndexes;
    public List<int> itemIndexes;
    public RoomController currentRoom;
    public ItemDatabase itemDatabase;
    // Update is called once per frame
    public void AddItem(int itemId, string itemType)
    {
        switch (itemType)
        {
            case "gun":
                gunIndexes.Add(itemId);
                Gun gun = itemDatabase.guns[itemId];
                GameObject newGun = Instantiate(gun.gunObject, rightHand.position, rightHand.rotation);
                newGun.transform.SetParent(gunsHolder);
                guns.Add(newGun);
                SwitchToNewWeapon(newGun);
                break;
            case "item":
                itemIndexes.Add(itemId);
                Item item = itemDatabase.items[itemId];
                item.function();
                break;
        }
        
    }
    public void RemoveItem(int itemId)
    {
        itemIndexes.Remove(itemId);
    }
    public void OnSwitchWeapon()
    {
        currentGun.SetActive(false);
        currentGunIndex += 1;
        if (currentGunIndex >= guns.Count)
        {
            currentGunIndex = 0;
        }
        currentGun = guns[currentGunIndex];
        currentGun.SetActive(true);
    }
    public void SwitchToNewWeapon(GameObject newWeapon)
    {
        currentGun.SetActive(false);
        currentGun = newWeapon;
        currentGunIndex = guns.Count - 1;
    }
    public void DestroyBullets()
    {

    }
    public void RemoveHealth(float damage)
    {
        health -= damage;
        //destroys player once health is lower than or equal to zero
        if (health <= 0)
        {
            isDead = true;
            Debug.Log("dead");
            //Destroy(gameObject);
        }
    }
}
