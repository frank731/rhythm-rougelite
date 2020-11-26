using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float baseSpeed = 3f;
    public float speed;
    public Rigidbody2D rb;
    public Animator animator;
    private Vector2 direction;
    private PlayerController playerController;
    private void Awake()
    {
        speed = baseSpeed;
        playerController = GetComponent<PlayerController>();
        playerController.resetStats.AddListener(ResetStats);
        FloorGlobal.Instance.levelChanged.AddListener(SaveSpeed);
        playerController.loadPlayerData.AddListener(LoadSpeed);
    }

    private void ResetStats()
    {
        speed = baseSpeed;
    }

    private void SaveSpeed()
    {
        ES3.Save("playerSpeed", speed);
    }

    private void LoadSpeed()
    {
        speed = ES3.Load("playerSpeed", baseSpeed);
    }

    private void FixedUpdate()
    {
        direction.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //move the character
        rb.velocity = (direction * speed * Time.deltaTime * 50);
        // change idle to run anim
        animator.SetFloat("player_speed", Mathf.Max(Mathf.Abs(rb.velocity.y), Mathf.Abs(rb.velocity.x)));
    }
}
