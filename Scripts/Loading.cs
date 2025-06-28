using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    public GameObject LoadingScreen; 
    public Image LoadingBarFill;

    public GameObject transitScreen;
    private string sceneToLoad;

    private string currentSceneName;
    public bool transitScreenActive = false;

    private void Start()
    {
        if (transitScreen == null)
        {

        }

        currentSceneName = SceneManager.GetActiveScene().name;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if (transitScreenActive == true)
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                // Unlock the cursor
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    // This method will be called to start loading a scene by its name
    public void LoadScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            // Start the loading process
            StartCoroutine(LoadSceneAsync(sceneName));
        }
        else
        {
            Debug.LogError("Scene name to load is not assigned or empty!");
        }
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        // Start loading the scene asynchronously
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        
        if (transitScreen != null)
        {
            transitScreen.SetActive(false);
        }
        

        // Show the loading screen
        LoadingScreen.SetActive(true);

        

        while (!operation.isDone)
        {
            // Update the loading bar's fill amount based on the loading progress
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);

            if (LoadingBarFill != null)
                LoadingBarFill.fillAmount = progressValue;

            yield return null;
        }

    }

    // This method is called after the new scene is fully loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // After the scene is loaded, set the player's position based on scene transitions
        SetPlayerPositionInNewScene(scene.name);
    }

    private void SetPlayerPositionInNewScene(string targetSceneName)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player"); // Assume your player has the "Player" tag

        if (player != null)


        {

            CharacterController cc = player.GetComponent<CharacterController>();
            if (cc != null)
            {
                cc.enabled = false; // Disable CharacterController temporarily
            }
            switch (targetSceneName)
            {
                case "Inside School":
                    if (currentSceneName == "Outside School")
                    {
                        player.transform.position = new Vector3(334.9f, 1.113f, 355.95f);
                        player.transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                    else if (currentSceneName == "English Classroom")
                    {
                        player.transform.position = new Vector3(266.94f, 1.113f, 373.87f);
                        player.transform.rotation = Quaternion.Euler(0, 90, 0);
                    }
                    else if (currentSceneName == "Maths Classroom")
                    {
                        player.transform.position = new Vector3(403.44f, 10.51f, 373.81f);
                        player.transform.rotation = Quaternion.Euler(0, -90, 0);
                    }
                    else if (currentSceneName == "Science Classroom")
                    {
                        player.transform.position = new Vector3(266.94f, 10.51f, 373.87f);
                        player.transform.rotation = Quaternion.Euler(0, 90, 0);
                    }
                    else if (currentSceneName == "Changing Room")
                    {
                        player.transform.position = new Vector3(403.44f, 1.113f, 373.81f);
                        player.transform.rotation = Quaternion.Euler(0, -90, 0);
                    }
                    break;

                case "Outside School":
                    if (currentSceneName == "Neighbourhood")
                    {
                        player.transform.position = new Vector3(56.84f, 0.363f, 62.35f);
                        player.transform.rotation = Quaternion.Euler(0, 35, 0);
                    }
                    else if (currentSceneName == "Inside School")
                    {
                        player.transform.position = new Vector3(102.153f, 1.475f, 137.181f);
                        player.transform.rotation = Quaternion.Euler(0, 180, 0);
                    }
                    break;

                case "Neighbourhood":
                    if (currentSceneName == "Outside School")
                    {
                        player.transform.position = new Vector3(121f, 0.175f, 22.57f);
                        player.transform.rotation = Quaternion.Euler(0, -90, 0);
                    }
                    break;


            }
            if (cc != null)
            {
                cc.enabled = true;
            }
        }
        else
        {
            Debug.LogError("Player object not found in the scene!");
        }

        // Update the currentSceneName to the newly loaded scene
        currentSceneName = targetSceneName;
    }


    private void OnTriggerStay(Collider other)
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {

                transitScreen.SetActive(true);
                transitScreenActive = true;
            }
        }
    }

    // This button method will set the scene to load when a specific button is clicked
    public void OnButtonClicked(string sceneName)
    {
        // I can name the scene to load in the inspector of the button with this script and method assigned
        sceneToLoad = sceneName;

        // ohhoo and this is the method to start loading the scene
        LoadScene(sceneToLoad);
    }


    public void CloseScreen()
    {
        transitScreen.SetActive(false);
    }
}
