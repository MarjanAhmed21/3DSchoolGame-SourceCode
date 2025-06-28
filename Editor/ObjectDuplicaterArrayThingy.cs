#if UNITY_EDITOR 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ObjectDuplicaterArrayThingy : EditorWindow
{
    // Public fields to set in the editor window
    public GameObject objectToDuplicate;
    public float x_Offset = 0.0f;
    public float y_Offset = 0.0f;
    public float z_Offset = 0.0f;

    public int numberOfDuplicates = 1;

    // List to store duplicated objects
    private List<GameObject> duplicatedObjects = new List<GameObject>();

    // Menu item to display the window in the Unity Editor under the "Tools" menu
    [MenuItem("Tools/Duplicate Tool or basically Array Modifier from Blender lmao")]
    public static void ShowWindow()
    {
        GetWindow<ObjectDuplicaterArrayThingy>("Duplicate Tool / Array Modifier");
    }

    // Draw the GUI elements in the window
    void OnGUI()
    {
        GUILayout.Label("Duplicate GameObject", EditorStyles.boldLabel);

        // Object field to select the GameObject to duplicate
        objectToDuplicate = (GameObject)EditorGUILayout.ObjectField("Object to Duplicate", objectToDuplicate, typeof(GameObject), true);

        // Float fields for offsets and integer field for number of duplicates
        x_Offset = EditorGUILayout.FloatField("X Offset", x_Offset);
        y_Offset = EditorGUILayout.FloatField("Y Offset", y_Offset);
        z_Offset = EditorGUILayout.FloatField("Z Offset", z_Offset);
        numberOfDuplicates = EditorGUILayout.IntField("Duplicate Count", numberOfDuplicates);

        // Button to trigger duplication
        if (GUILayout.Button("Duplicate"))
        {
            DuplicateObject();
        }
    }

    // Method to handle object duplication
    void DuplicateObject()
    {
        if (objectToDuplicate == null)
        {
            Debug.LogWarning("No object selected to duplicate.");
            return;
        }

        // Remove extra duplicates if the number of duplicates has decreased
        while (duplicatedObjects.Count > numberOfDuplicates)
        {
            DestroyImmediate(duplicatedObjects[duplicatedObjects.Count - 1]);
            duplicatedObjects.RemoveAt(duplicatedObjects.Count - 1);
        }

        // Add new duplicates if needed
        while (duplicatedObjects.Count < numberOfDuplicates)
        {
            Vector3 newPosition = objectToDuplicate.transform.position + new Vector3(
                (duplicatedObjects.Count + 1) * x_Offset,
                (duplicatedObjects.Count + 1) * y_Offset,
                (duplicatedObjects.Count + 1) * z_Offset);

            // Instantiate a new duplicate object and set its name
            GameObject newObject = Instantiate(objectToDuplicate, newPosition, objectToDuplicate.transform.rotation);
            newObject.name = objectToDuplicate.name + " (Duplicate " + (duplicatedObjects.Count + 1) + ")";

            // Add the new object to the list of duplicated objects
            duplicatedObjects.Add(newObject);
        }
    }
}

#endif // End of UNITY_EDITOR check
