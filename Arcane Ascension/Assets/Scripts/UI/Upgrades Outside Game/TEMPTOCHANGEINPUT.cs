using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMPTOCHANGEINPUT : MonoBehaviour
{
    [SerializeField] private bool switchToInput;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1);

        if (switchToInput)
            FindObjectOfType<PlayerInputCustom>().SwitchActionMapToUI();
    }


}
