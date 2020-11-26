using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnBeatRange : MonoBehaviour
{
    private List<GameObject> beatMarkers = new List<GameObject>();

    public void BeatAction()
    {
        if (FloorGlobal.Instance.isOnBeat)
        {
            beatMarkers.RemoveAll(item => item == null);
            foreach (GameObject indicator in beatMarkers)
            {
                Destroy(indicator.GetComponent<Image>());
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        beatMarkers.Add(collision.gameObject);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        FloorGlobal.Instance.isOnBeat = false;
        beatMarkers.RemoveAll(item => item == null);
        foreach (GameObject indicator in beatMarkers)
        {
            Destroy(indicator);
        }
    }
}
