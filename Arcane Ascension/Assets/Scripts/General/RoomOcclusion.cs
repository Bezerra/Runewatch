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

    // Coroutines
    private YieldInstruction wffu;

    private bool canCheckRenderers;

    private void Awake()
    {
        wffu = new WaitForFixedUpdate();
        thisLevelPiece = GetComponentInParent<LevelPiece>();

        StartCoroutine(FindLevelGenerator());
    }

    private IEnumerator FindLevelGenerator()
    {
        YieldInstruction wfs = new WaitForSeconds(0.2f);
        while (levelGenerator == null)
        {
            levelGenerator = FindObjectOfType<LevelGenerator>();
            levelGenerator.EndedGeneration += CanCheckRenderers;
            yield return wfs;
        }
    }

    private void OnDisable()
    {
        if (levelGenerator != null)
            levelGenerator.EndedGeneration -= CanCheckRenderers;
    }

    private void CanCheckRenderers() => canCheckRenderers = true;

    private void OnTriggerStay(Collider other)
    {
        if (canCheckRenderers)
        {
            if (other.TryGetComponent(out Player player))
            {
                StartCoroutine(ControlRenderersCoroutine());
            }
        }
    }

    private IEnumerator ControlRenderersCoroutine()
    {
        IList<LevelPiece> generatedRoomPieces = new List<LevelPiece>();
        for (int i = 0; i < levelGenerator.AllGeneratedLevelPieces.Count; i++)
        {
            generatedRoomPieces.Add(levelGenerator.AllGeneratedLevelPieces[i]);
        }

        StartCoroutine(thisLevelPiece.EnableRenderersCoroutine());
        generatedRoomPieces.Remove(thisLevelPiece);

        if (thisLevelPiece.ContactPointOfCreation != null)
        {
            StartCoroutine(thisLevelPiece.ContactPointOfCreation.ParentRoom.EnableRenderersCoroutine());
            generatedRoomPieces.Remove(thisLevelPiece.ContactPointOfCreation.ParentRoom);
        }
            
        for (int i = 0; i < thisLevelPiece.ChildPieces.Count; i++)
        {
            StartCoroutine(thisLevelPiece.ChildPieces[i].EnableRenderersCoroutine());
            generatedRoomPieces.Remove(thisLevelPiece.ChildPieces[i]);
            yield return null;
        }

        for (int i = 0; i < generatedRoomPieces.Count; i++)
        {
            StartCoroutine(generatedRoomPieces[i].DisableRenderersCoroutine());
            yield return null;
        }        
    }
}
