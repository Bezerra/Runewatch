#if UNITY_EDITOR
using UnityEditor;
#endif


using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object with potions information.
/// </summary>
[InlineEditor]
[CreateAssetMenu(menuName = "Loot/Potion", fileName ="Potion")]
public class PotionSO : ScriptableObject
{
    [InlineButton("ChangeFileName", "Update File Name")]
    [SerializeField] protected new string name = "New Spell";

    [SerializeField] private PotionType potionType;
    [Range(0f, 100f)] [SerializeField] private float percentage;

    public PotionType PotionType => potionType;
    public float Percentage => percentage;

#if UNITY_EDITOR
    protected void ChangeFileName()
    {
        string assetPath = AssetDatabase.GetAssetPath(this.GetInstanceID());
        AssetDatabase.RenameAsset(assetPath, name);
        AssetDatabase.SaveAssets();
    }
#endif
}
