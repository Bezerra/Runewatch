using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OPENDOORSONROOMAITESTS : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1);

        foreach (ContactPoint cp in FindObjectsOfType<ContactPoint>())
        {
            cp.gameObject.SetActive(false);
        }
    }
}
