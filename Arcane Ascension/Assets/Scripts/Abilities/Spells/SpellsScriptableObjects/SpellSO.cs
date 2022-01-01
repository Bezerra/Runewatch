#if UNITY_EDITOR
    using UnityEditor;
#endif

using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

/// <summary>
/// Scriptableobject for spells creation.
/// </summary>
[InlineEditor]
public class SpellSO : ScriptableObject, ISpell
{
    [BoxGroup("General")]
    [HorizontalGroup("General/Split", 72)]
    [HideLabel, PreviewField(72)] [SerializeField] protected Sprite icon;

    [VerticalGroup("General/Split/Middle", 1), LabelWidth(60)]
    [InlineButton("ChangeFileName", "Update File Name")]
    [SerializeField] protected new string name = "New Spell";

    [VerticalGroup("General/Split/Middle", 1), LabelWidth(60)]
    [Tooltip("Spell tier")]
    [Range(1, 3)] [SerializeField] private int spellTier;

    [VerticalGroup("General/Split/Right", 2)]
    [HideLabel, TextArea(4, 6), SerializeField] protected string description;

    /// ---------------
    [BoxGroup("Spell Attributes")]
    [HorizontalGroup("Spell Attributes/Split", 36)]
    [HideLabel, PreviewField(36)] [SerializeField] protected Sprite elementIcon;

    [VerticalGroup("Spell Attributes/Split/Middle", 1), LabelWidth(60)]
    [Tooltip("Element of the spell.")]
    [SerializeField, EnumToggleButtons, HideLabel] protected ElementType element;

    [VerticalGroup("Spell Attributes/Split/Middle", 1), LabelWidth(60)]
    [Tooltip("Mana cost of OneShotCast spells when used or every hit of a ContinuousCast spell")]
    [Range(0.01f, 100f)] [SerializeField] protected float manaCost;

    /// ---------------
    [BoxGroup("Damage")]
    [HorizontalGroup("Damage/Split", 120)]
    [HideLabel, PreviewField(120)] [SerializeField] protected Sprite targetTypeIcon;

    [VerticalGroup("Damage/Split/Middle", 1), LabelWidth(120)]
    [Tooltip("Interval between damage with Overtime damage spells or interval between damage with continuous spells.")]
    [Range(0.01f, 100)] [SerializeField] protected float timeInterval;

    [VerticalGroup("Damage/Split/Middle", 1), LabelWidth(120)]
    [Tooltip("Time duration of overtime spell damage.")]
    [Range(0.1f, 100)] [SerializeField] protected float maxTime;

    [VerticalGroup("Damage/Split/Middle", 1), LabelWidth(120)]
    [Tooltip("Radius of effect after an AreaDamage spell hits something")]
    [Range(1f, 10f)] [SerializeField] protected float areaOfEffect;

    [VerticalGroup("Damage/Split/Middle", 1), LabelWidth(120)]
    [Tooltip("Used for player damage. Random damage between these 2 values")]
    [SerializeField] [RangeMinMax(0, 200f)] protected Vector2 playerDamage;

    [VerticalGroup("Damage/Split/Middle", 1), LabelWidth(120)]
    [Tooltip("Used for enemy damage. Random damage between these 2 values")]
    [SerializeField] [RangeMinMax(0, 200f)] protected Vector2 enemyDamage;

    [VerticalGroup("Damage/Split/Middle", 1), LabelWidth(120)]
    [Tooltip("Delay to apply damage. This is usually used for area spells, that require some" +
        "time to explode.")]
    [SerializeField] [Range(0, 100f)] protected float delayToDoDamage;

    /// ---------------

    //[BoxGroup("Spell Type")]
    //[SerializeField, EnumToggleButtons]
    //[Tooltip("OneShotCast spells are spells that are spawned once. Continuous spells are spells that keep being spawned")]
    //private SpellCastType spellCastType;

    [BoxGroup("Spell Type")]
    [Tooltip("Type of this spell.")]
    [SerializeField] private SpellCastType spellCastType;

    // >>>>>>>>>>> 40 IS THE MAXIMUM FOR THE CURRENT FIXEDDELTATIME <<<<<<<<<<<<<<<<<<<
    [BoxGroup("Spell Type")]
    [Tooltip("Speed of the spell.")]
    [Range(0.01f, 40f)] [SerializeField] protected float speed;

    // Continuous spells have cooldown too (ex. player equiped a spell, it has 1 seconds cooldown until it's possible to use it)
    [BoxGroup("Spell Type")]
    [Tooltip("Cooldown of the spells WHEN EQUIPED. One Shot cooldowns after fire.")]
    [Range(0, 10)] [SerializeField] protected float cooldown;

    [BoxGroup("Spell Type")]
    [Tooltip("Maximum distance of a spell")]
    [Range(2, 45f)] [SerializeField] protected float maximumDistance;

    [BoxGroup("Prefabs")]
    [Tooltip("Spell prefab (vfx)")]
    [SerializeField] protected GameObject spellPrefab;
    [BoxGroup("Prefabs")]
    [Tooltip("Spawns when the spell hits something")]
    [SerializeField] protected GameObject spellHitPrefab;
    [BoxGroup("Prefabs")]
    [Tooltip("Spawns when the spell is cast")]
    [SerializeField] protected GameObject spellMuzzlePrefab;
    [BoxGroup("Prefabs")]
    [Tooltip("Effect that will be shown in player's hand")]
    [SerializeField] protected GameObject spellHandEffectPrefab;
    [BoxGroup("Prefabs")]
    [EnableIf("@spellCastType == SpellCastType.OneShotCastWithRelease")]
    [Tooltip("Effect that will be shown for spells that have an area target vfx")]
    [SerializeField] protected GameObject areaHoverPrefab;

