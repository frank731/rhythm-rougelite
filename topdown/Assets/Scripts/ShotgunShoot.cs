using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunShoot : PlayerShoot
{
    public int bulletCount;
    public float minSpread;
    public float maxSpread;
    public void Awake()
    {
        shootDelay = 1f;
        auto = false;
    }
    public new void Update()
    {
        //create bullet if mouse held and time between bullets has passed 
        if (canShoot && Input.GetMouseButtonDown(0))
        {
            canShoot = false;
            for (int i = 0; i <= bulletCount; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
                bullet.transform.Rotate(0, 0, Random.Range(minSpread, maxSpread));
            }
            StartCoroutine(bulletDelay());
            animator.SetTrigger("hasShot");
            //akAnimator.SetBool("hasShot", false);
        }

    }
}
