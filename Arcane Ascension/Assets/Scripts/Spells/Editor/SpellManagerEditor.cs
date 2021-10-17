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
    private CreateSelfHealOneShotBehaviourData selfHealOneShotBehaviourData;
    private CreateApplyDamagePierceBehaviourData applyDamagePierceBehaviourData;
    private CreateApplyDamageBehaviourData applyDamageBehaviourData;
    private CreateBounceOnHitBehaviourData bounceOnHitBehaviourData;
    private CreateCommonBehavioursData commonBehavioursData;
    private CreateDisableProjectileAfterSecondsData disableProjectileAfterSecondsData;
    private CreateDisableProjectileVelocityZeroData disableProjectileVelocityZeroData;
    private CreateSpawnHitPrefabData spawnHitPrefabData;
    private CreateSpawnMuzzlePrefabData spawnMuzzlePrefabData;
    private CreateStopSpellOnHitData stopSpellOnHitData;
    private CreateUpdateManaAndCooldownData updateManaAndCooldownData;

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
        selfHealOneShotBehaviourData = new CreateSelfHealOneShotBehaviourData();
        applyDamagePierceBehaviourData = new CreateApplyDamagePierceBehaviourData();
        applyDamageBehaviourData = new CreateApplyDamageBehaviourData();
        bounceOnHitBehaviourData = new CreateBounceOnHitBehaviourData();
        commonBehavioursData = new CreateCommonBehavioursData();
        disableProjectileAfterSecondsData = new CreateDisableProjectileAfterSecondsData();
        disableProjectileVelocityZeroData = new CreateDisableProjectileVelocityZeroData();
        spawnHitPrefabData = new CreateSpawnHitPrefabData();
        spawnMuzzlePrefabData = new CreateSpawnMuzzlePrefabData();
        stopSpellOnHitData = new CreateStopSpellOnHitData();
        updateManaAndCooldownData = new CreateUpdateManaAndCooldownData();

        tree.Add("Create New Behaviour/New Behaviour Forward", new CreateForwardBehaviourData());
        tree.Add("Create New Behaviour/New Behaviour Continuous", new CreateContinuousBehaviourData());
        tree.Add("Create New Behaviour/New Behaviour Self Heal", new CreateSelfHealOneShotBehaviourData());
        tree.Add("Create New Behaviour/New Behaviour Apply Damage Pierce", new CreateApplyDamagePierceBehaviourData());
        tree.Add("Create New Behaviour/New Behaviour Apply Damage", new CreateApplyDamageBehaviourData());
        tree.Add("Create New Behaviour/New Behaviour Bounce On Hit", new CreateBounceOnHitBehaviourData());
        tree.Add("Create New Behaviour/New Behaviour Common Behaviours", new CreateCommonBehavioursData());
        tree.Add("Create New Behaviour/New Behaviour Disable Projectile After Seconds", new CreateDisableProjectileAfterSecondsData());
        tree.Add("Create New Behaviour/New Behaviour Disable Projectile Velocity Zero", new CreateDisableProjectileVelocityZeroData());
        tree.Add("Create New Behaviour/New Behaviour Spawn Hit Prefab", new CreateSpawnHitPrefabData());
        tree.Add("Create New Behaviour/New Behaviour Spawn Muzzle Prefab", new CreateSpawnMuzzlePrefabData());
        tree.Add("Create New Behaviour/New Behaviour Stop Spell On Hit", new CreateStopSpellOnHitData());
        tree.Add("Create New Behaviour/New Behaviour Update Mana And Cooldown", new CreateUpdateManaAndCooldownData());

        tree.AddAllAssetsAtPath("Spell Behaviours", 
            "Assets/Resources/Scriptable Objects/Spell Behaviours", typeof(SpellBehaviourAbstractSO));
        tree.AddAllAssetsAtPath("Spell Behaviours", 
            "Assets/Resources/Scriptable Objects/Spell Behaviours/Apply Damage Behaviours", typeof(SpellBehaviourAbstractSO));
        tree.AddAllAssetsAtPath("Spell Behaviours",
            "Assets/Resources/Scriptable Objects/Spell Behaviours/Common Behaviours", typeof(SpellBehaviourAbstractSO));
        tree.AddAllAssetsAtPath("Spell Behaviours",
            "Assets/Resources/Scriptable Objects/Spell Behaviours/Stop Spells Behaviours", typeof(SpellBehaviourAbstractSO));

        tree.Add("Create New Spell", new CreateNewSpellData());
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

        if (continuousBehaviourData != null)
            DestroyImmediate(continuousBehaviourData.Spell);

        if (forwardBehaviourData != null)
            DestroyImmediate(forwardBehaviourData.Spell);

        if (selfHealOneShotBehaviourData != null)
            DestroyImmediate(selfHealOneShotBehaviourData.Spell);

        if (applyDamagePierceBehaviourData != null)
            DestroyImmediate(applyDamagePierceBehaviourData.Spell);

        if (applyDamageBehaviourData != null)
            DestroyImmediate(applyDamageBehaviourData.Spell);

        if (bounceOnHitBehaviourData != null)
            DestroyImmediate(bounceOnHitBehaviourData.Spell);

        if (commonBehavioursData != null)
            DestroyImmediate(commonBehavioursData.Spell);

        if (disableProjectileAfterSecondsData != null)
            DestroyImmediate(disableProjectileAfterSecondsData.Spell);

        if (disableProjectileVelocityZeroData != null)
            DestroyImmediate(disableProjectileVelocityZeroData.Spell);

        if (spawnHitPrefabData != null)
            DestroyImmediate(spawnHitPrefabData.Spell);

        if (spawnMuzzlePrefabData != null)
            DestroyImmediate(spawnMuzzlePrefabData.Spell);

        if (stopSpellOnHitData != null)
            DestroyImmediate(stopSpellOnHitData.Spell);

        if (updateManaAndCooldownData != null)
            DestroyImmediate(updateManaAndCooldownData.Spell);
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
                "Assets/Resources/Scriptable Objects/Spell Behaviours/New Spell Forward Behaviour " +
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
                "Assets/Resources/Scriptable Objects/Spell Behaviours/New Spell Continuous Behaviour " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Spell = ScriptableObject.CreateInstance<SpellBehaviourContinuousSO>();
        }
    }

    public class CreateSelfHealOneShotBehaviourData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellBehaviourOneShotSelfHealSO Spell { get; private set; }

        public CreateSelfHealOneShotBehaviourData()
        {
            Spell = ScriptableObject.CreateInstance<SpellBehaviourOneShotSelfHealSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Spell,
                "Assets/Resources/Scriptable Objects/Spell Behaviours/New Spell Self Heal Behaviour " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Spell = ScriptableObject.CreateInstance<SpellBehaviourOneShotSelfHealSO>();
        }
    }

    public class CreateApplyDamagePierceBehaviourData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellBehaviourApplyDamagePierceSO Spell { get; private set; }

        public CreateApplyDamagePierceBehaviourData()
        {
            Spell = ScriptableObject.CreateInstance<SpellBehaviourApplyDamagePierceSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Spell,
                "Assets/Resources/Scriptable Objects/Spell Behaviours/New Spell Apply Damage Pierce Behaviour " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Spell = ScriptableObject.CreateInstance<SpellBehaviourApplyDamagePierceSO>();
        }
    }

    public class CreateApplyDamageBehaviourData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellBehaviourApplyDamageSO Spell { get; private set; }

        public CreateApplyDamageBehaviourData()
        {
            Spell = ScriptableObject.CreateInstance<SpellBehaviourApplyDamageSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Spell,
                "Assets/Resources/Scriptable Objects/Spell Behaviours/New Spell Apply Damage Behaviour " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Spell = ScriptableObject.CreateInstance<SpellBehaviourApplyDamageSO>();
        }
    }

    public class CreateBounceOnHitBehaviourData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellBehaviourBounceOnHitSO Spell { get; private set; }

        public CreateBounceOnHitBehaviourData()
        {
            Spell = ScriptableObject.CreateInstance<SpellBehaviourBounceOnHitSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Spell,
                "Assets/Resources/Scriptable Objects/Spell Behaviours/New Spell Bounce On Hit Behaviour " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Spell = ScriptableObject.CreateInstance<SpellBehaviourBounceOnHitSO>();
        }
    }

    public class CreateCommonBehavioursData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellBehaviourCommonBehavioursSO Spell { get; private set; }

        public CreateCommonBehavioursData()
        {
            Spell = ScriptableObject.CreateInstance<SpellBehaviourCommonBehavioursSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Spell,
                "Assets/Resources/Scriptable Objects/Spell Behaviours/New Spell Common Behaviour " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Spell = ScriptableObject.CreateInstance<SpellBehaviourCommonBehavioursSO>();
        }
    }

    public class CreateDisableProjectileAfterSecondsData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellBehaviourDisableProjectileAfterSecondsSO Spell { get; private set; }

        public CreateDisableProjectileAfterSecondsData()
        {
            Spell = ScriptableObject.CreateInstance<SpellBehaviourDisableProjectileAfterSecondsSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Spell,
                "Assets/Resources/Scriptable Objects/Spell Behaviours/New Spell Disable Projectile After Seconds Behaviour " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Spell = ScriptableObject.CreateInstance<SpellBehaviourDisableProjectileAfterSecondsSO>();
        }
    }

    public class CreateDisableProjectileVelocityZeroData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellBehaviourDisableProjectileVelocityZeroSO Spell { get; private set; }

        public CreateDisableProjectileVelocityZeroData()
        {
            Spell = ScriptableObject.CreateInstance<SpellBehaviourDisableProjectileVelocityZeroSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Spell,
                "Assets/Resources/Scriptable Objects/Spell Behaviours/New Spell Disable Projectile Velocity Zero Behaviour " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Spell = ScriptableObject.CreateInstance<SpellBehaviourDisableProjectileVelocityZeroSO>();
        }
    }

    public class CreateSpawnHitPrefabData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellBehaviourSpawnHitPrefabSO Spell { get; private set; }

        public CreateSpawnHitPrefabData()
        {
            Spell = ScriptableObject.CreateInstance<SpellBehaviourSpawnHitPrefabSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Spell,
                "Assets/Resources/Scriptable Objects/Spell Behaviours/New Spell Spawn Hit Prefab Behaviour " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Spell = ScriptableObject.CreateInstance<SpellBehaviourSpawnHitPrefabSO>();
        }
    }

    public class CreateSpawnMuzzlePrefabData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellBehaviourSpawnMuzzlePrefabSO Spell { get; private set; }

        public CreateSpawnMuzzlePrefabData()
        {
            Spell = ScriptableObject.CreateInstance<SpellBehaviourSpawnMuzzlePrefabSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Spell,
                "Assets/Resources/Scriptable Objects/Spell Behaviours/New Spell Spawn Muzzle Prefab Behaviour " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Spell = ScriptableObject.CreateInstance<SpellBehaviourSpawnMuzzlePrefabSO>();
        }
    }

    public class CreateStopSpellOnHitData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellBehaviourStopSpellOnHitSO Spell { get; private set; }

        public CreateStopSpellOnHitData()
        {
            Spell = ScriptableObject.CreateInstance<SpellBehaviourStopSpellOnHitSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Spell,
                "Assets/Resources/Scriptable Objects/Spell Behaviours/New Spell Stop Spell On Hit Behaviour " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Spell = ScriptableObject.CreateInstance<SpellBehaviourStopSpellOnHitSO>();
        }
    }

    public class CreateUpdateManaAndCooldownData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellBehaviourUpdateManaAndCooldownSO Spell { get; private set; }

        public CreateUpdateManaAndCooldownData()
        {
            Spell = ScriptableObject.CreateInstance<SpellBehaviourUpdateManaAndCooldownSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Spell,
                "Assets/Resources/Scriptable Objects/Spell Behaviours/New Spell Update Mana And Cooldown Behaviour " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Spell = ScriptableObject.CreateInstance<SpellBehaviourUpdateManaAndCooldownSO>();
        }
    }
}
