using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class WASDMovementWithController : MonoBehaviour
{
    //============================= Properties
    CharacterController controller;
    public float speed;
    float fallSpeed;
    public float jumpSpeed;

    //============================= Methods
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Create move vector
        Vector3 moveVector = Vector3.zero;

        // Forward movement
        if (Input.GetKey(KeyCode.W))
            moveVector += transform.TransformDirection(Vector3.forward) * speed * Time.deltaTime;

        // Backward movement
        if (Input.GetKey(KeyCode.S))
            moveVector += transform.TransformDirection(Vector3.back) * 0.33f * speed * Time.deltaTime;

        // Left movement
        if (Input.GetKey(KeyCode.A))
            moveVector += transform.TransformDirection(Vector3.left) * 0.67f * speed * Time.deltaTime;

        // Right movement
        if (Input.GetKey(KeyCode.D))
            moveVector += transform.TransformDirection(Vector3.right) * 0.67f * speed * Time.deltaTime;

        // Add falling / jumping
        if (!controller.isGrounded)
            fallSpeed += Physics.gravity.y * Time.deltaTime;
        else
            if (Input.GetButtonDown("Jump"))
                fallSpeed = jumpSpeed;
            else
                fallSpeed = 0;
        moveVector.y = fallSpeed;

        // Move component
        controller.Move(moveVector);
    }
}
