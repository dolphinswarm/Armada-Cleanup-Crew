using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeenoExplosion : MonoBehaviour
{
    //============================= Properties
    public GameObject explosionPrefab;
    public AudioClip clip;

    //============================= Methods
    // On collision enter
    void OnCollisionEnter(Collision collision)
    {
        Rigidbody body = collision.gameObject.GetComponent<Rigidbody>();
        // Check collision
        if (body != null)
        {
            // Add explosion force
            body.AddExplosionForce(100.0f, transform.position, 5.0f);

            // Destry this deeno
            Destroy(this.gameObject);

            // Create explosion prefab
            ContactPoint contact = collision.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point;
            GameObject explosion = Instantiate(explosionPrefab, pos, rot) as GameObject;
            Destroy(explosion, 3.0f);

            // Play explosion sound
            AudioSource.PlayClipAtPoint(clip, transform.position);

        }
    }
}
