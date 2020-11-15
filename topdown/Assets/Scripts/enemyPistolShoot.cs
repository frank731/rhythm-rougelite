using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPistolShoot : EnemyController
{
    public GameObject bulletPrefab;
    [SerializeField]
    private Transform firePoint;
    private FloorGlobal floorGlobal;
    private int beatCount = 0;

    void OnBeat()
    {
        if (isActive && beatCount == 1) //shoot every second beat
        {
            Instantiate(bulletPrefab, firePoint.position, transform.rotation);
        }
        beatCount++;
        if(beatCount > 1)
        {
            beatCount = 0;
        }
    }

    void Start()
    {
        floorGlobal = GameObject.FindGameObjectWithTag("FloorGlobalHolder").GetComponent<FloorGlobal>();
        floorGlobal.onBeat.AddListener(OnBeat);
    }

}
