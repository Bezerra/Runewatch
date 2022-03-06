using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ExtensionMethods;

/// <summary>
/// Class responsible for enabling and disabling gameobjects depending on which
/// room the player triggered with.
/// </summary>
public class RoomOcclusion : MonoBehaviour, IFindPlayer
{
    // Components
    private LevelGenerator levelGenerator;
    private RoomOcclusionController roomOcclusionController;
    private LevelPiece thisLevelPiece;

    // Music control
    private LoopGameplayMusic gameplayMusic;
    private ShopkeeperMusic shopkeeperMusic;

    private Player player;

    // Coroutines
    private IEnumerator controlChildOccludeesCoroutine;

    private void Awake()
    {
        thisLevelPiece = GetComponentInParent<LevelPiece>();
        gameplayMusic = FindObjectOfType<LoopGameplayMusic>();
        shopkeeperMusic = FindObjectOfType<ShopkeeperMusic>();
        StartCoroutine(FindLevelGenerator());
    }

    private IEnumerator FindLevelGenerator()
    {
        YieldInstruction wfs = new WaitForSeconds(0.1f);
        while (levelGenerator == null)
        {
            levelGenerator = FindObjectOfType<LevelGenerator>();
            roomOcclusionController = FindObjectOfType<RoomOcclusionController>();
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

                // Resets if coroutine is already running
                this.StartCoroutineWithReset(ref controlChildOccludeesCoroutine,
                    ControlChildOccludeesCoroutine());

                roomOcclusionController.CurrentLevelPieceCollision = thisLevelPiece;
                roomOcclusionController.CurrentRoomOcclusion = this;
            }

            // If the player entered a room without a shopkeeper, the audio
            // will fade back to normal music (this happens if the player WAS in a room
            // with a shopkeeper and shopkeeper music was still playing)
            if (shopkeeperMusic.CurrentVolume > 0.05f)
            {
                gameplayMusic.FadeInVolume();
                shopkeeperMusic.FadeOutVolume();
            }
        }
    }

    /// <summary>
    /// Coroutine that enables all surrounding level pieces and disables the rest.
    /// </summary>
    /// <returns>Null.</returns>
    public IEnumerator ControlChildOccludeesCoroutine()
    {
        // Creates a new list with all level pieces
        IList<LevelPiece> generatedRoomPieces = new List<LevelPiece>();
        for (int i = 0; i < levelGenerator.AllLevelPiecesGenerated.Count; i++)
        {
            generatedRoomPieces.Add(levelGenerator.AllLevelPiecesGenerated[i]);
            yield return null;
        }
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
            {
                StartCoroutine(generatedRoomPieces[i].DisableChildOccludeesCoroutine());
            }
            yield return null;
        }

        OnOcclusionCompleted(thisLevelPiece);
    }

    /// <summary>
    /// After each occlusion is completed.
    /// Registered on doors.
    /// </summary>
    /// <param name="currentPiece"></param>
    protected virtual void OnOcclusionCompleted(LevelPiece currentPiece) => 
        OcclusionCompleted?.Invoke(currentPiece);
    public event Action<LevelPiece> OcclusionCompleted;

    public void FindPlayer(Player player)
    {
        this.player = player;
    }

    public void PlayerLost(Player player)
    {
        // Left blank on purpose
    }
}
