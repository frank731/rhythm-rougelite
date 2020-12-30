using UnityEngine;

public class PickupController : MonoBehaviour
{
    private PlayerController playerController;
    public int count;

    private void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    public void AddHealth()
    {
        playerController.AddHealth(count);
        Destroy(gameObject);
    }

}
