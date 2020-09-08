using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public IDictionary<int, Gun> guns;
    public IDictionary<int, Item> items;
    public PlayerMovement playerMovement;
    public void BuildDatabase()
    {
        //add guns here
        guns = new Dictionary<int, Gun>
        {
            { 1, new Gun(1, "Shotgun", "A regular shotgun", Resources.Load<GameObject>("Prefabs/Weapons/Shotgun"))},
            { 2, new Gun(2, "AK", "A regular AK", Resources.Load<GameObject>("Prefabs/Weapons/AK"))}
        };
        //add items here
        items = new Dictionary<int, Item>
        {
            { 1, new Item(1, "SPEED", "Boosts player speed", SPEED)}
        };
    }
    public void SPEED()
    {
        playerMovement.speed += 3f;
    }
    void Awake()
    {
        BuildDatabase();
    }

    
}
