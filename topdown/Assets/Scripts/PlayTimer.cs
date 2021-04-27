using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayTimer : MonoBehaviour
{
    public float playTime;
    public TextMeshProUGUI timeText;

    private string hours, minutes, seconds;
    private WaitForSeconds updateDelay = new WaitForSeconds(1f);

    private void Start()
    {
        StartCoroutine(UpdateText());
    }

    void Update()
    {
        playTime += Time.deltaTime;
    }

    private IEnumerator UpdateText()
    {
        yield return updateDelay;
        hours = Mathf.Floor((playTime % 216000) / 3600).ToString("00");
        minutes = Mathf.Floor((playTime % 3600) / 60).ToString("00");
        seconds = (playTime % 60).ToString("00");
        timeText.text = "Time: " + hours + ":" + minutes + ":" + seconds;
        StartCoroutine(UpdateText());
    }
}
