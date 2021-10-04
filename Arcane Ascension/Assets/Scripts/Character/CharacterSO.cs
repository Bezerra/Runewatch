using UnityEngine;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Scriptable object character whole information.
/// </summary>
public abstract class CharacterSO : ScriptableObject
{
    [PropertySpace(15)]
    [InlineButton("ChangeFileName", "Update File Name")]
    [SerializeField] protected new string name;

    [SerializeField] protected CharacterValuesSO characterValues;

    [PropertySpace(20)]
    [SerializeField] protected StatsSO characterStats;

    public CharacterValuesSO CharacterValues => characterValues;
    public StatsSO CharacterStats => characterStats;

#if UNITY_EDITOR
    private void ChangeFileName()
    {
        string assetPath = AssetDatabase.GetAssetPath(this.GetInstanceID());
        AssetDatabase.RenameAsset(assetPath, name);
        AssetDatabase.SaveAssets();
    }
#endif
}
