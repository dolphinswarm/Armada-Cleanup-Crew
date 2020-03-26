using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages events on the title screen UI.
/// </summary>
public class TitleEventManager : MonoBehaviour
{
    // ====================================================== Properties
    [Header("UI Objects")]
    private TMP_Text difficultySliderLabel;     // The label for the difficulty slider
    private TMP_Text enemySliderLabel;          // The label for the difficulty slider
    private static GameObject instance;         // An instance of this object, for checking.


    [Header("Game Settings")]
    public float numOfEnemies = 1.0f;           // The number of enemies of the upcoming game.
    public float difficulty = 1.0f;             // The difficulty of the upcoming game.

    // ====================================================== Methods
    /// <summary>
    /// On game start..
    /// </summary>
    void Start()
    {
        // Checks for duplicate instances
        DontDestroyOnLoad(gameObject);
        if (instance == null)
            instance = gameObject;
        else
            Destroy(gameObject);

        // Get difficulty slider label
        difficultySliderLabel = GameObject.Find("Difficulty Fun Text").GetComponent<TMP_Text>();

        // Get enemy slider label
        enemySliderLabel = GameObject.Find("Enemy Fun Text").GetComponent<TMP_Text>();
    }

    /// <summary>
    /// On frame update...
    /// </summary>
    void Update()
    {
        // If escape key pressed, quit game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    /// <summary>
    /// On difficulty slider move...
    /// </summary>
    public void OnDifficultySliderMove(float value)
    {
        // Do handling based on value
        if (value == 1.0f)
            difficultySliderLabel.text = "Easy peasy.";
        else if (value == 2.0f)
            difficultySliderLabel.text = "Pretty average.";
        else if (value == 3.0f)
            difficultySliderLabel.text = "A light challenge.";
        else if (value == 4.0f)
            difficultySliderLabel.text = "OOF";
        else
            difficultySliderLabel.text = "Error text!";

        // Set the number of enemies
        difficulty = value;
    }

    /// <summary>
    /// On difficulty slider move...
    /// </summary>
    public void OnEnemySliderMove(float value)
    {
        // Do handling based on value
        enemySliderLabel.text = value.ToString();

        // Set the number of enemies
        numOfEnemies = value;
    }

    /// <summary>
    /// Starts the game.
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene("Space");
    }
}
