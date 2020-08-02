using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpStorage : MonoBehaviour
{
    [SerializeField] private float warpStorageTimeInSeconds = 5f;
    [SerializeField] private float postWarpspeedMultiplier = 1.4f;
    [SerializeField] private float slowMotionTimeInSeconds = 2f;
    [SerializeField] private float slowMotionScale = 0.5f;
    [SerializeField] private float minimumFillToWarp = 0.2f;
    [SerializeField] private KeyCode KeyToWarp = KeyCode.E;
    [SerializeField] private Rigidbody rb = default;
    [SerializeField] private PlayerMovement pm = default;
    private Rigidbody rbToWarp = null;

    private int warpStackSize;

    public List<Vector3> previousPositions;
    public List<Vector3> previousVelocities;

    public float WarpFillPercentage
    {
        get => (float)previousPositions.Count / (float)warpStackSize;
    }

    void Start()
    {
        previousPositions = new List<Vector3>();
        previousVelocities = new List<Vector3>();
        Debug.Log(Time.fixedDeltaTime);
        warpStackSize = (int)(warpStorageTimeInSeconds * (1f / Time.fixedDeltaTime));
        rbToWarp = GetComponent<Rigidbody>();
    }


    private bool isWarping = false;
    void Update()
    {

        if (isWarping && Input.GetKeyUp(KeyToWarp))
        {
            StopWarping();
        }

        if (!isWarping && Input.GetKeyDown(KeyToWarp))
        {
            StartWarping();
        }
    }

    private float warpStartVelocityMagnitude = 0f;
    // Disable gravity, movement, and add slow motion for a few seconds
    private void StartWarping() {
        if (WarpFillPercentage < minimumFillToWarp)
            return;
        rb.useGravity = false;
        pm.DisableMovement();
        isWarping = true;
        warpStartVelocityMagnitude = rb.velocity.magnitude;
        //StartCoroutine(SlowDownTime(slowMotionTimeInSeconds, slowMotionScale));
    }

    private void StopWarping() {
        // Use gravity
        rb.useGravity = true;
        pm.EnableMovement();
        isWarping = false;
        if (previousVelocities.Count > 0)
            rbToWarp.velocity = -postWarpspeedMultiplier * previousVelocities[previousVelocities.Count - 1].normalized * (warpStartVelocityMagnitude);
    }

    private IEnumerator SlowDownTime(float time, float slowScale)
    {
        // Slow down time
        Time.timeScale = slowScale;
        yield return new WaitForSeconds(time);
        // Bring it back
        Time.timeScale = 1;
    }

    void FixedUpdate()
    {
        if (!isWarping)
        {
            previousPositions.Add(rbToWarp.transform.position);
            previousVelocities.Add(rbToWarp.velocity);

            if (previousPositions.Count > warpStackSize)
                previousPositions.RemoveAt(0);
            if (previousVelocities.Count > warpStackSize)
                previousVelocities.RemoveAt(0);
        }
        else
        {
            if (previousPositions.Count < 1 || previousVelocities.Count < 1)
            {
                StopWarping();
                return;
            }

            rbToWarp.MovePosition(previousPositions.Last());

            previousPositions.RemoveAt(previousPositions.Count - 1);
            previousVelocities.RemoveAt(previousVelocities.Count - 1);
        }
    }

}
