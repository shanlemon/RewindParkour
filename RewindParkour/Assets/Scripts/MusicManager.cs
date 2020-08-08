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
    }
    private void Update() {
        musicSound.source.volume = Mathf.Lerp(minVol, maxVol, rb.velocity.magnitude / maxVelocity);

    }

    public void StartMusic()
    {
        Managers.AudioManager.Play(musicSound.name);
    }


}
