using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelGenerator))]
public class LevelGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LevelGenerator levelGenerator = (LevelGenerator)target;

        GUILayout.Label("Button only for ProceduralGenerationDemonstration scene");
        GUILayout.BeginHorizontal();
        GUILayout.Label("Must be in playmode");

        if (GUILayout.Button("New Generation"))
        {
            levelGenerator.StartCoroutine(levelGenerator.
                ResetGeneration("Generating new random level.", null));
        }
        GUILayout.EndHorizontal();
        base.OnInspectorGUI();
    }
}