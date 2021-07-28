using UnityEngine;
using UnityEngine.UI;

public class BeatIndicatorMovement : MonoBehaviour { 

    public Image image;
    public float beatTime;
    public Transform beatMarker;
    public AudioSource audioSource;

    public void StartMove()
    {
        StartCoroutine(KinematicFunctions.MoveObjectAudioSynced(transform, transform.localPosition, beatMarker.localPosition, beatTime, BeatVisualiser.Instance));
    }
    /*
    private void OnEnable()
    {
        StartCoroutine(KinematicFunctions.MoveObjectAudioSynced(transform, transform.localPosition, beatMarker.localPosition, beatTime, BeatVisualiser.Instance));
    }
    */
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("BPMIndicator"))
        {
            //transform.SetParent(ObjectPooler.SharedInstance.transform);
            gameObject.SetActive(false);
        }
    }
}
