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

    private CreateNewAction createNewAction;
    private CreateNewDecision createNewDecision;
    private CreateNewState createNewState;

    protected override OdinMenuTree BuildMenuTree()
    {
        OdinMenuTree tree = new OdinMenuTree();

        createNewAction = new CreateNewAction();
        createNewDecision = new CreateNewDecision();
        createNewState = new CreateNewState();

        tree.Add("Create New Action", createNewAction);
        tree.Add("Create New Decision", createNewDecision);
        tree.Add("Create New State", createNewState);

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

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (createNewAction != null)
            DestroyImmediate(createNewAction.AIAction);

        if (createNewDecision != null)
            DestroyImmediate(createNewDecision.AIDecision);

        if (createNewState != null)
            DestroyImmediate(createNewState.AIState);
    }

    public class CreateNewAction
    {
        public FSMAction AIAction { get; private set; }

        [Button("Create action - Roll", ButtonSizes.Large)]
        private void CreateRoll()
        {
            AIAction = ScriptableObject.CreateInstance<ActionRoll>();
            AssetDatabase.CreateAsset(
                AIAction, "Assets/Resources/Scriptable Objects/AI/" + "Action Roll " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            ScriptableObject.CreateInstance<ActionRoll>();
        }

        [Button("Create action - Chase Player", ButtonSizes.Large)]
        private void CreateChasePlayer()
        {
            AIAction = ScriptableObject.CreateInstance<ActionChasePlayer>();
            AssetDatabase.CreateAsset(AIAction,
                "Assets/Resources/Scriptable Objects/AI/" + "Action Chase Player " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            ScriptableObject.CreateInstance<ActionChasePlayer>();
        }

        [Button("Create action - Random Patrol", ButtonSizes.Large)]
        private void CreateRandomPatrol()
        {
            AIAction = ScriptableObject.CreateInstance<ActionRandomPatrol>();
            AssetDatabase.CreateAsset(AIAction,
                "Assets/Resources/Scriptable Objects/AI/" + "Action Random Patrol " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            ScriptableObject.CreateInstance<ActionRandomPatrol>();
        }

        [Button("Create action - Random Attack", ButtonSizes.Large)]
        private void CreateRandomAttack()
        {
            AIAction = ScriptableObject.CreateInstance<ActionAttack>();
            AssetDatabase.CreateAsset(AIAction,
                "Assets/Resources/Scriptable Objects/AI/" + "Action Random Attack " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            ScriptableObject.CreateInstance<ActionAttack>();
        }
    }

    public class CreateNewDecision
    {
        public FSMDecision AIDecision { get; private set; }

        [Button("Create decision - Can see player", ButtonSizes.Large)]
        private void CreateCanSeePlayer()
        {
            AIDecision = ScriptableObject.CreateInstance<DecisionCanSeePlayer>();
            AssetDatabase.CreateAsset(
                AIDecision, "Assets/Resources/Scriptable Objects/AI/" + "Decision Can See Player " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            ScriptableObject.CreateInstance<DecisionCanSeePlayer>();
        }

        [Button("Create decision - Decision Chasing Player", ButtonSizes.Large)]
        private void DecisionChasingPlayer()
        {
            AIDecision = ScriptableObject.CreateInstance<DecisionChasingPlayer>();
            AssetDatabase.CreateAsset(
                AIDecision, "Assets/Resources/Scriptable Objects/AI/" + "Decision Chasing Player " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            ScriptableObject.CreateInstance<DecisionChasingPlayer>();
        }
    }

    public class CreateNewState
    {
        public FSMState AIState { get; private set; }

        [Button("Create New State", ButtonSizes.Large)]
        private void CreateState()
        {
            AIState = ScriptableObject.CreateInstance<FSMState>();
            AssetDatabase.CreateAsset(
                AIState, "Assets/Resources/Scriptable Objects/AI/" + "State " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            ScriptableObject.CreateInstance<FSMState>();
        }
    }
}
