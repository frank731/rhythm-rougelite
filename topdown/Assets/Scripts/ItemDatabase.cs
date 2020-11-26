using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemDatabase : Singleton<ItemDatabase>
{
    public IDictionary<int, Gun> guns = new Dictionary<int, Gun>();
    public IDictionary<int, Item> items = new Dictionary<int, Item>();
    public IDictionary<int, UnityAction> itemEffects;
    public PlayerMovement playerMovement;
    public void BuildDatabase()
    {
        foreach (Gun gun in Resources.LoadAll<Gun>("Guns"))
        {
            guns[gun.itemId] = gun;
        }

        itemEffects = new Dictionary<int, UnityAction>
        {
            {1, SPEED}
        };
        //add items here
        foreach (Item item in Resources.LoadAll<Item>("Items"))
        {
            item.action = itemEffects[item.itemId];
            items[item.itemId] = item;
        }
    }
    UnityAction stringFunctionToUnityAction(object target, string functionName)
    {
        UnityAction action = (UnityAction)Delegate.CreateDelegate(typeof(UnityAction), target, functionName);
        return action;
    }

    public void SPEED()
    {
        playerMovement.speed += 3f;
    }
    protected override void Awake()
    {
        base.Awake();
        BuildDatabase();
        DontDestroyOnLoad(gameObject);
    }
}