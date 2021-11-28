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
    [Header("Generation Values")]
    [SerializeField] private bool                                   randomSeed;
    [Range(-200000000, 200000000)] [SerializeField] private int     seed = 0;
    [Header("Generation Parameters")][SerializeField] private bool  allRandomGenerationParameters;
    [Range(75, 500)] [SerializeField] private int                   horizontalMaximumLevelSize;
    [Range(75, 500)] [SerializeField] private int                   forwardMaximumLevelSize;
    [SerializeField] private bool                                   randomMinimumNumberOfRooms;
    [Range(7, 9)] [SerializeField] private int                     minimumNumberOfRooms;
    [SerializeField] private bool                                   randomMaximumNumberOfRooms;
    [Range(12, 15)] [SerializeField] private int                     maximumNumberOfRooms;


    [Header("Level Pieces")] // TEMP
    [SerializeField] private LevelPiece     startingPiece;
    [SerializeField] private LevelPiece     bossRoom;
    [SerializeField] private LevelPiece[]   corridors;
    [SerializeField] private LevelPiece[]   rooms;
    [SerializeField] private LayerMask  roomColliderLayer;

    // Rooms lists
    private IList<LevelPiece> allRooms;
    private IList<LevelPiece> allRoomsAndCorridors;

    // Generation
    // This number will be set automatically and determines the number of attempts in each loop
    private int numberOfLoops;
    private IEnumerator generationCoroutine;
    private System.Random random;

    [SerializeField] private ElementType element;
    public ElementType Element => element;

    private float timeOfGeneration;

    private void Update() => timeOfGeneration += Time.deltaTime;

    /// <summary>
    /// Sets variables values.
    /// </summary>
    public void GetValues()
    {
        if (randomSeed) GenerateSeed();
        else random = new System.Random(seed);

        if (allRandomGenerationParameters)
        {
            horizontalMaximumLevelSize = random.Next(75, 500);
            forwardMaximumLevelSize = random.Next(75, 500);
            minimumNumberOfRooms = random.Next(7, 10);
            maximumNumberOfRooms = random.Next(12, 16);
        }
        else
        {
            if (randomMinimumNumberOfRooms)
                minimumNumberOfRooms = random.Next(7, 10);
            if (randomMaximumNumberOfRooms)
                maximumNumberOfRooms = random.Next(12, 16);
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
        timeOfGeneration = 0;
        YieldInstruction wffu = new WaitForEndOfFrame();
        bool bossRoomSpawned = false;
        IList<ContactPoint> openedContactPoints;

        // This number is the quantity of loops for each opened poinst loop. The higher the
        // number, the more attempts the algorithm will take to try and find a piece
        // for each opened contact point.
        // This number is directly connect with the limit of rooms that can be spawned,
        // so if the number of rooms exceeds a limit, it will adjust numberOfLoops variable.
        // For the number of rooms in our game, a number between 4 - 7 should be fine.
        numberOfLoops = 5;

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

            #region Initial Rooms
            // All points, rooms and rooms + corridors
            openedContactPoints = new List<ContactPoint>();
            allRooms = new List<LevelPiece>();
            allRoomsAndCorridors = new List<LevelPiece>();

            // Creates and places first corridor
            LevelPiece startingRoomPiece = Instantiate(startingPiece, Vector3.zero, Quaternion.identity);
            ContactPoint startingRoomContactPoint = 
                startingRoomPiece.ContactPoints[random.Next(0, startingRoomPiece.ContactPoints.Length)];
            startingRoomPiece.transform.parent = levelParent.transform;
            allRoomsAndCorridors.Add(startingRoomPiece);
            ////////////////////////////////////////////

            // Creates first corridor
            LevelPiece initialCorridor = Instantiate(corridors[random.Next(0, corridors.Length)]);
            ContactPoint initialCorridorContactPoint = 
                initialCorridor.ContactPoints[random.Next(0, initialCorridor.ContactPoints.Length)];
            initialCorridor.transform.parent = levelParent.transform;
            allRoomsAndCorridors.Add(initialCorridor);
            ////////////////////////////////////////////

            // Places first corridor and closes its contact points
            RotateAndSetPiece(initialCorridor, initialCorridorContactPoint, startingRoomContactPoint);
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
            #endregion

            // Level creation loop
            byte numberOfCurrentLoop = 0;

            // Creates a list of room weights 
            IList<int> roomWeights = new List<int>();
            for (int i = 0; i < rooms.Length; i++)
            {
                roomWeights.Add(rooms[i].RoomWeight);
            }

            // Main generation loop.
            // Spawns rooms and corridors and tries to connect them while there are opened contact points
            while (openedContactPoints.Count > 0 && numberOfCurrentLoop < numberOfLoops)
            {
                int openedContacts = openedContactPoints.Count;

                // For all opened contact points on this loop only < will ignore the ones added inside for now
                for (int i = 0; i < openedContacts; i++)
                {
                    // Creates a common levelPiece and desired contactPoint to connect
                    LevelPiece pieceToPlace = null;
                    ContactPoint pieceContactPoint = null;

                    // If this point is in a room
                    if (openedContactPoints[i].ParentRoom.Type == PieceType.Room)
                    {
                        // Creates a corridor/stairs
                        pieceToPlace = Instantiate(corridors[random.Next(0, corridors.Length)]);
                        pieceContactPoint = pieceToPlace.ContactPoints[
                            random.Next(0, pieceToPlace.ContactPoints.Length)];

                        // Skips incompatible pieces
                        if (openedContactPoints[i].IncompatiblePieces.Contains(
                            pieceToPlace.ConcreteType))
                        {
                            Destroy(pieceToPlace.gameObject);
                            continue;
                        }

                        RotateAndSetPiece(pieceToPlace, pieceContactPoint, openedContactPoints[i]);

                        yield return wffu;

                        ValidatePiece(pieceToPlace, pieceContactPoint, false, openedContactPoints, i, levelParent, random);
                    }

                    // This should be and else if, but if it's changed to else if, it's bugged idkwhy
                    // If it's a corridor/stairs
                    if (openedContactPoints[i].ParentRoom.Type == PieceType.Corridor ||
                        openedContactPoints[i].ParentRoom.Type == PieceType.Stairs)
                    {
                        // Creates rooms depending on their weight
                        pieceToPlace = Instantiate(rooms[random.RandomWeight(roomWeights)]);
                        pieceContactPoint = pieceToPlace.ContactPoints[random.Next(0, pieceToPlace.ContactPoints.Length)];

                        // Skips incompatible pieces
                        if (openedContactPoints[i].IncompatiblePieces.Contains(pieceToPlace.ConcreteType))
                        {
                            Destroy(pieceToPlace.gameObject);
                            continue;
                        }

                        RotateAndSetPiece(
                            pieceToPlace, pieceContactPoint, openedContactPoints[i]);

                        yield return wffu;

                        ValidatePiece(
                            pieceToPlace, pieceContactPoint, true, openedContactPoints, i, levelParent, random);
                    }
                }

                numberOfCurrentLoop++;
            }

            #region Check if number of rooms is valid

            // After main loop, if minimum or maximum number of rooms exceeds a limit, it starts another loop
            if (allRooms.Count < minimumNumberOfRooms || allRooms.Count > maximumNumberOfRooms)
            {
                if (allRooms.Count < minimumNumberOfRooms) numberOfLoops++;
                else if (allRooms.Count > minimumNumberOfRooms) numberOfLoops--;

                print("Invalid number of rooms. Attempting another time.");

                yield return new WaitForSeconds(0.25f);
                GameObject[] levelParents = GameObject.FindGameObjectsWithTag("LevelParent");
                foreach (GameObject lvlParent in levelParents)
                    Destroy(lvlParent);

                GenerateSeed();

                continue;
            }

            #endregion

            #region Boss Room
            // Organized a list with contact points by distance
            openedContactPoints = 
                openedContactPoints.OrderByDescending(
                    i => Vector3.Distance(i.transform.position,
                                                        startingRoomPiece.transform.position)).ToList();
            // Generate boss room
            LevelPiece bossRoomPiece = Instantiate(bossRoom);
            ContactPoint bossRoomContactPoint = 
                bossRoomPiece.ContactPoints[random.Next(0, bossRoomPiece.ContactPoints.Length)];
            bossRoomPiece.transform.parent = levelParent.transform;
            bossRoomSpawned = false;

            // For all remaining points by distance (first point is the farthest one)
            for (int i = 0; i < openedContactPoints.Count; i++)
            {
                // If it's a corridor
                if (openedContactPoints[i].ParentRoom.Type == PieceType.Corridor ||
                    openedContactPoints[i].ParentRoom.Type == PieceType.Stairs)
                {
                    // Sets and rotates piece
                    RotateAndSetPiece(bossRoomPiece, bossRoomContactPoint, openedContactPoints[i]);
                    yield return new WaitForSeconds(0.25f);

                    // If everything is valid, ends generation
                    if (IsPieceValid(bossRoomPiece, random, false))
                    {
                        print("Valid level generation.");

                        // Gets the wall on top of current piece to place contact point and deactivates it
                        if (openedContactPoints[i].transform.childCount > 0)
                            openedContactPoints[i].transform.GetChild(0).gameObject.SetActive(false);

                        // Closes and remove the involved points
                        openedContactPoints[i].Close();
                        bossRoomContactPoint.Close();
                        openedContactPoints.Remove(openedContactPoints[i]);

                        bossRoomSpawned = true;
                        allRoomsAndCorridors.Add(bossRoomPiece);

                        break;
                    }
                    else
                    {
                        // Continues to next point
                        continue;
                    }
                }

                // If it's a room
                // It will try to place every possible corridor and connect the boss
                // room to that point, if it can't, it will go to the next point
                else if (openedContactPoints[i].ParentRoom.Type == PieceType.Room)
                {
                    bool finalCorridorCreated = false;

                    // For all possible corridors
                    LevelPiece[] randomCorridors = corridors.OrderBy(i => Guid.NewGuid()).ToArray();

                    // For all rooms
                    for (int j = 0; j < randomCorridors.Length; j++)
                    {
                        // Creates a corridor/stairs
                        LevelPiece pieceToPlace = Instantiate(corridors[j]);
                        ContactPoint pieceContactPoint =
                            pieceToPlace.ContactPoints[random.Next(0, pieceToPlace.ContactPoints.Length)];

                        // Skips incompatible pieces
                        if (openedContactPoints[i].IncompatiblePieces.Contains(
                            pieceToPlace.ConcreteType))
                        {
                            Destroy(pieceToPlace.gameObject);
                            continue;
                        }

                        // Tries to set the room
                        RotateAndSetPiece(pieceToPlace, pieceContactPoint, openedContactPoints[i]);

                        yield return wffu;

                        // If it's valid
                        if (IsPieceValid(pieceToPlace, random, false))
                        {
                            ValidatePiece(
                                pieceToPlace, pieceContactPoint, false, openedContactPoints, i, levelParent, random);

                            finalCorridorCreated = true;

                            break;
                        }
                        else
                        {
                            Destroy(pieceToPlace.gameObject);
                            continue;
                        }
                    }

                    // Sets and rotates piece
                    if (finalCorridorCreated)
                    {
                        // From now on, it will be used the last index on opened contact points
                        // because this was the point added on the previous corridor
                        RotateAndSetPiece(bossRoomPiece, bossRoomContactPoint, 
                            openedContactPoints[openedContactPoints.Count - 1]);

                        yield return new WaitForSeconds(0.25f);

                        // If everything is valid, ends generation
                        if (IsPieceValid(bossRoomPiece, random, false))
                        {
                            print("Valid level generation.");

                            // Gets the wall on top of current piece to place contact point and deactivates it
                            if (openedContactPoints[openedContactPoints.Count - 1].
                                transform.childCount > 0)
                            {
                                openedContactPoints[openedContactPoints.Count - 1].
                                    transform.GetChild(0).gameObject.SetActive(false);
                            }                            

                            // Closes and remove the involved points
                            openedContactPoints[openedContactPoints.Count - 1].Close();
                            bossRoomContactPoint.Close();
                            openedContactPoints.Remove(openedContactPoints[openedContactPoints.Count - 1]);

                            bossRoomSpawned = true;
                            allRoomsAndCorridors.Add(bossRoomPiece);

                            break;
                        }

                        // Fails boss generation
                        print("Invalid level generation. Attempting another time.");
                        yield return new WaitForSeconds(0.25f);
                        GameObject[] levelParents = GameObject.FindGameObjectsWithTag("LevelParent");
                        foreach (GameObject lvlParent in levelParents)
                            Destroy(lvlParent);
                        bossRoomSpawned = false;

                        GenerateSeed();

                        break;
                    }
                    else
                    {
                        // Continues to the next point
                        continue;
                    }
                }
            }
            #endregion

            // After looping through all points, if it was able to generate boss room
            if (bossRoomSpawned)
            {
                generationCoroutine = GenerateWallsOnExits(levelParent, openedContactPoints, wffu);
                StartCoroutine(generationCoroutine);
            }
            // Else it will go back to the beggining of the whole loop, destroy everything and 
            // generate a new dungeon with different values
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
                yield return wffu;

                if (openedContactPoints[i].ParentRoom.Type == PieceType.Corridor ||
                    openedContactPoints[i].ParentRoom.Type == PieceType.Stairs)
                {
                    // Gets the connectected contact point of this piece and
                    // activates its wall
                    openedContactPoints[i].ParentRoom.ConnectedContactPoint.
                        transform.GetChild(0).gameObject.SetActive(true);

                    // Destroys that corridor/stairs and removes its contact point from the list
                    Destroy(openedContactPoints[i].ParentRoom.gameObject);
                    openedContactPoints.Remove(openedContactPoints[i]);
                }
                else if (openedContactPoints[i].ParentRoom.Type == PieceType.Room)
                {
                    openedContactPoints[i].Close();
                    openedContactPoints.Remove(openedContactPoints[i]);
                }
            }
        }

        FinalAdjustements();
    }

    /// <summary>
    /// Generates a nav mesh for all pieces.
    /// </summary>
    private void FinalAdjustements()
    {
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
                Destroy(lvlParent);
            }  
        }

        // Deactivates RoomCollision collider
        /*
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
        */

        Debug.Log("Took " + timeOfGeneration + " seconds to generate, with seed " + seed +  
            " and number of loops of " + numberOfLoops);

        OnEndedGeneration();
    }

    /// <summary>
    /// Generates a new seed for Random.
    /// </summary>
    private void GenerateSeed()
    {
        seed = UnityEngine.Random.Range(-200000000, 200000000);
        random = new System.Random(seed);
    }

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

        if (generationCoroutine != null) StopCoroutine(generationCoroutine);
        print(message);

        yield return new WaitForSeconds(1);

        // Destroys every piece previously generated
        GameObject[] levelParents = GameObject.FindGameObjectsWithTag("LevelParent");
        foreach (GameObject lvlParent in levelParents)
            Destroy(lvlParent);

        DestroyEveryPiece();

        yield return new WaitForSeconds(1);
 
        // Generates another level
        generationCoroutine = GenerateLevel(random, false);
        StartCoroutine(generationCoroutine);
    }

    /// <summary>
    /// Checks if a piece intersects with any other piece.
    /// </summary>
    /// <param name="levelPiece">Piece to check.</param>
    /// <returns>Returns true if a piece intersects with any other piece.</returns>
    private bool PieceIntersection(LevelPiece levelPiece)
    {
        foreach (BoxCollider boxCollider in levelPiece.BoxColliders)
        {
            Collider[] roomCollider = 
                Physics.OverlapBox(levelPiece.transform.position + boxCollider.center, boxCollider.size / 2,
                levelPiece.transform.rotation, roomColliderLayer);

            if (roomCollider.Length > 1)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Rotates a piece until it matches a contact point. The logic applied is that rooms
    /// point outside, while corridors point inside. This way, the room/corridor will keep
    /// rotating until they match the desired direction, depending on the other piece type.
    /// </summary>
    /// <param name="levelPiece">Piece to rotate.</param>
    /// <param name="levelPieceContact">Contact point of the piece.</param>
    /// <param name="originContact">Contact point to match.</param>
    private void RotatePiece(LevelPiece levelPiece, ContactPoint levelPieceContact, 
        ContactPoint originContact)
    {
        levelPieceContact.ParentRoom.transform.rotation =
            Quaternion.LookRotation(originContact.transform.forward, originContact.transform.up);

        while (levelPieceContact.transform.SameDirectionAs(originContact.transform) == false)
        {
            levelPieceContact.ParentRoom.transform.rotation *=
            (levelPieceContact.transform.localRotation);
        }
    }

    /// <summary>
    /// Checks if a piece is valid. If it's not valid, it destroys it.
    /// </summary>
    /// <param name="levelPiece">Piece to check.</param>
    /// <param name="random">Instance of random.</param>
    /// <param name="creatingLevelBase">True if it's not creating 
    /// walls or boss room (meaning it's still creating the base structure).</param>
    /// <returns>Returns true if the piece is valid, else it returns false.</returns>
    private bool IsPieceValid(LevelPiece levelPiece, System.Random random, bool creatingLevelBase = true)
    {
        if (creatingLevelBase)
        {
            // Horizontal limit
            if (levelPiece.transform.position.z > forwardMaximumLevelSize ||
                levelPiece.transform.position.z < -forwardMaximumLevelSize)
            {
                Destroy(levelPiece.gameObject);
                return false;
            }

            if (levelPiece.transform.position.x > horizontalMaximumLevelSize || 
                levelPiece.transform.position.x < -horizontalMaximumLevelSize)
            {
                Destroy(levelPiece.gameObject);
                return false;
            }
        }
        if (PieceIntersection(levelPiece))
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
    private void RotateAndSetPiece(LevelPiece pieceToPlace, 
        ContactPoint contactPoint, ContactPoint openedContactPoint)
    {
        // Rotates piece to match contact point rotation
        RotatePiece(pieceToPlace, contactPoint, openedContactPoint);

        // Sets a piece in a contact point
        SetPiece(pieceToPlace, contactPoint, openedContactPoint);
    }

    /// <summary>
    /// Checks if the last piece is valid.
    /// If it's not valid it will destroy it, else it will add its contact points to a list.
    /// This method also deactivates pre activated walls that are on top of contact points.
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
            // Closes this piece contact point
            pieceContactPoint.Close();

            // Gets the wall on top of this open contact point and deactivates it
            if (openedContactPoints[index].transform.childCount > 0)
                openedContactPoints[index].transform.GetChild(0).gameObject.SetActive(false);

            // Gets the wall on top of current piece to place contact point and deactivates it
            if (pieceContactPoint.transform.childCount > 0)
                pieceContactPoint.transform.GetChild(0).gameObject.SetActive(false);

            // Closes point (gizmos to red)
            openedContactPoints[index].Close();
            // Sets this piece connected contact point to the point that it just connected
            pieceToPlace.ConnectedContactPoint = openedContactPoints[index];
            // Removes the contact point that the piece just connected to from opened contact points. 
            openedContactPoints.Remove(openedContactPoints[index]);
            // Sets transform parent (to be organized)
            pieceToPlace.transform.parent = levelParent.transform;

            // A variable to have control of all rooms (contains all spawned rooms)
            if (addToAllRooms)
                allRooms.Add(pieceToPlace);

            // A variable to have control of all rooms and corridors (contains all spawned rooms and corridors)
            allRoomsAndCorridors.Add(pieceToPlace);

            // Adds NEW contact points to open contact point list
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
    public void SaveCurrentData(RunSaveData saveData)
    {
        saveData.DungeonSavedData.Seed = seed;
        saveData.DungeonSavedData.HorizontalMaximumLevelSize = horizontalMaximumLevelSize;
        saveData.DungeonSavedData.ForwardMaximumLevelSize = forwardMaximumLevelSize;
        saveData.DungeonSavedData.MinimumNumberOfRooms = minimumNumberOfRooms;
        saveData.DungeonSavedData.MaximumNumberOfRooms = maximumNumberOfRooms;
        saveData.DungeonSavedData.Element = element;
    }

    /// <summary>
    /// Loads saved data to variables and starts generation with that data.
    /// </summary>
    /// <param name="saveData">Saved data.</param>
    /// <returns>Null.</returns>
    public IEnumerator LoadData(RunSaveData saveData)
    {
        yield return new WaitForSeconds(0.25f);

        if (this != null) // < DO NOT REMOVE
        {
            seed = saveData.DungeonSavedData.Seed;
            horizontalMaximumLevelSize = saveData.DungeonSavedData.HorizontalMaximumLevelSize;
            forwardMaximumLevelSize = saveData.DungeonSavedData.ForwardMaximumLevelSize;
            minimumNumberOfRooms = saveData.DungeonSavedData.MinimumNumberOfRooms;
            maximumNumberOfRooms = saveData.DungeonSavedData.MaximumNumberOfRooms;
            element = saveData.DungeonSavedData.Element;

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
