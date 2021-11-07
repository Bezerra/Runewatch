using System;
using UnityEngine;

/// <summary>
/// Serializable struct for spell sounds.
/// </summary>
[Serializable]
public struct SpellSound
{
    [SerializeField] private AbstractSoundSO projectile;
    [SerializeField] private AbstractSoundSO muzzle;
    [SerializeField] private AbstractSoundSO hit;

    public AbstractSoundSO Projectile => projectile;
    public AbstractSoundSO Muzzle => muzzle;
    public AbstractSoundSO Hit => hit;
}
