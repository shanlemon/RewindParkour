using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSway : MonoBehaviour
{

    [Header("Sway")]
    [SerializeField] private float intensity = 1; 
    [SerializeField] private float smooth = 10; 

    [Header("Moving")]
    [SerializeField] private float posOffset = 1;
    [SerializeField] private float movingSmooth = 1;

    private Quaternion originalRotation = default;
    private Vector3 originalPosition = default;

    private Rigidbody rb;
    

    // Start is called before the first frame update
    void Start()
    {
        originalRotation = transform.localRotation;
        originalPosition = transform.localPosition;
        rb = Managers.Player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        GunSwaying();
        GunMoving();

    }

    private void GunMoving()
    {
        Vector3 velocity = Vector3.zero;

        Vector3 offset = Vector3.Project(rb.velocity, transform.right).normalized * posOffset;
        Vector3 desiredPos = originalPosition - new Vector3(offset.x, rb.velocity.normalized.y * posOffset, offset.y);
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, desiredPos, ref velocity, movingSmooth * Time.deltaTime);
    }

    private void GunSwaying()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        
        Quaternion xRot = Quaternion.AngleAxis(-intensity * x, Vector3.up);
        Quaternion yRot = Quaternion.AngleAxis(intensity * y, Vector3.right);
        Quaternion target = originalRotation * xRot * yRot;

        transform.localRotation = Quaternion.Lerp(transform.localRotation, target, Time.deltaTime * smooth);
    }
}
