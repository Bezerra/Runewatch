using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using ExtensionMethods;

public class LevelGenerator : MonoBehaviour, ISaveable
{
    [Header("etc")]
    [SerializeField] private bool instantiatePlayer;
    [SerializeField] private bool generateNavMeshOnStart;

    [SerializeField] private bool randomSeed;
    [Range(0, 10000)][SerializeField] private int seed;
    [Header("Generation Parameters")][SerializeField] private bool allRandomGenerationParameters;
    [SerializeField] private bool randomHorizontalMaximumLevelSize;
    [Range(15, 150)] [SerializeField] private int horizontalMaximumLevelSize;
    [SerializeField] private bool randomForwardMaximumLevelSize;
    [Range(15, 150)] [SerializeField] private int forwardMaximumLevelSize;
    [SerializeField] private bool randomMinimumNumberOfRooms;
    [Range(2, 50)] [SerializeField] private int minimumNumberOfRooms;
    [SerializeField] private bool randomMaximumNumberOfRooms;
    [Range(2, 100)] [SerializeField] private int maximumNumberOfRooms;
    [Tooltip("Builds level from starting room to front only.")][SerializeField] bool fromStartingRoomToFront;

    [Header("Level Pieces")]
    [SerializeField] private LevelPiece startingPiece;
    [SerializeField] private LevelPiece bossRoom;
    [SerializeField] private LevelPiece[] corridors;
    [SerializeField] private LevelPiece[] rooms;
    [SerializeField] private LevelPiece wall;
    [SerializeField] private LayerMask roomColliderLayer;

    private IList<LevelPiece> allRooms;
    private IList<LevelPiece> allRoomsAndCorridors;

    private IEnumerator generationCoroutine;
    private System.Random random;
    private int numberOfLoops;
    
    [SerializeField] private GameObject player;
    [SerializeField] private ElementType element;
    public ElementType Element => element;

    /// <summary>
    /// Sets variables values.
    /// </summary>
    public void GetValues()
    {
        if (randomSeed)
            GenerateSeed();
        else
            random = new System.Random(seed);

        if (allRandomGenerationParameters)
        {
            horizontalMaximumLevelSize = random.Next(15, 151);
            forwardMaximumLevelSize = random.Next(15, 151);
            minimumNumberOfRooms = random.Next(2, 21);
            maximumNumberOfRooms = random.Next(2, 50);
            fromStartingRoomToFront = random.Next(0, 2) == 1 ? fromStartingRoomToFront = true : fromStartingRoomToFront = false;
        }
        else
        {
            if (randomHorizontalMaximumLevelSize)
                horizontalMaximumLevelSize = random.Next(15, 151);
            if (randomForwardMaximumLevelSize)
                forwardMaximumLevelSize = random.Next(15, 151);
            if (randomMinimumNumberOfRooms)
                minimumNumberOfRooms = random.Next(2, 21);
            if (randomMaximumNumberOfRooms)
                maximumNumberOfRooms = random.Next(2, 100);
        }
    }

    /// <summary>
    /// Starts generation.
    /// Called after Getvalues (if level is starting normally) or called directly (if the game was loaded)
    /// </summary>
    /// <param name="loadedRandom">Checks if the game is being loaded.</param>
    /// <param name="seed">Seed to load.</param>
    public void StartGeneration(bool loadedRandom = false)
    {
        // Adjust some values common to every generation (starting game generation or loading game generation)
        if (minimumNumberOfRooms > maximumNumberOfRooms) minimumNumberOfRooms = maximumNumberOfRooms;
        if (maximumNumberOfRooms < minimumNumberOfRooms) maximumNumberOfRooms = minimumNumberOfRooms;

        // A number that grows or shrinks depending on the number of minimum or maximum rooms
        // Don't touch
        numberOfLoops = 7;
        if (minimumNumberOfRooms > numberOfLoops) numberOfLoops = minimumNumberOfRooms;
        if (maximumNumberOfRooms < numberOfLoops) numberOfLoops = maximumNumberOfRooms;
        ///////////////////////////////////////////////////////////////////////////////

        // Generation
        if (loadedRandom == false)
        {
            // This random can be from a user's seed or totally random
            generationCoroutine = GenerateLevel(random);
        }
        else
        {
            // This random will always be from a user's seed (loading game = already existing level)
            generationCoroutine = GenerateLevel(new System.Random(seed));
        }

        StartCoroutine(generationCoroutine);
    }

