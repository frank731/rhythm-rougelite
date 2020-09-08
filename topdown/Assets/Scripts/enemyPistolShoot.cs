using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPistolShoot : EnemyController
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float minShootDelay;
    public float maxShootDelay;
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
        shootDelay = Random.Range(minShootDelay, maxShootDelay);
        Invoke("randomizeShootDelay", shootDelay);
    }
}
