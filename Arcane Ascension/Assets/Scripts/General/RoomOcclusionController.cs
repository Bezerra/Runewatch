using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for containing information about every room occlusion.
/// </summary>
public class RoomOcclusionController : MonoBehaviour
{
    // Components
    private LevelGenerator levelGenerator;
    private GameObject startRoom;

    // Rooms Occuslion
    public IList<RoomOcclusion> RoomsOcclusion;
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

    /// <summary>
    /// Adds a room occlusion to a list and subscribes this class to OcclusionCompleted.
    /// </summary>
    /// <param name="roomOcclusion">Room occlusion to add.</param>
    public void AddRoomOcclusion(RoomOcclusion roomOcclusion)
    {
        RoomsOcclusion.Add(roomOcclusion);
        roomOcclusion.FirstOcclusionCompleted += FirstOnOcclusionCompleted; 
    }

    private void OnDisable()
    {
        for (int i = 0; i < RoomsOcclusion.Count; i++)
        {
            if (RoomsOcclusion[i] != null)
                RoomsOcclusion[i].FirstOcclusionCompleted -= FirstOnOcclusionCompleted;
        }
        levelGenerator.EndedGeneration -= EndedGeneration;
    }

    /// <summary>
    /// Method called after the generation is over.
    /// Starts occlusion from start piece.
    /// </summary>
    private void EndedGeneration()
    {
        startRoom = GameObject.FindGameObjectWithTag("StartRoom");
        RoomOcclusion startRoomOcclusion = startRoom.GetComponentInChildren<RoomOcclusion>();
        LevelPiece startRoomLevelPiece = startRoom.GetComponent<LevelPiece>();

        CurrentLevelPieceCollision = startRoomLevelPiece;

        // Calls first occlusion of the game
        StartCoroutine(startRoomOcclusion.ControlChildOccludeesCoroutine(true));
    }

    /// <summary>
    /// Method called everytime an occlusion is finished.
    /// </summary>
    private void FirstOnOcclusionCompleted()
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
}
