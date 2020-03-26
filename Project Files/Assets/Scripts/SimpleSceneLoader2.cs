using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleSceneLoader2 : MonoBehaviour
{
    //============================= Properties
    public string nextSceneName;

    //============================= Methods
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
            SceneManager.LoadScene(nextSceneName);
    }
}
