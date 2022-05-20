using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateBackgroundLayer : MonoBehaviour
{

    [SerializeField] SkillTreePassiveNode[] skillsNeeded;

    Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isActive = true;
        foreach (SkillTreePassiveNode stpn in skillsNeeded)
        {
            if (!stpn.IsUnlocked)
                isActive = false;
        }

        image.enabled = isActive;
    }
}
