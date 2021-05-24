using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemDatabase : Singleton<ItemDatabase>
{
    //public IDictionary<int, Gun> guns = new Dictionary<int, Gun>();
    public IDictionary<int, Item> items = new Dictionary<int, Item>();
    public IDictionary<int, UnityAction> abilityEffects;
    public void BuildDatabase()
    {
        foreach (Gun gun in Resources.LoadAll<Gun>("Guns"))
        {
            while (items.ContainsKey(gun.itemId)) gun.itemId++;
            items[gun.itemId] = gun;
        }
        
        foreach (Item item in Resources.LoadAll<Item>("Items"))
        {
            while (items.ContainsKey(item.itemId)) item.itemId++;
            items[item.itemId] = item;
        }
        foreach(Ability ability in Resources.LoadAll<Ability>("Abilities"))
        {
            while (items.ContainsKey(ability.itemId)) ability.itemId++;
            items[ability.itemId] = ability;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        BuildDatabase();
        DontDestroyOnLoad(gameObject);
        FloorGlobal.Instance.dontDestroys.Add(gameObject);
    }
}