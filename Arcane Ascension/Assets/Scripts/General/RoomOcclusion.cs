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
    private LevelPiece thisLevelPiece;

    private static IEnumerator controlChildOccludeesCoroutine;

    private bool canCheckChildOccludees;

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
            levelGenerator.EndedGeneration += CanCheckChildOccludees;
            yield return wfs;
        }
    }

    private void OnDisable()
    {
        if (levelGenerator != null)
            levelGenerator.EndedGeneration -= CanCheckChildOccludees;
    }

    private void CanCheckChildOccludees() => canCheckChildOccludees = true;

    private void OnTriggerEnter(Collider other)
    {
        if (canCheckChildOccludees)
        {
            if (other.TryGetComponent(out Player player))
            {
                this.StartCoroutineWithReset(ref controlChildOccludeesCoroutine, ControlChildOccludeesCoroutine());
            }
        }
    }

    private IEnumerator ControlChildOccludeesCoroutine()
    {
        IList<LevelPiece> generatedRoomPieces = new List<LevelPiece>();
        for (int i = 0; i < levelGenerator.AllGeneratedLevelPieces.Count; i++)
        {
            generatedRoomPieces.Add(levelGenerator.AllGeneratedLevelPieces[i]);
        }

        StartCoroutine(thisLevelPiece.EnableChildOccludeesCoroutine());
        generatedRoomPieces.Remove(thisLevelPiece);
            
        for (int i = 0; i < thisLevelPiece.ConnectedPieces.Count; i++)
        {
            StartCoroutine(thisLevelPiece.ConnectedPieces[i].EnableChildOccludeesCoroutine());
            generatedRoomPieces.Remove(thisLevelPiece.ConnectedPieces[i]);
            yield return null;
        }

        for (int i = 0; i < generatedRoomPieces.Count; i++)
        {
            if (generatedRoomPieces[i] != null)
                StartCoroutine(generatedRoomPieces[i].DisableChildOccludeesCoroutine());
            yield return null;
        }        
    }
}
