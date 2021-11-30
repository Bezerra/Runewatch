using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ExtensionMethods;

public class RoomOcclusionController : MonoBehaviour
{
    public IList<RoomOcclusion> RoomsOcclusion;

    private IEnumerator stopAllCoroutinesCoroutine;

    private void Awake()
    {
        RoomsOcclusion = new List<RoomOcclusion>();
    }

    public void AddRoomOcclusion(RoomOcclusion roomOcclusion)
    {
        RoomsOcclusion.Add(roomOcclusion);
        roomOcclusion.OcclusionCompleted += OnOcclusionCompleted; 
    }

    private void OnDisable()
    {
        for (int i = 0; i < RoomsOcclusion.Count; i++)
        {
            if (RoomsOcclusion[i] != null)
                RoomsOcclusion[i].OcclusionCompleted -= OnOcclusionCompleted;
        }
    }

    private void OnOcclusionCompleted()
    {

    }

    public GameObject CurrentLevelPieceCollision { get; set; }


    protected virtual void OnAllCoroutinesStopped() => AllCoroutinesStopped?.Invoke();
    public event Action AllCoroutinesStopped;
}
