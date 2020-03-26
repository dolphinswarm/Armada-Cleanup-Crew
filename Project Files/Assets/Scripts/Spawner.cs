using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //============================= Properties
    public GameObject objectToSpawn1;
    public GameObject objectToSpawn2;
    public int totalToSpawn;
    public float spawnInterval;

    //============================= Methods
    void Start()
    {
        StartCoroutine(Spawn());
    }

    // Coroutine for spawning
    IEnumerator Spawn()
    {
        // Repeat until max enemies spawned
        for (int i = 0; i < totalToSpawn; i++)
        {
            // Wait 3 seconds
            yield return new WaitForSeconds(spawnInterval);

            // Generate a position / rotation
            Vector3 position = new Vector3(Random.Range(-25.0f, 25.0f), 0.0f, Random.Range(-25.0f, 25.0f));
            Quaternion rotation = Quaternion.Euler(new Vector3(0.0f, Random.Range(0.0f, 360.0f), 0.0f));

            // Choose which enemy to spawn
            if (Random.Range(0.0f, 1.0f) > 0.66f)
                Instantiate(objectToSpawn1, position, rotation);
            else
                Instantiate(objectToSpawn2, position, rotation);
        }
    }
}
