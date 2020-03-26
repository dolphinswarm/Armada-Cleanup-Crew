using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Turns this object into a random color.
/// </summary>
public class RandomColor : MonoBehaviour
{
    /// <summary>
    /// When created, change the color to a random one.
    /// </summary>
    void Start()
    {
        GetComponent<Renderer>().material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 0.5f);
    }
}
