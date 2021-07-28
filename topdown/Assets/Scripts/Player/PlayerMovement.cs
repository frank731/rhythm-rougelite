using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool canMove = true;
    public float baseSpeed = 3f;
    public float dashSpeed = 100f;
    public float dashTime = 0.1f;
    public float dashTimeLeft;
    public float speed;
    public Rigidbody2D rb;
    public Animator animator;

    private Vector2 direction;
    private PlayerController playerController;
    [SerializeField]
    private ParticleSystem walkParticles;
    private Vector2 ZEROVECTOR = new Vector2(0, 0);
    private void Awake()
    {
        speed = baseSpeed;
        playerController = GetComponent<PlayerController>();
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

    private void CheckDash()
    {
        if (playerController.isDashing)
        {
            if(dashTimeLeft > 0)
            {
                canMove = false;
                direction = new Vector2((direction.x >= 0) ? Mathf.Ceil(direction.x) : Mathf.Floor(direction.x), (direction.y >= 0) ? Mathf.Ceil(direction.y) : Mathf.Floor(direction.y));
                if(direction.x != 0 && direction.y != 0) //check if diagonal
                {
                    rb.velocity = dashSpeed * direction / 2;
                }
                else
                {
                    if (direction.x == 0 && direction.y == 0) direction.Set(1, 0);
                    rb.velocity = dashSpeed * direction;
                }
                dashTimeLeft -= Time.deltaTime;
            }
            if(dashTimeLeft <= 0)
            {
                playerController.isDashing = false;
                playerController.isInvincible = false;
                canMove = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            direction.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if(direction == ZEROVECTOR)
            {
                if (walkParticles.isPlaying)
                {
                    walkParticles.Stop();
                }
            }
            else if (!walkParticles.isPlaying)
            {
                walkParticles.Play();
            }
            //move the character
            rb.velocity = (direction * speed * Time.deltaTime * 50);
            // change idle to run anim
            animator.SetFloat("player_speed", Mathf.Max(Mathf.Abs(rb.velocity.y), Mathf.Abs(rb.velocity.x)));
        }
        CheckDash();
        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            playerController.KillPlayer();
        }
    }
}
