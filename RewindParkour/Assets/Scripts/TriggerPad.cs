using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TriggerPad : MonoBehaviour
{
    [SerializeField] private bool isStartPad = default;
    [SerializeField] private TriggerPad target = default;
    [SerializeField] private int section = default; // Section this triggerpad is associated with.
    [SerializeField] private TextMeshProUGUI timeDisplay = default; // The sign that you want the time to display on

    public TriggerPad TargetPad => target;
    public int Section => section;

    private void Start()
    {
        if(timeDisplay != null)
        {
            timeDisplay.text = "00:00:00";
        }
    }

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
            Managers.TimeManager.StopTimer(section, this);

        }
    }

    /*
     * Displays the time for this section on the sign that the trigger pad is 
     * associated with
     */
    public void DisplayTime(float time)
    {
        timeDisplay.text = time.ToString();
    }
}
