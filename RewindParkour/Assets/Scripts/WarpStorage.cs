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
    [SerializeField] private GameObject spherePrefab = default;

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


    public bool IsWarping => isWarping;
    private bool isWarping = false;

    public Vector3 WarpDirection { private set; get; }
    private float timer = 0;

    private float customWarpDeltaTime = 0.04f;

    private Vector3 postWarpVelocity = Vector3.zero;
    void Update()
    {
        timer += Time.deltaTime;
        if (isWarping && Input.GetKeyUp(KeyToWarp))
        {
            StopWarping();
        }

        if (!isWarping && Input.GetKeyDown(KeyToWarp))
        {
            StartWarping();
        }

        if (isWarping)
        {
            if (previousPositions.Count < 1 )
            {
                StopWarping();
                return;
            }
            
            customWarpDeltaTime = Mathf.Lerp(customWarpDeltaTime, customWarpDeltaTime / 1.2f, customWarpDeltaTime);
            if (timer >= customWarpDeltaTime)
            {
                timer = 0;
                WarpMove();

                Vector3 warpDirection = rb.transform.position - previousPositions.Last();

                postWarpVelocity = -(warpDirection) / customWarpDeltaTime;
                previousPositions.RemoveAt(previousPositions.Count - 1);
            }
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
        customWarpDeltaTime = 0.04f;
        // Use gravity
        rb.useGravity = true;
        pm.EnableMovement();
        isWarping = false;
        rb.velocity = postWarpVelocity;
        // if (previousVelocities.Count > 0)
        //     rbToWarp.velocity = -postWarpspeedMultiplier * previousVelocities[previousVelocities.Count - 1].normalized * (warpStartVelocityMagnitude);
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
