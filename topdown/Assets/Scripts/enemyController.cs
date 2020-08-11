using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyController : MonoBehaviour
{
    public float health;
    public bool isActive;
    public Transform player;
    private roomController roomControllerHolder;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void OnEnable()
    {
        isActive = false;
        StartCoroutine(setActive());
    }
    
    private void Start()
    {
        roomControllerHolder = transform.parent.parent.gameObject.GetComponent<roomController>();
        //Debug.Log(roomControllerHolder.name);
        roomControllerHolder.AddEnemy(gameObject);
        //deactivate when player is not in its room
        if (!roomControllerHolder.isPlayerIn)
        {
            gameObject.SetActive(false);
        }
    }
    public void removeHealth(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            //destroys enemy once health is lower than zero and tells room that enemy has been destroyed
            Destroy(gameObject);
            roomControllerHolder.EnemyDestroyed();
        }
    }
    IEnumerator setActive()
    {
        yield return new WaitForSeconds(0.5f);
        isActive = true;
    }
}
