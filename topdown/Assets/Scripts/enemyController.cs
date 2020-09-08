using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float health;
    public bool isActive;
    public bool isDead = false;
    public Transform player;
    private RoomController roomControllerHolder;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void OnEnable()
    {
        isActive = false;
        StartCoroutine(SetActive());
    }
    
    private void Start()
    {
        roomControllerHolder = transform.parent.parent.gameObject.GetComponent<RoomController>();
        roomControllerHolder.AddEnemy(gameObject);
        //deactivate when player is not in its room
        if (!roomControllerHolder.isPlayerIn)
        {
            gameObject.SetActive(false);
        }
    }
    public void RemoveHealth(float damage)
    {
        health -= damage;
        if (health <= 0 && !isDead)
        {
            //destroys enemy once health is lower than zero and tells room that enemy has been destroyed
            isDead = true;
            Destroy(gameObject);
            roomControllerHolder.EnemyDestroyed();
            return;
        }
    }
    IEnumerator SetActive()
    {
        yield return new WaitForSeconds(0.5f);
        isActive = true;
    }
}
