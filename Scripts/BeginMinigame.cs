using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class BeginMinigame : MonoBehaviour
{
    public GameObject screen;
    ThirdPersonController personController;
    public Camera mainCamera;
    public Camera minigameCamera;

    public bool minigameActivated;

    public GameObject arrowForSeat;
    public GameObject arrowForDoor;
    public GameObject arrowForDoorRange;

    public GameObject[] npcs;
    // Start is called before the first frame update
    void Start()
    {
        minigameActivated = false;
        personController = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonController>();


        if (npcs == null || npcs.Length == 0)
        {
            Debug.Log("There are no NPCs assigned in the field just to let you know lmao");
        }


        // Lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        

        if (minigameActivated == true)
        {
            screen.SetActive(true);
            personController.enabled = false;
            mainCamera.enabled = false;
            minigameCamera.enabled = true;
            foreach (GameObject npc in npcs)
            {
                npc.SetActive(false);
            }

            if (Cursor.lockState == CursorLockMode.Locked)
            {
                // Unlock the cursor
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        else
        {
            screen.SetActive(false);
            personController.enabled = true;
            mainCamera.enabled = true;
            minigameCamera.enabled = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                minigameActivated = true;
                arrowForSeat.SetActive(false);
            }
        }
    }
}
