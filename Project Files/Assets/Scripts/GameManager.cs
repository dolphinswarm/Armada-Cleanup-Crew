using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Manages the game's settings and properties.
/// </summary>
public class GameManager : MonoBehaviour
{
    // ====================================================== Properties
    [Header("Audio / Music")]
    public AudioSource musicPlayer;                 // The AudioSource which plays the background music.
    public List<AudioClip> musicChoices;            // The music choices for the game.
    public AudioClip winClip;                       // The win fanfare.
    public AudioClip loseClip;                      // The lose fanfare.


    [Header("Level Properties")]
    [Range(0.1f, 2.0f)]                             
    public float gravityModifier = 1.0f;            // The gravity modifier of the level
    public bool isGameRunning = true;               // Is the game currently running?
    [Range(1.0f, 4.0f)]
    public float difficulty = 2.0f;                 // The difficulty of the upcoming game.
    private TitleEventManager titleEventManager;    // The inherited title event manager.
    private GameObject backButton;                  // The back to title button.


    [Header("Cameras")]
    public Camera overheadCamera;                   // The overhead camera.
    public Camera ballShoulderCamera;               // The ball's follow camera.
    public Camera ballFPSCamera;                    // The ball's first person camera.

    private Camera currentCamera;                   // The current (active) camera.
    private ParticleSystem overheadLocator;         // The overhead locator particle effect.
    private MyMouseLook mouseLook;                  // The mouselook script attached to the ball.


    [Header("Player and Enemies")]
    public GameObject player;                       // The player game object.
    public Transform respawnPoint;                  // The position the player should respawn from.
    public GameObject enemyPrefab;                  // A prefab of the enemy.
    [Range(1.0f, 3.0f)]
    public float numOfEnemies = 1.0f;               // The number of enemies of the upcoming game.
    public List<EnemyAI> enemies;                   // A list of all enemies.
    public List<Material> enemyMaterials;           // A list of enemy materials;


    [Header("Scoring")]
    public int score;                               // The current score of the player.

    private TMP_Text scoreText;                     // The score text of the player.


    [Header("Timing")]
    public float timeRemaining = 60.0f;             // The time remaining in the game.

    private Slider timer;                           // The timer.

    // ====================================================== Methods
    /// <summary>
    /// On game start...
    /// </summary>
    void Start()
    {
        // If the player is not set, set it
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        // If music not found, set it
        if (musicPlayer == null)
            musicPlayer = GetComponent<AudioSource>();

        // Set the song
        if (musicChoices.Count != 0)
            musicPlayer.clip = musicChoices[Random.Range(0, musicChoices.Count)];

        // Modify the background music
        musicPlayer.volume = 0.5f;
        musicPlayer.loop = true;
        musicPlayer.Play();

        // Set the cameras
        if (overheadCamera == null)
            overheadCamera = GameObject.Find("Overhead Camera").GetComponent<Camera>();
        if (ballShoulderCamera == null)
            ballShoulderCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        if (ballFPSCamera == null)
            ballFPSCamera = GameObject.Find("FPS Camera").GetComponent<Camera>();

        // Disable all but the shoulder camera
        overheadCamera.enabled = false;
        ballFPSCamera.enabled = false;
        currentCamera = ballShoulderCamera;

        // Get the overhead locator
        overheadLocator = GameObject.Find("Overhead Locator").GetComponent<ParticleSystem>();
        overheadLocator.Stop();

        // Find the mouse looker
        mouseLook = FindObjectOfType<MyMouseLook>();
        mouseLook.enabled = false;

        // Find the back button and disable it
        backButton = GameObject.Find("Back To Title Button");
        backButton.SetActive(false);

        // Find the score text
        scoreText = GameObject.Find("Score Number").GetComponent<TMP_Text>();

        // Find the timer
        timer = GameObject.Find("Timer").GetComponent<Slider>();

        // Set the respawn point
        if (respawnPoint == null)
            respawnPoint = GameObject.Find("Respawn Point").transform;

        // Get the title event manager and attempt to get some info
        titleEventManager = FindObjectOfType<TitleEventManager>();
        if (titleEventManager != null)
        {
            numOfEnemies = titleEventManager.numOfEnemies;
            difficulty = titleEventManager.difficulty;
        }
        CreateEnemies();
    }

