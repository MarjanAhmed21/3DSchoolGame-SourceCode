using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using StarterAssets;

public class EnglishMinigame1 : MonoBehaviour
{
    public GameObject titleParent, infoParent, beginParent;
    public GameObject[] novelDisplays;

    public GameObject[] questions;

    public BeginMinigame beginMinigameScript;


    public int score = 0;
    public TextMeshProUGUI scoreDisplayed;
    public int totalScore;

    public TMP_Dropdown[] Q6dropdowns;

    public TMP_Dropdown[] Q7dropdowns;

    public Button nextQuestionBtn1;
    public Button nextQuestionBtn2;

    public Button nextQuestionBtn3;
    public Button nextQuestionBtn4;

    CountdownTimer timeScript;


    public int[] correctAnswersForQ6;
    public int[] correctAnswersForQ7;


    // Start is called before the first frame update
    void Start()
    {
        timeScript = GetComponent<CountdownTimer>();
        timeScript.enabled = false;


        titleParent.SetActive(true);
        infoParent.SetActive(false);
        beginParent.SetActive(false);
        score = 0;

        foreach (GameObject question in questions)
        {
            if (question != null)
            {
                question.SetActive(false);
            }
        }

        foreach (GameObject novelDisplay in novelDisplays)
        {
            if (novelDisplay != null)
            {
                novelDisplay.SetActive(false);
            }
        }

        // Ensure buttons are initially inactive
        nextQuestionBtn1.gameObject.SetActive(false);
        nextQuestionBtn2.gameObject.SetActive(false);

        nextQuestionBtn3.gameObject.SetActive(false);
        nextQuestionBtn4.gameObject.SetActive(false);

        // Add listeners to Q6 dropdowns
        foreach (TMP_Dropdown dropdown in Q6dropdowns)
        {
            dropdown.onValueChanged.AddListener(delegate { CheckDropdownValues(Q6dropdowns, correctAnswersForQ6, nextQuestionBtn1); });
        }

        // Add listeners to Q7 dropdowns
        foreach (TMP_Dropdown dropdown in Q7dropdowns)
        {
            dropdown.onValueChanged.AddListener(delegate { CheckDropdownValues(Q7dropdowns, correctAnswersForQ7, nextQuestionBtn2); });
        }

        // Initial checks for both sets of dropdowns
        CheckDropdownValues(Q6dropdowns, correctAnswersForQ6, nextQuestionBtn1);
        CheckDropdownValues(Q7dropdowns, correctAnswersForQ7, nextQuestionBtn2);
    }

