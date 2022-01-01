using UnityEngine;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Abstract scriptable object used to create one shot spell behaviours.
/// </summary>
[InlineEditor]
public abstract class SpellBehaviourAbstractSO: ScriptableObject
{
    /// <summary>
    /// Executes on start.
    /// </summary>
    public abstract void StartBehaviour(SpellBehaviourOneShot parent);

    /// <summary>
    /// Executes on update.
    /// </summary>
    public abstract void ContinuousUpdateBehaviour(SpellBehaviourOneShot parent);

    /// <summary>
    /// Executes on update before spell is fired.
    /// Used for one shot casts with release, while player is pressing fire.
    /// </summary>
    public abstract void ContinuousUpdateBeforeSpellBehaviour(SpellBehaviourOneShot parent);

    /// <summary>
    /// Executes on fixed update.
    /// </summary>
    public abstract void ContinuousFixedUpdateBehaviour(SpellBehaviourOneShot parent);

    /// <summary>
    /// Executes on hit. Creates hit impact.
    /// </summary>
    /// <param name="other">Collider.</param>
    public abstract void HitTriggerBehaviour(Collider other, SpellBehaviourOneShot parent);

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
