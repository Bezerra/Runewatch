#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object to create room weights
/// </summary>
[CreateAssetMenu(menuName = "Create Once/RoomWeights", fileName = "RoomWeights")]
[InlineEditor]
public class RoomWeightsSO : ScriptableObject
{
    [InlineButton("ChangeFileName", "Update File Name")]
    [SerializeField] protected new string name = "New Spell";

    [SerializeField] private List<Weight> piecesWithWeight;
    public List<Weight> PiecesWithWeight => piecesWithWeight;

#if UNITY_EDITOR

    [Button]
    private void CalculateAllWeights()
    {
        int value = 0;
        foreach (Weight weight in piecesWithWeight)
            value += weight.RoomWeight;

        Debug.Log(value);
    }

    protected void ChangeFileName()
    {
        string assetPath = AssetDatabase.GetAssetPath(this.GetInstanceID());
        AssetDatabase.RenameAsset(assetPath, name);
        AssetDatabase.SaveAssets();
    }
#endif
}

[System.Serializable]
public class Weight
{
    [SerializeField] private LevelPiece piece;
    [Range(0f, 100f)] [SerializeField] int weight;

    public string Name => piece.Name;
    public int RoomWeight => weight;
}
