using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    void Update()
    {
        transform.position = new Vector3(player.position.x, player.position.y, -10);
    }
}
