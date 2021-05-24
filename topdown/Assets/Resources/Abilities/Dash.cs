using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : ItemEffect
{
    public override void OnUse()
    {
        if (!item.playerController.isDashing)
        {
            item.playerController.isDashing = true;
            item.playerController.isInvincible = true;
            item.playerMovement.dashTimeLeft = item.playerMovement.dashTime;
        }
    }
}
