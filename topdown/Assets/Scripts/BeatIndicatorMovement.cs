using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeatIndicatorMovement : KinematicFunctions
{
    public Image image;
    public float beatCreateTime;
    public Transform beatMarker;
    private void Start()
    {
        StartCoroutine(MoveObject(transform, transform.localPosition, beatMarker.localPosition, beatCreateTime));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        if (collision.tag == "BPMIndicator")
        {
            Destroy(image);
            Destroy(gameObject, beatCreateTime / 10);
        }
    }
}