    [BoxGroup("Behaviours")]
    [Tooltip("What kind of damage will it be")]
    [SerializeField] protected DamageBehaviourAbstractSO damageBehaviour;

    [BoxGroup("Behaviours")]
    [Tooltip("What kind of status will this spell cause")]
    [SerializeField] protected StatusBehaviourAbstractSO statusBehaviour;

    [BoxGroup("Behaviours")]
    [Tooltip("What happens after the spell is spawned. Should contain at least a movement and apply damage behaviour")]
    [SerializeField] protected List<SpellBehaviourAbstractSO> spellBehaviourOneShot;

    [BoxGroup("Behaviours")]
    [Tooltip("Attack behaviour of this spell")]
    [SerializeField] protected AttackBehaviourAbstractSO attackBehaviourOneShot;

    [BoxGroup("Behaviours Of Muzzle and Hit prefabs")]
    [Tooltip("What happens after the spell hit prefab is spawned (spawned after the spell hits something)")]
    [SerializeField] protected List<SpellOnHitBehaviourAbstractSO> onHitBehaviourOneShot;

    [BoxGroup("Behaviours Of Muzzle and Hit prefabs")]
    [Tooltip("What happens after the spell hit prefab is spawned (spawned after the spell hits something)")]
    [SerializeField] protected List<SpellMuzzleBehaviourAbstractSO> muzzleBehaviourOneShot;

    [BoxGroup("Sounds")]
    [SerializeField] protected SpellSound sounds;

    [BoxGroup("Sounds")]
    [TableList] [Header("Sounds for spells that hit surfaces ONLY")]
    [SerializeField] protected List<SoundAssetWithType> surfaceSounds;

    // Properties
    public Sprite Icon => icon;
    public Sprite ElementIcon => elementIcon;
    public Sprite TargetTypeIcon => targetTypeIcon;
    public string Name => name;
    public byte ID { get; set; }
    public int Tier => spellTier;
    public string Description => description;
    public ElementType Element => element;
    public float ManaCost => manaCost;
    public float TimeInterval { get => timeInterval; set => timeInterval = value; }
    public float MaxTime { get => maxTime; set => maxTime = value; }
    public float AreaOfEffect { get => areaOfEffect; set => areaOfEffect = value; }
    public float DelayToDoDamage => delayToDoDamage;
    public Vector2 MinMaxDamage => playerDamage;
    public SpellCastType CastType => spellCastType;
    public float Speed { get => speed; set => speed = value; }
    public float Cooldown => cooldown;
    public float CooldownCounter { get; set; }
    public float MaximumDistance => maximumDistance;
    public float Damage(CharacterType characterType)
    {
        float finalDamage = 0;
        if (characterType == CharacterType.Player) finalDamage = Random.Range(playerDamage.x, playerDamage.y);
        else finalDamage = Random.Range(enemyDamage.x, enemyDamage.y);
        return finalDamage;
    }

    public  IList<SpellBehaviourAbstractSO> SpellBehaviourOneShot => 
        spellBehaviourOneShot;

    public  IList<SpellOnHitBehaviourAbstractSO> OnHitBehaviourOneShot => 
        onHitBehaviourOneShot;

    public  IList<SpellMuzzleBehaviourAbstractSO> MuzzleBehaviourOneShot => 
        muzzleBehaviourOneShot;

    public  AttackBehaviourAbstractSO AttackBehaviour => 
        attackBehaviourOneShot;

    public DamageBehaviourAbstractSO DamageBehaviour => 
        damageBehaviour;

    public StatusBehaviourAbstractSO StatusBehaviour => 
        statusBehaviour;

    /// <summary>
    /// Item1 is spell Prefab. Item2 is spell hit prefab. Item 3 is spell muzzle prefab.
    /// Item4 is spell hand effect, Item5 is area hover prefab.
    /// </summary>
    public (GameObject, GameObject, GameObject, GameObject, GameObject) Prefab => 
        (spellPrefab, spellHitPrefab, spellMuzzlePrefab, spellHandEffectPrefab, areaHoverPrefab);

    /// <summary>
    /// Sounds of spell events.
    /// </summary>
    public SpellSound Sounds => sounds;

    /// <summary>
    /// Dictionary with possible surfaces and sounds.
    /// </summary>
    public IDictionary<SurfaceType, AbstractSoundSO> SurfaceSounds { get; private set; }

    public void Initialize()
    {
        SurfaceSounds = new Dictionary<SurfaceType, AbstractSoundSO>();
        if(surfaceSounds.Count > 0)
        {
            foreach(SoundAssetWithType sound in surfaceSounds)
            {
                SurfaceSounds.Add(sound.SurfaceType, sound.SurfaceSound);
            }
        }
    }

#if UNITY_EDITOR
    protected void ChangeFileName()
    {
        string assetPath = AssetDatabase.GetAssetPath(this.GetInstanceID());
        AssetDatabase.RenameAsset(assetPath, name);
        AssetDatabase.SaveAssets();
    }
#endif
}
