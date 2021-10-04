using System;
using System.Collections.Generic;
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

    private CreateNewSpellData createNewSpellData;
    private CreateContinuousBehaviourData continuousBehaviourData;
    private CreateForwardBehaviourData forwardBehaviourData;


    protected override void OnGUI()
    {
        SirenixEditorGUI.Title("Spell Manager", "", TextAlignment.Center, true, true);
        EditorGUILayout.Space(20);
        base.OnGUI();
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        OdinMenuTree tree = new OdinMenuTree();

        createNewSpellData = new CreateNewSpellData();
        continuousBehaviourData = new CreateContinuousBehaviourData();
        forwardBehaviourData = new CreateForwardBehaviourData();

        tree.Add("Create New Behaviour/New Forward Behaviour", new CreateForwardBehaviourData());
        tree.Add("Create New Behaviour/New Continuous Behaviour", new CreateContinuousBehaviourData());
        tree.Add("Create New Spell", new CreateNewSpellData());

        tree.AddAllAssetsAtPath("Spell Behaviours", "Assets/Resources/Scriptable Objects/Spells/Behaviours", typeof(SpellBehaviourAbstractSO));
        tree.AddAllAssetsAtPath("Spell Data", "Assets/Resources/Scriptable Objects/Spells", typeof(SpellSO));

        return tree;
    }

    protected override void OnBeginDrawEditors()
    {
        OdinMenuTreeSelection selected = this.MenuTree.Selection;

        SirenixEditorGUI.BeginHorizontalToolbar();
        {
            GUILayout.FlexibleSpace();
            if (SirenixEditorGUI.ToolbarButton("Delete current spell"))
            {
                ScriptableObject selectedObject = selected.SelectedValue as ScriptableObject;
                string path = AssetDatabase.GetAssetPath(selectedObject);
                AssetDatabase.DeleteAsset(path);

                createNewSpellData.UpdateAllSpellsList();

                AssetDatabase.SaveAssets();
            }
        }
        SirenixEditorGUI.EndHorizontalToolbar();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (createNewSpellData != null)
            DestroyImmediate(createNewSpellData.Spell);
    }

    /// <summary>
    /// Creates new spell data on odin window.
    /// </summary>
    public class CreateNewSpellData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellSO Spell { get; private set; }

        public CreateNewSpellData()
        {
            Spell = ScriptableObject.CreateInstance<SpellSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Spell, "Assets/Resources/Scriptable Objects/Spells/New Spell " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Spell = ScriptableObject.CreateInstance<SpellSO>();

            UpdateAllSpellsList();
        }

        public void UpdateAllSpellsList()
        {
            string[] spellsPaths = AssetDatabase.FindAssets("t:SpellSO");
            string[] allSpellsPath = AssetDatabase.FindAssets("t:AllSpellsSO");

            AllSpellsSO allSpells =
                (AllSpellsSO)AssetDatabase.LoadAssetAtPath(
                    AssetDatabase.GUIDToAssetPath(allSpellsPath[0]), typeof(AllSpellsSO));

            allSpells.SpellList = new List<SpellSO>();

            foreach (string spellPath in spellsPaths)
            {
                allSpells.SpellList.Add(
                    (SpellSO)AssetDatabase.LoadAssetAtPath(
                        AssetDatabase.GUIDToAssetPath(spellPath), typeof(SpellSO)));
            }
        }
    }

    public class CreateForwardBehaviourData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellBehaviourForwardSO Spell { get; private set; }

        public CreateForwardBehaviourData()
        {
            Spell = ScriptableObject.CreateInstance<SpellBehaviourForwardSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Spell, 
                "Assets/Resources/Scriptable Objects/Spells/Behaviours/New Spell Forward Behaviour " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Spell = ScriptableObject.CreateInstance<SpellBehaviourForwardSO>();
        }
    }

    public class CreateContinuousBehaviourData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellBehaviourContinuousSO Spell { get; private set; }

        public CreateContinuousBehaviourData()
        {
            Spell = ScriptableObject.CreateInstance<SpellBehaviourContinuousSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Spell, 
                "Assets/Resources/Scriptable Objects/Spells/Behaviours/New Spell Continuous Behaviour " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Spell = ScriptableObject.CreateInstance<SpellBehaviourContinuousSO>();
        }
    }
}
