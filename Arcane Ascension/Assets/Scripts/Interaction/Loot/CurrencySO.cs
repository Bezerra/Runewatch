#if UNITY_EDITOR
using UnityEditor;
#endif


using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object with potions information.
/// </summary>
[InlineEditor]
[CreateAssetMenu(menuName = "Loot/Currency", fileName = "Currency")]
public class CurrencySO : ScriptableObject
{
    [InlineButton("ChangeFileName", "Update File Name")]
    [SerializeField] protected new string name = "New Spell";

    [SerializeField] private CurrencyType currencyType;
    [RangeMinMax(1f, 100000f)] [SerializeField] private Vector2 defaultAmount;

    public CurrencyType CurrencyType => currencyType;
    public Vector2 Amount { get; set; }

    private void OnEnable()
    {
        Amount = defaultAmount;
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
