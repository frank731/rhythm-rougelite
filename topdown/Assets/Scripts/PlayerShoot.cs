using System.Collections;
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
    public AudioClip shootSFX;
    public AudioClip reloadSFX;
    protected PlayerController playerController;
    protected AudioSource audioSource;

    public void Awake()
    {
        currentAmmo = maxAmmo;
    }

    public void Start()
    {
        FloorGlobal.Instance.pausableScripts.Add(this);
        FloorGlobal.Instance.onBeat.AddListener(OnBeat);
        audioSource = GetComponent<AudioSource>();
        playerController = transform.parent.parent.GetComponent<PlayerController>();
    }

    public void OnBeat()
    {
        playerController.canShoot = true;
        //Shoot();
    }

    protected virtual void Shoot()
    {
        Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        playerController.canShoot = false;
        currentAmmo--;
        audioSource.PlayOneShot(shootSFX, 0.4f);
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
        if (playerController.canShoot && FloorGlobal.Instance.isOnBeat && !outOfAmmo && !reloading)
        {
            Shoot();
        }
    }

    public void OnReload()
    {
        if (currentAmmo < maxAmmo && !reloading && FloorGlobal.Instance.isOnBeat)
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
