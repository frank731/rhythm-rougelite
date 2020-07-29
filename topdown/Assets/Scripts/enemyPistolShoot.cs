using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyPistolShoot : enemyController
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    private float shootDelay;
    void Start()
    {
        randomizeShootDelay();
        Invoke("shoot", shootDelay);
    }

    void shoot()
    {
        if (isActive)
        {
            Instantiate(bulletPrefab, firePoint.position, transform.rotation);
        }
        Invoke("shoot", shootDelay);
    }
    void randomizeShootDelay()
    {
        shootDelay = Random.Range(0.5f, 3f);
        Invoke("randomizeShootDelay", shootDelay);
    }
}
