using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class RotateGun : MonoBehaviour
{
    [SerializeField] GrapplingGun gun = default;

    private Quaternion desiredRotation = default;
    private float rotationSpeed = 5f;

    private void Update()
    {
        if (!gun.IsGrappling())
        {
            desiredRotation = transform.parent.rotation;
        }
        else
        {
            desiredRotation = Quaternion.LookRotation(gun.GrapplePoint - transform.position);
        }

        transform.rotation = Quaternion.Lerp(a: transform.rotation, b: desiredRotation, t: Time.deltaTime * rotationSpeed); 

    }
}
