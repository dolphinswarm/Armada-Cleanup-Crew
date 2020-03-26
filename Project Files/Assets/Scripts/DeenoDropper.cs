using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeenoDropper : MonoBehaviour
{
    //============================= Properties
    public GameObject deeno;
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
        while(true)
        {
            // Wait 3 seconds
            yield return new WaitForSeconds(spawnInterval);

            // Generate a position / rotation
            Vector3 position = new Vector3(Random.Range(-25.0f, 25.0f), 20.0f, Random.Range(-25.0f, 25.0f));
            Quaternion rotation = Quaternion.Euler(new Vector3(0.0f, Random.Range(0.0f, 360.0f), 0.0f));

            // Choose which enemy to spawn
            Instantiate(deeno, position, rotation);
        }
    }
}
