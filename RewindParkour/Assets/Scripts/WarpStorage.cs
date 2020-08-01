using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpStorage : MonoBehaviour
{
    [SerializeField] private float warpTime = 0.2f;
    [SerializeField] private float warpSpeed = 1.4f;
    [SerializeField] private KeyCode KeyToWarp = KeyCode.E;
    private Rigidbody toWarp = null;

    private int stackSize;

    public List<Vector3> previousPositions;
    public List<Vector3> previousVelocities;

    void Start()
    {
        previousPositions = new List<Vector3>();
        previousVelocities = new List<Vector3>();

        stackSize = (int)(warpTime * (1f / 0.02f));
        toWarp = GetComponent<Rigidbody>();
    }


    private bool isWarping = false;
    void Update()
    {
        isWarping = Input.GetKey(KeyToWarp);

        if (Input.GetKeyUp(KeyToWarp))
        {
            if (previousVelocities.Count > 0)
                toWarp.velocity = -warpSpeed * previousVelocities[previousVelocities.Count - 1];
        }
    }

    void FixedUpdate()
    {
        if (!isWarping)
        {
            previousPositions.Add(toWarp.transform.position);
            previousVelocities.Add(toWarp.velocity);

            if (previousPositions.Count > stackSize)
                previousPositions.RemoveAt(0);
            if (previousVelocities.Count > stackSize)
                previousVelocities.RemoveAt(0);
        }
        else
        {
            if (previousPositions.Count < 1)
                return;
            if (previousVelocities.Count < 1)
                return;
            toWarp.transform.position = previousPositions.Last();
            previousPositions.RemoveAt(previousPositions.Count - 1);
            previousVelocities.RemoveAt(previousVelocities.Count - 1);
        }
    }
}
