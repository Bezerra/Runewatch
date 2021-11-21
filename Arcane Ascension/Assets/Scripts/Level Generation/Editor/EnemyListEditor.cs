using System;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;

/// <summary>
/// Class responsible for creating room weights editor window.
/// </summary>
public class EnemyListEditor : OdinMenuEditorWindow
{
    [MenuItem("CustomEditor/Enemy List Editor")]
    private static void OpenWindow()
    {
        GetWindow<EnemyListEditor>().Show();
    }

    protected override void OnGUI()
    {
        SirenixEditorGUI.Title("Enemy List Manager", "", TextAlignment.Center, true, true);
        EditorGUILayout.Space(20);
        base.OnGUI();
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        OdinMenuTree tree = new OdinMenuTree();

        tree.AddAllAssetsAtPath("Enemy List",
            "Assets/Resources/Scriptable Objects/Create Once _ General", 
            typeof(AvailableListOfEnemiesToSpawnSO), true);

        return tree;
    }
}
