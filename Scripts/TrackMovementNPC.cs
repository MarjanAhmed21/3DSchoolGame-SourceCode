using UnityEngine;
using UnityEngine.Splines;

public class TrackMovementNPC : MonoBehaviour
{
    SplineAnimate splineAnimate;  
    SplineContainer splineContainer;
    float splineLength;
    public float baseSpeed = 3f;    
    public float maxSpeed = 5f;          
    public float acceleration = 1f;      
    public float deceleration = 1f;       
    public float changeIntervalMin = 2f;  
    public float changeIntervalMax = 5f;  

    [SerializeField] private float currentSpeed;          
    public float currentProgress = 0f;  

    Animator anim;            

    private float targetSpeed;         
    private float timeUntilNextChange; 

    float lastSpeed;

    public int npcLapCount = 0; 
    public bool hasCrossedFinishLine = false; 
    public PEMinigame peMinigameScript; 



    void Start()
    {
        splineAnimate = GetComponent<SplineAnimate>();
        splineContainer = GetComponent<SplineAnimate>().Container;

        splineLength = splineContainer.Spline.GetLength();
        

        // Initialize current speed
        currentSpeed = baseSpeed;
        targetSpeed = baseSpeed;
        timeUntilNextChange = Random.Range(changeIntervalMin, changeIntervalMax);

        anim = GetComponent<Animator>();
       
    }

    void Update()
    {
        // Handle speed changes (called every frame for responsiveness)
        HandleSpeedChange();

        // Update the NPC's position along the spline in Update (no need for FixedUpdate)
        UpdateSplineMovement();

        // Only update the animator if the speed has changed
        if (Mathf.Abs(currentSpeed - lastSpeed) > 0.01f)  // Avoid small floating point errors
        {
            anim.SetFloat("Blend", currentSpeed);
            lastSpeed = currentSpeed;
        }

        CheckLapCompletion();
    }

    void CheckLapCompletion()
    {
        if (currentProgress >= 0.95f)
        {
            hasCrossedFinishLine = true;
        }
        else if (hasCrossedFinishLine && currentProgress <= 0.05f)
        {
          
            hasCrossedFinishLine = false;
            npcLapCount++;

            Debug.Log(gameObject.name + " completed lap: " + npcLapCount);

            // Check if the NPC has completed 2 laps
            if (npcLapCount == 2 && peMinigameScript != null && !peMinigameScript.raceEnded)
            {
                peMinigameScript.finishOrder.Add(gameObject.name); 
                Debug.Log(gameObject.name + " finished in position " + peMinigameScript.finishOrder.Count);
                StopNPCMovement(); 
            }
        }
    }

    void StopNPCMovement()
    {
        currentSpeed = 0f; // Stop the NPC's movement
        enabled = false; // Disable this script to stop further updates
    }



    void HandleSpeedChange()
    {
        // Decrease the timer until the next speed change
        timeUntilNextChange -= Time.deltaTime;

        // If it's time to change the speed
        if (timeUntilNextChange <= 0f)
        {
            // Set a new target speed between baseSpeed and maxSpeed
            targetSpeed = Random.Range(baseSpeed, maxSpeed);

            // Reset the timer for the next speed change
            timeUntilNextChange = Random.Range(changeIntervalMin, changeIntervalMax);
        }

        // Smoothly transition the current speed to the target speed
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed,
                                         (currentSpeed < targetSpeed ? acceleration : deceleration) * Time.deltaTime);
    }

    void UpdateSplineMovement()
    {

        // Calculate speed-based progress on the spline
        float distanceToMove = currentSpeed * Time.deltaTime; // Calculate how far the NPC moves based on speed

        // Update the current progress along the spline, normalized over spline length
        currentProgress += distanceToMove / splineLength;

        if (currentProgress >= 1f)
        {
            currentProgress %= 1f;
        }

        // Apply the calculated progress to the spline animator
        splineAnimate.NormalizedTime = Mathf.Clamp01(currentProgress);
    }
}
