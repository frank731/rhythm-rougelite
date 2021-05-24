using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPEED : ItemEffect
{
    public override void OnPickup()
    {
        item.playerMovement.speed++;
    }

    public override void OnRemove()
    {
        item.playerMovement.speed--;
    }
}
