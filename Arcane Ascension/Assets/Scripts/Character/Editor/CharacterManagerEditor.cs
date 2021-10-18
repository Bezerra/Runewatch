using System;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;

/// <summary>
/// Class responsible for creating a character manager editor window.
/// </summary>
public class CharacterManagerEditor : OdinMenuEditorWindow
{
    [MenuItem("CustomEditor/Character Manager Editor")]
    private static void OpenWindow()
    {
        GetWindow<CharacterManagerEditor>().Show();
    }

    private CreateNewStatsData createNewStatsData;
    private CreateNewPlayerValuesData createNewPlayerValuesData;
    private CreateNewEnemyValuesData createNewEnemyValuesData;
    private CreateNewPlayerCharacterData createNewPlayerCharacterData;
    private CreateNewEnemyCharacterData createNewEnemyCharacterData;

    protected override void OnGUI()
    {
        SirenixEditorGUI.Title("Character Creator", "", TextAlignment.Center, true, true);
        EditorGUILayout.Space(20);
        base.OnGUI();
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        OdinMenuTree tree = new OdinMenuTree();

        createNewStatsData = new CreateNewStatsData();
        createNewPlayerValuesData = new CreateNewPlayerValuesData();
        createNewEnemyValuesData = new CreateNewEnemyValuesData();
        createNewPlayerCharacterData = new CreateNewPlayerCharacterData();
        createNewEnemyCharacterData = new CreateNewEnemyCharacterData();

        tree.Add("Create New Character/Player Character", createNewPlayerCharacterData);
        tree.Add("Create New Character/Enemy Character", createNewEnemyCharacterData);
        tree.Add("Create New Values/Player", createNewPlayerValuesData);
        tree.Add("Create New Values/Enemy", createNewEnemyValuesData);
        tree.Add("Create New Stats", createNewStatsData);

        tree.AddAllAssetsAtPath(
            "Stats/Player Stats", "Assets/Resources/Scriptable Objects/Player", 
            typeof(StatsSO));
        tree.AddAllAssetsAtPath(
            "Stats/Enemy Stats", "Assets/Resources/Scriptable Objects/Enemy", 
            typeof(StatsSO));

        tree.AddAllAssetsAtPath(
            "Values/Player Values", "Assets/Resources/Scriptable Objects/Player", 
            typeof(PlayerValuesSO));
        tree.AddAllAssetsAtPath(
            "Values/Enemy Values", "Assets/Resources/Scriptable Objects/Enemy", 
            typeof(EnemyValuesSO));
        
        tree.AddAllAssetsAtPath(
            "Characters/Player Characters", "Assets/Resources/Scriptable Objects/Player", 
            typeof(PlayerCharacterSO));
        tree.AddAllAssetsAtPath(
            "Characters/Enemy Characters", "Assets/Resources/Scriptable Objects/Enemy", 
            typeof(EnemyCharacterSO));

        return tree;
    }

    protected override void OnBeginDrawEditors()
    {
        OdinMenuTreeSelection selected = this.MenuTree.Selection;

        SirenixEditorGUI.BeginHorizontalToolbar();
        {
            GUILayout.FlexibleSpace();
            if (SirenixEditorGUI.ToolbarButton("Delete current stats"))
            {
                ScriptableObject selectedSO = selected.SelectedValue as ScriptableObject;
                string path = AssetDatabase.GetAssetPath(selectedSO);
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.SaveAssets();
            }
        }
        SirenixEditorGUI.EndHorizontalToolbar();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (createNewStatsData != null)
            DestroyImmediate(createNewStatsData.CharacterStats);

        if (createNewPlayerCharacterData != null)
            DestroyImmediate(createNewPlayerCharacterData.CharacterData);

        if (createNewEnemyCharacterData != null)
            DestroyImmediate(createNewEnemyCharacterData.CharacterData);

        if (createNewPlayerValuesData != null)
            DestroyImmediate(createNewPlayerValuesData.Values);

        if (createNewEnemyValuesData != null)
            DestroyImmediate(createNewEnemyValuesData.Values);
    }

