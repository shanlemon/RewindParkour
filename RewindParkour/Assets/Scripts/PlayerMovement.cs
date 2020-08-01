using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = default;

    private Vector2 movementInput = default;
    private Rigidbody rb;

    private void Start() {
        movementInput = new Vector2();
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
        rb.velocity = transform.forward * movementInput * movementSpeed * Time.deltaTime;
    }

    private void GetInput()
    {
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");
    }
}
