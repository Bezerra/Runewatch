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
    [InlineButton("ChangeFileName", "Update File Name")]
    [SerializeField] protected new string name;

    [SerializeField] protected CharacterValuesSO characterValues;
    [SerializeField] protected StatsSO characterStats;

    /// <summary>
    /// Values for editor logic.
    /// </summary>
    public CharacterValuesSO CharacterValues => characterValues;

    /// <summary>
    /// Character gameplay stats.
    /// </summary>
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
