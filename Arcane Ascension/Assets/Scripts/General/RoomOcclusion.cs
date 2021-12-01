using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ExtensionMethods;

/// <summary>
/// Class responsible for enabling and disabling gameobjects depending on which
/// room the player triggered with.
/// </summary>
public class RoomOcclusion : MonoBehaviour
{
    // Components
    private LevelGenerator levelGenerator;
    private RoomOcclusionController roomOcclusionController;
    private LevelPiece thisLevelPiece;

    // Coroutines
    private IEnumerator controlChildOccludeesCoroutine;

    private void Awake()
    {
        thisLevelPiece = GetComponentInParent<LevelPiece>();
        StartCoroutine(FindLevelGenerator());
    }

    private IEnumerator FindLevelGenerator()
    {
        YieldInstruction wfs = new WaitForSeconds(0.2f);
        while (levelGenerator == null)
        {
            levelGenerator = FindObjectOfType<LevelGenerator>();
            roomOcclusionController = FindObjectOfType<RoomOcclusionController>();
            roomOcclusionController.AddRoomOcclusion(this);
            yield return wfs;
        }
    }

    /// <summary>
    /// Happens every time the player enters a new level piece.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            if (roomOcclusionController.CurrentLevelPieceCollision != thisLevelPiece)
            {
                if (roomOcclusionController.CurrentRoomOcclusion != null)
                {
                    roomOcclusionController.CurrentRoomOcclusion.StopAllCoroutines();
                }

                // Closes all passages
                StartCoroutine(thisLevelPiece.ClosePassagesCoroutine());

                // Resets if coroutine is already running
                this.StartCoroutineWithReset(ref controlChildOccludeesCoroutine,
                    ControlChildOccludeesCoroutine());

                roomOcclusionController.CurrentLevelPieceCollision = thisLevelPiece;
                roomOcclusionController.CurrentRoomOcclusion = this;
            }
        }
    }

    /// <summary>
    /// Coroutine that enables all surrounding level pieces and disables the rest.
    /// </summary>
    /// <param name="onGameBeggining">Is this the first occlusion in the game.</param>
    /// <returns>Null.</returns>
    public IEnumerator ControlChildOccludeesCoroutine(bool onGameBeggining = false)
    {
        // Creates a new lsit with all level pieces
        IList<LevelPiece> generatedRoomPieces = new List<LevelPiece>();
        for (int i = 0; i < levelGenerator.AllGeneratedLevelPieces.Count; i++)
        {
            generatedRoomPieces.Add(levelGenerator.AllGeneratedLevelPieces[i]);
            yield return null;
        }
        
        // Enables pieces of this current piece
        StartCoroutine(thisLevelPiece.EnableChildOccludeesCoroutine());
        generatedRoomPieces.Remove(thisLevelPiece);
            
        // Enables pieces for connect pieces to this piece
        for (int i = 0; i < thisLevelPiece.ConnectedPieces.Count; i++)
        {
            if (thisLevelPiece.ConnectedPieces[i] != null)
            {
                StartCoroutine(thisLevelPiece.ConnectedPieces[i].EnableChildOccludeesCoroutine());
                generatedRoomPieces.Remove(thisLevelPiece.ConnectedPieces[i]);
            }
            yield return null;
        }

        // Disables the rest of the pieces
        for (int i = 0; i < generatedRoomPieces.Count; i++)
        {
            if (generatedRoomPieces[i] != null)
                StartCoroutine(generatedRoomPieces[i].DisableChildOccludeesCoroutine());
            yield return null;
        }

        OnOcclusionCompleted(thisLevelPiece);

        if (onGameBeggining)
            OnFirstOcclusionCompleted();
    }

    protected virtual void OnOcclusionCompleted(LevelPiece currentPiece) => 
        OcclusionCompleted?.Invoke(currentPiece);
    public event Action<LevelPiece> OcclusionCompleted;

    protected virtual void OnFirstOcclusionCompleted() =>
        FirstOcclusionCompleted?.Invoke();
    public event Action FirstOcclusionCompleted;
}
