using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{
    [SerializeField] private Transform player = default;
    [SerializeField] private Transform receiver = default;
    
    private bool playerIsOverLapping = false;

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
                float rotationDiff = -Quaternion.Angle(transform.rotation, receiver.rotation);
                rotationDiff += 180;
                player.Rotate(Vector3.up, rotationDiff);

                Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
                player.position = receiver.position + positionOffset;

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
}
