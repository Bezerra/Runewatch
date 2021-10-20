using System;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;

/// <summary>
/// Class responsible for creating attack manager editor window.
/// </summary>
public class AttackManagerEditor : OdinMenuEditorWindow
{
    [MenuItem("CustomEditor/Attack Manager Editor")]
    private static void OpenWindow()
    {
        GetWindow<AttackManagerEditor>().Show();
    }

    protected override void OnGUI()
    {
        SirenixEditorGUI.Title("Attack Behaviour Manager", "", TextAlignment.Center, true, true);
        EditorGUILayout.Space(20);
        base.OnGUI();
    }

    private CreateAttackBehaviourOneShot createNewAttackBehaviourOneShotData;
    private CreateAttackBehaviourOneShotWithRelease createNewAttackBehaviourOneShotWithReleaseData;
    private CreateAttackBehaviourContinuous createNewAttackBehaviourContinuousData;

    protected override OdinMenuTree BuildMenuTree()
    {
        OdinMenuTree tree = new OdinMenuTree();

        createNewAttackBehaviourOneShotData = new CreateAttackBehaviourOneShot();
        createNewAttackBehaviourContinuousData = new CreateAttackBehaviourContinuous();

        tree.Add("Create New Behaviour/New Behaviour One Shot", createNewAttackBehaviourOneShotData);
        tree.Add("Create New Behaviour/New Behaviour One Shot With Release", createNewAttackBehaviourOneShotWithReleaseData);
        tree.Add("Create New Behaviour/New Behaviour Continuous", createNewAttackBehaviourContinuousData);

        tree.AddAllAssetsAtPath("Attack Behaviours",
            "Assets/Resources/Scriptable Objects/Attack Behaviours", typeof(AttackBehaviourAbstractSO));

        return tree;
    }

    protected override void OnBeginDrawEditors()
    {
        OdinMenuTreeSelection selected = this.MenuTree.Selection;

        SirenixEditorGUI.BeginHorizontalToolbar();
        {
            GUILayout.FlexibleSpace();
            if (SirenixEditorGUI.ToolbarButton("Delete current attack"))
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
        if (createNewAttackBehaviourOneShotData != null)
            DestroyImmediate(createNewAttackBehaviourOneShotData.Behaviour);

        if (createNewAttackBehaviourOneShotWithReleaseData != null)
            DestroyImmediate(createNewAttackBehaviourOneShotWithReleaseData.Behaviour);

        if (createNewAttackBehaviourContinuousData != null)
            DestroyImmediate(createNewAttackBehaviourContinuousData.Behaviour);        
    }

    public class CreateAttackBehaviourOneShot
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public AttackBehaviourOneShotSO Behaviour { get; private set; }

        public CreateAttackBehaviourOneShot()
        {
            Behaviour = ScriptableObject.CreateInstance<AttackBehaviourOneShotSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Behaviour,
                "Assets/Resources/Scriptable Objects/Spells/Attack Behaviours/New Attack Behaviour One Shot" +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Behaviour = ScriptableObject.CreateInstance<AttackBehaviourOneShotSO>();
        }
    }

    public class CreateAttackBehaviourContinuous
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public AttackBehaviourContinuousSO Behaviour { get; private set; }

        public CreateAttackBehaviourContinuous()
        {
            Behaviour = ScriptableObject.CreateInstance<AttackBehaviourContinuousSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Behaviour,
                "Assets/Resources/Scriptable Objects/Spells/Attack Behaviours/New Attack Behaviour Continuous" +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Behaviour = ScriptableObject.CreateInstance<AttackBehaviourContinuousSO>();
        }
    }

    public class CreateAttackBehaviourOneShotWithRelease
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public AttackBehaviourOneShotWithReleaseSO Behaviour { get; private set; }

        public CreateAttackBehaviourOneShotWithRelease()
        {
            Behaviour = ScriptableObject.CreateInstance<AttackBehaviourOneShotWithReleaseSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Behaviour,
                "Assets/Resources/Scriptable Objects/Spells/Attack Behaviours/New Attack Behaviour One Shot With Release" +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Behaviour = ScriptableObject.CreateInstance<AttackBehaviourOneShotWithReleaseSO>();
        }
    }
}
