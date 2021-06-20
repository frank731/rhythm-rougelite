using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerShoot : MonoBehaviour
{
    public float reloadDelay = 1f;
    public int maxAmmo;
    public int currentAmmo;
    public int reloadTime;
    public bool outOfAmmo = false;
    public bool reloading = false;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public Animator animator;
    public AudioClip shootSFX;
    public AudioClip reloadSFX;
    protected PlayerController playerController;
    protected AudioSource audioSource;
    protected ObjectPooler objectPooler;
    protected FloorGlobal floorGlobal;
    protected int bulletIndex;
    protected int currentReloadTime;
    protected long reloadBeat;

    public void Awake()
    {
        currentAmmo = maxAmmo;
    }

    public void Start()
    {
        objectPooler = ObjectPooler.SharedInstance;
        floorGlobal = FloorGlobal.Instance;
        floorGlobal.pausableScripts.Add(this);
        floorGlobal.onBeat.AddListener(OnBeat);
        audioSource = GetComponent<AudioSource>();
        playerController = transform.parent.parent.GetComponent<PlayerController>();

        Bullet bullet = bulletPrefab.GetComponent<Bullet>();
        bullet.bulletDestroyedEffectIndex = objectPooler.AddObject(bullet.bulletDestroyedEffect, 20, true);
        bulletIndex = objectPooler.AddObject(bulletPrefab, 20, true);
        
    }

    public void OnBeat()
    {
        //Debug.Log("beat");
        playerController.canShoot = true;
        if (reloading && reloadBeat != floorGlobal.beatNumber)
        {
            currentReloadTime--;
            if(currentReloadTime == 0)
            {
                outOfAmmo = false;
                reloading = false;
                currentAmmo = maxAmmo;
                playerController.gunAmmoText.text = currentAmmo.ToString() + "/" + maxAmmo.ToString();
            }
        }
        //Shoot();
    }

    protected virtual void Shoot(float multiplier)
    {
        multiplier += 0.5f;
        GameObject bullet = objectPooler.GetPooledObject(bulletIndex, bulletSpawn);
        bullet.GetComponent<Bullet>().bulletDamage *= multiplier;
        //Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        playerController.canShoot = false;
        currentAmmo--;
        audioSource.PlayOneShot(shootSFX, 0.6f);
        if (currentAmmo <= 0)
        {
            outOfAmmo = true;
        }
        animator.SetTrigger("Shoot");
        SendMessageUpwards("OnBeatAction");

        playerController.gunAmmoText.text = currentAmmo.ToString() + "/" + maxAmmo.ToString();
    }

    public void OnFire()
    {
        //Debug.Log(floorGlobal.IsOnBeat());
        //create bullet if mouse clicked and on beat 
        float offbeat = floorGlobal.IsOnBeat();
        if (playerController.canShoot  && !outOfAmmo && !reloading && offbeat != 0)
        {
            Shoot(offbeat);
        }
    }

    public void OnReload()
    {
        if (currentAmmo < maxAmmo && !reloading && floorGlobal.IsOnBeat() != 0)
        {
            //StartCoroutine(ReloadDelay());
            currentReloadTime = reloadTime;
            audioSource.PlayOneShot(reloadSFX, 0.3f);
            reloading = true;
            reloadBeat = floorGlobal.beatNumber;
        }
    }

    private IEnumerator ReloadDelay()
    {
        yield return new WaitForSeconds(reloadDelay);
        outOfAmmo = false;
        reloading = false;
        currentAmmo = maxAmmo;
        playerController.gunAmmoText.text = currentAmmo.ToString() + "/" + maxAmmo.ToString();
    }
}
