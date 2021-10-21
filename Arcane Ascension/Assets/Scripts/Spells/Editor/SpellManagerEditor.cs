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

    private readonly string CREATENEWONESHOTBEHAVIOUR = "Create New Behaviour/One Shot";
    private readonly string ONESHOTBEHAVIOURPATH = "Assets/Resources/Scriptable Objects/Spell Behaviours/One Shot";

    private readonly string CREATENEWCONTINUOUSBEHAVIOUR = "Create New Behaviour/Continuous";
    private readonly string CONTINUOUSBEHAVIOURPATH = "Assets/Resources/Scriptable Objects/Spell Behaviours/Continuous";

    private readonly string CREATENEWDAMAGEBEHAVIOUR = "Create New Damage Behaviour";
    private readonly string SPELLDAMAGEBEHAVIOURSPATH = "Assets/Resources/Scriptable Objects/Spell Damage Behaviours";

    private readonly string CREATENEWSPELL = "Create New Spell";
    private readonly string SPELLSPATH = "Assets/Resources/Scriptable Objects/Spells";

    private readonly string CREATENEWONHITONESHOTBEHAVIOUR = "Create New On Hit Behaviour/One Shot";
    private readonly string ONHITBEHAVIOURSONESHOTPATH = "Assets/Resources/Scriptable Objects/Spell Hit Behaviours/One Shot";

    private readonly string CREATENEWMUZZLEONESHOTBEHAVIOUR = "Create New Muzzle Behaviour/One Shot";
    private readonly string MUZZLEBEHAVIOURSONESHOTPATH = "Assets/Resources/Scriptable Objects/Spell Muzzle Behaviours/One Shot";

    private readonly string CREATENEWONHITCONTINUOUSBEHAVIOUR = "Create New On Hit Behaviour/Continuous";
    private readonly string ONHITBEHAVIOURSCONTINUOUSPATH = "Assets/Resources/Scriptable Objects/Spell Hit Behaviours/Continuous";

    private readonly string CREATENEWMUZZLECONTINUOUSBEHAVIOUR = "Create New Muzzle Behaviour/Continuous";
    private readonly string MUZZLEBEHAVIOURSCONTINUOUSPATH = "Assets/Resources/Scriptable Objects/Spell Muzzle Behaviours/Continuous";

    // New spells
    private CreateNewSpellOneShotData createNewSpellOneShotData;
    private CreateNewSpellContinuousData createNewSpellContinuousData;
    
    // One Shot Behaviours
    private CreateSpellForwardBehaviourData forwardBehaviourData;
    private CreateSpellSelfHealOneShotBehaviourData selfHealOneShotBehaviourData;
    private CreateSpellApplyDamagePierceBehaviourData applyDamagePierceBehaviourData;
    private CreateSpellApplyDamageBehaviourData applyDamageBehaviourData;
    private CreateSpellBounceOnHitBehaviourData bounceOnHitBehaviourData;
    private CreateSpellCommonBehavioursOneShotData commonBehavioursData;
    private CreateSpellDisableProjectileAfterSecondsOneShotData disableProjectileAfterSecondsData;
    private CreateSpellDisableProjectileAfterCollisionOneShotData disableProjectileAfterCollisionData;
    private CreateSpellStopSpellOnHitOneShotData stopSpellOnHitData;
    private CreateSpellUpdateManaAndCooldownOneShotData updateManaAndCooldownData;
    private CreateHitSpawnHitPrefabData spawnHitPrefabData;
    private CreateMuzzleSpawnMuzzlePrefabOneShotData spawnMuzzlePrefabData;
    private CreateSpellHoverAreaVFXFloorData spawnAreaHoverEffectWallsFloor;
    private CreateSpellHoverAreaVFXWallsFloorData spawnAreaHoverEffectFloor;

    // One Shot Hits and Muzzles
    private CreateSpellMuzzleDisableOneShotData spellMuzzleDisableData;
    private CreateSpellOnHitDisableOneShotData spellOnHitDisableData;
    private CreateSpellOnHitDisableSpellMaxTimeOneShotData spellOnHitDisableWithSpellMaxTimeData;

    // Continuous Behaviours
    private CreateSpellContinuousBehaviourData continuousBehaviourData;
    private CreateSpellContinuousApplyDamageData continuousApplyDamageBehaviourData;
    private CreateSpellContinuousManaUpdateData continuousSpellManaUpdateData;
    private CreateSpawnMuzzlePrefabContinuousData continuousSpawnMuzzlePrefabData;
    private CreateSpellBehaviourSpawnOnHitPrefabContinuousData continuousSpawnOnHitPrefabData;

    // Continuous Hits and Muzzles
    private CreateSpellContinuousUpdatePositionAndDisableData continuousUpdateMuzzleAndDisable;
    private CreateContinuousSpellOnHitBehaviourDisableData continuousHitAndDisable;

    // Damage
    private CreateSpellDamageSingleTargetData spellDamageSingleTarget;
    private CreateSpellDamageAoEData spellDamageAoE;
    private CreateSpellDamageAoEOvertimeData spellDamageAoEOvertime;
    private CreateSpellDamageOvertimeData spellDamageOvertime;

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
        
        // One Shot behaviours
        forwardBehaviourData = new CreateSpellForwardBehaviourData();
        selfHealOneShotBehaviourData = new CreateSpellSelfHealOneShotBehaviourData();
        applyDamagePierceBehaviourData = new CreateSpellApplyDamagePierceBehaviourData();
        applyDamageBehaviourData = new CreateSpellApplyDamageBehaviourData();
        bounceOnHitBehaviourData = new CreateSpellBounceOnHitBehaviourData();
        commonBehavioursData = new CreateSpellCommonBehavioursOneShotData();
        disableProjectileAfterSecondsData = new CreateSpellDisableProjectileAfterSecondsOneShotData();
        disableProjectileAfterCollisionData = new CreateSpellDisableProjectileAfterCollisionOneShotData();
        spawnHitPrefabData = new CreateHitSpawnHitPrefabData();
        spawnMuzzlePrefabData = new CreateMuzzleSpawnMuzzlePrefabOneShotData();
        stopSpellOnHitData = new CreateSpellStopSpellOnHitOneShotData();
        updateManaAndCooldownData = new CreateSpellUpdateManaAndCooldownOneShotData();
        spawnAreaHoverEffectWallsFloor = new CreateSpellHoverAreaVFXFloorData();
        spawnAreaHoverEffectFloor = new CreateSpellHoverAreaVFXWallsFloorData();

        // One Shot hits and muzzles
        spellMuzzleDisableData = new CreateSpellMuzzleDisableOneShotData();
        spellOnHitDisableData = new CreateSpellOnHitDisableOneShotData();
        spellOnHitDisableWithSpellMaxTimeData = new CreateSpellOnHitDisableSpellMaxTimeOneShotData();

        // Continuous Behavioiurs
        continuousBehaviourData = new CreateSpellContinuousBehaviourData();
        continuousApplyDamageBehaviourData = new CreateSpellContinuousApplyDamageData();
        continuousSpellManaUpdateData = new CreateSpellContinuousManaUpdateData();
        continuousSpawnMuzzlePrefabData = new CreateSpawnMuzzlePrefabContinuousData();
        continuousSpawnOnHitPrefabData = new CreateSpellBehaviourSpawnOnHitPrefabContinuousData();

        // Continuous Hits and muzzles
        continuousUpdateMuzzleAndDisable = new CreateSpellContinuousUpdatePositionAndDisableData();
        continuousHitAndDisable = new CreateContinuousSpellOnHitBehaviourDisableData();

        // Damage
        spellDamageSingleTarget = new CreateSpellDamageSingleTargetData();
        spellDamageAoE = new CreateSpellDamageAoEData();
        spellDamageAoEOvertime = new CreateSpellDamageAoEOvertimeData();
        spellDamageOvertime = new CreateSpellDamageOvertimeData();

        // One shot behaviours
        tree.Add($"{CREATENEWONESHOTBEHAVIOUR}/New Behaviour Forward", forwardBehaviourData);
        tree.Add($"{CREATENEWONESHOTBEHAVIOUR}/New Behaviour Self Heal", selfHealOneShotBehaviourData);
        tree.Add($"{CREATENEWONESHOTBEHAVIOUR}/New Behaviour Apply Damage Pierce", applyDamagePierceBehaviourData);
        tree.Add($"{CREATENEWONESHOTBEHAVIOUR}/New Behaviour Apply Damage", applyDamageBehaviourData);
        tree.Add($"{CREATENEWONESHOTBEHAVIOUR}/New Behaviour Bounce On Hit", bounceOnHitBehaviourData);
        tree.Add($"{CREATENEWONESHOTBEHAVIOUR}/New Behaviour Common Behaviours", commonBehavioursData);
        tree.Add($"{CREATENEWONESHOTBEHAVIOUR}/New Behaviour Disable Projectile After Seconds", disableProjectileAfterSecondsData);
        tree.Add($"{CREATENEWONESHOTBEHAVIOUR}/New Behaviour Disable Projectile After Collision", disableProjectileAfterCollisionData);
        tree.Add($"{CREATENEWONESHOTBEHAVIOUR}/New Behaviour Spawn Hit Prefab", spawnHitPrefabData);
        tree.Add($"{CREATENEWONESHOTBEHAVIOUR}/New Behaviour Spawn Muzzle Prefab", spawnMuzzlePrefabData);
        tree.Add($"{CREATENEWONESHOTBEHAVIOUR}/New Behaviour Stop Spell On Hit", stopSpellOnHitData);
        tree.Add($"{CREATENEWONESHOTBEHAVIOUR}/New Behaviour Mana And Cooldown Update", updateManaAndCooldownData);
        tree.Add($"{CREATENEWONESHOTBEHAVIOUR}/New Behaviour Area VFX Hover Walls Floor", spawnAreaHoverEffectWallsFloor);
        tree.Add($"{CREATENEWONESHOTBEHAVIOUR}/New Behaviour Area VFX Hover Floor", spawnAreaHoverEffectFloor);

        // Continuous Behaviours
        tree.Add($"{CREATENEWCONTINUOUSBEHAVIOUR}/New Behaviour Continuous", continuousBehaviourData);
        tree.Add($"{CREATENEWCONTINUOUSBEHAVIOUR}/New Behaviour Continuous Apply Damage", continuousApplyDamageBehaviourData);
        tree.Add($"{CREATENEWCONTINUOUSBEHAVIOUR}/New Behaviour Mana Update", continuousSpellManaUpdateData);
        tree.Add($"{CREATENEWCONTINUOUSBEHAVIOUR}/New Behaviour Spawn Muzzle Prefab", continuousSpawnMuzzlePrefabData);
        tree.Add($"{CREATENEWCONTINUOUSBEHAVIOUR}/New Behaviour Spawn On Hit Prefab", continuousSpawnOnHitPrefabData);

        // One shot on hit and muzzles
        tree.Add($"{CREATENEWMUZZLEONESHOTBEHAVIOUR}/New Muzzle Disable Behaviour", spellMuzzleDisableData);
        tree.Add($"{CREATENEWONHITONESHOTBEHAVIOUR}/New On Hit Disable Behaviour", spellOnHitDisableData);
        tree.Add($"{CREATENEWONHITONESHOTBEHAVIOUR}/New On Hit Disable With Spell Max TimeBehaviour", spellOnHitDisableWithSpellMaxTimeData);

        // Continuous on hit and muzzles
        tree.Add($"{CREATENEWMUZZLECONTINUOUSBEHAVIOUR}/New Muzzle Position And Disable Behaviour", continuousUpdateMuzzleAndDisable);
        tree.Add($"{CREATENEWONHITCONTINUOUSBEHAVIOUR}/New Hit Disable Behaviour", continuousHitAndDisable);

        // Damage behaviours
        tree.Add($"{CREATENEWDAMAGEBEHAVIOUR}/New Behaviour Damage Single Target", spellDamageSingleTarget);
        tree.Add($"{CREATENEWDAMAGEBEHAVIOUR}/New Behaviour Damage Overtime", spellDamageOvertime);
        tree.Add($"{CREATENEWDAMAGEBEHAVIOUR}/New Behaviour Damage AoE", spellDamageAoE);
        tree.Add($"{CREATENEWDAMAGEBEHAVIOUR}/New Behaviour Damage AoE Overtime", spellDamageAoEOvertime);

        // One Shot
        tree.AddAllAssetsAtPath("Spell Behaviours/One Shot",
            $"{ONESHOTBEHAVIOURPATH}", typeof(SpellBehaviourAbstractOneShotSO));
        tree.AddAllAssetsAtPath("Spell Behaviours/One Shot",
            $"{ONESHOTBEHAVIOURPATH}/Apply Damage Behaviours", typeof(SpellBehaviourAbstractSO));
        tree.AddAllAssetsAtPath("Spell Behaviours/One Shot",
            $"{ONESHOTBEHAVIOURPATH}/Common Behaviours", typeof(SpellBehaviourAbstractSO));
        tree.AddAllAssetsAtPath("Spell Behaviours/One Shot",
            $"{ONESHOTBEHAVIOURPATH}/Stop Spells Behaviours", typeof(SpellBehaviourAbstractSO));
        tree.AddAllAssetsAtPath("Spell On Hit Behaviours/One Shot",
            $"{ONHITBEHAVIOURSONESHOTPATH}", typeof(SpellOnHitBehaviourAbstractSO));
        tree.AddAllAssetsAtPath("Spell Muzzle Behaviours/One Shot",
            $"{MUZZLEBEHAVIOURSONESHOTPATH}", typeof(SpellMuzzleBehaviourAbstractSO));

        // Continuous
        tree.AddAllAssetsAtPath("Spell Behaviours/Continuous",
            $"{CONTINUOUSBEHAVIOURPATH}", typeof(SpellBehaviourAbstractContinuousSO));
        tree.AddAllAssetsAtPath("Spell On Hit Behaviours/Continuous",
            $"{ONHITBEHAVIOURSCONTINUOUSPATH}", typeof(SpellOnHitBehaviourAbstractSO));
        tree.AddAllAssetsAtPath("Spell Muzzle Behaviours/Continuous",
            $"{MUZZLEBEHAVIOURSCONTINUOUSPATH}", typeof(SpellMuzzleBehaviourAbstractSO));

        // Damage
        tree.AddAllAssetsAtPath("Spell Damage Behaviours",
            $"{SPELLDAMAGEBEHAVIOURSPATH}", typeof(DamageBehaviourAbstractSO));

        // Create new spells
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

        if (disableProjectileAfterCollisionData != null)
            DestroyImmediate(disableProjectileAfterCollisionData.Spell);

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

        if (spellOnHitDisableWithSpellMaxTimeData != null)
            DestroyImmediate(spellOnHitDisableWithSpellMaxTimeData.Spell);
        
        if (spellMuzzleDisableData != null)
            DestroyImmediate(spellMuzzleDisableData.Spell);

        if (continuousApplyDamageBehaviourData != null)
            DestroyImmediate(continuousApplyDamageBehaviourData.Spell);

        if (continuousSpellManaUpdateData != null)
            DestroyImmediate(continuousSpellManaUpdateData.Spell);

        if (continuousSpawnMuzzlePrefabData != null)
            DestroyImmediate(continuousSpawnMuzzlePrefabData.Spell);

        if (continuousUpdateMuzzleAndDisable != null)
            DestroyImmediate(continuousUpdateMuzzleAndDisable.Spell);

        if (continuousSpawnOnHitPrefabData != null)
            DestroyImmediate(continuousSpawnOnHitPrefabData.Spell);

        if (continuousHitAndDisable != null)
            DestroyImmediate(continuousHitAndDisable.Spell);

        if (spawnAreaHoverEffectWallsFloor != null)
            DestroyImmediate(spawnAreaHoverEffectWallsFloor.Spell);

        if (spawnAreaHoverEffectFloor != null)
            DestroyImmediate(spawnAreaHoverEffectFloor.Spell);
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

    public class CreateSpellForwardBehaviourData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellBehaviourForwardSO Spell { get; private set; }

        public CreateSpellForwardBehaviourData()
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

    public class CreateSpellContinuousBehaviourData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellBehaviourContinuousSO Spell { get; private set; }

        public CreateSpellContinuousBehaviourData()
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

    public class CreateSpellSelfHealOneShotBehaviourData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellBehaviourOneShotSelfHealSO Spell { get; private set; }

        public CreateSpellSelfHealOneShotBehaviourData()
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

    public class CreateSpellApplyDamagePierceBehaviourData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellBehaviourApplyDamagePierceSO Spell { get; private set; }

        public CreateSpellApplyDamagePierceBehaviourData()
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

    public class CreateSpellApplyDamageBehaviourData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellBehaviourApplyDamageSO Spell { get; private set; }

        public CreateSpellApplyDamageBehaviourData()
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

    public class CreateSpellBounceOnHitBehaviourData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellBehaviourBounceOnHitSO Spell { get; private set; }

        public CreateSpellBounceOnHitBehaviourData()
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

    public class CreateSpellCommonBehavioursOneShotData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellBehaviourCommonBehavioursSO Spell { get; private set; }

        public CreateSpellCommonBehavioursOneShotData()
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

    public class CreateSpellDisableProjectileAfterSecondsOneShotData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellBehaviourDisableProjectileAfterSecondsSO Spell { get; private set; }

        public CreateSpellDisableProjectileAfterSecondsOneShotData()
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

    public class CreateSpellDisableProjectileAfterCollisionOneShotData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellBehaviourDisableProjectileIfCollisionSO Spell { get; private set; }

        public CreateSpellDisableProjectileAfterCollisionOneShotData()
        {
            Spell = ScriptableObject.CreateInstance<SpellBehaviourDisableProjectileIfCollisionSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Spell,
                "Assets/Resources/Scriptable Objects/Spell Behaviours/One Shot/New Spell Disable Projectile After Collision Behaviour " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Spell = ScriptableObject.CreateInstance<SpellBehaviourDisableProjectileIfCollisionSO>();
        }
    }

    public class CreateHitSpawnHitPrefabData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellBehaviourSpawnHitPrefabOneShotSO Spell { get; private set; }

        public CreateHitSpawnHitPrefabData()
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

    public class CreateMuzzleSpawnMuzzlePrefabOneShotData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellBehaviourSpawnMuzzlePrefabOneShotSO Spell { get; private set; }

        public CreateMuzzleSpawnMuzzlePrefabOneShotData()
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

    public class CreateSpellStopSpellOnHitOneShotData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellBehaviourStopSpellOnHitSO Spell { get; private set; }

        public CreateSpellStopSpellOnHitOneShotData()
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

    public class CreateSpellUpdateManaAndCooldownOneShotData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellBehaviourManaAndCooldownUpdateSO Spell { get; private set; }

        public CreateSpellUpdateManaAndCooldownOneShotData()
        {
            Spell = ScriptableObject.CreateInstance<SpellBehaviourManaAndCooldownUpdateSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Spell,
                "Assets/Resources/Scriptable Objects/Spell Behaviours/One Shot/New Spell Mana And Cooldown Update Behaviour " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Spell = ScriptableObject.CreateInstance<SpellBehaviourManaAndCooldownUpdateSO>();
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

    public class CreateSpellMuzzleDisableOneShotData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellMuzzleBehaviourOneShotDisableSO Spell { get; private set; }

        public CreateSpellMuzzleDisableOneShotData()
        {
            Spell = ScriptableObject.CreateInstance<SpellMuzzleBehaviourOneShotDisableSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Spell,
                "Assets/Resources/Scriptable Objects/Spell Muzzle Behaviours/One Shot/New Spell Muzzle Behaviour Disable " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Spell = ScriptableObject.CreateInstance<SpellMuzzleBehaviourOneShotDisableSO>();
        }
    }

    public class CreateSpellOnHitDisableOneShotData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellOnHitBehaviourOneShotDisableSO Spell { get; private set; }

        public CreateSpellOnHitDisableOneShotData()
        {
            Spell = ScriptableObject.CreateInstance<SpellOnHitBehaviourOneShotDisableSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Spell,
                "Assets/Resources/Scriptable Objects/Spell Hit Behaviours/One Shot/New Spell Hit Behaviour Disable " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Spell = ScriptableObject.CreateInstance<SpellOnHitBehaviourOneShotDisableSO>();
        }
    }

    public class CreateSpellOnHitDisableSpellMaxTimeOneShotData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellOnHitBehaviourOneShotDisableWithSpellMaxTimeSO Spell { get; private set; }

        public CreateSpellOnHitDisableSpellMaxTimeOneShotData()
        {
            Spell = ScriptableObject.CreateInstance<SpellOnHitBehaviourOneShotDisableWithSpellMaxTimeSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Spell,
                "Assets/Resources/Scriptable Objects/Spell Hit Behaviours/One Shot/New Spell Hit Behaviour Disable With Spell Max Time " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Spell = ScriptableObject.CreateInstance<SpellOnHitBehaviourOneShotDisableWithSpellMaxTimeSO>();
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

    public class CreateSpellContinuousApplyDamageData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellBehaviourContinuousApplyDamageSO Spell { get; private set; }

        public CreateSpellContinuousApplyDamageData()
        {
            Spell = ScriptableObject.CreateInstance<SpellBehaviourContinuousApplyDamageSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Spell, 
                $"Assets/Resources/Scriptable Objects/Spell Behaviours/Continuous/New Behaviour Continuous Apply Damage " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Spell = ScriptableObject.CreateInstance<SpellBehaviourContinuousApplyDamageSO>();
        }
    }

    public class CreateSpellContinuousManaUpdateData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellBehaviourContinuousManaUpdateSO Spell { get; private set; }

        public CreateSpellContinuousManaUpdateData()
        {
            Spell = ScriptableObject.CreateInstance<SpellBehaviourContinuousManaUpdateSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Spell,
                $"Assets/Resources/Scriptable Objects/Spell Behaviours/Continuous/New Behaviour Continuous Mana Update " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Spell = ScriptableObject.CreateInstance<SpellBehaviourContinuousManaUpdateSO>();
        }
    }

    public class CreateSpawnMuzzlePrefabContinuousData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellBehaviourSpawnMuzzlePrefabContinuousSO Spell { get; private set; }

        public CreateSpawnMuzzlePrefabContinuousData()
        {
            Spell = ScriptableObject.CreateInstance<SpellBehaviourSpawnMuzzlePrefabContinuousSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Spell,
                $"Assets/Resources/Scriptable Objects/Spell Behaviours/Continuous/New Behaviour Spawn Muzzle Prefab " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Spell = ScriptableObject.CreateInstance<SpellBehaviourSpawnMuzzlePrefabContinuousSO>();
        }
    }

    public class CreateSpellContinuousUpdatePositionAndDisableData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellMuzzleBehaviourContinuousPositionDisableSO Spell { get; private set; }

        public CreateSpellContinuousUpdatePositionAndDisableData()
        {
            Spell = ScriptableObject.CreateInstance<SpellMuzzleBehaviourContinuousPositionDisableSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Spell,
                "Assets/Resources/Scriptable Objects/Spell Hit Behaviours/Continuous/New Spell Muzzle Update Position and Disable " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Spell = ScriptableObject.CreateInstance<SpellMuzzleBehaviourContinuousPositionDisableSO>();
        }
    }

    public class CreateSpellBehaviourSpawnOnHitPrefabContinuousData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellBehaviourSpawnOnHitPrefabContinuousSO Spell { get; private set; }

        public CreateSpellBehaviourSpawnOnHitPrefabContinuousData()
        {
            Spell = ScriptableObject.CreateInstance<SpellBehaviourSpawnOnHitPrefabContinuousSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Spell,
                "Assets/Resources/Scriptable Objects/Spell Behaviours/Continuous/New Spell Spawn On Hit Prefab " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Spell = ScriptableObject.CreateInstance<SpellBehaviourSpawnOnHitPrefabContinuousSO>();
        }
    }

    public class CreateContinuousSpellOnHitBehaviourDisableData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellOnHitBehaviourContinuousDisableSO Spell { get; private set; }

        public CreateContinuousSpellOnHitBehaviourDisableData()
        {
            Spell = ScriptableObject.CreateInstance<SpellOnHitBehaviourContinuousDisableSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Spell,
                "Assets/Resources/Scriptable Objects/Spell Hit Behaviours/Continuous/New Spell On Hit Disable " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Spell = ScriptableObject.CreateInstance<SpellOnHitBehaviourContinuousDisableSO>();
        }
    }

    public class CreateSpellHoverAreaVFXWallsFloorData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellBehaviourSpawnAreaHoverEffectWallsAndFloorSO Spell { get; private set; }

        public CreateSpellHoverAreaVFXWallsFloorData()
        {
            Spell = ScriptableObject.CreateInstance<SpellBehaviourSpawnAreaHoverEffectWallsAndFloorSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Spell,
                "Assets/Resources/Scriptable Objects/Spell Behaviours/One Shot/New Spell Area Hover VFX Walls And Floor " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Spell = ScriptableObject.CreateInstance<SpellBehaviourSpawnAreaHoverEffectWallsAndFloorSO>();
        }
    }

    public class CreateSpellHoverAreaVFXFloorData
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SpellBehaviourSpawnAreaHoverEffectOnFloorSO Spell { get; private set; }

        public CreateSpellHoverAreaVFXFloorData()
        {
            Spell = ScriptableObject.CreateInstance<SpellBehaviourSpawnAreaHoverEffectOnFloorSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Spell,
                "Assets/Resources/Scriptable Objects/Spell Behaviours/One Shot/New Spell Area Hover VFX On Floor " +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Spell = ScriptableObject.CreateInstance<SpellBehaviourSpawnAreaHoverEffectOnFloorSO>();
        }
    }
}
