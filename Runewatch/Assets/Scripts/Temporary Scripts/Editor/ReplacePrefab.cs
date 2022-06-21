using UnityEngine;
using UnityEditor;
using System.Collections;

public class ReplaceGameObjects : ScriptableWizard
{
    public bool copyValues = true;
    public GameObject newObject;
    public GameObject toReplace;
    

    [MenuItem("Custom/Replace GameObjects")]


    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard("Replace GameObjects", typeof(ReplaceGameObjects), "Replace");
    }

    void OnWizardCreate()
    {
        GameObject newParent = new GameObject(toReplace.name);
        newParent.transform.SetParent(toReplace.transform.parent);
        newParent.transform.position = toReplace.transform.position;
        newParent.transform.rotation = toReplace.transform.rotation;

        foreach (Transform t in toReplace.transform)
        {
            GameObject newObject;
            newObject = (GameObject)PrefabUtility.InstantiatePrefab(this.newObject);
            newObject.transform.SetParent(newParent.transform);
            newObject.transform.position = t.position;
            newObject.transform.rotation = t.rotation;
        }

        DestroyImmediate(toReplace);

    }
}