using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    [SerializeField] private LayerMask whatIsGrappleable = default;
    [SerializeField] private Transform gunTip = default;
    [SerializeField] private Transform camera = default;
    [SerializeField] private Transform player = default;
    [SerializeField] private float maxDistance = 100f;
    [SerializeField] private float force = default;

    private Vector3 grapplePoint = default;
    private SpringJoint joint = default;
    

    // Getters
    public Vector3 GrapplePoint => grapplePoint;
    public Transform GunTip => gunTip;

    private void Update()
    { 

        if (Input.GetMouseButtonDown(1))
        {
            StartGrapple();
        }

        else if (Input.GetMouseButtonUp(1))
        {
            StopGrapple();
        }
    }

    /// <summary>
    /// Called whenever we want to start a grapple
    /// </summary>
    private void StartGrapple()
    {

        RaycastHit hit;

        if (Physics.Raycast(origin: camera.position, direction: camera.forward, out hit, maxDistance, whatIsGrappleable))
        {
            hit.collider.attachedRigidbody?.AddForce((hit.collider.transform.position - transform.position) * force);

            Managers.AudioManager.PlayOneShot("GrappleShot");
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(a: player.position, b: grapplePoint);

            // The distance grapple will try to keep from grapple point
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * .25f;


            joint.spring = 10f;
            joint.damper = 2f;
            joint.massScale = 4.5f;
        }
    }

    /// <summary>
    /// Called whenever we want to stop a grapple
    /// </summary>
    private void StopGrapple()
    {
        Destroy(joint);

    }

    public bool IsGrappling()
    {
        return joint != null;
    }

}
