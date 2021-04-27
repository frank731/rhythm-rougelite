using UnityEngine;
using UnityEngine.UI;

public class BeatIndicatorMovement : MonoBehaviour { 

    public Image image;
    public float beatCreateTime;
    public Transform beatMarker;
    public AudioSource audioSource;
    public AudioClip test;
    private void Start()
    {
        StartCoroutine(KinematicFunctions.MoveObject(transform, transform.localPosition, beatMarker.localPosition, beatCreateTime));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("BPMIndicator"))
        {
            //Debug.Log(audioSource.time);
            Destroy(gameObject);
        }
    }
}
