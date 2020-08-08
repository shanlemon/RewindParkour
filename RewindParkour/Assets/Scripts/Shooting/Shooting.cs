using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private float damage = 50f, range = 100f, impactForce = 30f, fireRate = 15f;

    [SerializeField] private GrapplingGun grapplingGun;
    [SerializeField] private Transform camera;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private Sound gunshotSound;
    [SerializeField] private Animator anim;
    [Header("Camera Shake")]
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeMagnitude = 1f;
    [SerializeField] private float recoilForce = 100f;
    

    private float nextTimeToFire = .25f;
    private Rigidbody playerRB;

    private void Start() {
        playerRB = Managers.Player.GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        if(Managers.PauseMenu.SettingsOpen) return;
        if (!grapplingGun.IsGrappling() && Input.GetMouseButtonDown(0) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f/fireRate;
            Shoot();
        }
    }

    private void Shoot()
    {
        Debug.Log("SHOOT");
        muzzleFlash.Play();
        anim.SetTrigger("Gunshot");
        playerRB.AddForce(-transform.forward * recoilForce);
        StartCoroutine(CameraShake.Instance.Shake(shakeDuration, shakeMagnitude));
        Managers.AudioManager.PlayOneShot(gunshotSound.name);
        RaycastHit hit;
        if (Physics.Raycast(camera.position, camera.forward, out hit, range))
        {
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            GameObject impact =  Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impact, 2f);
        }
    }
}
