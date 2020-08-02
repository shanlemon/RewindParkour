﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpPosition : MonoBehaviour
{
    [SerializeField] private float warpStorageTimeInSeconds = 5f;
    [SerializeField] private float minimumFillToWarp = 0.2f;
    [SerializeField] private KeyCode KeyToWarp = KeyCode.E;
    [SerializeField] private Rigidbody rb = default;
    [SerializeField] private PlayerMovement pm = default;
    [SerializeField] private GameObject spherePrefab = default;
    [SerializeField] private GameObject spherePrefab2 = default;

    private int warpStackSize;

    public List<Vector3> previousPositions;

    public float WarpFillPercentage
    {
        get => (float)previousPositions.Count / (float)warpStackSize;
    }

    void Start()
    {
        previousPositions = new List<Vector3>();
        warpStackSize = (int)(warpStorageTimeInSeconds * (1f / Time.fixedDeltaTime));
    }


    public bool IsWarping {private set; get;}

    public Vector3 WarpDirection { private set; get; }
    private float timer = 0;
    private float customWarpDeltaTime = 0.04f;

    private Vector3 postWarpVelocity = Vector3.zero;
    void Update()
    {
        timer += Time.deltaTime;
        if (IsWarping && Input.GetKeyUp(KeyToWarp))
        {
            StopWarping();
        }

        if (!IsWarping && Input.GetKeyDown(KeyToWarp))
        {
            StartWarping();
        }

        if (IsWarping)
        {
            if (previousPositions.Count < 1 )
            {
                StopWarping();
                return;
            }
            
            customWarpDeltaTime = Mathf.Lerp(customWarpDeltaTime, customWarpDeltaTime / 1.2f, customWarpDeltaTime);
            //customWarpDeltaTime = Mathf.Clamp(customWarpDeltaTime, 0.005f, customWarpDeltaTime);
            if (timer >= customWarpDeltaTime)
            {
                timer = 0;
                WarpMove();

                WarpDirection = (rb.transform.position - previousPositions.Last()).normalized;

                postWarpVelocity = -(WarpDirection) / customWarpDeltaTime;
                previousPositions.RemoveAt(previousPositions.Count - 1);
            }
        }
        else
        {
            //Instantiate(spherePrefab2, transform.position, Quaternion.identity);
        }
    }

    private float warpStartVelocityMagnitude = 0f;
    // Disable gravity, movement, and add slow motion for a few seconds
    private void StartWarping() {
        if (WarpFillPercentage < minimumFillToWarp)
            return;
        rb.useGravity = false;
        pm.DisableMovement();
        IsWarping = true;
        warpStartVelocityMagnitude = rb.velocity.magnitude;
    }

    private void StopWarping() {
        customWarpDeltaTime = 0.04f;
        // Use gravity
        rb.useGravity = true;
        pm.EnableMovement();
        IsWarping = false;
        rb.velocity = postWarpVelocity;
    }

    void FixedUpdate()
    {
        if (!IsWarping)
        {
            previousPositions.Add(rb.transform.position);

            if (previousPositions.Count > warpStackSize)
                previousPositions.RemoveAt(0);
        }
    }

    private void WarpMove() {
        rb.velocity = Vector3.zero;
        rb.MovePosition(previousPositions.Last());
        Instantiate(spherePrefab, previousPositions.Last(), Quaternion.identity);
    }

}