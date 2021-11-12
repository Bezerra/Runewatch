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

    private CreateRunPassiveAbility createNewPassiveAbility;
    private CreateSkillTreePassiveAbility createNewSkillTreePassiveAbility;

    protected override OdinMenuTree BuildMenuTree()
    {
        OdinMenuTree tree = new OdinMenuTree();

        createNewPassiveAbility = new CreateRunPassiveAbility();
        createNewSkillTreePassiveAbility = new CreateSkillTreePassiveAbility();

        tree.Add("Create New Passive Ability/New Run Passive Ability", createNewPassiveAbility);
        tree.Add("Create New Passive Ability/New Skill Tree Passive Ability", createNewSkillTreePassiveAbility);

        tree.AddAllAssetsAtPath("Run Passive Abilities",
            "Assets/Resources/Scriptable Objects/Passives/Run Passives", typeof(RunPassiveSO));
        tree.AddAllAssetsAtPath("Skill Tree Passive Abilities",
            "Assets/Resources/Scriptable Objects/Passives/Skill Tree Passives", typeof(SkillTreePassiveSO));

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
        if (createNewPassiveAbility != null)
            DestroyImmediate(createNewPassiveAbility.Passive);
    }

    public class CreateRunPassiveAbility
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public RunPassiveSO Passive { get; private set; }

        public CreateRunPassiveAbility()
        {
            Passive = ScriptableObject.CreateInstance<RunPassiveSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Passive,
                "Assets/Resources/Scriptable Objects/Passives/Run Passives/New Passive Ability" +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Passive = ScriptableObject.CreateInstance<RunPassiveSO>();
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
}
