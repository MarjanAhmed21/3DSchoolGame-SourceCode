using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using StarterAssets;

public class MathsMinigame : MonoBehaviour
{
    public GameObject titleParent, infoParent, beginParent;
    public GameObject calculator;
    public GameObject calcActivate;
    [SerializeField] int state = 0;

    public TMP_InputField[] questionFields;
    public string[] fieldAnswers;
    public Button[] nextButton;
    public Button q8NxtBtn;

    public GameObject[] questions;

    ThirdPersonController personController;

    private int currentQuestionIndex = 0;


    public int score = 0;
    public TextMeshProUGUI scoreDisplayed;
    public int totalScore;

    public TMP_Dropdown[] Q8dropdowns;


    public BeginMinigame beginMinigameScript;


    CountdownTimer timeScript;


    public int[] correctAnswersForQ8;


    // Start is called before the first frame update
    void Start()
    {
        state = 0;

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

        if (questionFields.Length != nextButton.Length || fieldAnswers.Length != fieldAnswers.Length)
        {
            Debug.LogError("Question fields, nextButtons, and fieldAnswers arrays must be of the same length!");
            return;
        }

        for (int i = 0; i < questionFields.Length; i++)
        {
            int index = i;

            // Ensure the button is inactive initially
            nextButton[index].gameObject.SetActive(false);

            // Add a listener to the input field to handle changes
            questionFields[index].onValueChanged.AddListener((input) => OnInputFieldChanged(input, nextButton[index], fieldAnswers [index]));
        }

        // Add listeners to Q8 dropdowns
        foreach (TMP_Dropdown dropdown in Q8dropdowns)
        {
            dropdown.onValueChanged.AddListener(delegate { CheckDropdownValues(Q8dropdowns, correctAnswersForQ8, q8NxtBtn); });
        }


        // Initial checks for this set of dropdowns
        CheckDropdownValues(Q8dropdowns, correctAnswersForQ8, q8NxtBtn);



    }

    // Update is called once per frame
    void Update()
    {

        if (state == 1)
        {
            // Activate the GameObject
            if (!calculator.activeSelf)
                calculator.SetActive(true);

        }
        else if (state == 0)
        {
            // Deactivate the GameObject
            if (calculator.activeSelf)
                calculator.SetActive(false);
        }


        if (questions[15].activeInHierarchy)
        {
            scoreDisplayed.gameObject.SetActive(true);
            scoreDisplayed.text = score.ToString() + " / " + totalScore.ToString();
            calculator.SetActive(false);
            calcActivate.SetActive(false);
            timeScript.enabled = false;
            timeScript.countdownTimerText.gameObject.SetActive(false);
            timeScript.elapsedTimeText.gameObject.SetActive(true);
        }
    }

    public void CalculatorSetter()
    {

        // Toggle the state when button is clicked
        state = (state == 0) ? 1 : 0;
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

        state = 1;
        calcActivate.SetActive(true);
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
        // Increment the score for a correct answer
        score++;

        // Deactivate the current question
        questions[currentQuestionIndex].SetActive(false);

        // Move to the next question, if there is one
        if (currentQuestionIndex + 1 < questions.Length)
        {
            currentQuestionIndex++;
            questions[currentQuestionIndex].SetActive(true);
        }
        else
        {
            Debug.Log("No more questions available.");
        }
    }

    public void WrongAnswerButton()
    {
        Debug.Log("WrongAnswerButton pressed.");

        // Deactivate the current question
        questions[currentQuestionIndex].SetActive(false);

        // Move to the next question, if there is one
        if (currentQuestionIndex + 1 < questions.Length)
        {
            currentQuestionIndex++;
            questions[currentQuestionIndex].SetActive(true);
        }
        else
        {
            Debug.Log("No more questions available.");
        }
    }


    public void CloseGame()
    {
        beginMinigameScript.minigameActivated = false;
        beginMinigameScript.arrowForDoor.SetActive(true);
        beginMinigameScript.arrowForDoorRange.SetActive(true);
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


    // Method called when the input field value changes
    private void OnInputFieldChanged(string input, Button button, string correctAnswer)
    {
        if (string.IsNullOrEmpty(input))
        {
            // If the input is empty, deactivate the button
            button.gameObject.SetActive(false);
        }
        else
        {
            // Activate the button if input is present
            button.gameObject.SetActive(true);

            // Remove all previous listeners before adding new ones
            button.onClick.RemoveAllListeners();

            // Set the appropriate listener based on input
            if (input == correctAnswer)
            {
                button.onClick.AddListener(RightAnswerButton);
            }
            else
            {
                button.onClick.AddListener(WrongAnswerButton);
            }
        }
    }



}