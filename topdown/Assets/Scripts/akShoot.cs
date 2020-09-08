using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class AkShoot : PlayerShoot
{
    public void Awake()
    {
        shootDelay = 0.2f;
    }
}
