using System.Collections;
using UnityEngine;

public class RandomEnemyMovement : MonoBehaviour
{
    public Transform enemyGun;

    public float speed = 1f;

    public bool canMove = true;
    public float moveDelay;

    public float moveDistance;
    public Vector3 moveDirection;

    public Rigidbody2D rb;

    private EnemyController enemyController;

    [SerializeField]
    private ParticleSystem walkParticles;

    private void Awake()
    {
        //initalize values for random values
        moveDelay = Random.Range(2f, 4f);
        moveDistance = Random.Range(1f, 3f);
        moveDirection = Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.forward) * Vector3.up;
        enemyController = GetComponent<EnemyController>();
        enemyController.enemyScripts.Add(this);
    }
    void FixedUpdate()
    {
        //move the enemy in a random direction and distance at random intervals
        if (canMove == true && enemyController.isActive)
        {
            StartCoroutine(WaitOnSpot());
            rb.velocity = moveDirection * speed;
            walkParticles.Play();
        }
    }
    public IEnumerator WaitOnSpot()
    {
        canMove = false;
        yield return new WaitForSeconds(moveDelay);
        walkParticles.Stop();
        //set random distances and times
        moveDelay = Random.Range(2f, 4f);
        moveDistance = Random.Range(1f, 3f);
        moveDirection = Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.forward) * Vector3.up;
        canMove = true;
    }
}