    /// <summary>
    /// On frame update...
    /// </summary>
    void Update()
    {
        // If Z key pressed, pause game
        if (Input.GetKeyDown(KeyCode.Z) && timeRemaining > 0.0f)
        {
            PauseGame(isGameRunning);
            isGameRunning = !isGameRunning;
        }

        // If escape key pressed, quit game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        // Check to see if the camera keys are pressed
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchCamera(1);

        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchCamera(2);

        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchCamera(3);

        // Decrese the time remaining
        if (timeRemaining > 0.0f && isGameRunning)
        {
            timeRemaining -= Time.deltaTime;
            timer.value = timeRemaining;
        }
        // If time is below 0, check who won
        else if (isGameRunning)
        {
            isGameRunning = false;
            DisplayWinner();
        }
    }

    /// <summary>
    /// Switches the camera to a provided number.
    /// </summary>
    /// <param name="cameraNum"></param>
    public void SwitchCamera(int cameraNum)
    {
        // Disble the current camera
        currentCamera.enabled = false;
        
        // Swtich the current camera
        if (cameraNum == 1)
        {
            currentCamera = ballShoulderCamera;
            overheadLocator.Stop();
            mouseLook.enabled = false;
            player.transform.rotation = Quaternion.Euler(Vector3.zero);
        }
        else if (cameraNum == 2)
        {
            currentCamera = overheadCamera;
            overheadLocator.Play();
            mouseLook.enabled = false;
            player.transform.rotation = Quaternion.Euler(Vector3.zero);
        }
        else if (cameraNum == 3)
        {
            currentCamera = ballFPSCamera;
            overheadLocator.Stop();
            mouseLook.enabled = true;
        }

        // Re-enable the current camera
        currentCamera.enabled = true;
    }

    /// <summary>
    /// Modifies the score by a provided amount.
    /// </summary>
    /// <param name="amount">The amount to modify the score by.</param>
    public void ModifyScore(int amount)
    {
        // Modify the score and update the score text.
        score += amount;
        scoreText.SetText(score.ToString());
    }

    /// <summary>
    /// Retrieves the current active camera.
    /// </summary>
    /// <returns></returns>
    public Camera GetActiveCamera()
    {
        return currentCamera;
    }

    /// <summary>
    /// Displays the winner of that round.
    /// </summary>
    public void DisplayWinner()
    {
        // Mute the player audio source
        player.GetComponent<AudioSource>().volume = 0.0f;

        // Pause the game
        PauseGame(true);

        // Hide the "pause" and bring up the winner text
        GameObject.Find("Pause Text").GetComponent<TMP_Text>().enabled = false;
        GameObject.Find("Standings Text").GetComponent<TMP_Text>().enabled = true;
        GameObject.Find("Score Label").GetComponent<TMP_Text>().enabled = false;
        GameObject.Find("Score Number").GetComponent<TMP_Text>().enabled = false;
        //GameObject.Find("Pause Overlay").GetComponent<Image>().material.color = new Color(0, 0, 0, 1);

        // ==================================== Calcuate the winner!
        // Create a dictionary based on the materials of each player
        Dictionary<Material, int> scores = new Dictionary<Material, int>();

        // Add the player
        scores.Add(player.GetComponentInChildren<Renderer>().material, score);

        // Add all enemies
        foreach (EnemyAI enemyAI in enemies)
        {
            scores.Add(enemyAI.gameObject.GetComponentInChildren<Renderer>().material, enemyAI.enemyScore);
        }

        // Get the rankings text
        TMP_Text rankings = GameObject.Find("Rankings Text").GetComponent<TMP_Text>();
        rankings.text = "";

        // Compare the scores and add them to the rankings
        int totalNumOfPlayers = scores.Count;
        int placement = scores.Count;
        for (int i = 1; i < totalNumOfPlayers + 1; i++)
        {
            // Set the local variables
            rankings.text += i + ". ";
            int highestScore = int.MinValue;
            Material highestPlayerName = null;

            // Get the highest item remaining in the dictionary
            foreach(KeyValuePair<Material, int> entry in scores)
            {
                if (entry.Value > highestScore)
                {
                    highestScore = entry.Value;
                    highestPlayerName = entry.Key;
                }
            }

            // Add it to the text string, then remove the item from the dictionary
            if (highestPlayerName.name.Contains("Earth")) 
                placement = i;
            rankings.text += highestPlayerName.name.Substring(0, highestPlayerName.name.IndexOf(' ')) + " - " + highestScore + "\n";
            scores.Remove(highestPlayerName);
        }

        // If placement is first, change text to win
        if (placement == 1)
        {
            GameObject.Find("Status Text").GetComponent<TMP_Text>().text = "you win";
            musicPlayer.clip = winClip;
        }
        // Else, change text to lose
        else
        {
            GameObject.Find("Status Text").GetComponent<TMP_Text>().text = "you lose";
            musicPlayer.clip = loseClip;
        }

        // Show the scores
        musicPlayer.loop = false;
        musicPlayer.volume = 1.0f;
        musicPlayer.Play();
        GameObject.Find("Status Text").GetComponent<TMP_Text>().enabled = true;
        backButton.SetActive(true);
        rankings.enabled = true;

    }

