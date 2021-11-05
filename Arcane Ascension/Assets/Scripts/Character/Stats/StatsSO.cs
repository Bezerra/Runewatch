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
[CreateAssetMenu(menuName = "Stats/Common Stats", fileName = "Common Stats")]
public class StatsSO : ScriptableObject
{
    [PropertySpace(15)]
    [InlineButton("ChangeFileName", "Update File Name")]
    [SerializeField] private new string name;
    [SerializeField] [TextArea(3, 3)] private string notes;

    [BoxGroup("General Stats")]
    [EnumToggleButtons] [SerializeField] private ElementType element;

    [BoxGroup("General Stats")]
    [Range(1, 2000)] [SerializeField] private float defaultHealth;
    
    [BoxGroup("Damage Stats")]
    [DetailedInfoBox("Base Damage percentage of spell damage", "Base Damage percentage of spell damage")]
    [Range(1, 2)] [SerializeField] private float baseDamageMultiplier;
    [BoxGroup("Damage Stats")]
    [Range(1f, 2f)] [SerializeField] private float ignisMultiplier;
    [BoxGroup("Damage Stats")]
    [Range(1f, 2f)] [SerializeField] private float fulgurMultiplier;
    [BoxGroup("Damage Stats")]
    [Range(1f, 2f)] [SerializeField] private float aquaMultiplier;
    [BoxGroup("Damage Stats")]
    [Range(1f, 2f)] [SerializeField] private float terraMultiplier;
    [BoxGroup("Damage Stats")]
    [Range(1f, 2f)] [SerializeField] private float naturaMultiplier;
    [BoxGroup("Damage Stats")]
    [Range(1f, 2f)] [SerializeField] private float luxMultiplier;
    [BoxGroup("Damage Stats")]
    [Range(1f, 2f)] [SerializeField] private float umbraMultiplier;
    [BoxGroup("Damage Stats")]
    [Header("Chance of critical hit with each attack")]
    [Range(0, 1)] [SerializeField] private float criticalChance;
    [BoxGroup("Damage Stats")] [Required("A spell is required")]
    [Header("Character list of spells")]
    [SerializeField] private List<SpellSO> availableSpells;

    public string Name => name;
    public float MaxHealth { get; set; }
    
    public float BaseDamageMultiplier { get => baseDamageMultiplier; set => baseDamageMultiplier = value; }
    public float CriticalChance { get => criticalChance; set => criticalChance = value; }
    public ElementType Element => element;
    public List<SpellSO> AvailableSpells => availableSpells;

    // Dictionary for elements damage multiplier
    private IDictionary<ElementType, float> damageElementMultiplier;
    public IDictionary<ElementType, float> DamageElementMultiplier => damageElementMultiplier;

    protected virtual void OnEnable()
    {
        // If the game wasn't loaded it loads default stats
        MaxHealth = defaultHealth;

        damageElementMultiplier = new Dictionary<ElementType, float>
        {
            { ElementType.Fire, ignisMultiplier }, { ElementType.Electric, fulgurMultiplier },
            { ElementType.Water, aquaMultiplier }, { ElementType.Earth, terraMultiplier },
            { ElementType.Nature, naturaMultiplier }, { ElementType.Light, luxMultiplier },
            { ElementType.Dark, umbraMultiplier },
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
