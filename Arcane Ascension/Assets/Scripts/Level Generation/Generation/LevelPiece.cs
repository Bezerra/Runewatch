using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Class used by every level piece.
/// </summary>
public class LevelPiece : MonoBehaviour
{
    [SerializeField] private PieceType type;
    [SerializeField] private PieceConcreteType concreteType;
    [SerializeField] private ContactPoint[] contactPoints;
    [SerializeField] private GameObject boxCollidersParent;
    [SerializeField] private BoxCollider[] boxColliders;
    [SerializeField] private RoomWeightsSO roomWeights;

    // Components
    private LevelGenerator levelGenerator;
    public IList<ContactPointDoor> ContactPointsDoors { get; private set; }

    private void Awake()
    {
        ConnectedPieces = new List<LevelPiece>();
        levelGenerator = FindObjectOfType<LevelGenerator>();
        ContactPointsDoors = new List<ContactPointDoor>();
    }

    private void OnEnable() =>
        levelGenerator.EndedGeneration += GetchildOccludees;

    private void OnDisable() =>
        levelGenerator.EndedGeneration -= GetchildOccludees;
  
    private Transform[] childOccludees;

    /// <summary>
    /// General type of this piece.
    /// </summary>
    public PieceType Type => type;

    public string Name => 
        concreteType.ToString();

    /// <summary>
    /// Conrecrete type of this piece.
    /// </summary>
    public PieceConcreteType ConcreteType => concreteType;

    /// <summary>
    /// Piece contact points.
    /// </summary>
    public ContactPoint[] ContactPoints => contactPoints;
    
    /// <summary>
    /// Contact point
    /// </summary>
    public ContactPoint ContactPointOfCreation { get; set; }

    /// <summary>
    /// Pieces that are connect with this piece.
    /// </summary>
    public IList<LevelPiece> ConnectedPieces { get; set; }

    /// <summary>
    /// Parent of box colliders for procedural generation.
    /// </summary>
    public GameObject BoxCollidersParent => boxCollidersParent;

    /// <summary>
    /// Box collider for procedural generation.
    /// </summary>
    public BoxCollider[] BoxColliders => boxColliders;

    /// <summary>
    /// Room weight.
    /// </summary>
    public int RoomWeight
    {
        get
        {
            foreach (Weight weight in roomWeights.PiecesWithWeight)
            {
                if (weight.Name == Name)
                {
                    return weight.RoomWeight;
                }
            
            }
            return 0;
        }
    }

    private void GetchildOccludees()
    {
        childOccludees = GetComponentsInChildren<Transform>().
            Where(i => i.CompareTag("ChildOccludee")).ToArray();

        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (contactPoints[i].gameObject.TryGetComponentInChildrenFirstGen(out ContactPointDoor cpDoor))
                ContactPointsDoors.Add(cpDoor);
        }
    }

    /// <summary>
    /// Enables occludees gameobjects.
    /// </summary>
    /// <returns>Null.</returns>
    public IEnumerator EnableChildOccludeesCoroutine()
    {
        for (int i = 0; i < childOccludees.Length; i++)
        {
            if (childOccludees[i].gameObject.activeSelf == false)
            {
                childOccludees[i].gameObject.SetActive(true);
            }

            yield return null;
        }
    }

    /// <summary>
    /// Disables occludees gameobjects.
    /// </summary>
    /// <returns>Null.</returns>
    public IEnumerator DisableChildOccludeesCoroutine()
    {
        for (int i = 0; i < childOccludees.Length; i++)
        {
            if (childOccludees[i].gameObject.activeSelf)
            {
                childOccludees[i].gameObject.SetActive(false);
            }

            yield return null;
        }
    }
}
