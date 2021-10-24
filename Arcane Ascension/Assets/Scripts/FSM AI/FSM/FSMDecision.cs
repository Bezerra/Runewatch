using UnityEngine;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Abstract scriptable object for all decisions.
/// </summary>
[InlineEditor]
public abstract class FSMDecision : ScriptableObject
{
#if UNITY_EDITOR
    [InlineButton("ChangeFileName", "Update File Name")]
    [SerializeField] private string fileName;

    private void ChangeFileName()
    {
        string assetPath = AssetDatabase.GetAssetPath(this.GetInstanceID());
        AssetDatabase.RenameAsset(assetPath, fileName);
        AssetDatabase.SaveAssets();
    }
#endif

    [SerializeField] [TextArea(3, 3)] private string notes;
    [PropertySpace(30)]

    /// <summary>
    /// Checks if a decision is true or false.
    /// </summary>
    /// <param name="aiController">Ai Character.</param>
    /// <returns>True if decision is true, else it's false.</returns>
    public abstract bool CheckDecision(StateController<Enemy> aiCharacter);

    /// <summary>
    /// Executes once on enter.
    /// </summary>
    /// <param name="aiCharacter">AI Character.</param>
    public abstract void OnEnter(StateController<Enemy> aiCharacter);

    /// <summary>
    /// Executes once on exit.
    /// </summary>
    /// <param name="aiCharacter">AI Character.</param>
    public abstract void OnExit(StateController<Enemy> aiCharacter);
}
