using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the enemies' collisions.
/// </summary>
public class EnemyCollisionManager : MonoBehaviour
{
    // ====================================================== Properties
    [Header("Game Settings and Objects")]
    private EnemyAI enemyAI;

    // ====================================================== Methods
    /// <summary>
    /// On game start...
    /// </summary>
    void Start()
    {
        // Set the game manager
        enemyAI = GetComponentInParent<EnemyAI>();
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
            enemyAI.ModifyEnemyScore(1);
        }
    }
}
