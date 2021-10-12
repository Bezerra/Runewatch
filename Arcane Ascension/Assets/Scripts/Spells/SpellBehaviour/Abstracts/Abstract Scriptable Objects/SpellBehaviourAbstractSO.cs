using UnityEngine;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Abstract class responsible for spell behaviours.
/// This scriptable object only serves as the base type for child scriptable objects.
/// </summary>
[InlineEditor]
public abstract class SpellBehaviourAbstractSO: ScriptableObject
{
#if UNITY_EDITOR
    [PropertySpace(20)]
    [InlineButton("ChangeFileName", "Update File Name")]
    [SerializeField] private string fileName;

    private void ChangeFileName()
    {
        string assetPath = AssetDatabase.GetAssetPath(this.GetInstanceID());
        AssetDatabase.RenameAsset(assetPath, fileName);
        AssetDatabase.SaveAssets();
    }
#endif

    // This scriptable object only serves as the base type for child classes

    /// <summary>
    /// Immediatly deactives spell gameobject.
    /// </summary>
    /// <param name="parent">Gameobject to deactivate.</param>
    public virtual void DisableSpell(MonoBehaviour parent) =>
        parent.gameObject.SetActive(false);
}
