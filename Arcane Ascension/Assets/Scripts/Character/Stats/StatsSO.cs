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
[CreateAssetMenu(menuName = "Stats", fileName = "Stats")]
public class StatsSO : ScriptableObject
{
    [PropertySpace(15)]
    [InlineButton("ChangeFileName", "Update File Name")]
    [SerializeField] private new string name;
    [SerializeField] [TextArea(3, 3)] private string notes;

    [BoxGroup("General Stats")]
    [Range(1, 2000)] [SerializeField] private float defaultHealth;
    [BoxGroup("General Stats")]
    [Range(0, 2000)] [SerializeField] private float defaultArmor;
    [BoxGroup("General Stats")]
    [EnumToggleButtons] [SerializeField] private ElementType element;

    [BoxGroup("Mana Stats")]
    [Range(0, 2000)] [SerializeField] private float defaultMana;
    [BoxGroup("Mana Stats")]
    [Range(0.1f, 10)] [SerializeField] private float defaultManaRegenAmount;
    [BoxGroup("Mana Stats")]
    [Range(0.01f, 2)] [SerializeField] private float defaultManaRegenTime;

    [BoxGroup("Damage Stats")]
    [DetailedInfoBox("Base Damage percentage of spell damage", "Base Damage percentage of spell damage")]
    [Range(1, 2)] [SerializeField] private float baseDamageMultiplier;
    [BoxGroup("Damage Stats")] [Required("A spell is required")]
    [Header("If the character is an enemy, it will only fire OneShot cast type spells (ex: spells with Forward Behaviour)")]
    [SerializeField] private List<SpellSO> availableSpells;

    public string Name => name;
    public float MaxHealth { get; set; }
    public float MaxMana { get; set; }
    public float ManaRegenAmount { get; set; }
    public float ManaRegenTime { get; set; }
    public float MaxArmor { get; set; }
    public float BaseDamageMultiplier { get => baseDamageMultiplier; set => baseDamageMultiplier = value; }
    public ElementType Element => element;
    public List<SpellSO> AvailableSpells => availableSpells;

    private void OnEnable()
    {
        MaxHealth = defaultHealth;
        MaxMana = defaultMana;
        MaxArmor = defaultArmor;
        ManaRegenAmount = defaultManaRegenAmount;
        ManaRegenTime = defaultManaRegenTime;
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
