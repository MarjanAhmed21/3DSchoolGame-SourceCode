using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelection : MonoBehaviour
{

    public GameObject cam1;
    public GameObject cam2;
    public GameObject cam3;

    public GameObject parent1;
    public GameObject parent2;
    public GameObject parent3;

    public GameObject transitScreen;
    // Start is called before the first frame update
    void Start()
    {
        if (cam1 || cam2 || cam3 || parent1 || parent2 || parent3 == null)
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*private void OnTriggerStay(Collider other)
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if(currentScene.name == "Changing Room")
                {
                    GoToOutdoorTrack();
                }
                else
                {
                    GoToMenu();
                }
                
            }
        }
    }*/

    private void OnTriggerStay(Collider other)
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                transitScreen.SetActive(true);
            }
        }
    }

    public void TitleToMode()
    {
        cam1.SetActive(false);
        parent1.SetActive(false);

        cam2.SetActive(true);
        parent2.SetActive(true);
    }

    public void ModeToTitle()
    {
        cam1.SetActive(true);
        parent1.SetActive(true);

        cam2.SetActive(false);
        parent2.SetActive(false);
    }

    public void TitleToHelp()
    {
        cam1.SetActive(false);
        parent1.SetActive(false);

        cam3.SetActive(true);
        parent3.SetActive(true);
    }

    public void HelpToTitle()
    {
        cam1.SetActive(true);
        parent1.SetActive(true);

        cam3.SetActive(false);
        parent3.SetActive(false);
    }

    public void CloseScreen()
    {
        transitScreen.SetActive(false);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void GoToNeighbourhood()
    {
        SceneManager.LoadScene("Neighbourhood");
    }

    public void GoToInsideSchool()
    {
        SceneManager.LoadScene("Inside School");
    }

    public void GoToOutsideSchool()
    {
        SceneManager.LoadScene("Outside School");
    }


    public void GoToEnglish()
    {
        SceneManager.LoadScene("English Classroom");
    }

    public void GoToMaths()
    {
        SceneManager.LoadScene("Maths Classroom");
    }

    public void GoToScience()
    {
        SceneManager.LoadScene("Science Classroom");
    }

    public void GoToChanging()
    {
        SceneManager.LoadScene("Changing Room");
    }

    public void GoToOutdoorTrack()
    {
        SceneManager.LoadScene("Outdoor Track");
    }

    public void Quit()
    {
        Application.Quit();
    }


}
