using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float health;
    public float iFrames;
    public bool isActive;
    public bool isDead = false;
    public bool invincible = false;
    public List<MonoBehaviour> enemyScripts = new List<MonoBehaviour>();
    public List<GameObject> weapons = new List<GameObject>();
    public Transform player;
    public AudioClip enemyHurtSFX;
    private RoomController roomController;
    private AudioSource enemyAudioSource;
    protected Animator enemyAnimator;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        player.GetComponent<PlayerController>().playerKilled.AddListener(PlayerKilled);
        enemyAudioSource = GetComponent<AudioSource>();
        enemyAnimator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        isActive = false;
        StartCoroutine(SetActive());
    }

    private void Start()
    {
        roomController = transform.parent.parent.gameObject.GetComponent<RoomController>();
        roomController.AddEnemy(gameObject);
        //deactivate when player is not in its room
        if (!roomController.isPlayerIn)
        {
            gameObject.SetActive(false);
        }
    }
    public void RemoveHealth(float damage)
    {
        health -= damage;
        enemyAnimator.SetTrigger("Enemy Damaged");
        if (!enemyAudioSource.isPlaying) //stops stacking of sfx
        {
            enemyAudioSource.PlayOneShot(enemyHurtSFX, 0.3f);
        }
        if (health <= 0 && !isDead)
        {
            EnemyKilled();
            return;
        }
        StartCoroutine(IFrameDelay());
    }

    private void PlayerKilled()
    {
        foreach (MonoBehaviour script in enemyScripts)
        {
            script.enabled = false;
        }
    }

    private void EnemyKilled()
    {
        //destroys enemy once health is lower than zero and tells room that enemy has been destroyed
        isDead = true;
        isActive = false;
        roomController.EnemyDestroyed();
        enemyAnimator.SetTrigger("Enemy Killed");
        //disables movement and attack scripts
        foreach (MonoBehaviour script in enemyScripts)
        {
            script.enabled = false;
        }
        foreach (GameObject weapon in weapons)
        {
            weapon.SetActive(false);
        }
        GetComponent<Collider2D>().enabled = false;

    }

    private IEnumerator SetActive()
    {
        yield return new WaitForSeconds(0.5f);
        isActive = true;
    }

    private IEnumerator IFrameDelay()
    {
        invincible = true;
        yield return new WaitForSeconds(iFrames);
        invincible = false;
    }

}
