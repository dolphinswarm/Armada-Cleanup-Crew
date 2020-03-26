using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gets this text to always point at the active camera
/// </summary>
public class TextPointAtCamera : MonoBehaviour
{
    // ====================================================== Properties
    [Header("Game Settings and Objects")]
    private GameManager gameManager;            // The game's game manager.

    // ====================================================== Methods
    /// <summary>
    /// On game start...
    /// </summary>
    void Start()
    {
        // Set the game manager
        gameManager = FindObjectOfType<GameManager>();
    }

    /// <summary>
    /// On frame update...
    /// </summary>
    void Update()
    {
        // Look at the current active camera
        transform.rotation = gameManager.GetActiveCamera().transform.rotation;
    }
}
