using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for containing information about every room occlusion.
/// </summary>
public class RoomOcclusionController : MonoBehaviour
{
    public IList<RoomOcclusion> RoomsOcclusion;
    private LevelGenerator levelGenerator;
    private GameObject startRoom;
    private bool isPlayerSpawned;

    private void Awake()
    {
        RoomsOcclusion = new List<RoomOcclusion>();
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
        LevelPiece startRoomLevelPiece = startRoom.GetComponent<LevelPiece>();

        CurrentLevelPieceCollision = startRoomLevelPiece;
        StartCoroutine(startRoomOcclusion.ControlChildOccludeesCoroutine());
    }

    private void OnOcclusionCompleted()
    {
        startRoom.TryGetComponent(out PlayerSpawnLevelPiece playerSpawnLevelPiece);

        if (isPlayerSpawned == false)
        {
            isPlayerSpawned = true;
            FindObjectOfType<SceneControl>().SpawnPlayerTEST(playerSpawnLevelPiece.PlayerSpawnTransform);
        }
    }

    /// <summary>
    /// Current level piece the player is in.
    /// </summary>
    public LevelPiece CurrentLevelPieceCollision { get; set; }

    /// <summary>
    /// Current room occlusion the player is in.
    /// </summary>
    public RoomOcclusion CurrentRoomOcclusion { get; set; }

    protected virtual void OnAllCoroutinesStopped() => AllCoroutinesStopped?.Invoke();
    public event Action AllCoroutinesStopped;
}
