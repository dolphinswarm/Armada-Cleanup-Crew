using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows for the player sphere to be controlled.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class SphereController : MonoBehaviour
{
    // ====================================================== Properties
    [Header("Game Settings")]
    private GameManager gameManager;                // The game manager.


    [Header("Controller Properties")]
    public float movementSpeed = 5.0f;              // The movement speed of this sphere.
    public float jumpSpeed = 0.5f;                  // The jump speed (height) of this sphere.
    public float slideLimit = 15.0f;                // The slope limit before this object stars sliding

    private CharacterController controller;         // The character controller.
    public float fallSpeed;                         // The current fall speed of this sphere.
    private float maxFallSpeed = 10.0f;             // The maximum fall speed of the object.
    private Vector3 moveVector = Vector3.zero;      // The movement vector that is used in the character controller move.
    private Vector3 contactSurfaceNormal;           // The normal of the contacting surface.
    private float scale = 1.0f;                     // The amount of scaling on the current GameObject.


    [Header("Sound Effects")]
    public AudioClip jump;                          // The sound which plays when the character jumps.
    public AudioClip fall;                          // The sound for when the ball falls.
    public AudioClip collect;                       // The sound for when an object is collected.
    public AudioClip pause;                         // The sound for pausing the game
    private AudioSource ballAudioSource;            // The audiosource of the child camera

    // ====================================================== Methods
    /// <summary>
    /// On game start...
    /// </summary>
    void Start()
    {
        // Set the game manager
        gameManager = FindObjectOfType<GameManager>();

        // Set the character controller
        controller = GetComponent<CharacterController>();

        // Set the audio source
        ballAudioSource = GetComponent<AudioSource>();

        // Set the amx fall speed based on the gravity modifier
        maxFallSpeed *= gameManager.gravityModifier;
    }

    /// <summary>
    /// On frame update...
    /// </summary>
    void Update()
    {
        /* ========= MOVEMENT ========= */
        // Move forward if W or Up keys pressed
        //if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        //    moveVector += Vector3.forward * movementSpeed * Time.deltaTime * 0.05f * scale;

        //// Move backward if S or Down keys pressed
        //if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        //    moveVector += Vector3.back * movementSpeed * Time.deltaTime * 0.05f * scale;

        //// Move left if A or Left keys pressed
        //if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        //    moveVector += Vector3.left * movementSpeed * Time.deltaTime * 0.05f * scale;

        //// Move right if D or Right keys pressed
        //if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        //    moveVector += Vector3.right * movementSpeed * Time.deltaTime * 0.05f * scale;

        // Forward movement
        if (Input.GetKey(KeyCode.W))
            moveVector += transform.TransformDirection(Vector3.forward) * movementSpeed * Time.deltaTime * 0.05f * scale;

        // Backward movement
        if (Input.GetKey(KeyCode.S))
            moveVector += transform.TransformDirection(Vector3.back) * movementSpeed * Time.deltaTime * 0.05f * scale;

        // Left movement
        if (Input.GetKey(KeyCode.A))
            moveVector += transform.TransformDirection(Vector3.left) * movementSpeed * Time.deltaTime * 0.05f * scale;

        // Right movement
        if (Input.GetKey(KeyCode.D))
            moveVector += transform.TransformDirection(Vector3.right) * movementSpeed * Time.deltaTime * 0.05f * scale;



        /* ========= JUMPING / GRAVITY ========= */
        // If controller is not grounded, add fall speed to move vector
        if (!controller.isGrounded && fallSpeed < maxFallSpeed)
        {
            fallSpeed += Physics.gravity.y * Time.deltaTime * 0.05f * gameManager.gravityModifier;
        }
        // Else, if grounded..
        else
        {
            // If space key pressed, jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
                fallSpeed = jumpSpeed * 0.35f;
                PlaySound("jump");
            }
            // Else, set fall speed to 0 (i.e., not falling)
            else
            {
                fallSpeed = 0.0f;
            }
        }
        // Add fall speed to move vector
        moveVector.y = fallSpeed;



        /* ========= SLOPE MOVEMENT ========= */                                                            // IS BUGGY!
        //if (Vector3.Angle(Vector3.up, contactSurfaceNormal) >= slideLimit)
        //{
        //    moveVector.x += (1.0f - contactSurfaceNormal.y) * contactSurfaceNormal.x * 0.1f;
        //    moveVector.z += (1.0f - contactSurfaceNormal.y) * contactSurfaceNormal.z * 0.1f;
        //}



        /* ========= MOVEMENT ========= */
        // Move this controller
        controller.Move(moveVector);



        /* ========= FRICTION ========= */
        // X friction
        // If slightly above zero in movement, add friction
        if (Mathf.Abs(moveVector.x) > 0.001f) 
            moveVector.x += moveVector.x * -0.05f;
        // Else, clamp to 0
        else moveVector.x = 0.0f;

        // Z friction
        // If slightly above zero in movement, add friction
        if (Mathf.Abs(moveVector.z) > 0.001f)
            moveVector.z += moveVector.z * -0.05f;
        // Else, clamp to 0
        else moveVector.z = 0.0f;
    }

    /// <summary>
    /// On controller collider hit...
    /// </summary>
    /// <param name="hit"></param>
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Surface"))
            contactSurfaceNormal = hit.normal;
    }

    public void PlaySound(string sound)
    {
        // If sound is "jump"...
        if (sound == "jump")
        {
            ballAudioSource.clip = jump;
            ballAudioSource.Play();
        }
        // If sound is "fall"...
        else if (sound == "fall")
        {
            ballAudioSource.clip = fall;
            ballAudioSource.Play();
        }
        // If sound is "collect"...
        else if (sound == "collect")
        {
            ballAudioSource.clip = collect;
            ballAudioSource.Play();
        }
        // If sound is "collect"...
        else if (sound == "pause")
        {
            ballAudioSource.clip = pause;
            ballAudioSource.Play();
        }
    }
}
