using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private float minVol, maxVol, maxVelocity;
    [SerializeField] private Sound musicSound;

    private Rigidbody rb;
    private void Start() {
        rb = Managers.Player.GetComponent<Rigidbody>();

        Managers.AudioManager.Play(musicSound.name);
    }
    private void Update() {
        Debug.Log(rb.velocity.magnitude);
        musicSound.source.volume = Mathf.Lerp(minVol, maxVol, rb.velocity.magnitude / maxVelocity);

    }


}
