using UnityEngine;

/// <summary>
/// Class responsible for spawning a level generator in scene.
/// Used for demonstration only.
/// </summary>
public class GenerateLevelDemonstration : MonoBehaviour
{
    [SerializeField] private LevelGenerator levelGenerators;
    private static GenerateLevelDemonstration instance;

    private void Start()
    {
        instance = this;
        GenerateDungeon();
    }

    public static void GenerateDungeon()
    {
        GameObject levelGenerated = Instantiate(instance.levelGenerators.gameObject);
        LevelGenerator levelGeneratedScript = levelGenerated.GetComponent<LevelGenerator>();
        Destroy(instance.levelGenerators.gameObject);

        levelGeneratedScript.GetValues();
        instance.StartCoroutine(levelGeneratedScript.StartGeneration());
    }
}
