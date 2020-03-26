using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An enumeration for the shape of spawning.
/// </summary>
public enum SpawnShape { CIRCLE, RECTANGLE };

/// <summary>
/// An enumeration for the type of spawning
/// </summary>
public enum SpawnType { CONTINUOUS, ONCE, BATCHES };

/// <summary>
/// Spawns a given number of GameObjects.
/// </summary>
public class ObjectMaker : MonoBehaviour
{
    // ====================================================== Properties
    [Header("Object Maker Properties")]
    public int numOfObjects = 40;                       // The number of objects to spawn.
    public List<GameObject> objectsToSpawn;             // A list of objects to spawn.
    public SpawnShape spawnShape = SpawnShape.CIRCLE;   // The shape of the object's spawn.
    public SpawnType spawnType = SpawnType.ONCE;        // The type of spawning this maker has.
    public float spawnDelay = 10.0f;                    // The delay between each spawn.
    public float radiusOrSideLength1 = 10.0f;           // The radius of the spawn circle, OR half the side length of a rectangle / square
    public float sideLength2 = 10.0f;                   // Half the side length of a rectangle / square
    public bool spawning = true;                        // Is this object currently spawning?

    private Vector3 position;                           // The randomly-generated position for a spawned game object.
    private Quaternion rotation;                        // The randomly-generated rotation for a spawned game object.

    // ====================================================== Methods
    /// <summary>
    /// On game start...
    /// </summary>
    void Start()
    {
        // Check if we should spawn more. If so, start a coroutine
        if (spawnType != SpawnType.ONCE)
        {
            StartCoroutine("IntervalSpawn");
            spawning = true;
        }
        else
        {
            SpawnObjects(numOfObjects);
            spawning = false;
        }
    }

    /// <summary>
    /// A private method for spawning a specified number of objects
    /// </summary>
    void SpawnObjects(int number)
    {
        // Spawn "numOfObjects" number of objects
        for (int i = 0; i < number; i++)
        {
            // If a rectangle...
            if (spawnShape == SpawnShape.RECTANGLE)
            {
                // Generate a position / rotation
                position = new Vector3(transform.position.x + Random.Range(-radiusOrSideLength1, radiusOrSideLength1),
                                       transform.position.y, // + Random.Range(0.0f, 10.0f),
                                       transform.position.z + Random.Range(-sideLength2, sideLength2));
                rotation = Quaternion.Euler(new Vector3(0.0f, Random.Range(0.0f, 360.0f), 0.0f));
            }
            // Else, if a circle...
            else if (spawnShape == SpawnShape.CIRCLE)
            {
                // Generate a position / rotation
                position = transform.position + Random.insideUnitSphere * radiusOrSideLength1;
                position.y = transform.position.y; // Random.Range(0.0f, 10.0f);
                rotation = Quaternion.Euler(new Vector3(0.0f, Random.Range(0.0f, 360.0f), 0.0f));
            }

            // Spawn the object
            Instantiate(objectsToSpawn[Random.Range(0, objectsToSpawn.Count)], position, rotation);
        }
    }
    
    /// <summary>
    /// A coroutine for spawing objects, at an "interval of spawn" delay seconds.
    /// </summary>
    /// <returns></returns>
    IEnumerator IntervalSpawn()
    {
        // While we should spawn more objects...
        while (spawning)
        {
            // If we should continuously spawn...
            if (spawnType == SpawnType.CONTINUOUS)
            {
                SpawnObjects(1);
                // Wait a certain number of seconds
                yield return new WaitForSeconds(spawnDelay / numOfObjects);
            }

            // Else, it's a batch, so spawn the batch
            else
            {
                SpawnObjects(numOfObjects);
                // Wait a certain number of seconds
                yield return new WaitForSeconds(spawnDelay);
            }
        }
    }
}
