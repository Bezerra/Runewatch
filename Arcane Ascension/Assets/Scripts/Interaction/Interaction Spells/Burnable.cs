using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for burnable objects.
/// </summary>
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider))]
public class Burnable : MonoBehaviour, IBurnable
{
    private Animator anim;

    private void Awake() =>
        anim = GetComponent<Animator>();

    public void Burn() =>
        anim.SetTrigger("Burn");

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out SpellBehaviourAbstract spell))
        {
            if (spell.Spell.Element == ElementType.Ignis)
            {
                Burn();
            }
        }
    }
}
