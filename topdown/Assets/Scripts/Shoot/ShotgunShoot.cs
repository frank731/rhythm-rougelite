using UnityEngine;

public class ShotgunShoot : PlayerShoot
{
    public int bulletCount;
    public float minSpread;
    public float maxSpread;

    protected override void Shoot(float multiplier)
    {
        playerController.canShoot = false;
        currentAmmo--;

        audioSource.PlayOneShot(shootSFX, 0.3f);

        if (currentAmmo <= 0)
        {
            outOfAmmo = true;
        }
        for (int i = 0; i <= bulletCount; i++)
        {
            GameObject bullet = objectPooler.GetPooledObject(bulletIndex, bulletSpawn);
            bullet.GetComponent<Bullet>().bulletDamage *= multiplier;
            bullet.transform.Rotate(0, 0, Random.Range(minSpread, maxSpread));
        }

        animator.SetTrigger("Shoot");
        SendMessageUpwards("OnBeatAction");
        playerController.gunAmmoText.text = currentAmmo.ToString() + "/" + maxAmmo.ToString();
        //akAnimator.SetBool("hasShot", false);
    }
}
