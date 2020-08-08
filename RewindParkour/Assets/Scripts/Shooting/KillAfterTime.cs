using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAfterTime : MonoBehaviour
{
    [SerializeField] private float time;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this, time);
    }


}
