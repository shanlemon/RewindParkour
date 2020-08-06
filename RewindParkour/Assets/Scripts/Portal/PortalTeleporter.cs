using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Teleports the player from one portal to the other.
/// </summary>
public class PortalTeleporter : MonoBehaviour
{
    // Collider plane on other portal
    [SerializeField] private Transform otherPlaneCollider = default; 

    private Transform player = default;
    
    private bool playerIsOverLapping = false;

    private void Start()
    {
        player = Managers.Player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsOverLapping)
        {
            Vector3 portalToPlayer = player.position - transform.position;
            float dotProduct = Vector3.Dot(transform.up, portalToPlayer);

            // If this is true, the player has moved across the portal
            if (dotProduct < 0f)
            {
                // Teleport the player
                float rotationDiff = -Quaternion.Angle(transform.rotation, otherPlaneCollider.rotation);
                rotationDiff += 180;
                player.Rotate(Vector3.up, rotationDiff);

                Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
                player.position = otherPlaneCollider.position + positionOffset;

                playerIsOverLapping = false;
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerIsOverLapping = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerIsOverLapping = false;
        }
    }
}
