using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleSceneLoader : MonoBehaviour
{
    //============================= Properties
    public string nextSceneName;

    //============================= Methods
    private void OnMouseDown()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
