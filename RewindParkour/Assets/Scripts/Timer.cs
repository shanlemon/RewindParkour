using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeDisplay = default;

    private bool isTiming = false;

    // Used to set the time in hours, minutes and sec format
    private const int hours = 216000;
    private const int minutes = 3600;
    private const int seconds = 60;

    public float PlayerTime { get; set; }

    public void StartTimer()
    {
        // Don't want to start multiple timers
        if (!isTiming)
        {
            StartCoroutine(CoroutineTimer());
        }
    }

    private IEnumerator CoroutineTimer()
    {
        isTiming = true;
        while (isTiming)
        {
            PlayerTime += Time.deltaTime;

            // Change time display
            string hoursText = Mathf.Floor((PlayerTime % hours )/ minutes).ToString("00");
            string minutesText = Mathf.Floor((PlayerTime % minutes) / seconds).ToString("00");
            string secondsText = (PlayerTime % seconds).ToString("00");
            timeDisplay.text = hoursText + ":" + minutesText + ":" + secondsText;

            yield return null;
        }
    }

    public void StopTimer()
    {
        // Only stop timing if we're already timing
        if (isTiming)
        {
            isTiming = false;
            PlayerTime = 0f;
            timeDisplay.text = "00:00:00";
        }
    }
}
