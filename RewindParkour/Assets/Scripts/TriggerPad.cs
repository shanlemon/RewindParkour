using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPad : MonoBehaviour
{
    [SerializeField] private bool isEndPad = default;

    private void OnTriggerExit(Collider other)
    {
        // Player has left a starting trigger pad
        if (other.CompareTag("Player") && !isEndPad)
        {
            Managers.TimeManager.StartTimer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Player has entered an end trigger pad
        if (other.CompareTag("Player") && isEndPad)
        {
            Managers.TimeManager.StopTimer();
        }
    }
}
