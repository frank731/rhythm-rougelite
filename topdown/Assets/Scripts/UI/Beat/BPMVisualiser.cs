using System.Collections;
using UnityEngine;

public class BPMVisualiser : MonoBehaviour
{
    public float offset = 0.02f;
    public float songPosition;
    private float baseBPM;
    public float currBPM;
    private float minBPM;
    private float maxBPM;
    public float beatHangTime = 0.33f;
    public float adjustmentTime;
    public float pastBeatTime = 0;
    public float nextBeatTime = 0;
    public float currentBeatTime = 0;
    private float timeUntilNextBeat;
    private float baseTimeUntilNextBeat;
    //private float startHangTime;
    private bool isPlaying = false;
    //private bool startedHang = false;
    public AudioSource audioSource;
    public GameObject beatIndicator;
    private BeatIndicatorMovement beatIndicatorMovement;
    public Transform beatIndicatorHolder;
    public Transform beatSpawnPointLeft;
    public Transform beatSpawnPointRight;
    private float lastTime = 0f, deltaTime = 0f, timer = 0f;
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
        audioSource.outputAudioMixerGroup.audioMixer.SetFloat("PitchShift", 1f / audioSource.pitch);
        timeUntilNextBeat = baseTimeUntilNextBeat * audioSource.pitch;
        beatIndicatorMovement.beatCreateTime = timeUntilNextBeat;
        //timeUntilNextBeat = 60f / currBPM;
        //startHangTime = timeUntilNextBeat - beatHangTime;
    }

    void Awake()
    {
        floorGlobal = FloorGlobal.Instance;
        audioSource = Camera.main.GetComponent<AudioSource>();
        baseBPM = BPMDetector.AnalyzeBpm(audioSource.clip);
        timer -= offset;
        //prevents superfast creation
        while (baseBPM > 180)
        {
            baseBPM /= 2;
        }
        currBPM = baseBPM;
        timeUntilNextBeat = 60f / currBPM;
        //calculates time before new beat tick is spawned
        //beatCreateTime = 1 / (baseBPM / 60);

        minBPM = Mathf.Max(10, baseBPM - 30);
        maxBPM = Mathf.Min(240, baseBPM + 30);

        beatIndicatorMovement = beatIndicator.GetComponent<BeatIndicatorMovement>();
        beatIndicatorMovement.beatCreateTime = timeUntilNextBeat;
        beatIndicatorMovement.beatMarker = transform;
        beatIndicatorMovement.audioSource = audioSource;
        //startHangTime = timeUntilNextBeat - beatHangTime;

        //StartCoroutine(BeatHangDelay());
        floorGlobal.pausableScripts.Add(this);
        floorGlobal.bpmVisualiser = this;
    }


    private void FixedUpdate()
    {
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
        /*
        if (timer >= startHangTime && !startedHang)
        {
            floorGlobal.isOnBeat = true;
            floorGlobal.startBeat.Invoke();
            startedHang = true;
            //Invoke("test", beatHangTime * 3);
        }
        */
        if (audioSource.time >= nextBeatTime)//timer >= timeUntilNextBeat)
        {
            //Create the note
            floorGlobal.onBeat.Invoke();
            floorGlobal.beatNumber++;
            GameObject indicatorLeft = Instantiate(beatIndicator, beatSpawnPointLeft.position, beatSpawnPointLeft.rotation);
            indicatorLeft.transform.SetParent(beatIndicatorHolder);
            GameObject indicatorRight = Instantiate(beatIndicator, beatSpawnPointRight.position, beatSpawnPointRight.rotation);
            indicatorRight.transform.SetParent(beatIndicatorHolder);
            timer -= timeUntilNextBeat;
            //timer = 0;
            pastBeatTime = nextBeatTime;
            nextBeatTime += timeUntilNextBeat;// - timer;
            audioSource.PlayOneShot(metronome, 0.5f);
            //Debug.Log(pastBeatTime);
            //Debug.Log(nextBeatTime);
            //Debug.Log(audioSource.time);
        }
        if(audioSource.time >= nextBeatTime - beatHangTime)
        {
            currentBeatTime = nextBeatTime;
        }
        /*
        if (startedHang && hangTimer >= timeUntilNextBeat + beatHangTime * 3)
        {
            floorGlobal.isOnBeat = false;
            hangTimer -= timeUntilNextBeat;
            startedHang = false;
        }
        */
        lastTime = audioSource.time;
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
