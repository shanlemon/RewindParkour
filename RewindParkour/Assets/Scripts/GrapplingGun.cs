﻿using System;
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

    private LineRenderer lineRenderer = default;
    private Vector3 grapplePoint = default;
    private SpringJoint joint = default;

    // Getters
    public Vector3 GrapplePoint => grapplePoint;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    { 

        if (Input.GetMouseButtonDown(0))
        {
            StartGrapple();
        }

        else if (Input.GetMouseButtonUp(0))
        {
            StopGrapple();
        }
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    /// <summary>
    /// Called whenever we want to start a grapple
    /// </summary>
    private void StartGrapple()
    {
        RaycastHit hit;

        if (Physics.Raycast(origin: camera.position, direction: camera.forward, out hit, maxDistance, whatIsGrappleable))
        {
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

            lineRenderer.positionCount = 2;
        }
    }

    private void DrawRope()
    {
        // Not grappling so don't draw
        if (!joint)
        {
            return;
        }

        lineRenderer.SetPosition(index: 0, gunTip.position);
        lineRenderer.SetPosition(index: 1, grapplePoint);
    }

    /// <summary>
    /// Called whenever we want to stop a grapple
    /// </summary>
    private void StopGrapple()
    {
        lineRenderer.positionCount = 0;
        Destroy(joint);
    }

    public bool IsGrappling()
    {
        return joint != null;
    }

}