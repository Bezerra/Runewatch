using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ExtensionMethods;

public class RoomOcclusionController : MonoBehaviour
{
    public IList<RoomOcclusion> RoomsOcclusion;
    private LevelGenerator levelGenerator;
    private GameObject startRoom;

    private void Awake()
    {
        RoomsOcclusion = new List<RoomOcclusion>();
        CurrentLevelPieceCollision = gameObject;
        levelGenerator = GetComponent<LevelGenerator>();
    }

    private void OnEnable()
    {
        levelGenerator.EndedGeneration += EndedGeneration;
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
        levelGenerator.EndedGeneration -= EndedGeneration;
    }

    private void EndedGeneration()
    {
        startRoom = GameObject.FindGameObjectWithTag("StartRoom");
        RoomOcclusion startRoomOcclusion = startRoom.GetComponentInChildren<RoomOcclusion>();
        StartCoroutine(startRoomOcclusion.ControlChildOccludeesCoroutine());
    }

    private void OnOcclusionCompleted()
    {
        startRoom.TryGetComponent(out PlayerSpawnLevelPiece playerSpawnLevelPiece);
        FindObjectOfType<SceneControl>().SpawnPlayerTEST(playerSpawnLevelPiece.PlayerSpawnTransform);
    }

    public GameObject CurrentLevelPieceCollision { get; set; }
    public RoomOcclusion CurrentRoomOcclusion { get; set; }



    protected virtual void OnAllCoroutinesStopped() => AllCoroutinesStopped?.Invoke();
    public event Action AllCoroutinesStopped;
}
