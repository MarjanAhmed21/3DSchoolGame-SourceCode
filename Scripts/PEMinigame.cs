using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using StarterAssets;
using UnityEngine.Splines;
using UnityEngine.InputSystem;
using Cinemachine;

public class PEMinigame : MonoBehaviour
{
    public GameObject titleParent, infoParent, beginParent;



    public TrackMovement trackMovementScript;

    public GameObject staminaBar;

    CountdownTimer timeScript;

    public GameObject panelBG;
    public GameObject resultsScreen;

    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI timerResultsText;

    public TextMeshProUGUI positionResultText;

    public SplineAnimate playerSpline;

    public SplineAnimate[] npcSplines;
    public TrackMovementNPC[] trackMovementNPCs;

    [SerializeField]private int lapCount = 0; // To track the laps completed
    [SerializeField]private bool raceStarted = false; // Flag to track if the race has started
    [SerializeField] private bool hasCrossedFinishLine = false; // To detect when the player crosses the start/finish line

    public ThirdPersonController thirdPersonController;
    public CharacterController characterController;
    public PlayerInput playerInput;
    public Rigidbody rb;
    public GameObject playerCameraRootObj;
    public CinemachineVirtualCamera cinemachineVirtCam;

    public Animator animator;  // The animator component on the player or object
    public RuntimeAnimatorController newController;  // The new controller you want to assign

    public GameObject arrowForDoor;
    public GameObject arrowForDoorRange;

    public bool dontMakeNPCsRunWhenStart = false;


    public List<string> finishOrder = new List<string>();
    public bool raceEnded = false;


    // Start is called before the first frame update
    void Start()
    {
        timeScript = GetComponent<CountdownTimer>();
        timeScript.enabled = false;

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            // Unlock the cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        titleParent.SetActive(true);
        infoParent.SetActive(false);
        beginParent.SetActive(false);
        resultsScreen.SetActive(false);

        playerSpline.enabled = false;
        trackMovementScript.enabled = false;


        thirdPersonController.enabled = false;
        characterController.enabled = false;
        playerInput.enabled = false;


        foreach (SplineAnimate npcSpline in npcSplines)
        {
            if (npcSpline != null)
            {
                npcSpline.enabled = false;
            }
        }

        foreach (TrackMovementNPC trackMovementNPC in trackMovementNPCs)
        {
            if (trackMovementScript != null)
            {
                trackMovementNPC.enabled = false;
            }
        }



    }

    // Update is called once per frame
    void Update()
    {
        if (raceStarted)
        {
            CheckLapCompletion(); // Continuously check for lap completion
        }

    }



    public void TitleToInfo()
    {
        titleParent.SetActive(false);
        infoParent.SetActive(true);
    }

    public void InfoToBegin()
    {
        infoParent.SetActive(false);
        beginParent.SetActive(true);
        staminaBar.SetActive(true);
    }

    public void BeginDaGame()
    {
        beginParent.SetActive(false);
        panelBG.SetActive(false);
        timeScript.elapsedTimeText.gameObject.SetActive(true);
        StartCoroutine(StartCountdown());
        
    }

    // Coroutine for the 3-second countdown
    IEnumerator StartCountdown()
    {
        countdownText.gameObject.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }

        countdownText.text = "GO!";
        yield return new WaitForSeconds(1); // Display "GO!" for a second
        countdownText.gameObject.SetActive(false);

        Debug.Log("LMAOOO");

        // Start the game after the countdown
        timeScript.elapsedTimeText.gameObject.SetActive(true);
        timeScript.startElapsedTimer = true;
        timeScript.enabled = true;
        playerSpline.enabled = true;
        trackMovementScript.enabled = true;

        if (dontMakeNPCsRunWhenStart == false)
        {


            foreach (TrackMovementNPC trackMovementNPC in trackMovementNPCs)
            {
                if (trackMovementScript != null)
                {
                    trackMovementNPC.enabled = true;
                }
            }
        }

        //npcSplines[3].enabled = true;
        //trackMovementNpcScripts[3].enabled = true;
        foreach (SplineAnimate npcSpline in npcSplines)
        {
            if (npcSpline != null)
            {
                npcSpline.enabled = true;
            }
        }


        raceStarted = true; // Set race as started
    }

    void CheckLapCompletion()
    {

        // Check if the player crosses the finish line (from near 1 back to 0)
        if (trackMovementScript.currentProgress >= 0.95f)
        {
            hasCrossedFinishLine = true; // Player is near the end of the lap
        }
        else if (hasCrossedFinishLine && trackMovementScript.currentProgress <= 0.05f)
        {
            // Player has crossed the finish line and started a new lap
            hasCrossedFinishLine = false;
            lapCount++;

            Debug.Log("Lap completed! Current lap: " + lapCount);

            // Check if the player has completed 2 laps
            if (lapCount == 2)
            {
                // Add the player to the finish order if they haven't already finished
                if (!finishOrder.Contains("Player"))
                {
                    finishOrder.Add("Player");
                }

                EndRace();
            }
        }

        foreach (var npcTrackMovement in trackMovementNPCs)
        {
            if (npcTrackMovement.currentProgress >= 0.95f && !npcTrackMovement.hasCrossedFinishLine)
            {
                npcTrackMovement.hasCrossedFinishLine = true;
            }
            else if (npcTrackMovement.hasCrossedFinishLine && npcTrackMovement.currentProgress <= 0.05f)
            {
                npcTrackMovement.hasCrossedFinishLine = false;
                npcTrackMovement.npcLapCount++;

                if (npcTrackMovement.npcLapCount == 2)
                {
                    if (!finishOrder.Contains(npcTrackMovement.gameObject.name))
                    {
                        finishOrder.Add(npcTrackMovement.gameObject.name);
                    }
                }
            }
        }
    }

    void EndRace()
    {
        raceStarted = false;
        panelBG.SetActive(true);
        resultsScreen.SetActive(true);
        staminaBar.SetActive(false);
        timeScript.enabled = false; // Stop the timer
        timerResultsText.text = timeScript.elapsedTimeText.text;
        playerSpline.enabled = false;
        trackMovementScript.enabled = false;

        animator.runtimeAnimatorController = newController;
        thirdPersonController.enabled = true;
        characterController.enabled = true;
        Destroy(rb);
        playerCameraRootObj.transform.localPosition = new Vector3(0f, 1.35f, -0.1f);

        var thirdPersonFollow = cinemachineVirtCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        float newCamDist = 4.0f;
        thirdPersonFollow.CameraDistance = newCamDist;

        playerInput.enabled = true;

        arrowForDoor.SetActive(true);
        arrowForDoorRange.SetActive(true);

        raceEnded = true;

        // Find player's finishing position
        int playerPosition = finishOrder.IndexOf("Player") + 1; // Assuming "Player" is used in finishOrder for the player

        // Display the position result text based on the player's position
        switch (playerPosition)
        {
            case 1:
                positionResultText.text = "1st Place!";
                break;
            case 2:
                positionResultText.text = "2nd Place!";
                break;
            case 3:
                positionResultText.text = "3rd Place!";
                break;
            case 4:
                positionResultText.text = "4th Place!";
                break;
        }

        DisplayRaceResults();
    }


    void DisplayRaceResults()
    {
        Debug.Log("Race Results:");
        for (int i = 0; i < finishOrder.Count; i++)
        {
            Debug.Log((i + 1) + ": " + finishOrder[i]);
        }
    }


    public void CloseGame()
    {
        panelBG.SetActive(false);
        resultsScreen.SetActive(false);
        // Lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

}