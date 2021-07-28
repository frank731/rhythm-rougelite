using UnityEngine;

public class ShotgunShoot : PlayerShoot
{
    public int bulletCount;
    public float minSpread;
    public float maxSpread;

    protected override void Shoot(float multiplier)
    {
        multiplier += 0.5f;

        if (currentAmmo <= 0)
        {
            outOfAmmo = true;
        }
        for (int i = 0; i <= bulletCount; i++)
        {
            GameObject bullet = CreateBullet(multiplier);
            bullet.transform.Rotate(0, 0, Random.Range(minSpread, maxSpread));
        }

        AfterShoot();
    }
}