    /// <summary>
    /// Creates new stats data on odin window.
    /// </summary>
    public class CreateNewStatsData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public StatsSO CharacterStats { get; private set; }

        public CreateNewStatsData()
        {
            CharacterStats = ScriptableObject.CreateInstance<StatsSO>();
        }

        [Button("Create New Enemy Stats - Adds to Enemies Folder", ButtonSizes.Large)]
        private void CreateNewEnemy()
        {
            AssetDatabase.CreateAsset(
                CharacterStats,
                "Assets/Resources/Scriptable Objects/Enemy/New Enemy Stats " + 
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            CharacterStats = ScriptableObject.CreateInstance<StatsSO>();
        }

        [Button("Create New Player Stats - Adds to Player Folder", ButtonSizes.Large)]
        private void CreateNewPlayer()
        {
            AssetDatabase.CreateAsset(
                CharacterStats,
                "Assets/Resources/Scriptable Objects/Player/New Player Stats " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            CharacterStats = ScriptableObject.CreateInstance<StatsSO>();
        }
    }

    /// <summary>
    /// Creates new player character data on odin window.
    /// </summary>
    public class CreateNewPlayerCharacterData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public PlayerCharacterSO CharacterData { get; private set; }

        public CreateNewPlayerCharacterData()
        {
            CharacterData = ScriptableObject.CreateInstance<PlayerCharacterSO>();
        }

        [Button("Create New Player Character - Adds to Player Folder", ButtonSizes.Large)]
        private void CreateNewPlayer()
        {
            AssetDatabase.CreateAsset(
                CharacterData,
                "Assets/Resources/Scriptable Objects/Player/New Player Character " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            CharacterData = ScriptableObject.CreateInstance<PlayerCharacterSO>();
        }
    }

    /// <summary>
    /// Creates new player character data on odin window.
    /// </summary>
    public class CreateNewEnemyCharacterData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public EnemyCharacterSO CharacterData { get; private set; }

        public CreateNewEnemyCharacterData()
        {
            CharacterData = ScriptableObject.CreateInstance<EnemyCharacterSO>();
        }

        [Button("Create New Enemy Character - Adds to Enemies Folder", ButtonSizes.Large)]
        private void CreateNewEnemy()
        {
            AssetDatabase.CreateAsset(
                CharacterData,
                "Assets/Resources/Scriptable Objects/Enemy/New Enemy Character " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            CharacterData = ScriptableObject.CreateInstance<EnemyCharacterSO>();
        }
    }

    /// <summary>
    /// Creates new player values data on odin window.
    /// </summary>
    public class CreateNewPlayerValuesData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public PlayerValuesSO Values { get; private set; }

        public CreateNewPlayerValuesData()
        {
            Values = ScriptableObject.CreateInstance<PlayerValuesSO>();
        }

        [Button("Create New Player Values - Adds to Player Folder", ButtonSizes.Large)]
        private void CreateNewEnemy()
        {
            AssetDatabase.CreateAsset(
                Values,
                "Assets/Resources/Scriptable Objects/Player/New Player Values " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Values = ScriptableObject.CreateInstance<PlayerValuesSO>();
        }
    }

    /// <summary>
    /// Creates new player values data on odin window.
    /// </summary>
    public class CreateNewEnemyValuesData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public EnemyValuesSO Values { get; private set; }
        
        public CreateNewEnemyValuesData()
        {
            Values = ScriptableObject.CreateInstance<EnemyValuesSO>();
        }
        
        [Button("Create New Enemy Values - Adds to Enemies Folder", ButtonSizes.Large)]
        private void CreateNewEnemy()
        {
            AssetDatabase.CreateAsset(
                Values,
                "Assets/Resources/Scriptable Objects/Enemy/New Enemy Values " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();
        
            Values = ScriptableObject.CreateInstance<EnemyValuesSO>();
        }
    }
}
