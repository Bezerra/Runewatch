using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;

/// <summary>
/// Class responsible for create miniman manager editor.
/// </summary>
public class MinimapEditor : OdinMenuEditorWindow
{
    [MenuItem("CustomEditor/Minimap Editor")]
    private static void OpenWindow()
    {
        GetWindow<MinimapEditor>().Show();
    }

    protected override void OnGUI()
    {
        SirenixEditorGUI.Title("Minimap Manager", "", TextAlignment.Center, true, true);
        EditorGUILayout.Space(20);
        base.OnGUI();
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        OdinMenuTree tree = new OdinMenuTree();


        tree.AddAllAssetsAtPath("Minimap Data", $"Assets/Resources/Scriptable Objects/Create Once _ General",
            typeof(MinimapIconsSO));

        return tree;
    }
}
