using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = default;

    private Vector3 movementInput = default;
    private Rigidbody rb;

    private void Start()
    {
        movementInput = new Vector3();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        DoMovement();
    }

    private Vector3 previousVelocity = Vector3.zero;
    private void DoMovement()
    {
        Vector3 movementVector = (movementInput.x * transform.right + movementInput.z * transform.forward).normalized;

        rb.velocity -= previousVelocity;
        rb.velocity += movementVector * movementSpeed;

        previousVelocity = movementVector * movementSpeed;

        //rb.AddForce((movementInput.x * transform.right + movementInput.z * transform.forward).normalized * movementSpeed, ForceMode.Acceleration);
    }

    private void GetInput()
    {
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.z = Input.GetAxisRaw("Vertical");
    }
}
