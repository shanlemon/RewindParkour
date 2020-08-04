using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{

    private bool isTiming = false;

    // Used to set the time in hours, minutes and sec format
    private const int hours = 216000;
    private const int minutes = 3600;
    private const int seconds = 60;

    public float PlayerTime { get; set; }

    public void StartTimer()
    {
        print("Timer started");
        isTiming = true;
        StartCoroutine(CoroutineTimer());
    }

    private IEnumerator CoroutineTimer()
    {
        PlayerTime = 0f;

        while (isTiming)
        {
            PlayerTime += Time.deltaTime;
            yield return null;
        }
    }

    public void StopTimer()
    {
        if (isTiming)
        {
            print("Timer stopped");
            isTiming = false;
            print($"Player time is: {PlayerTime}");
            PlayerTime = 0f;
        }
    }
}
