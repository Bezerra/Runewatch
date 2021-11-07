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

    private CreatePassiveAbility createNewPassiveAbility;

    protected override OdinMenuTree BuildMenuTree()
    {
        OdinMenuTree tree = new OdinMenuTree();

        createNewPassiveAbility = new CreatePassiveAbility();

        tree.Add("Create New Passive Ability/New Passive Ability", createNewPassiveAbility);

        tree.AddAllAssetsAtPath("Passive Abilities",
            "Assets/Resources/Scriptable Objects/Passives", typeof(PassiveSO));

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

    public class CreatePassiveAbility
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public PassiveSO Passive { get; private set; }

        public CreatePassiveAbility()
        {
            Passive = ScriptableObject.CreateInstance<PassiveSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Passive,
                "Assets/Resources/Scriptable Objects/Passives/New Passive Ability" +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Passive = ScriptableObject.CreateInstance<PassiveSO>();
        }
    }
}
