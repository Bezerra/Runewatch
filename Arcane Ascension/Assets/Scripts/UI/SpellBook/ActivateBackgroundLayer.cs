using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ActivateBackgroundLayer : MonoBehaviour
{

    [SerializeField] SkillTreePassiveNode[] skillsNeeded;

    Image image;
    bool active = false;

    Color colorFull = Color.white;
    Color colorFade = new Color(1f, 1f, 1f, 0.1f);
    Color oldColor;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        bool ShouldBeActive = true;
        foreach (SkillTreePassiveNode stpn in skillsNeeded)
        {
            if (!stpn.IsUnlocked)
                ShouldBeActive = false;
        }

        //image.enabled = isActive;

        if (!active && ShouldBeActive)
        {
            active = true;
            DoFadeIn();
        }

        else if(active && !ShouldBeActive)
        {
            active = false;
            image.color = colorFade;
        }

    }

    void DoFadeIn()
    {
        image.DOColor(colorFull, 1);
        
    }
}
