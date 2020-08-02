using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFacesWarp : MonoBehaviour
{
    [SerializeField] private WarpPosition warp = default;
    [SerializeField] private float speed = 0.2f;

    void Update()
    {
        if(warp.IsWarping) {
            Quaternion targetRot = Quaternion.LookRotation(-warp.WarpDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime *  speed);
            //transform.rotation = Quaternion.LookRotation(-warp.WarpDirection);
        }        
    }
}