    /// <summary>
    /// Generates a level. Creates rooms and corridors with contact points in order to connect all level pieces.
    /// </summary>
    /// <param name="random">Instance of Random.</param>
    /// <param name="firstAttempt">Parameter that defines if this is the firsts attempt creating the level.</param>
    /// <returns>Null.</returns>
    private IEnumerator GenerateLevel(System.Random random, bool firstAttempt = true)
    { 
        YieldInstruction wffu = new WaitForFixedUpdate();
        bool bossRoomSpawned = false;
        while (bossRoomSpawned == false)
        {
            // Starts generating random seeds after first failed attempt
            if (firstAttempt == false)
                GenerateSeed();

            DestroyEveryPiece();

            // Creates a gameobject to put all the pieces
            GameObject levelParent = new GameObject();
            levelParent.name = "Level Pieces Parent";
            levelParent.tag = "LevelParent";

            IList<ContactPoint> openedContactPoints = new List<ContactPoint>();
            allRooms = new List<LevelPiece>();
            allRoomsAndCorridors = new List<LevelPiece>();
            bossRoomSpawned = false;

            // Creates and places first corridor
            LevelPiece startingRoomPiece = Instantiate(startingPiece, Vector3.zero, Quaternion.identity);
            ContactPoint startingRoomContactPoint = startingRoomPiece.ContactPoints[random.Next(0, startingRoomPiece.ContactPoints.Length)];
            startingRoomPiece.transform.parent = levelParent.transform;
            allRoomsAndCorridors.Add(startingRoomPiece);

            // Creates first corridor
            LevelPiece initialCorridor = Instantiate(corridors[random.Next(0, corridors.Length)]);
            ContactPoint initialCorridorContactPoint = initialCorridor.ContactPoints[random.Next(0, initialCorridor.ContactPoints.Length)];
            initialCorridor.transform.parent = levelParent.transform;
            allRoomsAndCorridors.Add(initialCorridor);

            // Places first corridor and closes its contact points
            RotateAndSetPiece(initialCorridor, initialCorridorContactPoint, startingRoomContactPoint, random);
            initialCorridorContactPoint.Close();
            startingRoomContactPoint.Close();
            // Gets the wall on top of this open contact point and deactivates it
            if (initialCorridorContactPoint.transform.childCount > 0)
                initialCorridorContactPoint.transform.GetChild(0).gameObject.SetActive(false);

            // Adds corridor open points to open points list
            foreach (ContactPoint newContactPoint in initialCorridor.ContactPoints)
            {
                if (newContactPoint.transform.position != initialCorridorContactPoint.transform.position)
                {
                    newContactPoint.Open();
                    openedContactPoints.Add(newContactPoint);
                }
            }

            // Level creation loop
            byte numberOfLoop = 0;
            while (openedContactPoints.Count > 0 && numberOfLoop < numberOfLoops)
            {
                int openedContacts = openedContactPoints.Count;
                for (int i = 0; i < openedContacts; i++)
                {
                    // Creates a common levelPiece and desired contactPoint to connect
                    LevelPiece pieceToPlace = null;
                    ContactPoint pieceContactPoint = null;

                    if (openedContactPoints[i].ParentRoom.Type == PieceType.Room)
                    {
                        pieceToPlace = Instantiate(corridors[random.Next(0, corridors.Length)]);
                        pieceContactPoint = pieceToPlace.ContactPoints[random.Next(0, pieceToPlace.ContactPoints.Length)];

                        // Skips incompatible pieces
                        if (openedContactPoints[i].IncompatiblePieces.Contains(pieceToPlace.ConcreteType))
                        {
                            Destroy(pieceToPlace.gameObject);
                            continue;
                        }

                        RotateAndSetPiece(pieceToPlace, pieceContactPoint, openedContactPoints[i], random);

                        pieceContactPoint.Close();

                        yield return wffu;

                        ValidatePiece(pieceToPlace, pieceContactPoint, false, openedContactPoints, i, levelParent, random);
                    }

                    if (openedContactPoints[i].ParentRoom.Type == PieceType.Corridor ||
                        openedContactPoints[i].ParentRoom.Type == PieceType.Stairs)
                    {
                        pieceToPlace = Instantiate(rooms[random.Next(0, rooms.Length)]);
                        pieceContactPoint = pieceToPlace.ContactPoints[random.Next(0, pieceToPlace.ContactPoints.Length)];

                        // Skips incompatible pieces
                        if (openedContactPoints[i].IncompatiblePieces.Contains(pieceToPlace.ConcreteType))
                        {
                            Destroy(pieceToPlace.gameObject);
                            continue;
                        }

                        RotateAndSetPiece(pieceToPlace, pieceContactPoint, openedContactPoints[i], random);

                        pieceContactPoint.Close();

                        yield return wffu;

                        ValidatePiece(pieceToPlace, pieceContactPoint, true, openedContactPoints, i, levelParent, random);
                    }
                }

                numberOfLoop++;
            }

            if (allRooms.Count < minimumNumberOfRooms || allRooms.Count > maximumNumberOfRooms)
            {
                if (allRooms.Count < minimumNumberOfRooms)
                {
                    if (numberOfLoops < 15) numberOfLoops++;
                }
                else if (allRooms.Count > maximumNumberOfRooms)
                {
                    if (numberOfLoops > 2) numberOfLoops--;
                }

                print("Invalid number of rooms. Attempting another time.");
                yield return new WaitForSeconds(0.25f);
                GameObject[] levelParents = GameObject.FindGameObjectsWithTag("LevelParent");
                foreach (GameObject lvlParent in levelParents)
                    Destroy(lvlParent.gameObject);

                GenerateSeed();

                continue;
            }

            // Generate boss room
            // Organized a list with contact points by distance
            List<ContactPoint> contactPointsDistance = openedContactPoints.OrderByDescending(i => Vector3.Distance(i.transform.position,
                                                        startingRoomPiece.transform.position)).ToList();
            LevelPiece bossRoomPiece = Instantiate(bossRoom);
            ContactPoint bossRoomContactPoint = bossRoomPiece.ContactPoints[random.Next(0, bossRoomPiece.ContactPoints.Length)];
            bossRoomPiece.transform.parent = levelParent.transform;

            for (int i = 0; i < contactPointsDistance.Count; i++)
            {
                if (contactPointsDistance[i].ParentRoom.Type == PieceType.Corridor ||
                    contactPointsDistance[i].ParentRoom.Type == PieceType.Stairs)
                {
                    RotateAndSetPiece(bossRoomPiece, bossRoomContactPoint, contactPointsDistance[i], random);

                    yield return new WaitForSeconds(0.25f);

                    if (IsPieceValid(bossRoomPiece, random, false))
                    {
                        print("Valid level generation.");
                        contactPointsDistance[i].Close();
                        bossRoomContactPoint.Close();
                        openedContactPoints.Remove(contactPointsDistance[i]);
                        bossRoomSpawned = true;
                        allRoomsAndCorridors.Add(bossRoomPiece);

                        // Gets the wall on top of current piece to place contact point and deactivates it
                        if (contactPointsDistance[i].transform.childCount > 0)
                            contactPointsDistance[i].transform.GetChild(0).gameObject.SetActive(false);

                        break;
                    }
                    else
                    {
                        print("Invalid level generation. Attempting another time.");
                        yield return new WaitForSeconds(0.25f);
                        GameObject[] levelParents = GameObject.FindGameObjectsWithTag("LevelParent");
                        foreach (GameObject lvlParent in levelParents)
                            Destroy(lvlParent.gameObject);
                        bossRoomSpawned = false;

                        GenerateSeed();

                        break;
                    }
                }
            }

            if (bossRoomSpawned)
            {
                generationCoroutine = GenerateWallsOnExits(levelParent, openedContactPoints, wffu);
                StartCoroutine(generationCoroutine);
            }
        }
    }

