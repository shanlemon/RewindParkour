using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves the camera on the other portal based on the player's position.
/// </summary
public class PortalCamera : MonoBehaviour
{
    [SerializeField] private Transform playerCamera = default;
    [SerializeField] private Transform portal = default;
    [SerializeField] private Transform otherPortal = default;

    // Update is called once per frame
    void LateUpdate()
    {
        // Move camera based on player's camera
        Vector3 playerOffsetFromPortal = playerCamera.position - otherPortal.position;
        transform.position = portal.position + playerOffsetFromPortal;

        // Difference between angle rotation of both portals
        float angularDiffBWPortalRotations = Quaternion.Angle(portal.rotation, otherPortal.rotation);
        
        // Create the diff as a quaternion
        Quaternion portalRotationalDiff = Quaternion.AngleAxis(angularDiffBWPortalRotations, Vector3.up);

        // Direction that we need face the camera
        Vector3 newCameraDirection = portalRotationalDiff * playerCamera.forward;

        // Actually rotate the camera
        transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);
    }
}
