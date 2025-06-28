#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

[CustomEditor(typeof(BlendTreeController))]
public class BlendTreeControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Reference to the actual controller script
        BlendTreeController controller = (BlendTreeController)target;

        // Draw the default inspector
        DrawDefaultInspector();

        Animator animator = controller.GetComponent<Animator>();

        if (animator != null && animator.runtimeAnimatorController != null)
        {
            // Cast the RuntimeAnimatorController to AnimatorController if possible
            AnimatorController animatorController = animator.runtimeAnimatorController as AnimatorController;

            if (animatorController != null)
            {
                // Get the layers from the AnimatorController
                AnimatorControllerLayer[] layers = animatorController.layers;

                foreach (var layer in layers)
                {
                    // Access each state's state machine and its states
                    foreach (var state in layer.stateMachine.states)
                    {
                        if (state.state.motion is BlendTree blendTree)
                        {
                            // Collect motion names from the blend tree
                            controller.motionNames = new string[blendTree.children.Length];
                            for (int i = 0; i < blendTree.children.Length; i++)
                            {
                                // Get the name of each motion in the blend tree
                                if (blendTree.children[i].motion != null)
                                {
                                    controller.motionNames[i] = blendTree.children[i].motion.name;
                                }
                            }

                            // Show a dropdown in the inspector to select a motion
                            controller.selectedMotionIndex = EditorGUILayout.Popup("Select Motion", controller.selectedMotionIndex, controller.motionNames);
                        }
                    }
                }
            }
            else
            {
                EditorGUILayout.HelpBox("AnimatorController could not be found or is not assigned.", MessageType.Warning);
            }
        }
        else
        {
            // Provide feedback if the Animator component or runtimeAnimatorController is null
            EditorGUILayout.HelpBox("Animator or RuntimeAnimatorController not assigned.", MessageType.Warning);
        }
    }
}

#endif // End of UNITY_EDITOR check