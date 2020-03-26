using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ControllerMover : MonoBehaviour
{
    //============================= Properties
    CharacterController controller;
    public float forwardSpeed;
    public float maxRotationSpeed;
    Vector3 directionVector;

    //============================= Methods
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        directionVector = transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = Vector3.Slerp(transform.forward, directionVector, Time.deltaTime * maxRotationSpeed);
        controller.Move(directionVector * forwardSpeed * Time.deltaTime);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        directionVector = Vector3.Reflect(directionVector, hit.normal);
        directionVector.y = 0;
        directionVector.Normalize();
    }
}