    /// <summary>
    /// Generates walls to cover all exits with opened contact points.
    /// </summary>
    /// <param name="levelParent">Level parent game object with all pieces.</param>
    /// <param name="openedContactPoints">List with all opened contact points.</param>
    /// <param name="allContactPoints">List with all contact points.</param>
    /// <param name="wffu">Wait for fixed update.</param>
    /// <returns>Null.</returns>
    private IEnumerator GenerateWallsOnExits(GameObject levelParent, IList<ContactPoint> openedContactPoints,
        YieldInstruction wffu)
    {
        print("Doing final adjustements...");
        // Close every opened exit with a wall
        while (openedContactPoints.Count > 0)
        {
            for (int i = openedContactPoints.Count - 1; i >= 0; i--)
            {
                
                // 
                // // Rotates piece to match contact point rotation
                // if (openedContactPoints[i].ParentRoom.Type == PieceType.Room)
                //     RotatePiece(wallPiece, wallContactPoint, openedContactPoints[i], random);
                // else if (openedContactPoints[i].ParentRoom.Type == PieceType.Corridor ||
                //         openedContactPoints[i].ParentRoom.Type == PieceType.Stairs)
                //         RotatePiece(wallPiece, wallContactPoint, openedContactPoints[i], random, true);
                // 
                // // Sets a piece in a contact point
                // SetPiece(wallPiece, wallContactPoint, openedContactPoints[i]);
                // 
                // wallContactPoint.Close();

                yield return wffu;

                if (openedContactPoints[i].ParentRoom.Type == PieceType.Corridor)
                {
                    // If corridor piece is valid, it sets the piece normally
                    if (IsPieceValid(openedContactPoints[i].ParentRoom, random, false))
                    {
                        // LevelPiece wallPiece = Instantiate(wall);
                        // ContactPoint wallContactPoint = wallPiece.ContactPoints[random.Next(0, wallPiece.ContactPoints.Length)];
                        // 
                        // RotateAndSetPiece(wallPiece, wallContactPoint, openedContactPoints[i].ParentRoom.ConnectedContactPoint, random);

                        openedContactPoints[i].Close();
                        openedContactPoints.Remove(openedContactPoints[i]);

                        // wallPiece.transform.parent = levelParent.transform;
                    }
                    else // Else it destroys the invalid corridor and sets the wall in its connected contact point.
                    {
                        // RotateAndSetPiece(wallPiece, wallContactPoint, openedContactPoints[i].ParentRoom.ConnectedContactPoint, random);

                        Destroy(openedContactPoints[i].ParentRoom.gameObject);
                        openedContactPoints.Remove(openedContactPoints[i]);
                    }
                }
                // If it's a stairs piece, it destroys it and sets the wall on its connected contact point.
                else if (openedContactPoints[i].ParentRoom.Type == PieceType.Stairs)
                {
                    // LevelPiece wallPiece = Instantiate(wall);
                    // ContactPoint wallContactPoint = wallPiece.ContactPoints[random.Next(0, wallPiece.ContactPoints.Length)];
                    // 
                    // RotateAndSetPiece(wallPiece, wallContactPoint, openedContactPoints[i].ParentRoom.ConnectedContactPoint, random);

                    Destroy(openedContactPoints[i].ParentRoom.gameObject);
                    openedContactPoints.Remove(openedContactPoints[i]);

                    // wallPiece.transform.parent = levelParent.transform;
                }
                else if (openedContactPoints[i].ParentRoom.Type == PieceType.Room)
                {
                    openedContactPoints[i].Close();
                    openedContactPoints.Remove(openedContactPoints[i]);
                }

                //wallPiece.transform.parent = levelParent.transform;
            }
        }

        GenerateNavMesh();
    }

