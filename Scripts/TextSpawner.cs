using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextSpawner : MonoBehaviour
{
    public TMP_InputField inputField;
    public GameObject textPrefab;
    public Canvas canvas;

    public RectTransform[] spawnAreas; // Array of spawn areas
    public string[] allowedWords;

    private List<RectTransform> placedTexts = new List<RectTransform>();
    private List<string> placedWords = new List<string>();

    EnglishMinigame1 englishMinigame1Script;

    void Start()
    {
        inputField.onEndEdit.AddListener(SubmitWord);
        englishMinigame1Script = GetComponentInParent<EnglishMinigame1>();
    }

    void SubmitWord(string word)
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (!string.IsNullOrEmpty(word) && IsWordAllowed(word) && !IsWordAlreadyPlaced(word))
            {
                PlaceRandomText(word);

                placedWords.Add(word.ToLower());
                englishMinigame1Script.score++;
                inputField.text = "";  

            }
            else if (IsWordAlreadyPlaced(word))
            {
                Debug.LogWarning("Entered word is already placed on the screen.");
            }
            else
            {
                Debug.LogWarning("Entered word is not allowed.");
            }
        }
    }

    bool IsWordAllowed(string word)
    {
        foreach (string allowedWord in allowedWords)
        {
            if (word.Equals(allowedWord, System.StringComparison.OrdinalIgnoreCase))
            {
                return true; 
            }
        }
        return false; 
    }

    bool IsWordAlreadyPlaced(string word)
    {
        return placedWords.Contains(word.ToLower());
    }

    void PlaceRandomText(string word)
    {
        GameObject newTextObj = Instantiate(textPrefab, canvas.transform);
        TextMeshProUGUI newText = newTextObj.GetComponent<TextMeshProUGUI>();
        newText.text = word;

        RectTransform newTextRect = newText.GetComponent<RectTransform>();
        Vector2 randomPosition = GetRandomPosition(newTextRect);

        newTextRect.anchoredPosition = randomPosition;
        placedTexts.Add(newTextRect);
    }

    Vector2 GetRandomPosition(RectTransform newTextRect)
    {
        RectTransform selectedArea = spawnAreas[Random.Range(0, spawnAreas.Length)];
        Vector2 randomPos;
        bool overlap;

        int maxAttempts = 100;
        int attempts = 0;
        Vector2 anchoredPos = Vector2.zero;

        do
        {
            randomPos = new Vector2(
                Random.Range(selectedArea.rect.xMin, selectedArea.rect.xMax),
                Random.Range(selectedArea.rect.yMin, selectedArea.rect.yMax)
            );

            anchoredPos = selectedArea.anchoredPosition + randomPos;

            newTextRect.anchoredPosition = anchoredPos;
            overlap = false;

            foreach (RectTransform placedText in placedTexts)
            {
                if (RectOverlaps(newTextRect, placedText))
                {
                    overlap = true;
                    break;
                }
            }

            attempts++;
            if (attempts >= maxAttempts)
            {
                Debug.LogWarning("Could not find a non-overlapping position after multiple attempts.");
                break;
            }

        } while (overlap);

        return anchoredPos; // Return the last checked position
    }

    bool RectOverlaps(RectTransform rect1, RectTransform rect2)
    {
        Rect rect1Bounds = new Rect(rect1.anchoredPosition - rect1.rect.size / 2, rect1.rect.size);
        Rect rect2Bounds = new Rect(rect2.anchoredPosition - rect2.rect.size / 2, rect2.rect.size);
        return rect1Bounds.Overlaps(rect2Bounds);
    }

}
