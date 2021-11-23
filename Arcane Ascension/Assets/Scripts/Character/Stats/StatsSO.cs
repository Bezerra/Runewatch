using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Scriptable object with stats for a character.
/// </summary>
[InlineEditor]
public abstract class StatsSO : ScriptableObject
{
    [PropertySpace(15)]
    [InlineButton("ChangeFileName", "Update File Name")]
    [SerializeField] private new string name;
    [SerializeField] [TextArea(3, 3)] private string description;

    [BoxGroup("General Stats")]
    [HorizontalGroup("General Stats/Split", 35)]
    [HideLabel, PreviewField(35)] [SerializeField] protected Sprite icon;

    [VerticalGroup("General Stats/Split/Middle", 1), LabelWidth(60)]
    [EnumToggleButtons] [SerializeField] private ElementType element;

    [BoxGroup("General Stats")]
    [SerializeField] private CharacterType characterType;
    public CharacterType Type => characterType;

    [BoxGroup("General Stats")]
    [Range(1, 2000)] [SerializeField] private float defaultHealth;
    [BoxGroup("General Stats")]
    [Range(0f, 1f)] [SerializeField] private float damageResistance;
    [BoxGroup("General Stats")]
    [Range(0f, 2f)] [SerializeField] private float defaultMovementSpeedMultiplier;
    [BoxGroup("Damage Stats")]
    [DetailedInfoBox("Base Damage percentage of spell damage", "Base Damage percentage of spell damage")]
    [Range(0f, 2f)] [SerializeField] private float defaultBaseDamageMultiplier;
    [BoxGroup("Damage Stats")]
    [Range(1f, 2f)] [SerializeField] private float defaultNeutralMultiplier;
    [BoxGroup("Damage Stats")]
    [Range(1f, 2f)] [SerializeField] private float defaultIgnisMultiplier;
    [BoxGroup("Damage Stats")]
    [Range(1f, 2f)] [SerializeField] private float defaultFulgurMultiplier;
    [BoxGroup("Damage Stats")]
    [Range(1f, 2f)] [SerializeField] private float defaultAquaMultiplier;
    [BoxGroup("Damage Stats")]
    [Range(1f, 2f)] [SerializeField] private float defaultTerraMultiplier;
    [BoxGroup("Damage Stats")]
    [Range(1f, 2f)] [SerializeField] private float defaultNaturaMultiplier;
    [BoxGroup("Damage Stats")]
    [Range(1f, 2f)] [SerializeField] private float defaultLuxMultiplier;
    [BoxGroup("Damage Stats")]
    [Range(1f, 2f)] [SerializeField] private float defaultUmbraMultiplier;
    [BoxGroup("Damage Stats")]
    [Range(0f, 1f)] [SerializeField] private float defaultCriticalChance;
    [BoxGroup("Damage Stats")]
    [Range(1f, 2f)] [SerializeField] private float defaultCriticalDamageModifier;

    public Sprite Icon => icon;
    public string Name => name;
    public float MaxHealth { get; set; }
    public float DamageResistance { get; set; }
    public float BaseDamageMultiplier { get; set; }
    public float CriticalChance { get; set; }
    public float CriticalDamageModifier { get; set; }
    public float MovementSpeedMultiplier { get; set; }

    public ElementType Element => element;
    

    // Dictionary for elements damage multiplier
    private IDictionary<ElementType, float> damageElementMultiplier;
    public IDictionary<ElementType, float> DamageElementMultiplier => damageElementMultiplier;

    protected virtual void OnEnable()
    {
        // If the game wasn't loaded it loads default stats
        MaxHealth = defaultHealth;
        DamageResistance = 1 - damageResistance;
        BaseDamageMultiplier = defaultBaseDamageMultiplier;
        CriticalChance = defaultCriticalChance;
        CriticalDamageModifier = defaultCriticalDamageModifier;
        MovementSpeedMultiplier = defaultMovementSpeedMultiplier;

        damageElementMultiplier = new Dictionary<ElementType, float>
        {
            { ElementType.Fire, defaultIgnisMultiplier }, { ElementType.Electric, defaultFulgurMultiplier },
            { ElementType.Water, defaultAquaMultiplier }, { ElementType.Earth, defaultTerraMultiplier },
            { ElementType.Nature, defaultNaturaMultiplier }, { ElementType.Light, defaultLuxMultiplier },
            { ElementType.Dark, defaultUmbraMultiplier }, {ElementType.Neutral, defaultNeutralMultiplier },
        };
    }


#if UNITY_EDITOR
    private void ChangeFileName()
    {
        string assetPath = AssetDatabase.GetAssetPath(this.GetInstanceID());
        AssetDatabase.RenameAsset(assetPath, name);
        AssetDatabase.SaveAssets();
    }
#endif
}
