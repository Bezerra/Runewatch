using UnityEngine;

/// <summary>
/// Class responsible for spawning a level generator in scene.
/// Used for demonstration only.
/// </summary>
public class GenerateLevelDemonstration : MonoBehaviour
{
    [SerializeField] private LevelGenerator levelGenerators;

    private void Start()
    {
        GameObject levelGenerated = Instantiate(levelGenerators.gameObject);
        LevelGenerator levelGeneratedScript = levelGenerated.GetComponent<LevelGenerator>();
        Destroy(levelGenerators.gameObject);

        levelGeneratedScript.GetValues();
        levelGeneratedScript.StartGeneration();
    }
}
