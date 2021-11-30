using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
using ExtensionMethods;
using System.Linq;

public class RoomOcclusion : MonoBehaviour
{
    // Components
    private LevelGenerator levelGenerator;
    private RoomOcclusionController roomOcclusionController;
    private LevelPiece thisLevelPiece;

    private static IEnumerator controlChildOccludeesCoroutine;

    private bool canStartOcclusion;

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
            //levelGenerator.EndedGeneration += EndedGeneration;
            yield return wfs;
        }
    }

    private void OnDisable()
    {
        if (levelGenerator != null)
            levelGenerator.EndedGeneration -= EndedGeneration;
    }

    /// <summary>
    /// As soon as the generation ends, this method is called occlusion logic will start.
    /// </summary>
    private void EndedGeneration() =>
        canStartOcclusion = true;

    /// <summary>
    /// Happens every time the player enters a new level piece.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (canStartOcclusion)
        {
            if (other.TryGetComponent(out Player player))
            {
                if (roomOcclusionController.CurrentLevelPieceCollision != thisLevelPiece.gameObject)
                {
                    if (roomOcclusionController.CurrentRoomOcclusion != null)
                    {
                        roomOcclusionController.CurrentRoomOcclusion.StopAllCoroutines();
                    }

                    // Resets if coroutine is already running
                    this.StartCoroutineWithReset(ref controlChildOccludeesCoroutine,
                        ControlChildOccludeesCoroutine());

                    roomOcclusionController.CurrentLevelPieceCollision = thisLevelPiece.gameObject;
                    roomOcclusionController.CurrentRoomOcclusion = this;
                }
            }
        }
    }

    /// <summary>
    /// Coroutine that enables all surrounding level pieces and disables the rest.
    /// </summary>
    /// <returns></returns>
    public IEnumerator ControlChildOccludeesCoroutine()
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

        OnOcclusionCompleted();
    }

    protected virtual void OnOcclusionCompleted() => OcclusionCompleted?.Invoke();
    public event Action OcclusionCompleted;
}
