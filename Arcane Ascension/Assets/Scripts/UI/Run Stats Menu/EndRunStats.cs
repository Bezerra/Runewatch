using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRunStats : MonoBehaviour
{
    private void Start()
    {
        // Update stats













        // Deletes run progress file
        RunSaveDataController runSaveDataController =
            FindObjectOfType<RunSaveDataController>();
        runSaveDataController.DeleteFile();
    }
}
