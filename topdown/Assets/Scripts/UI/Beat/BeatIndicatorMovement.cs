using UnityEngine;
using UnityEngine.UI;

public class BeatIndicatorMovement : MonoBehaviour { 

    public Image image;
    public float beatCreateTime;
    public Transform beatMarker;
    private void Start()
    {
        StartCoroutine(KinematicFunctions.MoveObject(transform, transform.localPosition, beatMarker.localPosition, beatCreateTime));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("BPMIndicator"))
        {
            Destroy(gameObject);
        }
    }
}
