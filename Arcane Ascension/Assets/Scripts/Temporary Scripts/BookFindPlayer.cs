using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class BookFindPlayer : MonoBehaviour
{

    private GameObject player;
    private LookAtConstraint lac;
    private ConstraintSource cs;

    // Start is called before the first frame update
    void OnEnable()
    {
        player = GameObject.Find("Main Camera");
        lac = GetComponent<LookAtConstraint>();

        if (player != null)
        {
            cs.sourceTransform = player.transform;
            cs.weight = 1;
            lac.AddSource(cs);
            lac.constraintActive = true;
        } 
    }
}
