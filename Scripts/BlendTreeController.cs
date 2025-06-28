using UnityEngine;

public class BlendTreeController : MonoBehaviour
{
    // Reference to the Animator component
     Animator animator;




    // Name of the blend tree parameter controlling blending
    public string blendParameter = "Blend";

    // Index of the selected motion from the dropdown (in the editor)
    public int selectedMotionIndex = 0;

    // The names of the motions in the blend tree
    public string[] motionNames;

    private void Start()
    {
        animator = GetComponent<Animator>();
        // Automatically play the selected motion as soon as the game starts
        PlaySelectedMotion();
    }



    // Method to set the blend tree's threshold value
    void SetBlendTreeThreshold(float value)
    {
        if (animator != null)
        {
            animator.SetFloat(blendParameter, value);
        }
    }

    // Call this to trigger the selected motion
    public void PlaySelectedMotion()
    {
        if (motionNames != null && motionNames.Length > selectedMotionIndex)
        {
            Debug.Log($"Playing motion: {motionNames[selectedMotionIndex]}");
            SetBlendTreeThreshold(selectedMotionIndex); // Map the dropdown index to blend tree threshold value
        }
    }

}
