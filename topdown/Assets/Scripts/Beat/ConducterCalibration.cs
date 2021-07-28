using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConducterCalibration : BeatVisualiser
{
    public float calibrationOffset;
    private float diffSum = 0;
    private int clickCount = 0;
    private float diff;
    public TextMeshProUGUI offDisplay;
    public TextMeshProUGUI countDisplay;
    public GameObject finished;
    public MenuButtonController menuButtonController;

    protected override void Awake()
    {
        dspSongTime = (float)AudioSettings.dspTime;
        timeUntilNextBeat = 60f / currBPM;

        beatIndicator.transform.localScale = new Vector3(6, 6, 1);
        beatIndicatorMovement = beatIndicator.GetComponent<BeatIndicatorMovement>();
        beatIndicatorMovement.beatTime = timeUntilNextBeat;
        beatIndicatorMovement.beatMarker = transform;
        beatIndicatorMovement.audioSource = audioSource;
        //beatIndicator.transform.localScale = new Vector3(6, 6, 1);

        audioSource.Play();
    }
    private void Start()
    {
        objectPooler = ObjectPooler.SharedInstance;
        beatIndicatorIndex = objectPooler.AddObject(beatIndicator, 10);
    }
    protected override void Update()
    {
        songPos = (float)(AudioSettings.dspTime - dspSongTime) * audioSource.pitch - offset;
        if (songPos >= nextBeatTime)
        {
            GameObject indicatorLeft = objectPooler.GetPooledObject(beatIndicatorIndex, beatSpawnPointLeft);
            indicatorLeft.transform.SetParent(beatIndicatorHolder);
            GameObject indicatorRight = objectPooler.GetPooledObject(beatIndicatorIndex, beatSpawnPointRight);
            indicatorRight.transform.SetParent(beatIndicatorHolder);
            pastBeatTime = nextBeatTime;
            nextBeatTime += timeUntilNextBeat;
        }
    }
    public void ResetCalibration()
    {
        finished.SetActive(false);
        dspSongTime = (float)AudioSettings.dspTime;
        audioSource.Stop();
        audioSource.Play();
        clickCount = 0;
        diffSum = 0;
        pastBeatTime = 0;
        countDisplay.text = "0/30";
        nextBeatTime = timeUntilNextBeat;
        foreach(GameObject ind in objectPooler.GetAllPooledObjects(beatIndicatorIndex)){
            ind.SetActive(false);
        }
    }

    public void OnClick()
    {
        //Debug.Log(songPos);
        //Debug.Log(Mathf.Abs(nextBeatTime - songPos));
        //Debug.Log(Mathf.Abs(pastBeatTime - songPos));
        if (!finished.activeInHierarchy)
        {
            if (Mathf.Abs(nextBeatTime - songPos) > Mathf.Abs(pastBeatTime - songPos))
            {
                diff = pastBeatTime - songPos;
                //Debug.Log("past" + diff);
            }
            else
            {
                diff = nextBeatTime - songPos;
            }
            offDisplay.text = (diff * 1000).ToString("F0") + " ms";
            diffSum += diff;
            clickCount++;
            countDisplay.text = clickCount.ToString() + "/30";
            calibrationOffset = diffSum / clickCount;
            if (clickCount == 30)
            {
                ES3.Save("inputOffset", calibrationOffset);
                finished.GetComponent<OffsetDisplay>().UpdateRec();
                finished.SetActive(true);
            }
        }
    }
    public void OnCancel()
    {
        menuButtonController.OpenTitle();
    }
}
