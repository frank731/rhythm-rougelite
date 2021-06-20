using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnBeatRange : MonoBehaviour
{
    private List<GameObject> beatMarkers = new List<GameObject>();
    private FloorGlobal floorGlobal;

    private void Start()
    {
        floorGlobal = FloorGlobal.Instance;
    }

    public void BeatAction()
    {

        beatMarkers.RemoveAll(item => item == null);
        foreach (GameObject indicator in beatMarkers)
        {
            indicator.SetActive(false);
            //Destroy(indicator.GetComponent<Image>());
        }
        
    }
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        beatMarkers.Add(collision.gameObject);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //FloorGlobal.Instance.isOnBeat = false;
        beatMarkers.RemoveAll(item => item == null);
        foreach (GameObject indicator in beatMarkers)
        {
            //indicator.SetActive(false);
        }
    }
    */
}