    /// <summary>
    /// Generates a nav mesh for all pieces.
    /// </summary>
    private void GenerateNavMesh()
    {
        // Create navmesh for every component with NavMeshSurface script
        if (generateNavMeshOnStart)
        {
            print("Generating Navmesh...");
            GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (GameObject rootGameObject in rootGameObjects)
            {
                NavMeshSurface[] childrenNavMeshes =
                    rootGameObject.GetComponentsInChildren<NavMeshSurface>();

                foreach (NavMeshSurface navmesh in childrenNavMeshes)
                {
                    navmesh.BuildNavMesh();
                }
            }
            print("Navmesh generated.");
        }

        // Destroys empty levelParents game objects
        GameObject[] levelParents = GameObject.FindGameObjectsWithTag("LevelParent");
        foreach (GameObject lvlParent in levelParents)
        {
            try
            {
                lvlParent.transform.GetChild(0);
            }
            catch (UnityException)
            {
                Destroy(lvlParent.gameObject);
            }  
        }

        foreach(LevelPiece piece in allRoomsAndCorridors)
        {
            if (piece != null)
            {
                if (piece.BoxCollidersParent != null)
                {
                    piece.BoxCollidersParent.gameObject.SetActive(false);
                }
            }
        }

        if (instantiatePlayer && player != null)
            Instantiate(player, Vector3.zero, Quaternion.identity);

        Debug.Log("Took " + Time.time + " seconds to generate, with seed " + seed + '.');

        OnEndedGeneration();
    }

