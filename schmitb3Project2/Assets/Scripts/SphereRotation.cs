using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the sphere's rotation animation.
/// </summary>
public class SphereRotation : MonoBehaviour
{
    // ====================================================== Properties
    private float circumference;                    // The circumference of the sphere.
    private Vector3 prevPos;                        // The current previous position of the sphere.
    private Vector3 currentPos;                     // The current position of the sphere.
    private Vector3 moveVec;                        // The movement vector of this object.

    // ====================================================== Methods
    /// <summary>
    /// On game start...
    /// </summary>
    void Start()
    {
        // Set the circumference
        circumference = GetComponentInParent<SphereCollider>().radius * 2.0f * Mathf.PI;
    }

    /// <summary>
    /// On frame update...
    /// </summary>
    void Update()
    {
        /* ========= ROLLING ========= */
        // Set the previous position and current position, then calculate the movement vector from those
        prevPos = currentPos;
        currentPos = transform.position;
        moveVec = currentPos - prevPos;

        // Rotate the sphere
        transform.Rotate(Vector3.Cross(Vector3.up, moveVec).normalized, (moveVec.magnitude / circumference) * 360.0f, Space.World);
    }
}
