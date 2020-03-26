using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the collision between other objects, and handles scoring.
/// </summary>
public class CollisionManager : MonoBehaviour
{
    // ====================================================== Properties
    [Header("Game Settings and Objects")]
    private GameManager gameManager;            // The game's game manager.
    private GameObject player;                  // The player game object.

    // ====================================================== Methods
    /// <summary>
    /// On game start...
    /// </summary>
    void Start()
    {
        // Set the game manager
        gameManager = FindObjectOfType<GameManager>();

        // Set the player
        player = GameObject.FindGameObjectWithTag("Player");
    }

    /// <summary>
    /// On frame update...
    /// </summary>
    void Update()
    {
        
    }

    /// <summary>
    /// On collision enter...
    /// </summary>
    /// <param name="collision">The other collision.</param>
    void OnCollisionEnter(Collision collision)
    {
        // Check the tag. If a pickup obstacle...
        if (collision.gameObject.CompareTag("PickupObstacle") && !collision.gameObject.GetComponent<PickupObjectManager>().isPickedUp)
        {
            // Temporarily deatch all children
            List<Transform> children = new List<Transform>();
            for (int i = 0; i < transform.childCount; i++)
            {
                children.Add(transform.GetChild(i));
            }
            transform.DetachChildren();

            // Increase the size of the sphere
            gameObject.transform.parent.localScale += Vector3.one * 0.125f;

            // Set its parent to this game object
            collision.gameObject.transform.parent = gameObject.transform;

            // Reattach all children
            foreach (Transform child in children)
                child.parent = gameObject.transform;

            // Disable its rigidbody components and its colliders
            foreach (Rigidbody rigidbody in collision.gameObject.GetComponentsInChildren<Rigidbody>())
                rigidbody.isKinematic = true;

            foreach (Collider collider in collision.gameObject.GetComponentsInChildren<Collider>())
                collider.enabled = false;

            // Disable pickupable
            collision.gameObject.GetComponent<PickupObjectManager>().isPickedUp = true;

            // Change the tag of the picked-up object
            collision.gameObject.tag = "Untagged";

            // Update the score
            gameManager.ModifyScore(1);
            player.GetComponent<SphereController>().PlaySound("collect");
        }
    }

    /// <summary>
    /// On trigger enter (only with fall zone)...
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        // Detatch this sphere's camera temporarily
        transform.parent.Find("Main Camera").parent = null;

        // Play the fall sound effect
        player.GetComponent<SphereController>().PlaySound("fall");

        // Switch to the shoulder camera
        gameManager.SwitchCamera(1);

        // Decrease the player's score by 5
        if (gameManager.score - 5 < 0)
        {
            gameManager.ModifyScore(-5 - (gameManager.score - 5));
        } 
        else
        {
            gameManager.ModifyScore(-5);
        }
    }

    /// <summary>
    /// On trigger exit...
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        // Teleport the player back on the stage
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = gameManager.respawnPoint.position;
        player.GetComponent<CharacterController>().enabled = true;

        // Re-add the camera as a child
        GameObject.Find("Main Camera").transform.parent = player.transform;
        GameObject.Find("Main Camera").transform.localPosition = new Vector3(0.0f, 5.0f, -5.0f);

        // Set the sphere's falling speed to 0
        player.GetComponent<SphereController>().fallSpeed = 0.0f;
    }
}
