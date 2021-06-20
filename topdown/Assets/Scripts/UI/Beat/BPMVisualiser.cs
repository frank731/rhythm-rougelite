using System.Collections;
using UnityEngine;

public class BPMVisualiser : Singleton<BPMVisualiser>
{
    public float offset = 0.02f;
    public float baseBPM;
    public float currBPM;
    private float minBPM;
    private float maxBPM;
    public float beatHangTime = 0.33f;
    public float adjustmentTime;
    public float pastBeatTime = 0;
    public float nextBeatTime = 0;
    public float dspSongTime;
    public float inputOffset;
    public float songPos;
    protected float timeUntilNextBeat;
    //private float baseTimeUntilNextBeat;
    //private float startHangTime;
    private bool isPlaying = false;
    //private bool startedHang = false;
    public AudioSource audioSource;
    public GameObject beatIndicator;
    protected BeatIndicatorMovement beatIndicatorMovement;
    protected ObjectPooler objectPooler;
    protected int beatIndicatorIndex;
    public Transform beatIndicatorHolder;
    public Transform beatSpawnPointLeft;
    public Transform beatSpawnPointRight;
    public PlayerController playerController;
    public AudioClip metronome;

    private FloorGlobal floorGlobal;

    void OnTempoChange(bool isUp)
    {
        //beatCreateTime = 1 / (currBPM / 60);
        if (isUp)
        {
            audioSource.pitch += 0.05f;
        }
        else
        {
            audioSource.pitch -= 0.05f;
        }
        currBPM = baseBPM * audioSource.pitch;
        audioSource.outputAudioMixerGroup.audioMixer.SetFloat("PitchShift", 1f / audioSource.pitch);
        //timeUntilNextBeat = baseTimeUntilNextBeat * audioSource.pitch;
        //beatIndicatorMovement.beatTime = timeUntilNextBeat;
        //timeUntilNextBeat = 60f / currBPM;
        //startHangTime = timeUntilNextBeat - beatHangTime;
    }

    protected override void Awake()
    {
        base.Awake();
        floorGlobal = FloorGlobal.Instance;
        floorGlobal.bpmVisualiser = this;
        audioSource = Camera.main.GetComponent<AudioSource>();
        baseBPM = BPMDetector.AnalyzeBpm(audioSource.clip);
        dspSongTime = (float)AudioSettings.dspTime;
        audioSource.Play();

        objectPooler = ObjectPooler.SharedInstance;
        Debug.Log(objectPooler.name);
        //prevents superfast creation
        while (baseBPM > 180)
        {
            baseBPM /= 2;
        }
        currBPM = baseBPM;
        timeUntilNextBeat = 60f / currBPM;
        //baseTimeUntilNextBeat = timeUntilNextBeat;
        //calculates time before new beat tick is spawned
        //beatCreateTime = 1 / (baseBPM / 60);

        minBPM = Mathf.Max(10, baseBPM - 60);
        maxBPM = Mathf.Min(240, baseBPM + 60);

        beatIndicator.transform.localScale = new Vector3(1, 1, 1);
        beatIndicatorMovement = beatIndicator.GetComponent<BeatIndicatorMovement>();
        beatIndicatorMovement.beatTime = timeUntilNextBeat;
        beatIndicatorMovement.beatMarker = transform;
        beatIndicatorMovement.audioSource = audioSource;

        beatIndicatorIndex = objectPooler.AddObject(beatIndicator, 4);
        //startHangTime = timeUntilNextBeat - beatHangTime;
        inputOffset = ES3.Load("inputOffset", 0f);
        //StartCoroutine(BeatHangDelay());
        floorGlobal.pausableScripts.Add(this);
    }


    protected virtual void Update()
    {
        songPos = (float)(AudioSettings.dspTime - dspSongTime) * audioSource.pitch - offset;
        //changes tempo of song and shifts pitch using output mixer group
        if (Input.GetKeyDown("q"))
        {
            // currBPM = Mathf.Clamp((currBPM - (audioSource.pitch * 7)), minBPM, maxBPM);
            if (currBPM > minBPM) 
            {
                OnTempoChange(false);
            }
        }
        else if (Input.GetKeyDown("e"))
        {
            //currBPM = Mathf.Clamp((currBPM + (audioSource.pitch * 7)), minBPM, maxBPM);
            if (maxBPM > currBPM)
            {
                OnTempoChange(true);
            }
        }
        if (songPos >= nextBeatTime)
        {
            //Create the note
            floorGlobal.onBeat.Invoke();
            floorGlobal.beatNumber++;
            GameObject indicatorLeft = objectPooler.GetPooledObject(beatIndicatorIndex, beatSpawnPointLeft);
            indicatorLeft.transform.SetParent(beatIndicatorHolder);
            GameObject indicatorRight = objectPooler.GetPooledObject(beatIndicatorIndex, beatSpawnPointRight);
            indicatorRight.transform.SetParent(beatIndicatorHolder);
            //timer = 0;
            pastBeatTime = nextBeatTime;
            nextBeatTime += timeUntilNextBeat;// - timer;
            audioSource.PlayOneShot(metronome, 0.5f);
            //Debug.Log(songPos);
            //Debug.Log(pastBeatTime);
            //Debug.Log(nextBeatTime);
            //Debug.Log(audioSource.time);
        }
        /*
        if (startedHang && hangTimer >= timeUntilNextBeat + beatHangTime * 3)
        {
            floorGlobal.isOnBeat = false;
            hangTimer -= timeUntilNextBeat;
            startedHang = false;
        }
        */
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
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
}
