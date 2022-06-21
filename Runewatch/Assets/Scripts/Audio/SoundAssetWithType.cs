using UnityEngine;
using System;

/// <summary>
/// Serializable struct with a sound and surface type.
/// </summary>
[Serializable]
public struct SoundAssetWithType
{
    [SerializeField] private AbstractSoundSO sound;
    [SerializeField] private SurfaceType surfaceType;

    public AbstractSoundSO SurfaceSound => sound;
    public SurfaceType SurfaceType => surfaceType;
}
