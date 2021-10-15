using System;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;

/// <summary>
/// Class responsible for creating a elements damage manager editor window.
/// </summary>
public class ElementsDamageManagerEditor : OdinMenuEditorWindow
{
    [MenuItem("CustomEditor/Elements Damage Manager Editor")]
    private static void OpenWindow()
    {
        GetWindow<ElementsDamageManagerEditor>().Show();
    }

    protected override void OnGUI()
    {
        SirenixEditorGUI.Title("Elements Damage Editor", "", TextAlignment.Center, true, true);
        EditorGUILayout.Space(20);
        base.OnGUI();
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        OdinMenuTree tree = new OdinMenuTree();

        tree.AddAllAssetsAtPath(
            "Elements Damage", "Assets/Resources/Scriptable Objects/Elements Damage",
            typeof(ElementsDamageSO));

        return tree;
    }
}
