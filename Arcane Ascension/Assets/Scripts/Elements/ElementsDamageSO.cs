using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object responsible for calculating damage.
/// </summary>
[CreateAssetMenu(menuName = "Create Once/Elements Damage", fileName = "Elements Damage")]
public class ElementsDamageSO : ScriptableObject
{
    [Range(0, 200)] [SerializeField] private float neutralDamagePercentage;
    [Range(0, 200)] [SerializeField] private float advantageDamagePercentage;
    [Range(0, 200)] [SerializeField] private float disadvantageDamagePercentage;
    [PropertySpace(20)]

    [InfoBox("AFTER UPDATING THE ELEMENTS DON'T FORGET TO SAVE BY PRESSING THE BUTTON IN THE END.")]
    // List used only to create the elements in inspector
    [SerializeField] private List<ElementsCompare> elementsList;

    // Dictionary to improve performance when calculating damage
    private Dictionary<string, float> elementsDictionary;

    /// <summary>
    /// Instantiates and updates a dictionary with all values from a list.
    /// </summary>
    private void OnEnable()
    {
        elementsDictionary = new Dictionary<string, float>();

        float elementResult = 0;
        foreach (ElementsCompare elementCompare in elementsList)
        {
            switch (elementCompare.Result)
            {
                case ElementDamageResult.NeutralDamagePercentage:
                    elementResult = neutralDamagePercentage;
                    break;
                case ElementDamageResult.AdvantageDamagePercentage:
                    elementResult = advantageDamagePercentage;
                    break;
                case ElementDamageResult.DisadvantageDamagePercentage:
                    elementResult = disadvantageDamagePercentage;
                    break;
            }

            elementsDictionary.Add(
                elementCompare.El1.ToString() + "On" + elementCompare.El2.ToString(), elementResult);
        }
    }

    /// <summary>
    /// Damage of first element on second element.
    /// </summary>
    /// <param name="damageSource">Element of the damage source.</param>
    /// <param name="receivingSource">Element of the receiving source.</param>
    /// <returns>Returns final damage.</returns>
    public float CalculateDamage(ElementType damageSource, ElementType receivingSource) =>
        elementsDictionary[damageSource.ToString() + "On" + receivingSource.ToString()];

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
        public ElementDamageResult Result => result;
    }

    /// <summary>
    /// Instantiates and updates a dictionary with all values from a list.
    /// </summary>
    [Button("Update Elements Dictionary")]
    private void UpdateDictionary()
    {
        elementsDictionary = new Dictionary<string, float>();

        float elementResult = 0;
        foreach (ElementsCompare elementCompare in elementsList)
        {
            switch (elementCompare.Result)
            {
                case ElementDamageResult.NeutralDamagePercentage:
                    elementResult = neutralDamagePercentage;
                    break;
                case ElementDamageResult.AdvantageDamagePercentage:
                    elementResult = advantageDamagePercentage;
                    break;
                case ElementDamageResult.DisadvantageDamagePercentage:
                    elementResult = disadvantageDamagePercentage;
                    break;
            }

            elementsDictionary.Add(
                elementCompare.El1.ToString() + "On" + elementCompare.El2.ToString(), elementResult);
        }

        //foreach(KeyValuePair<string, float> ahh in elementsDictionary)
        //{
        //    Debug.Log(ahh);
        //}
    }

    /// <summary>
    /// Enum with damage percentages.
    /// </summary>
    public enum ElementDamageResult
    {
        NeutralDamagePercentage,
        AdvantageDamagePercentage,
        DisadvantageDamagePercentage
    }
}

