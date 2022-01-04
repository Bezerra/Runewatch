using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OPENDOORSONROOMAITESTS : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1);

        foreach (Door door in FindObjectsOfType<Door>())
        {
            door.IsDoorRoomFullyLoaded = true;
            door.CanOpen = true;
            door.Open();
        }
    }
}
