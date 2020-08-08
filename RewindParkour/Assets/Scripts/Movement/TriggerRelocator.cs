using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerRelocator : MonoBehaviour
{
    [SerializeField] private Vector3 position;

    private void OnTriggerEnter(Collider other) {
        if (other.tag.Equals("Player"))
        {
            other.GetComponent<StrafeMovement>().StopCrouch();
            other.transform.localPosition = position;
        }
    }
}
