using System;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;

/// <summary>
/// Class responsible for creating audio assets editor window.
/// </summary>
public class AudioAssetsManagerEditor : OdinMenuEditorWindow
{
    [MenuItem("CustomEditor/Audio Assets Editor")]
    private static void OpenWindow()
    {
        GetWindow<AudioAssetsManagerEditor>().Show();
    }

    protected override void OnGUI()
    {
        SirenixEditorGUI.Title("Audio Assets Manager", "", TextAlignment.Center, true, true);
        EditorGUILayout.Space(20);
        base.OnGUI();
    }

    private CreateAudioAsset createNewAudioAsset;

    protected override OdinMenuTree BuildMenuTree()
    {
        OdinMenuTree tree = new OdinMenuTree();

        createNewAudioAsset = new CreateAudioAsset();

        tree.AddAllAssetsAtPath("Loot Audio", "Assets/Resources/Scriptable Objects/Audio", typeof(LootSoundsSO), true);

        tree.Add("Create New Audio Asset", createNewAudioAsset);

        tree.AddAllAssetsAtPath("Audio Asset", "Assets/Resources/Scriptable Objects/Audio", typeof(AbstractSoundSO), true);

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
        if (createNewAudioAsset != null)
            DestroyImmediate(createNewAudioAsset.SoundAsset);
    }

    private class CreateAudioAsset
    {
        public AbstractSoundSO SoundAsset { get; private set; }

        [DetailedInfoBox("After creating a sound, drag it to its type folder \n \n \n Plays one sound with pre-defined values", "")]
        [Button("Create Audio Asset - Simple", ButtonSizes.Large)]
        private void CreateSimpleSound()
        {
            SoundAsset = ScriptableObject.CreateInstance<SimpleSoundSO>();
            AssetDatabase.CreateAsset(
                SoundAsset, "Assets/Resources/Scriptable Objects/Audio/" +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            ScriptableObject.CreateInstance<SimpleSoundSO>();
        }

        [DetailedInfoBox("Plays one sound with random values", "")]
        [Button("Create Audio Asset - Simple With Random Values", ButtonSizes.Large)]
        private void CreateSimpleSoundRandomValues()
        {
            SoundAsset = ScriptableObject.CreateInstance<SimpleSoundRandomValuesSO>();
            AssetDatabase.CreateAsset(
                SoundAsset, "Assets/Resources/Scriptable Objects/Audio/" +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            ScriptableObject.CreateInstance<SimpleSoundRandomValuesSO>();
        }

        [DetailedInfoBox("Plays random sound with random values", "")]
        [Button("Create Audio Asset - Random With Random Values", ButtonSizes.Large)]
        private void CreateRandomSoundRandomValues()
        {
            SoundAsset = ScriptableObject.CreateInstance<RandomSoundRandomValuesSO>();
            AssetDatabase.CreateAsset(
                SoundAsset, "Assets/Resources/Scriptable Objects/Audio/" +
                DateTime.Now.Millisecond.ToString() + ".asset");
            AssetDatabase.SaveAssets();

            ScriptableObject.CreateInstance<RandomSoundRandomValuesSO>();
        }
    }
}

