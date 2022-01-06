#if UNITY_EDITOR
using UnityEditor;
#endif


using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Rendering;

/// <summary>
/// Abstract scriptable object responsible for executing a status behaviour.
/// </summary>
[InlineEditor]
public abstract class StatusBehaviourAbstractSO : ScriptableObject
{
    [BoxGroup("General")]
    [HorizontalGroup("General/Split", 72)]
    [HideLabel, PreviewField(72)] [SerializeField] protected Sprite icon;

    [VerticalGroup("General/Split/Middle", 1), LabelWidth(60)]
    [InlineButton("ChangeFileName", "Update File Name")]
    [SerializeField] protected new string name = "New Spell";

    [VerticalGroup("General/Split/Right", 2)]
    [HideLabel, TextArea(4, 6), SerializeField] protected string description;

    [EnumToggleButtons]
    [SerializeField] protected StatusEffectType statusEffectType;

    [SerializeField] protected GameObject enemyVFX;
    [SerializeField] protected GameObject playerVFX;
    [SerializeField] protected VolumeProfile postProcess;

    [Range(0, 400f)] [SerializeField] protected float durationSeconds;

    /// <summary>
    /// Executes when prefab is enabled.
    /// </summary>
    public abstract void StartBehaviour(StatusBehaviour parent);

    /// <summary>
    /// Executes on update.
    /// </summary>
    public abstract void ContinuousUpdateBehaviour(StatusBehaviour parent);

#if UNITY_EDITOR
    protected void ChangeFileName()
    {
        string assetPath = AssetDatabase.GetAssetPath(this.GetInstanceID());
        AssetDatabase.RenameAsset(assetPath, name);
        AssetDatabase.SaveAssets();
    }
#endif
}
