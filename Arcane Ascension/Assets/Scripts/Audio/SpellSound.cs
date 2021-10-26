using System;
using UnityEngine;

/// <summary>
/// Serializable struct for spell sounds.
/// </summary>
[Serializable]
public struct SpellSound
{
    [SerializeField] private AbstractSoundScriptableObject projectile;
    [SerializeField] private AbstractSoundScriptableObject muzzle;
    [SerializeField] private AbstractSoundScriptableObject hit;

    public AbstractSoundScriptableObject Projectile => projectile;
    public AbstractSoundScriptableObject Muzzle => muzzle;
    public AbstractSoundScriptableObject Hit => hit;
}
