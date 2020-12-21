using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemDatabase : Singleton<ItemDatabase>
{
    public IDictionary<int, Gun> guns = new Dictionary<int, Gun>();
    public IDictionary<int, Item> items = new Dictionary<int, Item>();
    public IDictionary<int, Ability> abilities = new Dictionary<int, Ability>();
    public IDictionary<int, UnityAction> itemEffects;
    public IDictionary<int, UnityAction> abilityEffects;
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
        
        foreach (Item item in Resources.LoadAll<Item>("Items"))
        {
            item.action = itemEffects[item.itemId];
            items[item.itemId] = item;
        }

        abilityEffects = new Dictionary<int, UnityAction>
        {
            {1, Dash}
        };

        foreach(Ability ability in Resources.LoadAll<Ability>("Abilities"))
        {
            ability.action = abilityEffects[ability.abilityID];
            abilities[ability.abilityID] = ability;
        }
    }

    private void SPEED()
    {
        playerMovement.speed += 3f;
    }
    private void Dash()
    {
        playerMovement.Dash();
    }
    protected override void Awake()
    {
        base.Awake();
        BuildDatabase();
        DontDestroyOnLoad(gameObject);
    }
}