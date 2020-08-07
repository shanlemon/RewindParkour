using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSway : MonoBehaviour
{
    [SerializeField] private float intensity = 1; 
    [SerializeField] private float smooth = 10; 
    private Quaternion originalRotation = default;

    // Start is called before the first frame update
    void Start()
    {
        originalRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        
        Quaternion xRot = Quaternion.AngleAxis(-intensity * x, Vector3.up);
        Quaternion yRot = Quaternion.AngleAxis(intensity * y, Vector3.right);
        Quaternion target = originalRotation * xRot * yRot;

        transform.localRotation = Quaternion.Lerp(transform.localRotation, target, Time.deltaTime * smooth);
    }
}
