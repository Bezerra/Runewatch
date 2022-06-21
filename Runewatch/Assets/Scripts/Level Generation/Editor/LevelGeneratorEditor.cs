using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelGenerator))]
public class LevelGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LevelGenerator levelGenerator = (LevelGenerator)target;

        GUILayout.Label("Button ONLY for ProceduralGenerationDemonstration scene");
        GUILayout.BeginHorizontal();
        GUILayout.Label("Must be in playmode");

        if (GUILayout.Button("New Generation"))
        {
            levelGenerator.StartCoroutine(levelGenerator.
                ResetGeneration("Generating new random level.", null));

            SelectionBase[] characters = FindObjectsOfType<SelectionBase>();
            if (characters.Length > 0)
            {
                foreach (SelectionBase sb in characters)
                    Destroy(sb.gameObject);

                DeathScreen deathScreen = FindObjectOfType<DeathScreen>();
                if (deathScreen != null) 
                    deathScreen.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        GUILayout.EndHorizontal();
        base.OnInspectorGUI();
    }
}