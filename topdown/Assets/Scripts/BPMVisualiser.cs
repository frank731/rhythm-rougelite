using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BPMVisualiser : MonoBehaviour
{
    private float baseBPM;
    public float currBPM;
    private float beatCreateTime;
    private float minBPM;
    private float maxBPM;
    private bool isPlaying = false;
    public AudioSource audioSource;
    public AudioSource audioTest;
    public GameObject beatIndicator;
    //private GameObject beatIndicatorLeft;
    private BeatIndicatorMovement beatIndicatorMovement;
    //private BeatIndicatorMovement beatIndicatorMovementLeft;
    public Transform beatIndicatorHolder;
    public Transform beatSpawnPointLeft;
    public Transform beatSpawnPointRight;
    public FloorGlobal floorGlobal;

    void OnTempoChange(bool isUp)
    {
        beatCreateTime = 1 / (currBPM / 60);
        if (isUp)
        {
            audioSource.pitch += 0.05f;
        }
        else
        {
            audioSource.pitch -= 0.05f;
        }
        audioSource.outputAudioMixerGroup.audioMixer.SetFloat("PitchShift", 1f / audioSource.pitch);
        //beatIndicatorMovementLeft.movementSpeed = currBPM * -1.5f;
        //beatIndicatorMovement.movementSpeed = currBPM * 1.5f;
        //beatIndicatorMovementLeft.beatCreateTime = beatCreateTime;
        beatIndicatorMovement.beatCreateTime = beatCreateTime;
    }

    void Awake()
    {
        baseBPM = BPMDetector.AnalyzeBpm(audioSource.clip);
        
        //prevents superfast creation
        while (baseBPM >= 240)
        {
            baseBPM /= 2;
        }
        currBPM = baseBPM;
        //calculates time before new beat tick is spawned
        beatCreateTime = 1 / (baseBPM / 60);

        minBPM = Mathf.Max(10, baseBPM - 30);
        maxBPM = Mathf.Min(240, baseBPM + 30);

        //beatIndicatorLeft = GameObject.Instantiate(beatIndicator) as GameObject;
        //beatIndicatorMovementLeft = beatIndicatorLeft.GetComponent<BeatIndicatorMovement>();
        beatIndicatorMovement = beatIndicator.GetComponent<BeatIndicatorMovement>();
        //beatIndicatorMovementLeft.movementSpeed = currBPM * -1.5f;
        //beatIndicatorMovement.movementSpeed = currBPM * 1.5f;
        //beatIndicatorMovementLeft.beatCreateTime = beatCreateTime;
        beatIndicatorMovement.beatCreateTime = beatCreateTime;
        beatIndicatorMovement.beatMarker = transform;

        StartCoroutine(BeatDelay());
        floorGlobal.pausableScripts.Add(this);
    }

    
    private void Update()
    {
        //changes tempo of song and shifts pitch using output mixer group
        if (Input.GetKeyDown("q"))
        {
            currBPM = Mathf.Clamp((currBPM - (audioSource.pitch * 7)), minBPM, maxBPM);
            if(currBPM > minBPM)
            {
                OnTempoChange(false);
            }
        }
        else if(Input.GetKeyDown("e"))
        {
            currBPM = Mathf.Clamp((currBPM + (audioSource.pitch * 7)), minBPM, maxBPM);
            if (maxBPM > currBPM)
            {
                OnTempoChange(true);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        audioTest.Play();
        if (!isPlaying)
        {
            audioSource.Play();
            isPlaying = true;
        }
    }
    private void OnDisable()
    {
        audioSource.Pause();
    }
    private void OnEnable()
    {
        //starts audio clip
        if (isPlaying)
        {
            audioSource.Play();
        }
    }
    public IEnumerator BeatDelay()
    {
        yield return new WaitForSeconds(beatCreateTime);
        GameObject indicatorLeft = Instantiate(beatIndicator, beatSpawnPointLeft.position, beatSpawnPointLeft.rotation);
        indicatorLeft.transform.SetParent(beatIndicatorHolder);
        GameObject indicatorRight = Instantiate(beatIndicator, beatSpawnPointRight.position, beatSpawnPointRight.rotation);
        indicatorRight.transform.SetParent(beatIndicatorHolder);
        StartCoroutine(BeatDelay());
    }
}
