using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPad : MonoBehaviour
{
    [SerializeField] private bool isStartPad = default;
    [SerializeField] private TriggerPad target = default;

    public TriggerPad TargetPad => target;

    private void OnTriggerExit(Collider other)
    {
        // Player has left a starting trigger pad
        if (other.CompareTag("Player") && isStartPad)
        {
            Managers.GameManager.CurrTarget = target;
            Managers.TimeManager.StartTimer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Player has entered an end trigger pad
        if (other.CompareTag("Player") && this.Equals(Managers.GameManager.CurrTarget))
        {
            Managers.GameManager.CurrTarget = null;
            Managers.TimeManager.StopTimer();
        }
    }
}
