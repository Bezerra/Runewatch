using System;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;

/// <summary>
/// Class responsible for creating spell manager editor window.
/// </summary>
public class SpellManagerEditor : OdinMenuEditorWindow
{
    [MenuItem("CustomEditor/Spell Manager Editor")]
    private static void OpenWindow()
    {
        GetWindow<SpellManagerEditor>().Show();
    }

    private readonly string CREATENEWSPELL = "Create New Spell";
    private readonly string SPELLSPATH = "Assets/Resources/Scriptable Objects/Spells/Spells Scriptable Objects";

    private readonly string STATUSPATH = "Assets/Resources/Scriptable Objects/Spells/Status Behaviours";

    // New spells
    private CreateNewSpellOneShotData createNewSpellOneShotData;
    private CreateNewSpellContinuousData createNewSpellContinuousData;

    protected override void OnGUI()
    {
        SirenixEditorGUI.Title("Spell Manager", "", TextAlignment.Center, true, true);
        EditorGUILayout.Space(20);
        base.OnGUI();
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        OdinMenuTree tree = new OdinMenuTree();

        createNewSpellOneShotData = new CreateNewSpellOneShotData();
        createNewSpellContinuousData = new CreateNewSpellContinuousData();
        
        // Create new spells
        tree.Add($"{CREATENEWSPELL}/Create New One Shot Spell", createNewSpellOneShotData);
        tree.Add($"{CREATENEWSPELL}/Create New Continuous Spell", createNewSpellContinuousData);
        tree.AddAllAssetsAtPath("Status Data", $"{STATUSPATH}", typeof(StatusBehaviourAbstractSO));
        tree.AddAllAssetsAtPath("Spell Data", $"{SPELLSPATH}", typeof(SpellSO));
        tree.SortMenuItemsByName(true);

        return tree;
    }

    protected override void OnBeginDrawEditors()
    {
        OdinMenuTreeSelection selected = null;

        if (MenuTree != null)
        {
            selected = MenuTree.Selection;
        }

        SirenixEditorGUI.BeginHorizontalToolbar();
        {
            GUILayout.FlexibleSpace();
            if (SirenixEditorGUI.ToolbarButton("Delete current spell"))
            {
                ScriptableObject selectedObject = selected?.SelectedValue as ScriptableObject;
                string path = AssetDatabase.GetAssetPath(selectedObject);
                AssetDatabase.DeleteAsset(path);

                AssetDatabase.SaveAssets();
            }
        }
        SirenixEditorGUI.EndHorizontalToolbar();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (createNewSpellOneShotData != null)
            DestroyImmediate(createNewSpellOneShotData.Spell);

        if (createNewSpellContinuousData != null)
            DestroyImmediate(createNewSpellContinuousData.Spell);
    }

    /// <summary>
    /// Creates new spell data on odin window.
    /// </summary>
    public class CreateNewSpellOneShotData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellOneShotSO Spell { get; private set; }

        public CreateNewSpellOneShotData()
        {
            Spell = ScriptableObject.CreateInstance<SpellOneShotSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Spell, "Assets/Resources/Scriptable Objects/Spells/Spells Scriptable Objects/New Spell " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Spell = ScriptableObject.CreateInstance<SpellOneShotSO>();
        }
    }

    public class CreateNewSpellContinuousData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellContinuousSO Spell { get; private set; }

        public CreateNewSpellContinuousData()
        {
            Spell = ScriptableObject.CreateInstance<SpellContinuousSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Spell, $"Assets/Resources/Scriptable Objects/Spells/Spells Scriptable Objects/New Spell " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Spell = ScriptableObject.CreateInstance<SpellContinuousSO>();
        }
    }
}
