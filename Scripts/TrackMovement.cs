using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TrackMovement : MonoBehaviour
{
    public SplineAnimate splineAnimate; // Reference to the SplineAnimate component
    public float baseSpeed = 1f;         // The base speed at which the character moves
    public float topSpeed = 5f;         // Maximum speed the character can reach
    public float staminaLimit = 4f;
    public float acceleration = 0.5f;       // Acceleration when space is pressed
    public float deceleration = 1f;       // Deceleration when space is not pressed
    

    [SerializeField] private float currentSpeed;           // Current speed of the character
    public float currentProgress = 0f;   // Current progress along the spline

    public Animator anim;

    private bool spacePressed = false;

    public Slider staminaBar;
    [SerializeField] bool isPaused;
    [SerializeField] float pauseSeconds = 1f;

    [SerializeField] float beforePauseSeconds = 3f;



    void Start()
    {

        // Initialize current speed
        currentSpeed = baseSpeed;


        anim = GetComponent<Animator>();

        baseSpeed = splineAnimate.MaxSpeed;


    }

    void Update()
    {
        // Update animator speed parameter
        anim.SetFloat("Blend", currentSpeed); // Set the speed parameter in the animator

        HandleInput();
        UpdateSplineMovement();


    }

    void HandleInput()
    {
        // Check if the space bar is pressed and handle speed increase
        if (Input.GetKeyDown(KeyCode.Space) && !isPaused)
        {
            // Increase speed up to maxSpeed on space press
            currentSpeed += acceleration; // Increase speed immediately
            if (currentSpeed > topSpeed)
            {
                currentSpeed = topSpeed; // Clamp to maximum speed
            }
            spacePressed = true; // Mark that the space was pressed
        }

        // Check if the space bar is released
        if (Input.GetKeyUp(KeyCode.Space))
        {
            spacePressed = false; // Mark that the space was released
        }

        // Decelerate if the space bar is not being pressed
        if (!spacePressed)
        {
            currentSpeed -= deceleration * Time.deltaTime; // Decrease speed gradually
            if (currentSpeed < baseSpeed)
            {
                currentSpeed = baseSpeed; // Clamp to base speed
            }
        }

        if (staminaBar.value != currentSpeed)
        {
            staminaBar.value = currentSpeed; 
        }

        if (currentSpeed > staminaLimit && !isPaused)
        {
            
            StartCoroutine(PauseSpaceBar());
        }
    }

    IEnumerator PauseSpaceBar()
    {
        // Wait for `beforePauseSeconds` while monitoring the staminaBar
        float elapsedTime = 0f;

        // While we're waiting, we keep checking the stamina value
        while (elapsedTime < beforePauseSeconds)
        {
            // If the stamina drops below the limit, cancel the pause
            if (staminaBar.value < staminaLimit)
            {
                yield break; // Exit the coroutine, don't pause the space bar
            }

            elapsedTime += Time.deltaTime; // Increment the elapsed time
            yield return null; // Wait for the next frame
        }

        isPaused = true;

        yield return new WaitForSeconds(pauseSeconds);

        isPaused = false;
    }


    void UpdateSplineMovement()
    {
        // Update the progress along the spline based on the current speed
        currentProgress += (currentSpeed / 100) * Time.deltaTime; // Adjust progress based on speed


        // Loop progress back to start when it exceeds 1
        if (currentProgress > 1f)
        {
            currentProgress -= 1f; // Loop the progress
        }

        // Set the normalized time based on the current progress
        splineAnimate.NormalizedTime = currentProgress; // Set normalized time correctly
    }

}
