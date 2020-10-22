using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPistolShoot : EnemyController
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    private FloorGlobal floorGlobal;

    void OnBeat()
    {
        if (isActive)
        {
            Instantiate(bulletPrefab, firePoint.position, transform.rotation);
        }
    }

    void Start()
    {
        floorGlobal = GameObject.FindGameObjectWithTag("FloorGlobalHolder").GetComponent<FloorGlobal>();
        floorGlobal.onBeat.AddListener(OnBeat);
    }

}
