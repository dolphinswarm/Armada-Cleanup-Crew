using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

/// <summary>
/// The AI for enemy spheres.
/// </summary>
public class EnemyAI : MonoBehaviour
{
    // ====================================================== Properties
    [Header("AI Settings")]
    private NavMeshAgent navMeshAgent;                  // The NavMeshAgent for this object
    public PickupObjectManager pickupObjectManager;     // The current target's pickup manager.
    public float recheckTime = 10.0f;                   // The amount of time before this object should re-check if there's a closer object.
    private float recheckTimer = 0.0f;                  // The timer for re-checking.

    [Header("AI Score")]
    public int enemyScore = 0;                          // This enemy's score.
    private TMP_Text enemyScoreText;                    // The label for this enemy's score

    // ====================================================== Methods
    // Start is called before the first frame update
    void Start()
    {
        // Get the NavMeshAgent
        navMeshAgent = GetComponent<NavMeshAgent>();

        // Get the enemy score text
        enemyScoreText = GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if current target is still valid
        if (pickupObjectManager == null)
        {
            ChangeTarget();
        } 
        else if (pickupObjectManager.isPickedUp)
        {
            ChangeTarget();
        }
        else if (recheckTimer >= recheckTime)
        {
            ChangeTarget();
            recheckTimer = 0.0f;
        }

        // Update the recheck timer
        recheckTimer += Time.deltaTime;
    }

    /// <summary>
    /// Changes the target to the nearest GameObject
    /// </summary>
    public void ChangeTarget()
    {
        // Get a list of pickup objects and find the closest one
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        foreach (GameObject pickupObj in GameObject.FindGameObjectsWithTag("PickupObstacle"))
        {
            // If the distance between the two is less than the current min distance, set this transform
            float dist = (pickupObj.transform.position - transform.position).magnitude;
            if (dist < minDist)
            {
                pickupObjectManager = pickupObj.GetComponent<PickupObjectManager>();
                tMin = pickupObj.transform;
                minDist = dist;
            }
        }

        // Set the destination to the closest object
        navMeshAgent.SetDestination(tMin.position);
    }

    /// <summary>
    /// Modifies the enemy score by a provided amount.
    /// </summary>
    /// <param name="amount">The amount to modify the score by.</param>
    public void ModifyEnemyScore(int amount)
    {
        // Modify the score and update the score text.
        enemyScore += amount;
        enemyScoreText.SetText(enemyScore.ToString());
    }
}
