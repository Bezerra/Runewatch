#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Abstract Scriptable object responsible for creating passive abilities.
/// </summary>
[InlineEditor]
public abstract class AbstractPassiveSO<T> : ScriptableObject, IPassive<T>
{
    [BoxGroup("General")]
    [HorizontalGroup("General/Split", 72)]
    [HideLabel, PreviewField(72)] [SerializeField] protected Sprite icon;

    [VerticalGroup("General/Split/Middle", 1), LabelWidth(60)]
    [InlineButton("ChangeFileName", "Update File Name")]
    [SerializeField] protected new string name = "New passive";

    [VerticalGroup("General/Split/Middle", 1), LabelWidth(60)]
    [Tooltip("Passive tier")]
    [Range(1, 7)] [SerializeField] protected int passiveTier;

    [VerticalGroup("General/Split/Middle", 1), LabelWidth(60)]
    [Tooltip("Does this ability affects or is affected by some stats")]
    [SerializeField] protected AffectsType relation;

    [VerticalGroup("General/Split/Right", 2)]
    [HideLabel, TextArea(4, 6), SerializeField] protected string description;

    public abstract T PassiveType { get; }
    public string Name => name;
    public string Description => description;
    public AffectsType Relation => relation;
    public int Tier => passiveTier;
    public byte ID { get; set; }
    public Sprite Icon => icon;

#if UNITY_EDITOR
    protected void ChangeFileName()
    {
        string assetPath = AssetDatabase.GetAssetPath(this.GetInstanceID());
        AssetDatabase.RenameAsset(assetPath, name);
        AssetDatabase.SaveAssets();
    }
#endif
}
