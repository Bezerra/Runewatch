using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable object responsible for applying piercing damage, meaning it won't disable after hiting the first enemy.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Apply Damage Pierce", 
    fileName = "Spell Behaviour Apply Damage Pierce")]
sealed public class SpellBehaviourApplyDamagePierceSO : SpellBehaviourAbstractOneShotSO
{
    [Space(20)]
    [Header("THIS BEHAVIOUR ALSO STOPS SPELL SPEED AFTER HIT QUANTITY IS REACHED")]
    [SerializeField] private TypeOfPierce typeOfPierce;
    [Range(2, 20)] [SerializeField] private byte hitQuantity;
    [Tooltip("Divides or multiplies damage by this factor, depending on type of pierce")]
    [Range(1, 4)] [SerializeField] private byte damageModifier = 2;

    private IList<int> layersToDamage;

    public override void StartBehaviour(SpellBehaviourOneShot parent)
    {
        layersToDamage = new List<int>
        {
            Layers.EnemyLayerNum,
            Layers.EnemySensiblePointNum,
            Layers.PlayerLayerNum,
        };
    }

    public override void ContinuousUpdateBeforeSpellBehaviour(SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }

    public override void ContinuousUpdateBehaviour(SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }

    public override void ContinuousFixedUpdateBehaviour(SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }

    public override void HitTriggerBehaviour(Collider other, SpellBehaviourOneShot parent)
    {
        int layerNumber = other.gameObject.layer;

        if (layersToDamage.Contains(layerNumber))
        {
            // Applies damage with a modifier
            parent.Spell.DamageBehaviour.Damage(parent, other, CalculateModifier(parent.CurrentPierceHitQuantity));

            // Adds a hit counter
            if (++parent.CurrentPierceHitQuantity >= hitQuantity)
                parent.Rb.velocity = Vector3.zero;
        }
        else if (layerNumber == Layers.EnemyImmuneLayerNum)
        {
            parent.Spell.DamageBehaviour.Damage(parent, other, 0);
        }
    }

    /// <summary>
    /// Gets damage modifier depending on the quantity received on the parameter.
    /// </summary>
    /// <param name="currentHitQuantity">Quantity of hits.</param>
    /// <returns>Returns a value depending on the quantity received on the parameter.</returns>
    private float CalculateModifier(byte currentHitQuantity)
    {
        float value = 1;
        byte counter = 0;
        while (counter < currentHitQuantity)
        {
            counter++;
            if (typeOfPierce == TypeOfPierce.Division)
                value /= damageModifier;
            else // If it's TypeOfPierce.Multiplication
                value *= damageModifier;
        }
        return value;
    }

    private enum TypeOfPierce { Division, Multiplication, }
}
