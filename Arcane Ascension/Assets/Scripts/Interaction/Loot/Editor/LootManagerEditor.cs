using System;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;

/// <summary>
/// Class responsible for creating loot manager editor window.
/// </summary>
public class LootManagerEditor : OdinMenuEditorWindow
{
    [MenuItem("CustomEditor/Loot Manager Editor")]
    private static void OpenWindow()
    {
        GetWindow<LootManagerEditor>().Show();
    }

    protected override void OnGUI()
    {
        SirenixEditorGUI.Title("Loot Manager", "", TextAlignment.Center, true, true);
        EditorGUILayout.Space(20);
        base.OnGUI();
    }

    private CreateNewPotion createNewPotion;
    private CreateNewCurrency createNewCurrency;

    protected override OdinMenuTree BuildMenuTree()
    {
        OdinMenuTree tree = new OdinMenuTree();

        createNewPotion = new CreateNewPotion();
        createNewCurrency = new CreateNewCurrency();

        tree.Add("Create New Potion", createNewPotion);
        tree.Add("Create New Currency", createNewCurrency);

        tree.AddAllAssetsAtPath("Loot", "Assets/Resources/Scriptable Objects/Loot", typeof(CurrencySO), true);
        tree.AddAllAssetsAtPath("Loot", "Assets/Resources/Scriptable Objects/Loot", typeof(PotionSO), true);

        return tree;
    }

    protected override void OnBeginDrawEditors()
    {
        OdinMenuTreeSelection selectedAsset = this.MenuTree?.Selection;

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
        if (createNewCurrency != null)
            DestroyImmediate(createNewCurrency.Loot);

        if (createNewPotion != null)
            DestroyImmediate(createNewPotion.Loot);
    }

    private class CreateNewPotion
    {
        public PotionSO Loot { get; private set; }

        [Button("Create New Potion", ButtonSizes.Large)]
        private void CreateSimpleSound()
        {
            Loot = ScriptableObject.CreateInstance<PotionSO>();
            AssetDatabase.CreateAsset(
                Loot, "Assets/Resources/Scriptable Objects/Loot/" +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            ScriptableObject.CreateInstance<PotionSO>();
        }
    }

    private class CreateNewCurrency
    {
        public CurrencySO Loot { get; private set; }

        [Button("Create New Currency", ButtonSizes.Large)]
        private void CreateSimpleSound()
        {
            Loot = ScriptableObject.CreateInstance<CurrencySO>();
            AssetDatabase.CreateAsset(
                Loot, "Assets/Resources/Scriptable Objects/Loot/" +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            ScriptableObject.CreateInstance<CurrencySO>();
        }
    }
}
