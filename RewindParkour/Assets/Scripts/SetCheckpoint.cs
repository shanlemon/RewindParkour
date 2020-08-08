using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCheckpoint : MonoBehaviour
{
    private TeleportToOnKeyPress checkpoint = default;
    [SerializeField] private Transform location = default;

    private void Start()
    {
        checkpoint = Managers.Player.GetComponent<TeleportToOnKeyPress>();
    }

    public void OnTriggerEnter(Collider other)
    {
        checkpoint.SetCheckpoint(location.position);
        Destroy(gameObject);
    }
}
