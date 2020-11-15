using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    protected PlayerController playerController;
    protected FloorGlobal floorGlobal;

    public void Awake()
    {
        currentAmmo = maxAmmo;
    }

    public void Start()
    {
        floorGlobal = GameObject.FindGameObjectWithTag("FloorGlobalHolder").GetComponent<FloorGlobal>();
        floorGlobal.pausableScripts.Add(this);
        floorGlobal.onBeat.AddListener(OnBeat);
        playerController = transform.parent.parent.GetComponent<PlayerController>();
    }

    private void OnBeat()
    {
        playerController.canShoot = true;
    }

    protected virtual void Shoot()
    {
        Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        playerController.canShoot = false;
        currentAmmo--;
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
