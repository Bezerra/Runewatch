using UnityEngine;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Abstract Scriptable object with general values for characters.
/// </summary>
[InlineEditor]
public abstract class CharacterValuesSO : ScriptableObject
{
    [PropertySpace(15)]
    [InlineButton("ChangeFileName", "Update File Name")]
    [SerializeField] private new string name;

    // Type of this character
    [SerializeField] private CharacterType characterType;
    public CharacterType Type => characterType;

    // Movement speed
    [Range(1, 10)] [SerializeField] protected float speed;
    public float Speed => speed;


#if UNITY_EDITOR
    private void ChangeFileName()
    {
        string assetPath = AssetDatabase.GetAssetPath(this.GetInstanceID());
        AssetDatabase.RenameAsset(assetPath, name);
        AssetDatabase.SaveAssets();
    }
#endif
}
