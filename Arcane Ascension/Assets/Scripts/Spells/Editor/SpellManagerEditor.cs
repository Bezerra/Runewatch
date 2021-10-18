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

    private readonly string CREATENEWONESHOTBEHAVIOUR = "Create New One Shot Behaviour";
    private readonly string ONESHOTBEHAVIOURPATH = "Assets/Resources/Scriptable Objects/Spell Behaviours/One Shot";

    private readonly string CREATENEWCONTINUOUSBEHAVIOUR = "Create New Continuous Behaviour";
    private readonly string CONTINUOUSBEHAVIOURPATH = "Assets/Resources/Scriptable Objects/Spell Behaviours/Continuous";

    private readonly string CREATENEWDAMAGEBEHAVIOUR = "Create New Damage Behaviour";
    private readonly string SPELLDAMAGEBEHAVIOURSPATH = "Assets/Resources/Scriptable Objects/Spell Damage Behaviours";

    private readonly string CREATENEWSPELL = "Create New Spell";
    private readonly string SPELLSPATH = "Assets/Resources/Scriptable Objects/Spells";

    private readonly string CREATENEWONHITONESHOTBEHAVIOUR = "Create New On Hit Behaviour/One Shot";
    private readonly string ONHITBEHAVIOURSONESHOTPATH = "Assets/Resources/Scriptable Objects/Spell Hit Behaviours";

    private readonly string CREATENEWMUZZLEBEHAVIOUR = "Create New Muzzle Behaviour/One Shot";
    private readonly string MUZZLEBEHAVIOURSONESHOTPATH = "Assets/Resources/Scriptable Objects/Spell Muzzle Behaviours";

    private CreateNewSpellOneShotData createNewSpellOneShotData;
    private CreateNewSpellContinuousData createNewSpellContinuousData;
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
    private CreateSpellDamageSingleTargetData spellDamageSingleTarget;
    private CreateSpellDamageAoEData spellDamageAoE;
    private CreateSpellDamageAoEOvertimeData spellDamageAoEOvertime;
    private CreateSpellDamageOvertimeData spellDamageOvertime;
    private CreateSpellMuzzleDisableData spellMuzzleDisableData;
    private CreateSpellOnHitDisableData spellOnHitDisableData;

    protected override void OnGUI()
    {
        SirenixEditorGUI.Title("Spell Manager", "", TextAlignment.Center, true, true);
        EditorGUILayout.Space(20);
        base.OnGUI();
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        OdinMenuTree tree = new OdinMenuTree();

        createNewSpellOneShotData = new CreateNewSpellOneShotData();
        createNewSpellContinuousData = new CreateNewSpellContinuousData();
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
        spellDamageSingleTarget = new CreateSpellDamageSingleTargetData();
        spellDamageAoE = new CreateSpellDamageAoEData();
        spellDamageAoEOvertime = new CreateSpellDamageAoEOvertimeData();
        spellDamageOvertime = new CreateSpellDamageOvertimeData();
        spellMuzzleDisableData = new CreateSpellMuzzleDisableData();
        spellOnHitDisableData = new CreateSpellOnHitDisableData();

        tree.Add($"{CREATENEWONESHOTBEHAVIOUR}/New Behaviour Forward", forwardBehaviourData);
        tree.Add($"{CREATENEWONESHOTBEHAVIOUR}/New Behaviour Self Heal", selfHealOneShotBehaviourData);
        tree.Add($"{CREATENEWONESHOTBEHAVIOUR}/New Behaviour Apply Damage Pierce", applyDamagePierceBehaviourData);
        tree.Add($"{CREATENEWONESHOTBEHAVIOUR}/New Behaviour Apply Damage", applyDamageBehaviourData);
        tree.Add($"{CREATENEWONESHOTBEHAVIOUR}/New Behaviour Bounce On Hit", bounceOnHitBehaviourData);
        tree.Add($"{CREATENEWONESHOTBEHAVIOUR}/New Behaviour Common Behaviours", commonBehavioursData);
        tree.Add($"{CREATENEWONESHOTBEHAVIOUR}/New Behaviour Disable Projectile After Seconds", disableProjectileAfterSecondsData);
        tree.Add($"{CREATENEWONESHOTBEHAVIOUR}/New Behaviour Disable Projectile Velocity Zero", disableProjectileVelocityZeroData);
        tree.Add($"{CREATENEWONESHOTBEHAVIOUR}/New Behaviour Spawn Hit Prefab", spawnHitPrefabData);
        tree.Add($"{CREATENEWONESHOTBEHAVIOUR}/New Behaviour Spawn Muzzle Prefab", spawnMuzzlePrefabData);
        tree.Add($"{CREATENEWONESHOTBEHAVIOUR}/New Behaviour Stop Spell On Hit", stopSpellOnHitData);
        tree.Add($"{CREATENEWONESHOTBEHAVIOUR}/New Behaviour Update Mana And Cooldown", updateManaAndCooldownData);
        tree.Add($"{CREATENEWCONTINUOUSBEHAVIOUR}/New Behaviour Continuous", continuousBehaviourData);

        tree.Add($"{CREATENEWDAMAGEBEHAVIOUR}/New Behaviour Damage Single Target", spellDamageSingleTarget);
        tree.Add($"{CREATENEWDAMAGEBEHAVIOUR}/New Behaviour Damage Overtime", spellDamageOvertime);
        tree.Add($"{CREATENEWDAMAGEBEHAVIOUR}/New Behaviour Damage AoE", spellDamageAoE);
        tree.Add($"{CREATENEWDAMAGEBEHAVIOUR}/New Behaviour Damage AoE Overtime", spellDamageAoEOvertime);

        tree.Add($"{CREATENEWONHITONESHOTBEHAVIOUR}/New On Hit Disable Behaviour", spellOnHitDisableData);

        tree.Add($"{CREATENEWMUZZLEBEHAVIOUR}/New Muzzle Disable Behaviour", spellMuzzleDisableData);

        tree.AddAllAssetsAtPath("Spell Behaviours/One Shot",
            $"{ONESHOTBEHAVIOURPATH}", typeof(SpellBehaviourAbstractOneShotSO));
        tree.AddAllAssetsAtPath("Spell Behaviours/One Shot",
            $"{ONESHOTBEHAVIOURPATH}/Apply Damage Behaviours", typeof(SpellBehaviourAbstractSO));
        tree.AddAllAssetsAtPath("Spell Behaviours/One Shot",
            $"{ONESHOTBEHAVIOURPATH}/Common Behaviours", typeof(SpellBehaviourAbstractSO));
        tree.AddAllAssetsAtPath("Spell Behaviours/One Shot",
            $"{ONESHOTBEHAVIOURPATH}/Stop Spells Behaviours", typeof(SpellBehaviourAbstractSO));
        tree.AddAllAssetsAtPath("Spell Damage Behaviours",
            $"{SPELLDAMAGEBEHAVIOURSPATH}", typeof(DamageBehaviourAbstractSO));
        tree.AddAllAssetsAtPath("Spell On Hit Behaviours/One Shot",
            $"{ONHITBEHAVIOURSONESHOTPATH}", typeof(SpellOnHitBehaviourAbstractSO));
        tree.AddAllAssetsAtPath("Spell Muzzle Behaviours/One Shot",
            $"{MUZZLEBEHAVIOURSONESHOTPATH}", typeof(SpellMuzzleBehaviourAbstractSO));
        tree.AddAllAssetsAtPath("Spell Behaviours/Continuous",
            $"{CONTINUOUSBEHAVIOURPATH}", typeof(SpellBehaviourAbstractContinuousSO));


        tree.Add($"{CREATENEWSPELL}/Create New One Shot Spell", createNewSpellOneShotData);
        tree.Add($"{CREATENEWSPELL}/Create New Continuous Spell", createNewSpellContinuousData);
        tree.AddAllAssetsAtPath("Spell Data", $"{SPELLSPATH}", typeof(SpellSO));

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

                createNewSpellOneShotData.UpdateAllSpellsList();
                createNewSpellContinuousData.UpdateAllSpellsList();

                AssetDatabase.SaveAssets();
            }
        }
        SirenixEditorGUI.EndHorizontalToolbar();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (createNewSpellOneShotData != null)
            DestroyImmediate(createNewSpellOneShotData.Spell);

        if (createNewSpellContinuousData != null)
            DestroyImmediate(createNewSpellContinuousData.Spell);

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

        if (spellDamageSingleTarget != null)
            DestroyImmediate(spellDamageSingleTarget.Spell);

        if (spellDamageOvertime != null)
            DestroyImmediate(spellDamageOvertime.Spell);

        if (spellDamageAoE != null)
            DestroyImmediate(spellDamageAoE.Spell);

        if (spellDamageAoEOvertime != null)
            DestroyImmediate(spellDamageAoEOvertime.Spell);

        if (spellOnHitDisableData != null)
            DestroyImmediate(spellOnHitDisableData.Spell);

        if (spellMuzzleDisableData != null)
            DestroyImmediate(spellMuzzleDisableData.Spell);
    }

    /// <summary>
    /// Creates new spell data on odin window.
    /// </summary>
    public class CreateNewSpellOneShotData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellOneShotSO Spell { get; private set; }

        public CreateNewSpellOneShotData()
        {
            Spell = ScriptableObject.CreateInstance<SpellOneShotSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Spell, "Assets/Resources/Scriptable Objects/Spells/New Spell " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Spell = ScriptableObject.CreateInstance<SpellOneShotSO>();

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
                "Assets/Resources/Scriptable Objects/Spell Behaviours/One Shot/New Spell Continuous Behaviour " +
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
                "Assets/Resources/Scriptable Objects/Spell Behaviours/One Shot/New Spell Self Heal Behaviour " +
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
                "Assets/Resources/Scriptable Objects/Spell Behaviours/One Shot/New Spell Apply Damage Pierce Behaviour " +
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
                "Assets/Resources/Scriptable Objects/Spell Behaviours/One Shot/New Spell Apply Damage Behaviour " +
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
                "Assets/Resources/Scriptable Objects/Spell Behaviours/One Shot/New Spell Bounce On Hit Behaviour " +
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
                "Assets/Resources/Scriptable Objects/Spell Behaviours/One Shot/New Spell Common Behaviour " +
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
                "Assets/Resources/Scriptable Objects/Spell Behaviours/One Shot/New Spell Disable Projectile After Seconds Behaviour " +
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
                "Assets/Resources/Scriptable Objects/Spell Behaviours/One Shot/New Spell Disable Projectile Velocity Zero Behaviour " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Spell = ScriptableObject.CreateInstance<SpellBehaviourDisableProjectileVelocityZeroSO>();
        }
    }

    public class CreateSpawnHitPrefabData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellBehaviourSpawnHitPrefabOneShotSO Spell { get; private set; }

        public CreateSpawnHitPrefabData()
        {
            Spell = ScriptableObject.CreateInstance<SpellBehaviourSpawnHitPrefabOneShotSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Spell,
                "Assets/Resources/Scriptable Objects/Spell Behaviours/One Shot/New Spell Spawn Hit Prefab Behaviour " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Spell = ScriptableObject.CreateInstance<SpellBehaviourSpawnHitPrefabOneShotSO>();
        }
    }

    public class CreateSpawnMuzzlePrefabData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellBehaviourSpawnMuzzlePrefabOneShotSO Spell { get; private set; }

        public CreateSpawnMuzzlePrefabData()
        {
            Spell = ScriptableObject.CreateInstance<SpellBehaviourSpawnMuzzlePrefabOneShotSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Spell,
                "Assets/Resources/Scriptable Objects/Spell Behaviours/One Shot/New Spell Spawn Muzzle Prefab Behaviour " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Spell = ScriptableObject.CreateInstance<SpellBehaviourSpawnMuzzlePrefabOneShotSO>();
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
                "Assets/Resources/Scriptable Objects/Spell Behaviours/One Shot/New Spell Stop Spell On Hit Behaviour " +
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
                "Assets/Resources/Scriptable Objects/Spell Behaviours/One Shot/New Spell Update Mana And Cooldown Behaviour " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Spell = ScriptableObject.CreateInstance<SpellBehaviourUpdateManaAndCooldownSO>();
        }
    }

    public class CreateSpellDamageSingleTargetData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public DamageSingleTargetSO Spell { get; private set; }

        public CreateSpellDamageSingleTargetData()
        {
            Spell = ScriptableObject.CreateInstance<DamageSingleTargetSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Spell,
                "Assets/Resources/Scriptable Objects/Spell Damage Behaviours/New Spell Damage Single Target " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Spell = ScriptableObject.CreateInstance<DamageSingleTargetSO>();
        }
    }

    public class CreateSpellDamageOvertimeData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public DamageOvertimeSO Spell { get; private set; }

        public CreateSpellDamageOvertimeData()
        {
            Spell = ScriptableObject.CreateInstance<DamageOvertimeSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Spell,
                "Assets/Resources/Scriptable Objects/Spell Damage Behaviours/New Spell Damage Overtime " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Spell = ScriptableObject.CreateInstance<DamageOvertimeSO>();
        }
    }

    public class CreateSpellDamageAoEData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public DamageAoESO Spell { get; private set; }

        public CreateSpellDamageAoEData()
        {
            Spell = ScriptableObject.CreateInstance<DamageAoESO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Spell,
                "Assets/Resources/Scriptable Objects/Spell Damage Behaviours/New Spell Damage AoE " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Spell = ScriptableObject.CreateInstance<DamageAoESO>();
        }
    }

    public class CreateSpellDamageAoEOvertimeData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public DamageAoEOvertimeSO Spell { get; private set; }

        public CreateSpellDamageAoEOvertimeData()
        {
            Spell = ScriptableObject.CreateInstance<DamageAoEOvertimeSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Spell,
                "Assets/Resources/Scriptable Objects/Spell Damage Behaviours/New Spell Damage AoE Overtime " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Spell = ScriptableObject.CreateInstance<DamageAoEOvertimeSO>();
        }
    }

    public class CreateSpellMuzzleDisableData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellMuzzleBehaviourDisable Spell { get; private set; }

        public CreateSpellMuzzleDisableData()
        {
            Spell = ScriptableObject.CreateInstance<SpellMuzzleBehaviourDisable>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Spell,
                "Assets/Resources/Scriptable Objects/Spell Muzzle Behaviours/New Spell Muzzle Behaviour Disable " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Spell = ScriptableObject.CreateInstance<SpellMuzzleBehaviourDisable>();
        }
    }

    public class CreateSpellOnHitDisableData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellOnHitBehaviourDisable Spell { get; private set; }

        public CreateSpellOnHitDisableData()
        {
            Spell = ScriptableObject.CreateInstance<SpellOnHitBehaviourDisable>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Spell,
                "Assets/Resources/Scriptable Objects/Spell Hit Behaviours/New Spell Hit Behaviour Disable " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Spell = ScriptableObject.CreateInstance<SpellOnHitBehaviourDisable>();
        }
    }

    public class CreateNewSpellContinuousData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellContinuousSO Spell { get; private set; }

        public CreateNewSpellContinuousData()
        {
            Spell = ScriptableObject.CreateInstance<SpellContinuousSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Spell, $"Assets/Resources/Scriptable Objects/Spells/New Spell " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Spell = ScriptableObject.CreateInstance<SpellContinuousSO>();

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
}
