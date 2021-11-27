using System;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;

/// <summary>
/// Class responsible for creating room weights editor window.
/// </summary>
public class RoomWeightsEditor : OdinMenuEditorWindow
{
    [MenuItem("CustomEditor/Room Weights Editor")]
    private static void OpenWindow()
    {
        GetWindow<RoomWeightsEditor>().Show();
    }

    protected override void OnGUI()
    {
        SirenixEditorGUI.Title("Room Weights Manager", "", TextAlignment.Center, true, true);
        EditorGUILayout.Space(20);
        base.OnGUI();
    }

    private CreateRoomWeights createNewRoomWeight;

    protected override OdinMenuTree BuildMenuTree()
    {
        OdinMenuTree tree = new OdinMenuTree();

        createNewRoomWeight = new CreateRoomWeights();

        tree.Add("Create New Room Weight/New Room Weight", createNewRoomWeight);

        tree.AddAllAssetsAtPath("Room Weights",
            "Assets/Resources/Scriptable Objects/Create Once _ General/Room Weights", 
            typeof(RoomWeightsSO));

        return tree;
    }

    protected override void OnBeginDrawEditors()
    {
        OdinMenuTreeSelection selected = this.MenuTree.Selection;

        SirenixEditorGUI.BeginHorizontalToolbar();
        {
            GUILayout.FlexibleSpace();
            if (SirenixEditorGUI.ToolbarButton("Delete current room weight"))
            {
                ScriptableObject selectedObject = selected.SelectedValue as ScriptableObject;
                string path = AssetDatabase.GetAssetPath(selectedObject);
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.SaveAssets();
            }
        }
        SirenixEditorGUI.EndHorizontalToolbar();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (createNewRoomWeight != null)
            DestroyImmediate(createNewRoomWeight.Room);
    }

    public class CreateRoomWeights
    {
        [ShowInInspector]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public RoomWeightsSO Room { get; private set; }

        public CreateRoomWeights()
        {
            Room = ScriptableObject.CreateInstance<RoomWeightsSO>();
        }

        [Button("Create", ButtonSizes.Large)]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(Room,
                "Assets/Resources/Scriptable Objects/Create Once _ General/Room Weights/New Room Weight" +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            Room = ScriptableObject.CreateInstance<RoomWeightsSO>();
        }
    }
}
