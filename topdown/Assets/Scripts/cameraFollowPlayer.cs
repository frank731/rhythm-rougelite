using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public Transform player;

    void Update()
    {
        transform.position = new Vector3(player.position.x, player.position.y, -10);
    }
}
