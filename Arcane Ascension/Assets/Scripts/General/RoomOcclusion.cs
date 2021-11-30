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
            levelGenerator.EndedGeneration += EndedGeneration;

            yield return wfs;
        }
    }

    private void OnDisable()
    {
        if (levelGenerator != null)
            levelGenerator.EndedGeneration -= EndedGeneration;
    }

    private void EndedGeneration() =>
        canStartOcclusion = true;

    private void OnTriggerEnter(Collider other)
    {
        if (canStartOcclusion)
        {
            if (other.TryGetComponent(out Player player))
            {
                if (roomOcclusionController.CurrentLevelPieceCollision != other.gameObject)
                {
                    this.StartCoroutineWithReset(ref controlChildOccludeesCoroutine,
                        ControlChildOccludeesCoroutine());
                    roomOcclusionController.CurrentLevelPieceCollision = thisLevelPiece.gameObject;
                }
            }
        }
    }

    private IEnumerator ControlChildOccludeesCoroutine()
    {
        for (int i = 0; i < roomOcclusionController.RoomsOcclusion.Count; i++)
        {
            if (roomOcclusionController.RoomsOcclusion[i] != null)
            {
                if (roomOcclusionController.RoomsOcclusion[i] != this)
                {
                    roomOcclusionController.RoomsOcclusion[i].StopAllCoroutines();
                }
            }    
            yield return null;
        }

        IList<LevelPiece> generatedRoomPieces = new List<LevelPiece>();
        for (int i = 0; i < levelGenerator.AllGeneratedLevelPieces.Count; i++)
        {
            generatedRoomPieces.Add(levelGenerator.AllGeneratedLevelPieces[i]);
        }
        
        StartCoroutine(thisLevelPiece.EnableChildOccludeesCoroutine());
        generatedRoomPieces.Remove(thisLevelPiece);
            
        for (int i = 0; i < thisLevelPiece.ConnectedPieces.Count; i++)
        {
            if (thisLevelPiece.ConnectedPieces[i] != null)
            {
                StartCoroutine(thisLevelPiece.ConnectedPieces[i].EnableChildOccludeesCoroutine());
                generatedRoomPieces.Remove(thisLevelPiece.ConnectedPieces[i]);
            }
                
            yield return null;
        }

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
