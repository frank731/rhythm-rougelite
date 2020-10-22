using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunShoot : PlayerShoot
{
    public int bulletCount;
    public float minSpread;
    public float maxSpread;
    public bool reloaded = true;
    public new void Update()
    {
        //create bullet if mouse pressed and on beat
        if (Input.GetMouseButtonDown(0) && floorGlobal.isOnBeat && playerController.canShoot && reloaded)
        {
            playerController.canShoot = false;
            reloaded = false;
            for (int i = 0; i <= bulletCount; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
                bullet.transform.Rotate(0, 0, Random.Range(minSpread, maxSpread));
            }
            animator.SetTrigger("hasShot");
            SendMessageUpwards("OnBeatAction");
            //akAnimator.SetBool("hasShot", false);
        }
        else if (Input.GetKeyDown(KeyCode.R) && floorGlobal.isOnBeat && !reloaded)
        {
            reloaded = true;
            SendMessageUpwards("OnBeatAction");
        }

    }
}
