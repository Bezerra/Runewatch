using UnityEngine;

/// <summary>
/// Scriptable object responsible for creating a spell's behaviour.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/Spell Behaviour Apply Damage Pierce", fileName = "Spell Behaviour Apply Damage Pierce")]
sealed public class SpellBehaviourApplyDamagePierceSO : SpellBehaviourAbstractOneShotSO
{
    [Space(20)]
    [SerializeField] private TypeOfPierce typeOfPierce;
    [Range(2, 20)] [SerializeField] private byte hitQuantity;
    [Tooltip("Divides or multiplies damage by this factor, depending on type of pierce")]
    [Range(1, 4)] [SerializeField] private byte damageModifier = 2;

    public override void StartBehaviour(SpellBehaviourOneShot parent)
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
        base.DamageBehaviour(other, parent, CalculateModifier(parent.CurrentHitQuantity, typeOfPierce));

        if (++parent.CurrentHitQuantity >= hitQuantity)
            base.StopSpellSpeed(parent);
    }

    /// <summary>
    /// Gets damage modifier depending on the quantity received on the parameter.
    /// </summary>
    /// <param name="currentHitQuantity">Quantity of hits.</param>
    /// <returns>Returns a value depending on the quantity received on the parameter.</returns>
    private float CalculateModifier(byte currentHitQuantity, TypeOfPierce typeofPierce)
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
