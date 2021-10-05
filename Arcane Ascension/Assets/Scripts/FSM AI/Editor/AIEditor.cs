using System;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;

/// <summary>
/// Class responsible for creating AI manager editor window.
/// </summary>
public class AIEditor : OdinMenuEditorWindow
{
    [MenuItem("CustomEditor/AI Manager Editor")]
    private static void OpenWindow()
    {
        GetWindow<AIEditor>().Show();
    }

    protected override void OnGUI()
    {
        SirenixEditorGUI.Title("Artificial Intelligence Manager Editor", "", TextAlignment.Center, true, true);
        EditorGUILayout.Space(20);
        base.OnGUI();
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        OdinMenuTree tree = new OdinMenuTree();

        tree.Add("Create New Action", new CreateNewAction());
        tree.Add("Create New Decision", new CreateNewDecision());
        tree.Add("Create New State", new CreateNewState());

        tree.AddAllAssetsAtPath("Actions", "Assets/Resources/Scriptable Objects/AI", typeof(FSMAction));
        tree.AddAllAssetsAtPath("Decisions", "Assets/Resources/Scriptable Objects/AI", typeof(FSMDecision));
        tree.AddAllAssetsAtPath("States", "Assets/Resources/Scriptable Objects/AI", typeof(FSMState));

        return tree;
    }

    protected override void OnBeginDrawEditors()
    {
        OdinMenuTreeSelection selectedAsset = this.MenuTree.Selection;

        SirenixEditorGUI.BeginHorizontalToolbar();
        {
            GUILayout.FlexibleSpace();
            if (SirenixEditorGUI.ToolbarButton("Delete current asset"))
            {
                ScriptableObject asset = selectedAsset.SelectedValue as ScriptableObject;
                string path = AssetDatabase.GetAssetPath(asset);
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.SaveAssets();
            }
        }
        SirenixEditorGUI.EndHorizontalToolbar();
    }

    public class CreateNewAction
    {
        [Button("Create action - Roll", ButtonSizes.Large)]
        private void CreateRoll()
        {
            ActionRoll roll = ScriptableObject.CreateInstance<ActionRoll>();
            AssetDatabase.CreateAsset(
                roll, "Assets/Resources/Scriptable Objects/AI/" + "Action Roll " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            ScriptableObject.CreateInstance<ActionRoll>();
        }

        [Button("Create action - Chase Player", ButtonSizes.Large)]
        private void CreateChasePlayer()
        {
            ActionChasePlayer chase = ScriptableObject.CreateInstance<ActionChasePlayer>();
            AssetDatabase.CreateAsset(chase,
                "Assets/Resources/Scriptable Objects/AI/" + "Action Chase Player " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            ScriptableObject.CreateInstance<ActionChasePlayer>();
        }

        [Button("Create action - Random Patrol", ButtonSizes.Large)]
        private void CreateRandomPatrol()
        {
            ActionRandomPatrol patrol = ScriptableObject.CreateInstance<ActionRandomPatrol>();
            AssetDatabase.CreateAsset(patrol,
                "Assets/Resources/Scriptable Objects/AI/" + "Action Random Patrol " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            ScriptableObject.CreateInstance<ActionRandomPatrol>();
        }

        [Button("Create action - Random Attack", ButtonSizes.Large)]
        private void CreateRandomAttack()
        {
            ActionAttack randomAttack = ScriptableObject.CreateInstance<ActionAttack>();
            AssetDatabase.CreateAsset(randomAttack,
                "Assets/Resources/Scriptable Objects/AI/" + "Action Random Attack " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            ScriptableObject.CreateInstance<ActionAttack>();
        }
    }

    public class CreateNewDecision
    {
        [Button("Create decision - Can see player", ButtonSizes.Large)]
        private void CreateCanSeePlayer()
        {
            DecisionCanSeePlayer canSee = ScriptableObject.CreateInstance<DecisionCanSeePlayer>();
            AssetDatabase.CreateAsset(
                canSee, "Assets/Resources/Scriptable Objects/AI/" + "Decision Can See Player " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            ScriptableObject.CreateInstance<DecisionCanSeePlayer>();
        }

        [Button("Create decision - Decision Chasing Player", ButtonSizes.Large)]
        private void DecisionChasingPlayer()
        {
            DecisionChasingPlayer chasePlayer = ScriptableObject.CreateInstance<DecisionChasingPlayer>();
            AssetDatabase.CreateAsset(
                chasePlayer, "Assets/Resources/Scriptable Objects/AI/" + "Decision Chasing Player " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            ScriptableObject.CreateInstance<DecisionChasingPlayer>();
        }
    }

    public class CreateNewState
    {
        [Button("Create New State", ButtonSizes.Large)]
        private void CreateState()
        {
            FSMState state = ScriptableObject.CreateInstance<FSMState>();
            AssetDatabase.CreateAsset(
                state, "Assets/Resources/Scriptable Objects/AI/" + "State " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            ScriptableObject.CreateInstance<FSMState>();
        }
    }
}
