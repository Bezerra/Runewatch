using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour, IInterectable
{
    [SerializeField] private bool isBossRaid;
    

    public void Execute()
    {
        Debug.Log("fewqgeqw");
        LoadingScreenWithTrigger[] loadingScreen = FindObjectsOfType<LoadingScreenWithTrigger>();

        if (isBossRaid)
        {
            foreach (LoadingScreenWithTrigger load in loadingScreen)
            {
                if (load.FloorControl == FloorSceneControl.BossRaid)
                {
                    load.LoadSceneOnSerializeField();
                    break;
                }
            }
            return;
        }

        RunSaveData saveData = FindObjectOfType<RunSaveDataController>().SaveData;

        // If not last floor
        // loads next floor
        if (saveData.DungeonSavedData.Floor < 9)
        {
            foreach (LoadingScreenWithTrigger load in loadingScreen)
            {
                if (load.FloorControl == FloorSceneControl.NextFloor)
                {
                    load.LoadSceneOnSerializeField();
                    break;
                }
            }
        }
        else // Else it will load the final scene
        {
            foreach (LoadingScreenWithTrigger load in loadingScreen)
            {
                if (load.FloorControl == FloorSceneControl.FinalFloor)
                {
                    load.LoadSceneOnSerializeField();
                    break;
                }
            }
        }

        Destroy(this); // destroys script
    }
}
