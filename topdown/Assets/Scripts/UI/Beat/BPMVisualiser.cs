using System.Collections;
using UnityEngine;

public class BPMVisualiser : MonoBehaviour
{
    private float baseBPM;
    public float currBPM;
    private float beatCreateTime;
    private float minBPM;
    private float maxBPM;
    public float beatHangDivisor;
    private float beatHangTime;
    private float timeUntilNextBeat;
    private float startHangTime;
    private bool isPlaying = false;
    private bool startedHang = false;
    public AudioSource audioSource;
    public GameObject beatIndicator;
    private BeatIndicatorMovement beatIndicatorMovement;
    public Transform beatIndicatorHolder;
    public Transform beatSpawnPointLeft;
    public Transform beatSpawnPointRight;
    private float lastTime = 0f, deltaTime = 0f, timer = 0f, hangTimer = 0f;
    public PlayerController playerController;
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
        beatIndicatorMovement.beatCreateTime = beatCreateTime;
        beatHangTime = beatCreateTime / beatHangDivisor;
        timeUntilNextBeat = 60f / currBPM;
        startHangTime = timeUntilNextBeat - beatHangTime;
    }

    void Awake()
    {
        audioSource = Camera.main.GetComponent<AudioSource>();
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

        beatIndicatorMovement = beatIndicator.GetComponent<BeatIndicatorMovement>();
        beatIndicatorMovement.beatCreateTime = beatCreateTime;
        beatIndicatorMovement.beatMarker = transform;

        beatHangTime = beatCreateTime / beatHangDivisor;
        timeUntilNextBeat = 60f / currBPM;
        startHangTime = timeUntilNextBeat - beatHangTime;

        //StartCoroutine(BeatHangDelay());
        FloorGlobal.Instance.pausableScripts.Add(this);
    }


    private void Update()
    {
        //changes tempo of song and shifts pitch using output mixer group
        if (Input.GetKeyDown("q"))
        {
            currBPM = Mathf.Clamp((currBPM - (audioSource.pitch * 7)), minBPM, maxBPM);
            if (currBPM > minBPM)
            {
                OnTempoChange(false);
            }
        }
        else if (Input.GetKeyDown("e"))
        {
            currBPM = Mathf.Clamp((currBPM + (audioSource.pitch * 7)), minBPM, maxBPM);
            if (maxBPM > currBPM)
            {
                OnTempoChange(true);
            }
        }

        deltaTime = audioSource.time - lastTime;
        timer += deltaTime;
        hangTimer += deltaTime;
        
        if (timer >= startHangTime && !startedHang)
        {
            FloorGlobal.Instance.isOnBeat = true;
            FloorGlobal.Instance.startBeat.Invoke();
            startedHang = true;
            //Invoke("test", beatHangTime * 3);
        }
        
        if (timer >= timeUntilNextBeat)
        {
            //Create the note
            FloorGlobal.Instance.onBeat.Invoke();
            GameObject indicatorLeft = Instantiate(beatIndicator, beatSpawnPointLeft.position, beatSpawnPointLeft.rotation);
            indicatorLeft.transform.SetParent(beatIndicatorHolder);
            GameObject indicatorRight = Instantiate(beatIndicator, beatSpawnPointRight.position, beatSpawnPointRight.rotation);
            indicatorRight.transform.SetParent(beatIndicatorHolder);
            timer -= timeUntilNextBeat;
        }
        
        if (startedHang && hangTimer >= timeUntilNextBeat + beatHangTime * 3)
        {
            FloorGlobal.Instance.isOnBeat = false;
            hangTimer -= timeUntilNextBeat;
            startedHang = false;
        }
        
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
    /*
    public IEnumerator BeatHangDelay()
    {
        
        yield return beatHangStartDelay;
        //hang time before actual beat
        FloorGlobal.Instance.isOnBeat = true;

        yield return beatHangDelay;
        //actually on beat

        GameObject indicatorLeft = Instantiate(beatIndicator, beatSpawnPointLeft.position, beatSpawnPointLeft.rotation);
        indicatorLeft.transform.SetParent(beatIndicatorHolder);
        GameObject indicatorRight = Instantiate(beatIndicator, beatSpawnPointRight.position, beatSpawnPointRight.rotation);
        indicatorRight.transform.SetParent(beatIndicatorHolder);
        FloorGlobal.Instance.onBeat.Invoke();

        StartCoroutine(BeatHangTime());
        
        while (true)
        {
            t = Time.timeSinceLevelLoad;
            yield return beatHangStartDelay;
            //hang time before actual beat
            FloorGlobal.Instance.isOnBeat = true;
            
            yield return beatHangDelay;
            //actually on beat
            
            indicatorLeft = Instantiate(beatIndicator, beatSpawnPointLeft.position, beatSpawnPointLeft.rotation);
            indicatorLeft.transform.SetParent(beatIndicatorHolder);
            indicatorRight = Instantiate(beatIndicator, beatSpawnPointRight.position, beatSpawnPointRight.rotation);
            indicatorRight.transform.SetParent(beatIndicatorHolder);
            FloorGlobal.Instance.onBeat.Invoke();

            float diff = (Time.timeSinceLevelLoad - t);
            Debug.Log(diff);
            Debug.Log(diff - beatCreateTime);
            beatHangStartDelay = new WaitForSeconds(beatCreateTime - (beatCreateTime / beatHangDivisor) - (diff - beatCreateTime) / 2);
            Debug.Log(beatCreateTime - (beatCreateTime / beatHangDivisor));
            Debug.Log(beatCreateTime - (beatCreateTime / beatHangDivisor) - (diff - beatCreateTime));
            //Debug.Log(Time.timeSinceLevelLoad - t);
            StartCoroutine(BeatHangTime());
        }
    }

    private IEnumerator BeatHangTime()
    {
        yield return beatHangDelay;
        //hang time after beat
        Debug.Log(Time.timeSinceLevelLoad - t + " " + FloorGlobal.Instance.isOnBeat);
        FloorGlobal.Instance.isOnBeat = false;
    }
    */
}
