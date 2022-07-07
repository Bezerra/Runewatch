using System;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;

/// <summary>
/// Class responsible for creating passive abilities editor window.
/// </summary>
public class PassiveAbilitiesManagerEditor : OdinMenuEditorWindow
{
    [MenuItem("CustomEditor/Passive Abilities Editor")]
    private static void OpenWindow()
    {
        GetWindow<PassiveAbilitiesManagerEditor>().Show();
    }

    protected override void OnGUI()
    {
        SirenixEditorGUI.Title("Passive Abilities Manager", "", TextAlignment.Center, true, true);
        EditorGUILayout.Space(20);
        base.OnGUI();
    }

    private CreateRunPassiveStatAbility createNewPassiveStatAbility;
    private CreateSkillTreePassiveAbility createNewSkillTreePassiveAbility;
    private CreateRunPassiveSpellAbility createNewPassiveSpellatAbility;

    protected override OdinMenuTree BuildMenuTree()
    {
        OdinMenuTree tree = new OdinMenuTree();

        createNewPassiveStatAbility = new CreateRunPassiveStatAbility();
        createNewSkillTreePassiveAbility = new CreateSkillTreePassiveAbility();

        tree.Add("Create New Passive Ability/New Run Stat Passive Ability", createNewPassiveStatAbility);
        tree.Add("Create New Passive Ability/New Skill Tree Passive Ability", createNewSkillTreePassiveAbility);

        tree.AddAllAssetsAtPath("Run Stat Passive Abilities",
            "Assets/Resources/Scriptable Objects/Passives/Run Passives", typeof(RunStatPassiveSO));
        tree.AddAllAssetsAtPath("Skill Tree Passive Abilities",
            "Assets/Resources/Scriptable Objects/Passives/Skill Tree Passives", typeof(SkillTreePassiveSO));

        tree.SortMenuItemsByName();

        return tree;
    }

    protected override void OnBeginDrawEditors()
    {
        OdinMenuTreeSelection selected = this.MenuTree?.Selection;

        SirenixEditorGUI.BeginHorizontalToolbar();
        {
            GUILayout.FlexibleSpace();
            if (SirenixEditorGUI.ToolbarButton("Delete current passive ability"))
            {
                ScriptableObject selectedObject = selected.SelectedValue as ScriptableObject;
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
        if (createNewPassiveStatAbility != null)
            DestroyImmediate(createNewPassiveStatAbility.StatPassive);
    }

    public class CreateRunPassiveStatAbility
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public RunStatPassiveSO StatPassive { get; private set; }

        public CreateRunPassiveStatAbility()
        {
            StatPassive = ScriptableObject.CreateInstance<RunStatPassiveSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(StatPassive,
                "Assets/Resources/Scriptable Objects/Passives/Run Passives/New Passive Ability" +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            StatPassive = ScriptableObject.CreateInstance<RunStatPassiveSO>();
        }
    }

    public class CreateSkillTreePassiveAbility
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SkillTreePassiveSO Passive { get; private set; }

        public CreateSkillTreePassiveAbility()
        {
            Passive = ScriptableObject.CreateInstance<SkillTreePassiveSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Passive,
                "Assets/Resources/Scriptable Objects/Passives/Skill Tree Passives/New Passive Ability" +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Passive = ScriptableObject.CreateInstance<SkillTreePassiveSO>();
        }
    }

    public class CreateRunPassiveSpellAbility
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public RunSpellPassiveSO SpellPassive { get; private set; }

        public CreateRunPassiveSpellAbility()
        {
            SpellPassive = ScriptableObject.CreateInstance<RunSpellPassiveSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(SpellPassive,
                "Assets/Resources/Scriptable Objects/Passives/Run Passives/New Spell Passive Ability" +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            SpellPassive = ScriptableObject.CreateInstance<RunSpellPassiveSO>();
        }
    }
}
