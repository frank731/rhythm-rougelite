using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public float shootDelay = 0.2f;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public Animator animator;
    protected PlayerController playerController;
    protected FloorGlobal floorGlobal;

    public void Start()
    {
        floorGlobal = GameObject.FindGameObjectWithTag("FloorGlobalHolder").GetComponent<FloorGlobal>();
        floorGlobal.pausableScripts.Add(this);
        floorGlobal.onBeat.AddListener(OnBeat);
        playerController = transform.parent.parent.GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        //prevent multiple bulletDelays from running when reenabled.
        //StopAllCoroutines();
        //StartCoroutine(BulletDelay());
    }
    private void OnBeat()
    {
        playerController.canShoot = true;
    }

    public void Update()
    {
        //create bullet if mouse clicked and on beat 
        if (Input.GetMouseButtonDown(0) && playerController.canShoot && floorGlobal.isOnBeat)
        {
            Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
            playerController.canShoot = false;
            //StartCoroutine(BulletDelay());
            animator.SetBool("hasShot", true);
            SendMessageUpwards("OnBeatAction");
            //akAnimator.SetBool("hasShot", false);
        }
        

    }
    //delay between bullets
    /*
    public IEnumerator BulletDelay()
    {
        playerController.canShoot = false;
        yield return new WaitForSeconds(shootDelay);
        playerController.canShoot = true;
    }
    */
}