    /// <summary>
    /// Method for pausing / unpausing the game, based on a boolean value
    /// </summary>
    /// <param name="pause"></param>
    public void PauseGame(bool pause)
    {
        // Stop all spawners
        foreach (ObjectMaker objectMaker in FindObjectsOfType<ObjectMaker>())
        {
            objectMaker.spawning = !pause;
        }

        // Disable all rigidbodies
        foreach (Rigidbody rigidbody in FindObjectsOfType<Rigidbody>())
        {
            rigidbody.detectCollisions = !pause;
            if (rigidbody.gameObject.name != "Fall Zone" && !rigidbody.gameObject.GetComponent<PickupObjectManager>().isPickedUp) 
                rigidbody.isKinematic = pause;
        }
        
        // Disable the player's movement controller
        player.GetComponent<SphereController>().enabled = !pause;

        // Disable all enemies' AI scripts
        foreach (EnemyAI enemyAI in enemies)
        {
            enemyAI.enabled = !pause;
            if (pause)
            {
                enemyAI.gameObject.GetComponent<NavMeshAgent>().SetDestination(enemyAI.transform.position);
            }
            else
            {
                enemyAI.ChangeTarget();
            }
            
        }

        // Pause all particle systems
        foreach (ParticleSystem particleSystem in FindObjectsOfType<ParticleSystem>())
        {
            if (pause)
                particleSystem.Pause();
            else
                particleSystem.Play();
        }
        if (currentCamera != overheadCamera) overheadLocator.Stop();

        // Disable all fighter ship movement
        foreach (FighterShipMovement fighterShipMovement in FindObjectsOfType<FighterShipMovement>())
        {
            fighterShipMovement.enabled = !pause;
        }

        // Pause the music
        if (pause)
            musicPlayer.Pause();
        else
            musicPlayer.UnPause();

        // Toggle the visibilty of the pause overlay
        GameObject.Find("Pause Overlay").GetComponent<Image>().enabled = pause;
        GameObject.Find("Pause Text").GetComponent<TMP_Text>().enabled = pause;

        // Play the pause sfx
        player.GetComponent<SphereController>().PlaySound("pause");
    }

    /// <summary>
    /// Instantiates a number of enemies, based off the tite event manager's input
    /// </summary>
    public void CreateEnemies()
    {
        // Create x number of enemies
        for (int i = 0; i < numOfEnemies; i++)
        {
            // Create the enemy
            GameObject enemy = Instantiate(enemyPrefab, new Vector3(-72.0f + i * 5.0f, 1.3f, 0.0f), Quaternion.identity);

            // Set the material of the enemy
            if (i < enemyMaterials.Count)
                enemy.GetComponentInChildren<Renderer>().material = enemyMaterials[i];

            // Set the movement speed based on the difficulty
            enemy.GetComponent<NavMeshAgent>().speed = 3.5f * (difficulty / 3.0f) + (difficulty / 3.0f);

            // Add this to the list of enemies
            enemies.Add(enemy.GetComponent<EnemyAI>());
        }
    }

    /// <summary>
    /// Go back to the title screen.
    /// </summary>
    public void BackToTitle()
    {
        SceneManager.LoadScene("SpaceTitle");
    }
}
