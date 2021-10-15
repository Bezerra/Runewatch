#if UNITY_EDITOR
    using UnityEditor;
#endif

using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptableobject for spells creation.
/// </summary>
[CreateAssetMenu(menuName = "Spells/New Spell", fileName = "Spell Name")]
[InlineEditor]
public class SpellSO : ScriptableObject, ISpell
{
    [BoxGroup("General")]
    [HorizontalGroup("General/Split", 72)]
    [HideLabel, PreviewField(72)] [SerializeField] private Texture icon;

    [VerticalGroup("General/Split/Middle", 1), LabelWidth(60)]
    [InlineButton("ChangeFileName", "Update File Name")]
    [SerializeField] private new string name = "New Spell";

    [VerticalGroup("General/Split/Middle", 1), LabelWidth(60)]
    [Tooltip("Single ID for every spell. No spells should have the same ID")]
    [SerializeField] private byte spellID;

    [VerticalGroup("General/Split/Right", 2)]
    [HideLabel, TextArea(4, 6), SerializeField]
    private string description;

    /// ---------------
    [BoxGroup("Spell General Attributes")]
    [SerializeField, EnumToggleButtons, HideLabel]
    [Tooltip("Element of the spell.")]
    private ElementType element;

    [BoxGroup("Spell General Attributes")]
    [Tooltip("Mana cost of OneShotCast spells when used or every hit of a ContinuousCast spell")]
    [Range(0.01f, 100f)] [SerializeField] private float manaCost;

    /// ---------------

    [BoxGroup("Damage")]
    [SerializeField, EnumToggleButtons]
    //[DisableIf("spellCastType", SpellCastType.ContinuousCast)]
    [Tooltip("Type of damage of the spell")]
    private SpellDamageType damageType;

    [BoxGroup("Damage")]
    [EnableIf("damageType", SpellDamageType.AreaDamage)]
    [Tooltip("When this option is turned on, remember that MaxTime is the time the spell will remain active on the area.")]
    [SerializeField] private bool appliesDamageOvertime;

    [BoxGroup("Damage")]
    //[EnableIf("@this.spellCastType == SpellCastType.ContinuousCast || this.damageType == SpellDamageType.Overtime")]
    [Tooltip("Interval between damage with Overtime damage spells or interval between damage with continuous spells.")]
    [Range(0.01f, 100)] [SerializeField] private float timeInterval;

    [BoxGroup("Damage")]
    [Tooltip("Time duration of overtime spell damage.")]
    [Range(0.1f, 100)] [SerializeField] private float maxTime;

    [BoxGroup("Damage")]
    [DisableIf("damageType", SpellDamageType.Self)]
    [Tooltip("Radius of effect after an AreaDamage spell hits something")]
    [Range(2f, 10f)] [SerializeField] private float areaOfEffect;

    [BoxGroup("Damage")]
    [Tooltip("Random damage between these 2 values")]
    [SerializeField] [RangeMinMax(0, 100)] private Vector2 damage;

    /// ---------------

    [BoxGroup("Spell Type")]
    [SerializeField, EnumToggleButtons]
    [Tooltip("OneShotCast spells are spells that are spawned once. Continuous spells are spells that keep being spawned")]
    private SpellCastType spellCastType;

    [BoxGroup("Spell Type")]
    [EnableIf("spellCastType", SpellCastType.OneShotCast)]
    [DisableIf("damageType", SpellDamageType.Self)]
    [Tooltip("Speed of the spell")]
    [Range(1f, 100)] [SerializeField] private float speed;

    // Continuous spells have cooldown too (ex. player equiped a spell, it has 1 seconds cooldown until it's possible to use it)
    [BoxGroup("Spell Type")]
    [Tooltip("Cooldown of the spells WHEN EQUIPED and OneShotCasts spells after being used")]
    [Range(0, 10)] [SerializeField] private float cooldown;

    [BoxGroup("Behaviours")]
    [SerializeField] private DamageBehaviourAbstractSO damageBehaviour;

    [BoxGroup("Behaviours")]
    [Tooltip("What happens after the spell is spawned")]
    [SerializeField] private SpellBehaviourAbstractSO spellBehaviour;

    [BoxGroup("Behaviours")]
    [Tooltip("What happens after the spell hit prefab is spawned (spawned after the spell hits something)")]
    [SerializeField] private SpellOnHitBehaviourSO onHitBehaviour;

    [BoxGroup("Behaviours")]
    [Tooltip("Attack behaviour of this spell")]
    [SerializeField] private AttackBehaviourAbstractSO attackBehaviour;

    [BoxGroup("Prefabs")]
    [Tooltip("Spell prefab (vfx)")]
    [SerializeField] private GameObject spellPrefab;
    [BoxGroup("Prefabs")]
    [Tooltip("Spawns when the spell hits something")]
    [SerializeField] private GameObject spellHitPrefab;
    [BoxGroup("Prefabs")]
    [Tooltip("Spawns when the spell is cast")]
    [SerializeField] private GameObject spellMuzzlePrefab;
    [BoxGroup("Prefabs")]
    [Tooltip("Effect that will be shown in player's hand")]
    [SerializeField] private GameObject spellHandEffectPrefab;

#if UNITY_EDITOR
    private void ChangeFileName()
    {
        string assetPath = AssetDatabase.GetAssetPath(this.GetInstanceID());
        AssetDatabase.RenameAsset(assetPath, name);
        AssetDatabase.SaveAssets();
    }
#endif

    // Properties
    public Texture Icon => icon;
    public string Name => name;
    public byte SpellID => spellID;
    public ElementType Element => element;
    public float ManaCost => manaCost;
    public SpellDamageType DamageType => damageType;
    public bool AppliesDamageOvertime => appliesDamageOvertime;
    public float TimeInterval { get => timeInterval; set => timeInterval = value; }
    public float MaxTime { get => maxTime; set => maxTime = value; }
    public float AreaOfEffect { get => areaOfEffect; set => areaOfEffect = value; }
    public float Damage => Random.Range(damage.x, damage.y);
    public SpellCastType CastType { get => spellCastType; set => spellCastType = value; }
    public float Speed { get => speed; set => speed = value; }
    public float Cooldown => cooldown;
    public float CooldownCounter { get; set; }
    public SpellOnHitBehaviourSO OnHitBehaviour => onHitBehaviour;
    public SpellBehaviourAbstractSO SpellBehaviour => spellBehaviour;
    public AttackBehaviourAbstractSO AttackBehaviour => attackBehaviour;
    public DamageBehaviourAbstractSO DamageBehaviour => damageBehaviour;
    /// <summary>
    /// Item1 is spell Prefab. Item2 is spell hit prefab. Item 3 is spell muzzle prefab.
    /// </summary>
    public (GameObject, GameObject, GameObject, GameObject) Prefab => (spellPrefab, spellHitPrefab, spellMuzzlePrefab, spellHandEffectPrefab);
}