    /// <summary>
    /// Generates a new seed for Random.
    /// </summary>
    private void GenerateSeed() =>
        random = new System.Random();

    /// <summary>
    /// Destroys every level piece.
    /// </summary>
    private void DestroyEveryPiece()
    {
        GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject rootGameObject in rootGameObjects)
        {
            LevelPiece[] childrenLevelPiecess =
                rootGameObject.GetComponentsInChildren<LevelPiece>();

            foreach (LevelPiece levelPiece in childrenLevelPiecess)
            {
                if (levelPiece != null)
                    Destroy(levelPiece.gameObject);
            }
        }
    }

    /// <summary>
    /// Resets level generation.
    /// </summary>
    /// <param name="message">Message to print.</param>
    /// <param name="random">Instance of random.</param>
    /// <returns>Null.</returns>
    public IEnumerator ResetGeneration(string message, System.Random random )
    {
        if (random == null)
            random = new System.Random();

        StopCoroutine(generationCoroutine);
        print(message);

        yield return new WaitForSeconds(1);

        GameObject[] levelParents = GameObject.FindGameObjectsWithTag("LevelParent");
        foreach (GameObject lvlParent in levelParents)
            Destroy(lvlParent.gameObject);
        DestroyEveryPiece();

        yield return new WaitForSeconds(1);

        generationCoroutine = GenerateLevel(random, false);
        StartCoroutine(generationCoroutine);
    }

    /// <summary>
    /// Checks if a piece intersects with any other piece.
    /// </summary>
    /// <param name="levelPiece">Piece to check.</param>
    /// <param name="random">Instance of random.</param> 
    /// <returns>Returns true if a piece intersects with any other piece.</returns>
    private bool PieceIntersection(LevelPiece levelPiece, System.Random random)
    {
        foreach (BoxCollider boxCollider in levelPiece.BoxColliders)
        {
            try
            {
                Collider[] roomCollider = Physics.OverlapBox(levelPiece.transform.position + boxCollider.center, boxCollider.size / 2,
                boxCollider.transform.root.rotation, roomColliderLayer);

                if (roomCollider.Length > 1)
                {
                    return true;
                }
            }
            catch
            {
                StartCoroutine(ResetGeneration("Something with colliders went wrong.Attempting to build another level.", random));
            }
        }
        return false;
    }

    /// <summary>
    /// Rotates a piece until it matches a contact point.
    /// </summary>
    /// <param name="levelPiece">Piece to rotate.</param>
    /// <param name="levelPieceContact">Contact point of the piece.</param>
    /// <param name="originContact">Contact point to match.</param>
    /// /// <param name="random">Instance of random.</param>
    /// <param name="inversedContact">Parameter that checks if the origin contact is inversed (true = corridor) or not (false = room).</param>
    private void RotatePiece(LevelPiece levelPiece, ContactPoint levelPieceContact, ContactPoint originContact, System.Random random, 
        bool inversedContact = false)
    {
        byte placementCount = 0;
        while (placementCount < 4)
        {
            try
            {
                if (inversedContact)
                {
                    if (levelPieceContact.transform.InverseDirectionAs(originContact.transform) == false)
                    {
                        placementCount++;
                        levelPiece.transform.Rotate(new Vector3(0, 90, 0));
                    }
                    else
                    {
                        placementCount = 4;
                    }
                }
                else
                {
                    if (levelPieceContact.transform.SameDirectionAs(originContact.transform) == false)
                    {
                        placementCount++;
                        levelPiece.transform.Rotate(new Vector3(0, 90, 0));
                    }
                    else
                    {
                        placementCount = 4;
                    }
                }
            }
            catch
            {
                StartCoroutine(ResetGeneration("Something with contact points went wrong. Attempting to build another level.", random));
            }
        }
    }

    /// <summary>
    /// Checks if a piece is valid. If it's not valid, it destroys it.
    /// </summary>
    /// <param name="levelPiece">Piece to check.</param>
    /// <param name="random">Instance of random.</param>
    /// <param name="creatingLevelBase">True if it's not creating walls or boss room (meaning it's still creating the base structure).</param>
    /// <returns>Returns true if the piece is valid, else it returns false.</returns>
    private bool IsPieceValid(LevelPiece levelPiece, System.Random random, bool creatingLevelBase = true)
    {
        if (creatingLevelBase)
        {
            // Horizontal limit
            if (levelPiece.transform.position.z > horizontalMaximumLevelSize ||
                levelPiece.transform.position.z < -horizontalMaximumLevelSize)
            {
                Destroy(levelPiece.gameObject);
                return false;
            }

            // Vertical limit (forward)
            if (fromStartingRoomToFront)
            {
                if (levelPiece.transform.position.x > forwardMaximumLevelSize ||
                levelPiece.transform.position.x < -10)
                {
                    Destroy(levelPiece.gameObject);
                    return false;
                }
            }
            else
            {
                if (levelPiece.transform.position.x > forwardMaximumLevelSize || 
                    levelPiece.transform.position.x < -forwardMaximumLevelSize)
                {
                    Destroy(levelPiece.gameObject);
                    return false;
                }
            }
        }
        if (PieceIntersection(levelPiece, random))
        {
            Destroy(levelPiece.gameObject);
            return false;
        }

        return true;
    }

    /// <summary>
    /// Places a piece in a contact point.
    /// </summary>
    /// <param name="levelPiece">Piece to place.</param>
    /// <param name="levelPieceContact">Contact point of the piece.</param>
    /// <param name="originContact">Contact point to place the piece.</param>
    private void SetPiece(LevelPiece levelPiece, ContactPoint levelPieceContact, ContactPoint originContact)
    {
        levelPiece.transform.position = Vector3.zero;

        Vector3 dir = levelPieceContact.transform.Direction(originContact.transform);
        float dist = Vector3.Distance(levelPieceContact.transform.position, originContact.transform.position);

        levelPiece.transform.position += dir * dist;
    }

    /// <summary>
    /// Rotates and sets a piece in the level.
    /// </summary>
    /// <param name="pieceToPlace">Piece to place.</param>
    /// <param name="contactPoint">Piece contact point.</param>
    /// <param name="openedContactPoint">Opened contact point to place the piece.</param>
    /// <param name="random">Instance of Random.</param>
    private void RotateAndSetPiece(LevelPiece pieceToPlace, ContactPoint contactPoint, ContactPoint openedContactPoint, System.Random random)
    {
        // Rotates piece to match contact point rotation
        RotatePiece(pieceToPlace, contactPoint, openedContactPoint, random);

        // Sets a piece in a contact point
        SetPiece(pieceToPlace, contactPoint, openedContactPoint);
    }

    /// <summary>
    /// Checks if the last piece is valid.
    /// If it's not valid it will destroy it, else it will add its contact points to a list.
    /// <param name="pieceToPlace">Piece to place.</param>
    /// <param name="pieceContactPoint">Piece contact point.</param>
    /// <param name="addToAllRooms">Bool that determines if this piece should be add to all rooms list.</param>
    /// <param name="openedContactPoints">List with all opened contact points.</param>
    /// <param name="index">Index of opened contact points list.</param>
    /// <param name="levelParent">Gameobject parent of all level pieces.</param>
    /// <param name="random">Instance of Random.</param>
    private void ValidatePiece(LevelPiece pieceToPlace, ContactPoint pieceContactPoint, bool addToAllRooms,
        IList<ContactPoint> openedContactPoints,
        int index, GameObject levelParent, System.Random random)
    {
        // Checks if the last piece is valid
        // If it's not valid it will destroy it, else it will add its contact points to a list
        if (IsPieceValid(pieceToPlace, random))
        {
            // Gets the wall on top of this open contact point and deactivates it
            if (openedContactPoints[index].transform.childCount > 0)
                openedContactPoints[index].transform.GetChild(0).gameObject.SetActive(false);

            // Gets the wall on top of current piece to place contact point and deactivates it
            if (pieceContactPoint.transform.childCount > 0)
                pieceContactPoint.transform.GetChild(0).gameObject.SetActive(false);

            openedContactPoints[index].Close();
            pieceToPlace.ConnectedContactPoint = openedContactPoints[index];
            openedContactPoints.Remove(openedContactPoints[index]);
            pieceToPlace.transform.parent = levelParent.transform;

            // A variable to have control of all rooms (contains all spawned rooms)
            if (addToAllRooms)
                allRooms.Add(pieceToPlace);

            // A variable to have control of all rooms and corridors (contains all spawned rooms and corridors)
            allRoomsAndCorridors.Add(pieceToPlace);

            foreach (ContactPoint newContactPoint in pieceToPlace.ContactPoints)
            {
                if (newContactPoint.transform.position != pieceContactPoint.transform.position)
                {
                    newContactPoint.Open();
                    openedContactPoints.Add(newContactPoint);
                }
            }
        }
        else
        {
            openedContactPoints[index].IncompatiblePieces.Add(pieceToPlace.ConcreteType);
        }
    }
    
    /// <summary>
    /// Saves current values to saveData.
    /// </summary>
    /// <param name="saveData">Save data.</param>
    public void SaveCurrentData(SaveData saveData)
    {
        saveData.DungeonSavedData.Seed = seed;
        saveData.DungeonSavedData.HorizontalMaximumLevelSize = horizontalMaximumLevelSize;
        saveData.DungeonSavedData.ForwardMaximumLevelSize = forwardMaximumLevelSize;
        saveData.DungeonSavedData.MinimumNumberOfRooms = minimumNumberOfRooms;
        saveData.DungeonSavedData.MaximumNumberOfRooms = maximumNumberOfRooms;
        saveData.DungeonSavedData.FromStartingRoomToFront = fromStartingRoomToFront;
        saveData.DungeonSavedData.Element = element;
    }

    /// <summary>
    /// Loads saved data to variables and starts generation with that data.
    /// </summary>
    /// <param name="saveData">Saved data.</param>
    /// <returns>Null.</returns>
    public IEnumerator LoadData(SaveData saveData)
    {
        yield return new WaitForSeconds(0.25f);

        if (this != null) // < DO NOT REMOVE
        {
            seed = saveData.DungeonSavedData.Seed;
            horizontalMaximumLevelSize = saveData.DungeonSavedData.HorizontalMaximumLevelSize;
            forwardMaximumLevelSize = saveData.DungeonSavedData.ForwardMaximumLevelSize;
            minimumNumberOfRooms = saveData.DungeonSavedData.MinimumNumberOfRooms;
            maximumNumberOfRooms = saveData.DungeonSavedData.MaximumNumberOfRooms;
            fromStartingRoomToFront = saveData.DungeonSavedData.FromStartingRoomToFront;
            element = saveData.DungeonSavedData.Element;

            instantiatePlayer = false;

            StartGeneration(true);
        }
        yield return null;
    }

    /// <summary>
    /// Triggered when the level ends its generation.
    /// </summary>
    protected virtual void OnEndedGeneration() => EndedGeneration?.Invoke();

    public event Action EndedGeneration; 
}
