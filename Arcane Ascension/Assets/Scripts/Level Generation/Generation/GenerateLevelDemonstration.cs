using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        // Else loads saved data (after loading data it will start generation)
    }
}
