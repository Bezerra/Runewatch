using UnityEngine;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Abstract scriptable object for all actions.
/// </summary>
[InlineEditor]
public abstract class FSMAction : ScriptableObject
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
    /// Executes a certain action.
    /// </summary>
    /// <param name="aiCharacter">AI Character.</param>
    public abstract void Execute(StateController aiCharacter);

    /// <summary>
    /// Executes once on enter.
    /// </summary>
    /// <param name="aiCharacter">AI Character.</param>
    public abstract void OnEnter(StateController aiCharacter);

    /// <summary>
    /// Executes once on exit.
    /// </summary>
    /// <param name="aiCharacter">AI Character.</param>
    public abstract void OnExit(StateController aiCharacter);
}
