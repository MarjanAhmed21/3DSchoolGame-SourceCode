using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    public TextMeshProUGUI countdownTimerText;
    public TextMeshProUGUI elapsedTimeText;

    public float elapsedTimer;
    public float remainingTime;

    public bool startElapsedTimer;
    public bool startCountdown;
    // Start is called before the first frame update
    void Start()
    {
        if (countdownTimerText == null)
        {
            Debug.Log("Nothing has been assigned to the countdowntimer text property lmao");
        }

        if (elapsedTimeText == null)
        {
            Debug.Log("Nothing has been assigned to the elapsedTimer text property lmao");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (startCountdown)
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
            }

            else if (remainingTime < 0)
            {
                remainingTime = 0;
            }

            int mins = Mathf.FloorToInt(remainingTime / 60);
            int secs = Mathf.FloorToInt(remainingTime % 60);
            countdownTimerText.text = string.Format("{0:00}:{1:00}", mins, secs);
        }

        if (startElapsedTimer)
        {
            elapsedTimer += Time.deltaTime;
            int mins = Mathf.FloorToInt(elapsedTimer / 60);
            int secs = Mathf.FloorToInt(elapsedTimer % 60);
            elapsedTimeText.text = string.Format("{0:00}:{1:00}", mins, secs);
        }
    }
}
