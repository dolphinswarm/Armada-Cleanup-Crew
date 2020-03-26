using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  A short script for making the fighter enemies
/// </summary>
public class FighterShipMovement : MonoBehaviour
{
    // ====================================================== Properties
    [Header("Movement Options")]
    public float speed = 1.0f;                  // How fast the ship shakes.
    public float amount = 0.2f;                 // How much the ship shakes
    private float xOffset, yOffset, zOffset;    // The randomized offset of each ship.

    // ====================================================== Methods
    /// <summary>
    /// On game start...
    /// </summary>
    void Start()
    {
        // Set the offsets per ship
        xOffset = Random.Range(0.0f, 10.0f);
        yOffset = Random.Range(0.0f, 10.0f);
        zOffset = Random.Range(0.0f, 10.0f);
    }

    /// <summary>
    /// On frame update...
    /// </summary>
    void Update()
    {
        transform.position += new Vector3(Mathf.Sin((Time.time + xOffset) * speed) * amount, Mathf.Sin((Time.time + yOffset) * speed) * amount, Mathf.Sin((Time.time + zOffset) * speed) * amount);
    }
}
