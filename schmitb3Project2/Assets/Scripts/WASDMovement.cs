using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WASDMovement : MonoBehaviour
{
    //============================= Properties
    public float speed;

    //============================= Methods
    void Update()
    {
        // Forward movement
        if (Input.GetKey(KeyCode.W))
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Backward movement
        if (Input.GetKey(KeyCode.S))
            transform.Translate(Vector3.back * (0.33f * speed * Time.deltaTime));

        // Left movement
        if (Input.GetKey(KeyCode.A))
            transform.Translate(Vector3.left * (0.67f * speed * Time.deltaTime));

        // Right movement
        if (Input.GetKey(KeyCode.D))
            transform.Translate(Vector3.right * (0.67f * speed * Time.deltaTime));
    }
}
