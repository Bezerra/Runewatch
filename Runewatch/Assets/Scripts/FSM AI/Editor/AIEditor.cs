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

        tree.AddAllAssetsAtPath("Actions", "Assets/Resources/Scriptable Objects/AI", typeof(FSMAction), true);
        tree.AddAllAssetsAtPath("Decisions", "Assets/Resources/Scriptable Objects/AI", typeof(FSMDecision), true);
        tree.AddAllAssetsAtPath("States", "Assets/Resources/Scriptable Objects/AI", typeof(FSMState), true);

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
            if (SirenixEditorGUI.ToolbarButton("Delete current asset"))
            {
                ScriptableObject asset = selected?.SelectedValue as ScriptableObject;
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
            AIAction = ScriptableObject.CreateInstance<ActionSideMovement>();
            AssetDatabase.CreateAsset(
                AIAction, "Assets/Resources/Scriptable Objects/AI/" + "Action Roll " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            ScriptableObject.CreateInstance<ActionSideMovement>();
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

        [Button("Create action - Move To Last Known Position", ButtonSizes.Large)]
        private void ActionMoveLastKnownPosition()
        {
            AIAction = ScriptableObject.CreateInstance<ActionMoveLastKnownPosition>();
            AssetDatabase.CreateAsset(AIAction,
                "Assets/Resources/Scriptable Objects/AI/" + "Action Move To Last Known Position " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            ScriptableObject.CreateInstance<ActionMoveLastKnownPosition>();
        }

        [Button("Create action - Look To Player", ButtonSizes.Large)]
        private void ActionLookToPlayer()
        {
            AIAction = ScriptableObject.CreateInstance<ActionLookToPlayer>();
            AssetDatabase.CreateAsset(AIAction,
                "Assets/Resources/Scriptable Objects/AI/" + "Action Look To Player " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            ScriptableObject.CreateInstance<ActionLookToPlayer>();
        }
        
        [Button("Create action - Get Distant From Player", ButtonSizes.Large)]
        private void ActionGetDistantFromPlayer()
        {
            AIAction = ScriptableObject.CreateInstance<ActionGetDistantFromPlayer>();
            AssetDatabase.CreateAsset(AIAction,
                "Assets/Resources/Scriptable Objects/AI/" + "Action Get Distant From Player " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            ScriptableObject.CreateInstance<ActionGetDistantFromPlayer>();
        }

        [Button("Create action - Run From Player", ButtonSizes.Large)]
        private void ActionRunFromPlayer()
        {
            AIAction = ScriptableObject.CreateInstance<ActionRunFromPlayer>();
            AssetDatabase.CreateAsset(AIAction,
                "Assets/Resources/Scriptable Objects/AI/" + "Action Run From Player " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            ScriptableObject.CreateInstance<ActionRunFromPlayer>();
        }

        [Button("Create action - Look Away From Player", ButtonSizes.Large)]
        private void ActionLookAwayFromPlayer()
        {
            AIAction = ScriptableObject.CreateInstance<ActionLookAwayFromPlayer>();
            AssetDatabase.CreateAsset(AIAction,
                "Assets/Resources/Scriptable Objects/AI/" + "Action Look Away From Player " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            ScriptableObject.CreateInstance<ActionLookAwayFromPlayer>();
        }

        [Button("Create action - Get Player Target", ButtonSizes.Large)]
        private void ActionGetPlayerTarget()
        {
            AIAction = ScriptableObject.CreateInstance<ActionGetPlayerTarget>();
            AssetDatabase.CreateAsset(AIAction,
                "Assets/Resources/Scriptable Objects/AI/" + "Action Get Player Target " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            ScriptableObject.CreateInstance<ActionGetPlayerTarget>();
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
            AIDecision = ScriptableObject.CreateInstance<DecisionTargettingPlayer>();
            AssetDatabase.CreateAsset(
                AIDecision, "Assets/Resources/Scriptable Objects/AI/" + "Decision Targetting Player " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            ScriptableObject.CreateInstance<DecisionTargettingPlayer>();
        }

        [Button("Create decision - Took Damage", ButtonSizes.Large)]
        private void DecisionTookDamage()
        {
            AIDecision = ScriptableObject.CreateInstance<DecisionTookDamage>();
            AssetDatabase.CreateAsset(
                AIDecision, "Assets/Resources/Scriptable Objects/AI/" + "Decision Took Damage " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            ScriptableObject.CreateInstance<DecisionTookDamage>();
        }

        [Button("Create decision - Max Time In State When Stopped", ButtonSizes.Large)]
        private void DecisionMaxTimeInStateWhenStopped()
        {
            AIDecision = ScriptableObject.CreateInstance<DecisionMaxTimeInStateWhenStopped>();
            AssetDatabase.CreateAsset(
                AIDecision, "Assets/Resources/Scriptable Objects/AI/" + "Decision Max Time In State When Stopped " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            ScriptableObject.CreateInstance<DecisionMaxTimeInStateWhenStopped>();
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
