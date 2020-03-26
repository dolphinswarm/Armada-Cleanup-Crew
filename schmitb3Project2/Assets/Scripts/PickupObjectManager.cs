using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Destroys a prefab after it falls below a certain height.
/// </summary>
public class PickupObjectManager : MonoBehaviour
{
    // ====================================================== Properties
    [Header("Game Settings and Objects")]
    private GameManager gameManager;            // The game's game manager.
    public bool isPickedUp = false;             // Has this object been picked up?

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
        // Nudge this item to keep it awake
        if (gameManager.isGameRunning)
            transform.position += Vector3.one * 0.001f;

        // If below a certain y position, destroy this object
        if (transform.position.y < -100) Destroy(gameObject);
    }
}
