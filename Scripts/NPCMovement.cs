using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class NPCMovement : MonoBehaviour
{
    SplineAnimate splineAnimate;
    public float movementSpeed;

    public bool loop = false;
    public float minimumTimeSpeedChange = 2f; // Minimum time between speed changes
    public float maximumTimeSpeedChange = 5f; // Maximum time between speed changes

    public float baseSpeed = 3f;  // Minimum forward speed
    public float maxSpeed = 5f;   // Maximum forward speed

    public float speedChangeDuration = 1.5f; // Time taken to transition to the new speed

    Animator anim;

    private float currentProgress = 0f;  // Tracks the NPC's progress on the spline

    void Start()
    {
        splineAnimate = GetComponent<SplineAnimate>();
        if (splineAnimate == null)
        {
            Debug.LogError("No SplineAnimate component found on the object!");
            return;
        }

        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogError("No Animator component found on the object!");
            return;
        }

        // Set the spline loop mode
        splineAnimate.Loop = loop ? SplineAnimate.LoopMode.Loop : SplineAnimate.LoopMode.Once;

        // Initialize the movement speed and progress
        splineAnimate.MaxSpeed = movementSpeed;
        splineAnimate.Play();

        // Start coroutine to change the speed at random intervals
        StartCoroutine(ChangeSpeedAtRandomIntervals());
    }

    void Update()
    {
        // Keep track of progress along the spline
        currentProgress = splineAnimate.NormalizedTime;
    }

    IEnumerator ChangeSpeedAtRandomIntervals()
    {
        while (true)
        {
            // Wait for a random delay before changing the speed again
            float randomDelay = Random.Range(minimumTimeSpeedChange, maximumTimeSpeedChange);
            yield return new WaitForSeconds(randomDelay);

            // Generate a random target speed
            float randomSpeed = Random.Range(baseSpeed, maxSpeed);

            // Smoothly transition to the new speed
            yield return StartCoroutine(SmoothSpeedChange(randomSpeed));

            Debug.Log("Spline speed smoothly changed to: " + randomSpeed);
        }
    }

    // Coroutine to smoothly change the speed over time
    IEnumerator SmoothSpeedChange(float targetSpeed)
    {
        float elapsedTime = 0f;
        float initialSpeed = splineAnimate.MaxSpeed;  // Capture the current speed

        // Gradually change the speed over the specified duration
        while (elapsedTime < speedChangeDuration)
        {
            elapsedTime += Time.deltaTime;

            // Interpolate the speed (Lerp)
            splineAnimate.MaxSpeed = Mathf.Lerp(initialSpeed, targetSpeed, elapsedTime / speedChangeDuration);

            // Update the animator's "Blend" parameter to reflect the current speed
            anim.SetFloat("Blend", splineAnimate.MaxSpeed);

            // Wait for the next frame
            yield return null;
        }

        // Ensure the final speed is set to the target speed
        splineAnimate.MaxSpeed = targetSpeed;

        // Update animator to ensure consistency
        anim.SetFloat("Blend", splineAnimate.MaxSpeed);
    }
}
