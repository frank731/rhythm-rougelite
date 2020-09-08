using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public float shootDelay = 0.2f;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public bool canShoot = true;
    public bool auto = false;
    public Animator animator;

    private void OnEnable()
    {
        StartCoroutine(bulletDelay());
    }
    public void Update()
    {
        if (auto)
        {
            //create bullet if mouse held and time between bullets has passed 
            if (Input.GetMouseButton(0) && canShoot)
            {
                Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
                canShoot = false;
                StartCoroutine(bulletDelay());
                animator.SetBool("hasShot", true);
                //akAnimator.SetBool("hasShot", false);
            }
        }
        else
        {
            //create bullet if mouse clicked and time between bullets has passed 
            if (Input.GetMouseButtonDown(0) && canShoot)
            {
                Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
                canShoot = false;
                StartCoroutine(bulletDelay());
                animator.SetBool("hasShot", true);
                //akAnimator.SetBool("hasShot", false);
            }
        }
        

    }
    //delay between bullets
    public IEnumerator bulletDelay()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootDelay);
        canShoot = true;
    }
}
