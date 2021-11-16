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



    public IList<byte> Passives { get; set; }

    private void Awake()
    {
        //IList<byte> temp = new List<byte> { 0, 83, };
        //string passivesTemp = JsonUtility.ToJson(temp);
        //PlayerPrefs.SetString("SkillTreePassives", passivesTemp);
        //Debug.Log(PlayerPrefs.GetString("SkillTreePassives"));
        //
        //string ok = PlayerPrefs.GetString("SkillTreePlassives");
        //Debug.Log(ok);

        //Passives = new List<byte> { (byte)PlayerPrefs.GetString("SkillTreePlassives")};
        //Passives = new List<byte> { 0, 1, 10, 83, 84, 85, 86, 87 };

        Passives = new List<byte> { 0 };
    }
}
