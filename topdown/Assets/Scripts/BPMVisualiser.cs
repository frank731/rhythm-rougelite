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
    public AudioClip bgm;
    public AudioSource audioSource;
    public GameObject beatIndicator;
    public Transform beatIndicatorHolder;
    public Transform beatCreatePoint;
    void Awake()
    {
        baseBPM = BPMDetector.AnalyzeBpm(bgm);
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
        StartCoroutine(BeatDelay());
    }

    public IEnumerator BeatDelay()
    {
        yield return new WaitForSeconds(beatCreateTime);
        GameObject indicator = Instantiate(beatIndicator, beatCreatePoint.position, beatCreatePoint.rotation);
        indicator.transform.SetParent(beatIndicatorHolder);
        StartCoroutine(BeatDelay());
    }
    private void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            currBPM = Mathf.Clamp((currBPM - (audioSource.pitch * 5)), minBPM, maxBPM);
            beatCreateTime = 1 / (currBPM / 60);
            if(currBPM > minBPM)
            {
                audioSource.pitch -= 0.05f;
                audioSource.outputAudioMixerGroup.audioMixer.SetFloat("PitchShift", 1f / audioSource.pitch);
            }
        }
        else if(Input.GetKeyDown("e"))
        {
            currBPM = Mathf.Clamp((currBPM + (audioSource.pitch * 5)), minBPM, maxBPM);
            beatCreateTime = 1 / (currBPM / 60);
            if (maxBPM > currBPM)
            {
                audioSource.pitch += 0.05f;
                audioSource.outputAudioMixerGroup.audioMixer.SetFloat("PitchShift", 1f / audioSource.pitch);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isPlaying)
        {
            audioSource.Play();
            isPlaying = true;
        }
    }
}
