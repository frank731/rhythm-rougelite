using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnBeatRange : MonoBehaviour
{
    public FloorGlobal floorGlobal;
    private List<GameObject> beatMarkers = new List<GameObject>();

    private void Start()
    {
        Debug.Log("d");
    }
    public void BeatAction()
    {
        if (floorGlobal.isOnBeat)
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
        Debug.Log("s");
        floorGlobal.isOnBeat = true;
        floorGlobal.onBeat.Invoke();
        beatMarkers.Add(collision.gameObject);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        floorGlobal.isOnBeat = false;
        beatMarkers.RemoveAll(item => item == null);
        //destroy later to give bit of extra hang time for on beat
        foreach (GameObject indicator in beatMarkers)
        {
            Destroy(indicator, 1f);
        }
    }
}
