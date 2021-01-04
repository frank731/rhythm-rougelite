using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerShoot : MonoBehaviour
{
    public float reloadDelay = 1f;
    public int maxAmmo;
    public int currentAmmo;
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

    public void Awake()
    {
        currentAmmo = maxAmmo;
    }

    public void Start()
    {
        objectPooler = ObjectPooler.SharedInstance;
        floorGlobal = FloorGlobal.Instance;
        floorGlobal.pausableScripts.Add(this);
        floorGlobal.startBeat.AddListener(StartBeat);
        audioSource = GetComponent<AudioSource>();
        playerController = transform.parent.parent.GetComponent<PlayerController>();

        Bullet bullet = bulletPrefab.GetComponent<Bullet>();
        bullet.bulletDestroyedEffectIndex = objectPooler.AddObject(bullet.bulletDestroyedEffect, 20, true);
        bulletIndex = objectPooler.AddObject(bulletPrefab, 20, true);
        
    }

    public void StartBeat()
    {
        //Debug.Log("beat");
        playerController.canShoot = true;
        //Shoot();
    }

    protected virtual void Shoot()
    {
        objectPooler.GetPooledObject(bulletIndex, bulletSpawn);
        //Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        playerController.canShoot = false;
        currentAmmo--;
        audioSource.PlayOneShot(shootSFX, 0.6f);
        if (currentAmmo <= 0)
        {
            outOfAmmo = true;
        }
        animator.SetBool("hasShot", true);
        SendMessageUpwards("OnBeatAction");

        playerController.gunAmmoText.text = currentAmmo.ToString() + "/" + maxAmmo.ToString();
        //akAnimator.SetBool("hasShot", false);
    }

    public void OnFire()
    {
        //Debug.Log(floorGlobal.isOnBeat);
        //create bullet if mouse clicked and on beat 
        if (playerController.canShoot && floorGlobal.isOnBeat && !outOfAmmo && !reloading)
        {
            Shoot();
        }
    }

    public void OnReload()
    {
        if (currentAmmo < maxAmmo && !reloading && floorGlobal.isOnBeat)
        {
            StartCoroutine(ReloadDelay());
            audioSource.PlayOneShot(reloadSFX, 0.3f);
            reloading = true;
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