    // Update is called once per frame
    void Update()
    {

       

        if (questions[7].activeInHierarchy || questions[8].activeInHierarchy)
        {
            novelDisplays[0].SetActive(false);
            novelDisplays[1].SetActive(true);
            CheckTextPrefabsWithTag();
        }

        if (questions[9].activeInHierarchy)
        {
            scoreDisplayed.gameObject.SetActive(true);
            scoreDisplayed.text = score.ToString() + " / " + totalScore.ToString();
            novelDisplays[1].SetActive(false);
            timeScript.enabled = false;
            timeScript.countdownTimerText.gameObject.SetActive(false);
            timeScript.elapsedTimeText.gameObject.SetActive(true);
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
        novelDisplays[0].SetActive(true);
    }

    public void BeginToQ1()
    {
        beginParent.SetActive(false);
        questions[0].SetActive(true);
        timeScript.countdownTimerText.gameObject.SetActive(true);
        timeScript.startCountdown = true;
        timeScript.startElapsedTimer = true;
        timeScript.enabled = true;
    }

    public void RightAnswerButton()
    {
        score++;


        for (int i = 0; i < questions.Length; i++)
        {
            // Check if the current question is active
            if (questions[i].activeSelf)
            {
                // Deactivate the current question
                questions[i].SetActive(false);

                // Check if there is a next question in the array
                if (i + 1 < questions.Length)
                {
                    // Activate the next question
                    questions[i + 1].SetActive(true);
                }
                break;  // Exit the loop after finding the active question
            }
        }
    }

    public void WrongAnswerButton()
    {

        Debug.Log("WrongAnswerButton pressed.");

        if (questions[7].activeInHierarchy || questions[8].activeInHierarchy)
        {
            if (questions[7].activeInHierarchy)
            {
                Debug.Log("Question 7 is active. Attempting to delete text prefabs.");
            }

            if (questions[8].activeInHierarchy)
            {
                Debug.Log("Question 8 is active. Attempting to delete text prefabs.");
            }

            DestroyTextPrefabs();
        }


        for (int i = 0; i < questions.Length; i++)
        {
            // Check if the current question is active
            if (questions[i].activeSelf)
            {
                // Deactivate the current question
                questions[i].SetActive(false);

                // Check if there is a next question in the array
                if (i + 1 < questions.Length)
                {
                    // Activate the next question
                    questions[i + 1].SetActive(true);
                }
                break;  // Exit the loop after finding the active question
            }
        }
    }

    public void CloseGame()
    {
        beginMinigameScript.minigameActivated = false;
        beginMinigameScript.arrowForDoor.SetActive(true);
        beginMinigameScript.arrowForDoorRange.SetActive(true);
    }

    void DestroyTextPrefabs()
    {
        Debug.Log("Deleting all existing textprefabs");
        // Find all objects with the "TextPrefab" tag
        GameObject[] textPrefabs = GameObject.FindGameObjectsWithTag("TextPrefab");

        // Destroy each of the text prefabs
        foreach (GameObject textPrefab in textPrefabs)
        {
            Destroy(textPrefab);
        }

        if (textPrefabs.Length == 0)
        {
            Debug.LogWarning("No text prefabs found with the 'TextPrefab' tag.");
        }
    }


    void CheckDropdownValues(TMP_Dropdown[] dropdownArray, int[] correctAnswers, Button button)
    {
        bool isCorrect = true;
        bool allSelected = true;

        // Check if all dropdowns have non-placeholder (non-zero) values
        for (int i = 0; i < dropdownArray.Length; i++)
        {
            if (dropdownArray[i].value == 0)
            {
                allSelected = false;
                break;
            }
        }

        if (allSelected)
        {
            // If all dropdowns are selected, check if the answers are correct
            for (int i = 0; i < dropdownArray.Length; i++)
            {
                if (dropdownArray[i].value != correctAnswers[i])
                {
                    isCorrect = false;
                    break;
                }
            }

            // Remove previous listeners to avoid stacking
            button.onClick.RemoveAllListeners();

            if (isCorrect)
            {
                // All answers are correct
                button.onClick.AddListener(RightAnswerButton);
                Debug.Log("All answers correct! RightAnswerButton assigned.");
            }
            else
            {
                // Some answers are incorrect
                button.onClick.AddListener(WrongAnswerButton);
                Debug.Log("Some answers are incorrect. WrongAnswerButton assigned.");
            }

            // Activate the button only if all dropdowns are selected
            button.gameObject.SetActive(true);
        }
        else
        {
            // Deactivate the button if not all dropdowns are selected
            button.gameObject.SetActive(false);
            Debug.Log("Not all dropdowns are selected, button deactivated.");
        }
    }

    void CheckTextPrefabsWithTag()
    {
        // Find all objects with the "TextPrefab" tag
        GameObject[] textPrefabs = GameObject.FindGameObjectsWithTag("TextPrefab");

        bool anyTextPrefabActive = false;

        // Iterate over the text prefabs and check if any are active in the scene
        foreach (GameObject textPrefab in textPrefabs)
        {
            if (textPrefab.activeInHierarchy)  // Check if the prefab is active in the scene
            {
                anyTextPrefabActive = true;
                break;
            }
        }

        if (anyTextPrefabActive && questions[7].activeInHierarchy)
        {
            nextQuestionBtn3.gameObject.SetActive(true);
        }
        else
        {
            nextQuestionBtn3.gameObject.SetActive(false);
        }

        if (anyTextPrefabActive && questions[8].activeInHierarchy)
        {
            nextQuestionBtn4.gameObject.SetActive(true);
        }
        else
        {
            nextQuestionBtn4.gameObject.SetActive(false);
        }
    }
}