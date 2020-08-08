using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeDisplay = default;
    [SerializeField] private int numLevels = 3;

    private bool isTiming = false;
    private string allTimesKey = "AllTimes";

    // Used to set the time in hours, minutes and sec format
    private const int hours = 216000;
    private const int minutes = 3600;
    private const int seconds = 60;


    public float PlayerTime { get; set; }
    public float[] AllTimes { get; set;}

    private void Start()
    {
        AllTimes = PlayerPrefsX.GetFloatArray(allTimesKey, 0f, numLevels);

        // AllTimes doesn't have the right number of levels so make a new array
        if (AllTimes.Length != numLevels)
        {
            float[] temp = new float[numLevels];
            for (int i = 0; i < AllTimes.Length; i++)
            {
                temp[i] = AllTimes[i];
            }
        }

        foreach(float time in AllTimes)
        {
            print($"Time: {time}");
        }
    }

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

    public void StopTimer(int section, TriggerPad triggerPad)
    {
        // Only stop timing if we're already timing
        if (isTiming)
        {
            isTiming = false;
            if (PlayerTime < AllTimes[section])
            {
                AllTimes[section] = PlayerTime;
                PlayerPrefsX.SetFloatArray(allTimesKey, AllTimes);
            }
            triggerPad.DisplayTime(PlayerTime);
            PlayerTime = 0f;
            timeDisplay.text = "00:00:00";
        }
    }
}
