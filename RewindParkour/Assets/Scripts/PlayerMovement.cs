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

    private void Start() {
        movementInput = new Vector3();
        rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        GetInput();
    }

    private void FixedUpdate() {
        DoMovement();
    }

    private void DoMovement()
    {
        rb.velocity = (movementInput.x * transform.right + movementInput.z * transform.forward).normalized * movementSpeed;
    }

    private void GetInput()
    {
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.z = Input.GetAxisRaw("Vertical");
    }
}
