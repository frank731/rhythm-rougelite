using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunShoot : PlayerShoot
{
    public int bulletCount;
    public float minSpread;
    public float maxSpread;

    protected override void Shoot()
    {
        playerController.canShoot = false;
        currentAmmo--;
        if (currentAmmo <= 0)
        {
            outOfAmmo = true;
        }
        for (int i = 0; i <= bulletCount; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
            bullet.transform.Rotate(0, 0, Random.Range(minSpread, maxSpread));
        }
        animator.SetTrigger("hasShot");
        SendMessageUpwards("OnBeatAction");
        playerController.gunAmmoText.text = currentAmmo.ToString() + "/" + maxAmmo.ToString();
        //akAnimator.SetBool("hasShot", false);
    }
}
