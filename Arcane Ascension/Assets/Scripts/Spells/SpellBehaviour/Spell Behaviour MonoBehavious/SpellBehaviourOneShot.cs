using UnityEngine;

/// <summary>
/// Parent spell behaviour for one shot spells.
/// </summary>
public class SpellBehaviourOneShot : SpellBehaviourAbstract
{
    // Variables to control spell behaviour
    public float TimeSpawned { get; private set; }
    public float TimeOfImpact { get; private set; }
    public Rigidbody Rb { get; private set; }
    public SphereCollider ColliderSphere { get; private set; }

    /// <summary>
    /// Gets SpellBehaviourAbstract as SpellBehaviourAbstractOneShotSO.
    /// </summary>
    private SpellBehaviourAbstractOneShotSO SpellBehaviour => spell.SpellBehaviour as SpellBehaviourAbstractOneShotSO;

    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        ColliderSphere = GetComponent<SphereCollider>();
    }

    private void OnEnable()
    {
        TimeSpawned = Time.time;
    }

    /// <summary>
    /// Method called after instantiating the spell.
    /// Must be called manually through this method instead of OnEnable or Start in order to prevent bugs.
    /// </summary>
    public override void TriggerStartBehaviour() =>
        SpellBehaviour.StartBehaviour(this);

    private void Update() =>
        SpellBehaviour.ContinuousUpdateBehaviour(this);

    private void FixedUpdate() =>
        SpellBehaviour.ContinuousFixedUpdateBehaviour(this);

    private void OnTriggerEnter(Collider other)
    {
        TimeOfImpact = Time.time;
        SpellBehaviour.HitBehaviour(other, this);

        SpawnGizmosAreaSpell(); // TEMPPPPPPPPPPPPPPPPPPPPP
    }

    // TEMPPPPPPPPPPPPPPPPPPPPPPPPP /////////////////////////////////////
    // TEMPPPPPPPPPPPPPPPPPPPPPPPPP /////////////////////////////////////
    // TEMPPPPPPPPPPPPPPPPPPPPPPPPP /////////////////////////////////////
    [SerializeField] GameObject tempSpellHitTESTS;
    public void SpawnGizmosAreaSpell()
    {
        if (tempSpellHitTESTS != null && spell.DamageType == SpellDamageType.AreaDamage)
        {
            GameObject temp = Instantiate(tempSpellHitTESTS, transform.position, Quaternion.identity);
            temp.GetComponent<GizmosAreaSpell>().SpellRange = spell.AreaOfEffect;
        }
    }
    // TEMPPPPPPPPPPPPPPPPPPPPPPPPP /////////////////////////////////////
    // TEMPPPPPPPPPPPPPPPPPPPPPPPPP /////////////////////////////////////
    // TEMPPPPPPPPPPPPPPPPPPPPPPPPP /////////////////////////////////////
}
