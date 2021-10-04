using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object responsible for calculating damage.
/// </summary>
[CreateAssetMenu(menuName = "Create Once/Elements Damage", fileName = "Elements Damage")]
public class ElementsDamageSO : ScriptableObject
{
    [Range(0, 200)][SerializeField] private float neutralDamagePercentage;
    [Range(0, 200)] [SerializeField] private float advantageDamagePercentage;
    [Range(0, 200)] [SerializeField] private float disadvantageDamagePercentage;
    [PropertySpace(20)]

    [SerializeField] private List<ElementsCompare> elementsCompare;

    /// <summary>
    /// Damage of first element on second element.
    /// </summary>
    /// <param name="damageSource">Element of the damage source.</param>
    /// <param name="receivingSource">Element of the receiving source.</param>
    /// <returns></returns>
    public float CalculateDamage(ElementType damageSource, ElementType receivingSource)
    {
        float finalDamageResult = 0;
        ElementDamageResult temporaryDamageResult = ElementDamageResult.NeutralDamagePercentage;

        foreach(ElementsCompare comparation in elementsCompare)
        {
            if (comparation.El1 == damageSource && comparation.El2 == receivingSource)
            {
                temporaryDamageResult = comparation.Calculate(damageSource, receivingSource);
            }
        }

        switch(temporaryDamageResult)
        {
            case ElementDamageResult.NeutralDamagePercentage:
                finalDamageResult = neutralDamagePercentage;
                break;
            case ElementDamageResult.AdvantageDamagePercentage:
                finalDamageResult = advantageDamagePercentage;
                break;
            case ElementDamageResult.DisadvantageDamagePercentage:
                finalDamageResult = disadvantageDamagePercentage;
                break;
        }

        return finalDamageResult;
    }

    [System.Serializable]
    public class ElementsCompare
    {
        [PropertySpace(10)]
        [Header("Element doing damage")]
        [EnumToggleButtons, HideLabel, SerializeField] private ElementType el1;
        [Header("Element receiving damage")]
        [EnumToggleButtons, HideLabel, SerializeField] private ElementType el2;
        [Header("Damage result")]
        [EnumToggleButtons, HideLabel, SerializeField] private ElementDamageResult result;

        public ElementType El1 => el1;
        public ElementType El2 => el2;

        public ElementDamageResult Calculate(ElementType el1, ElementType el2) =>
            result;
    }

    public enum ElementDamageResult
    {
        NeutralDamagePercentage,
        AdvantageDamagePercentage,
        DisadvantageDamagePercentage
    }
}

