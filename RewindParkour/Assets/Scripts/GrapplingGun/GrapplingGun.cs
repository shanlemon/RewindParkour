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
    [SerializeField] public float maxDistance = 100f;
    [SerializeField] private float force = default;

    private Vector3 grapplePoint = default;
    private SpringJoint joint = default;
    private GrapplingRope rope = default;
    private bool hasRigidBody = false;
    

    // Getters
    public Vector3 GrapplePoint { set; get; }
    public Transform GunTip => gunTip;

    private void Start()
    {
        rope = GetComponent<GrapplingRope>();
    }

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
            Vector3 offset = hit.collider.transform.position - transform.position;
            GrapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = GrapplePoint;
            
            float distanceFromPoint = Vector3.Distance(a: player.position, b: GrapplePoint);

            rope.Hook.transform.SetParent(hit.collider.transform);
            rope.Hook.transform.position = hit.point;

            if (hit.collider.attachedRigidbody != null)
            {
                joint.maxDistance = distanceFromPoint * 0.1f;
                joint.minDistance = distanceFromPoint * 0.1f;
                hasRigidBody = true;
                joint.connectedBody = hit.collider.attachedRigidbody;

                joint.spring = 0f;
                joint.damper = 2f;
                joint.massScale = 4.5f;
            }

            // The distance grapple will try to keep from grapple point
            if (!hasRigidBody)
            {
                joint.maxDistance = distanceFromPoint * 0.8f;
                joint.minDistance = distanceFromPoint * .25f;

                joint.spring = 10f;
                joint.damper = 2f;
                joint.massScale = 4.5f;
            }


            
        }
    }

    /// <summary>
    /// Called whenever we want to stop a grapple
    /// </summary>
    private void StopGrapple()
    {
        hasRigidBody = false;
        Destroy(joint);

    }

    public bool IsGrappling()
    {
        return joint != null;
    }

}
