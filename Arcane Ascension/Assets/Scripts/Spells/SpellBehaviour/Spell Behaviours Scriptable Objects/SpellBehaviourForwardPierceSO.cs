using UnityEngine;

/// <summary>
/// Scriptable object responsible for creating a spell's behaviour.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/Spell Behaviour Forward Pierce", fileName = "Spell Behaviour Forward Pierce")]
sealed public class SpellBehaviourForwardPierceSO : SpellBehaviourAbstractOneShotSO
{
    [Header("In this spell, this variable only checks the direction of the spell")]
    [Range(15, 50)] [SerializeField] private float spellDistance;

    [Space(20)]
    [SerializeField] private TypeOfPierce typeOfPierce;
    [Range(2, 20)] [SerializeField] private byte hitQuantity;
    [Tooltip("Divides or multiplies damage by this factor, depending on type of pierce")]
    [Range(1, 4)] [SerializeField] private byte damageModifier = 2;

    public override void StartBehaviour(SpellBehaviourOneShot parent)
    {
        base.StartBehaviour(parent);

        // Direction of the spell
        Ray forward = new Ray(parent.Eyes.position, parent.Eyes.forward);

        if (Physics.Raycast(forward, out RaycastHit objectHit, spellDistance)) // Creates a raycast to see if eyes are hiting something
        {
            parent.transform.LookAt(objectHit.point);
        }
        else
        {
            Vector3 finalDirection = parent.Eyes.position + parent.Eyes.forward * 15f;
            parent.transform.LookAt(finalDirection);
        }

        // Moves the spell forward
        parent.Rb.velocity = parent.transform.forward * parent.Spell.Speed;
    }

    public override void ContinuousFixedUpdateBehaviour(SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }

    public override void HitTriggerBehaviour(Collider other, SpellBehaviourOneShot parent)
    {
        base.DamageBehaviour(other, parent, CalculateModifier(parent.CurrentHitQuantity, typeOfPierce));


        base.HitTriggerBehaviour(other, parent);

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
