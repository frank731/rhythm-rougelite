using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class akShoot : MonoBehaviour
{
    public static float shootDelay = 0.2f;
    public GameObject bulletPrefab;
    public Transform akBulletSpawn;
    public bool canShoot = true;
    public Animator akAnimator;
    void Update()
    {
        //create bullet if mouse held and time between bullets has passed 
        if(canShoot == true && Input.GetMouseButton(0))
        {
            Instantiate(bulletPrefab, akBulletSpawn.position, akBulletSpawn.rotation);
            StartCoroutine(bulletDelay());
            akAnimator.SetBool("hasShot", true);
            //akAnimator.SetBool("hasShot", false);
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
