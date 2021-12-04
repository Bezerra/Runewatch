using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using ExtensionMethods;
using System.Threading.Tasks;

/// <summary>
/// Class responsible for generating a level.
/// </summary>
public class LevelGenerator : MonoBehaviour, ISaveable
{
    [Header("Generation Values")]
    [Tooltip("FixedUpdate will require more performance but it's faster")]
    [SerializeField] private YieldType                              yieldType = YieldType.FixedUpdate;
    [Tooltip("Occlues all room non first gen. pieces and spawns the player")]
    [SerializeField] private bool                                   occludeAndSpawnPlayer = true;
    [Tooltip("Uses a random seed on initial creation")]
    [SerializeField] private bool                                   randomSeed;
    [Tooltip("Seed used on initial creation")]
    [Range(-200000000, 200000000)] [SerializeField] private int     seed = 0;
    
    [Header("Generation Parameters")]
    [Tooltip("Sets all generation parameters to a random value (inside their values)")]
    [SerializeField] private bool                                   allRandomGenerationParameters;
    [Tooltip("Maximum horizontal size of the level")]
    [Range(75, 500)] [SerializeField] private int                   horizontalMaximumLevelSize;
    [Tooltip("Maximum forward size of the level")]
    [Range(75, 500)] [SerializeField] private int                   forwardMaximumLevelSize;
    [Tooltip("Sets a random minimum number of rooms for each generation(inside its limits)")]
    [SerializeField] private bool                                   randomMinimumNumberOfRooms;
    [Tooltip("Minimum number of rooms on current generation")]
    [Range(7, 9)] [SerializeField] private int                      minimumNumberOfRooms;
    [Tooltip("Sets a random maximum number of rooms for each generation(inside its limits)")]
    [SerializeField] private bool                                   randomMaximumNumberOfRooms;
    [Tooltip("Maximum number of rooms on current generation")]
    [Range(12, 15)] [SerializeField] private int                    maximumNumberOfRooms;

    [Header("Level Pieces")]
    [Tooltip("Empty room. Serves as base of creating to starting piece and originated piece of boss room")]
    [SerializeField] private LevelPiece     ghostRoomPiece;
    [Tooltip("Starting piece of generation")]
    [SerializeField] private LevelPiece[]   startingPieces;
    [Tooltip("Final piece of generation")]
    [SerializeField] private LevelPiece     bossRoom;
    [Tooltip("Corridors and stairs only")]
    [SerializeField] private LevelPiece[]   corridors;
    [Tooltip("Rooms only")]
    [SerializeField] private LevelPiece[]   rooms;
    [Tooltip("Layer to check if a room is colliding with another")]
    [SerializeField] private LayerMask      roomColliderLayer;

    // Rooms lists
    private IList<LevelPiece> allRooms;
    public IList<LevelPiece> AllGeneratedLevelPieces { get; private set; }

    // Generation variables
    private int numberOfLoops;
    private IEnumerator generateLevelCoroutine;
    private System.Random random;
    private YieldInstruction yi;

    [Header("Dungeon element")]
    [SerializeField] private ElementType element;
    public ElementType Element => element;

    // Starting time of a generation
    private float timeOfGenerationStart;
    private float timeOfTotalGeneration;

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
    public IEnumerator StartGeneration(bool loadedRandom = false)
    {
        // Adjust some values common to every generation (starting game generation or loading game generation)
        if (minimumNumberOfRooms > maximumNumberOfRooms) minimumNumberOfRooms = maximumNumberOfRooms;
        if (maximumNumberOfRooms < minimumNumberOfRooms) maximumNumberOfRooms = minimumNumberOfRooms;

        // Generation
        if (loadedRandom == false)
        {
            // This random can be from a user's seed or totally random
            generateLevelCoroutine = GenerateLevel(random);
        }
        else
        {
            // This random will always be from a user's seed (loading game = already existing level)
            generateLevelCoroutine = GenerateLevel(new System.Random(seed));
        }

        yield return generateLevelCoroutine;
    }

    /// <summary>
    /// Generates a level. Creates rooms and corridors with contact points in order to connect all level pieces.
    /// </summary>
    /// <param name="random">Instance of Random.</param>
    /// <param name="firstAttempt">Parameter that defines if this is the firsts attempt creating the level.</param>
    /// <returns>Null.</returns>
    private IEnumerator GenerateLevel(System.Random random, bool firstAttempt = true)
    {
        timeOfTotalGeneration = Time.time;
        
        switch(yieldType)
        {
            case YieldType.FixedUpdate:
                yi = new WaitForFixedUpdate();
                break;
            case YieldType.Update:
                yi = null;
                break;
            case YieldType.WaitForFrame:
                yi = new WaitForEndOfFrame();
                break;
            case YieldType.WaitForSeconds:
                yi = new WaitForSeconds(0.15f);
                break;
        }

        bool bossRoomSpawned = false;
        IList<ContactPoint> openedContactPoints;

        // This number is the quantity of loops for each opened poinst loop. The higher the
        // number, the more attempts the algorithm will take to try and find a piece
        // for each opened contact point.
        // This number is directly connect with the limit of rooms that can be spawned,
        // so if the number of rooms exceeds a limit, it will adjust numberOfLoops variable.
        // For the number of rooms in our game, a number between 4 - 7 should be fine.
        numberOfLoops = 5;
        //5

        while (bossRoomSpawned == false)
        {
            timeOfGenerationStart = Time.time;

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

            // Creates and places first corridor
            LevelPiece ghostRoom = Instantiate(ghostRoomPiece);
            ghostRoom.transform.parent = levelParent.transform;

            LevelPiece startingRoomPiece = Instantiate(startingPieces[
                random.Next(0, startingPieces.Length)], Vector3.zero, Quaternion.identity);
            ContactPoint startingRoomContactPoint = 
                startingRoomPiece.ContactPoints[random.Next(0, startingRoomPiece.ContactPoints.Length)];
            startingRoomPiece.transform.parent = levelParent.transform;
            openedContactPoints.Add(startingRoomContactPoint);
            startingRoomPiece.ContactPointOfCreation = ghostRoom.ContactPoints[0];
            ////////////////////////////////////////////

            // Creates first corridor
            LevelPiece initialCorridor = Instantiate(corridors[random.Next(0, corridors.Length)]);
            ContactPoint initialCorridorContactPoint = 
                initialCorridor.ContactPoints[random.Next(0, initialCorridor.ContactPoints.Length)];
            RotateAndSetPiece(initialCorridor, initialCorridorContactPoint, startingRoomContactPoint);
            int indexOfOpenContacts = 0;
            ValidatePiece(initialCorridor, initialCorridorContactPoint, false, 
                openedContactPoints, ref indexOfOpenContacts, levelParent, random);
            #endregion

            // Level creation loop
            byte numberOfCurrentLoop = 0;

            // Creates a list of room weights 
            IList<int> roomWeights = new List<int>();
            for (int z = 0; z < rooms.Length; z++)
            {
                roomWeights.Add(rooms[z].RoomWeight);
            }

            // Main generation loop.
            // Spawns rooms and corridors and tries to connect them while there are opened contact points
            do
            {
                Debug.ClearDeveloperConsole();
                int openedContacts = openedContactPoints.Count;

                // For all opened contact points on this loop only < will ignore the ones added inside for now
                for (int i = 0; i < openedContacts; i++)
                {
                    // Creates a common levelPiece and desired contactPoint to connect
                    LevelPiece pieceToPlace = null;
                    ContactPoint pieceContactPoint = null;

                    // If this point is a room
                    if (openedContactPoints[i].ParentRoom.Type == PieceType.Room)
                    {
                        // For all possible corridors in a random order
                        corridors.Shuffle(random);
                        int loopIndex = 0;

                        // While this loop doesn't run all corridors, it will keep looping
                        // until it finds a corridor that's able to fit
                        do
                        {
                            pieceToPlace = null;

                            // For all corridors in that random order
                            for (int j = 0; j < corridors.Length; j++)
                            {
                                // Skips incompatible pieces
                                if (openedContactPoints[i].IncompatiblePieces.Contains(
                                    corridors[j].ConcreteType))
                                {
                                    loopIndex++;
                                    continue;
                                }

                                // Creates new piece
                                pieceToPlace = Instantiate(corridors[j]);
                                pieceContactPoint = pieceToPlace.ContactPoints[
                                    random.Next(0, pieceToPlace.ContactPoints.Length)];

                                // Sets it
                                yield return RotateAndSetPiece(pieceToPlace, pieceContactPoint, openedContactPoints[i]);

                                break;
                            }

                            loopIndex++;

                            // If loop reaches corridor's max size, it will break this loop
                            if (loopIndex == corridors.Length)
                                break;

                            if (pieceToPlace == null)
                                break;

                            // Else
                            // If that piece is not valid, it will try another piece
                        } while (IsPieceValid(pieceToPlace, openedContactPoints[i], random) == false);

                        // If it didn't find any piece for this contact point, skips the point
                        if (pieceToPlace == null)
                            continue;

                        yield return yi;

                        // Open/Close/Remove/Add points logic happens here
                        ValidatePiece(pieceToPlace, pieceContactPoint, false,
                            openedContactPoints, ref i, levelParent, random);

                        continue;
                    }

                    // This should be and else if, but if it's changed to else if, it's bugged.....
                    // If the point is a corridor
                    else if (openedContactPoints[i].ParentRoom.Type == PieceType.Corridor)
                    {
                        // Creates rooms depending on their weight
                        pieceToPlace = Instantiate(rooms[random.RandomWeight(roomWeights)]);
                        pieceContactPoint = pieceToPlace.ContactPoints[
                            random.Next(0, pieceToPlace.ContactPoints.Length)];

                        // Skips incompatible pieces
                        if (openedContactPoints[i].IncompatiblePieces.Contains(
                            pieceToPlace.ConcreteType))
                        {
                            Destroy(pieceToPlace.gameObject);
                            continue;
                        }

                        yield return RotateAndSetPiece(
                            pieceToPlace, pieceContactPoint, openedContactPoints[i]);

                        ValidatePiece(
                            pieceToPlace, pieceContactPoint, true, openedContactPoints,
                            ref i, levelParent, random);
                    }
                }

                numberOfCurrentLoop++;

                // After every loop, if maximum number of rooms exceeds a limit, it breaks the current another
                if (allRooms.Count > maximumNumberOfRooms)
                {
                    break;
                }

                if (Time.time - timeOfGenerationStart > 8f)
                {
                    Debug.Log("Maximum generation time exceeded.");
                    break;
                }

            } while (openedContactPoints.Count > 0 && numberOfCurrentLoop < numberOfLoops);

            // After the main loop is over, if minimum or maximum number of
            // rooms exceeds a limit, it starts another loop
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

            #region Boss Room
                // Organized a list with contact points by distance
                openedContactPoints = 
                openedContactPoints.OrderByDescending(
                    i => Vector3.Distance(i.transform.position,
                                            startingRoomPiece.transform.position)).ToList();

            // Generate boss room
            LevelPiece bossRoomPiece = Instantiate(bossRoom);
            LevelPiece corridorToPlace = null;
            ContactPoint bossRoomContactPoint = 
                bossRoomPiece.ContactPoints[0];
            bossRoomPiece.transform.parent = levelParent.transform;
            bossRoomSpawned = false;

            // For all remaining points by distance (first point is the farthest one)
            for (int i = 0; i < openedContactPoints.Count; i++)
            {
                // If it's a corridor
                if (openedContactPoints[i].ParentRoom.Type == PieceType.Corridor)
                {
                    // Sets and rotates piece
                    yield return RotateAndSetPiece(bossRoomPiece, bossRoomContactPoint, openedContactPoints[i]);
                    yield return new WaitForSeconds(0.1f);

                    // If everything is valid, ends generation
                    if (IsPieceValid(bossRoomPiece, openedContactPoints[i], random, false))
                    {
                        print("Valid level generation.");

                        ValidatePiece(
                            bossRoomPiece, bossRoomContactPoint, true, 
                            openedContactPoints, ref i, levelParent, random);

                        bossRoomPiece.ContactPoints[0].OriginatedRoom = ghostRoom;

                        bossRoomSpawned = true;

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

                    // For all possible corridors in a random order
                    corridors.Shuffle(random);

                    // For all rooms
                    for (int j = 0; j < corridors.Length; j++)
                    {
                        // Creates a corridor/stairs
                        corridorToPlace = Instantiate(corridors[j]);
                        ContactPoint pieceContactPoint =
                            corridorToPlace.ContactPoints[random.Next(0, corridorToPlace.ContactPoints.Length)];

                        // Skips incompatible pieces
                        if (openedContactPoints[i].IncompatiblePieces.Contains(
                            corridorToPlace.ConcreteType))
                        {
                            Destroy(corridorToPlace.gameObject);
                            continue;
                        }

                        // Tries to set the room
                        yield return RotateAndSetPiece(corridorToPlace, pieceContactPoint, openedContactPoints[i]);

                        // If it's valid
                        if (IsPieceValid(corridorToPlace, openedContactPoints[i], random, false))
                        {
                            ValidatePiece(
                                corridorToPlace, pieceContactPoint, false, 
                                openedContactPoints, ref i, levelParent, random);

                            finalCorridorCreated = true;

                            break;
                        }
                        else
                        {
                            Destroy(corridorToPlace.gameObject);
                            continue;
                        }
                    }

                    // Sets and rotates piece
                    if (finalCorridorCreated)
                    {
                        // From now on, it will be used the last index on opened contact points
                        // because this was the point added on the previous corridor
                        yield return RotateAndSetPiece(bossRoomPiece, bossRoomContactPoint, 
                            openedContactPoints[openedContactPoints.Count - 1]);

                        yield return new WaitForSeconds(0.25f);

                        // If everything is valid, ends generation
                        if (IsPieceValid(bossRoomPiece, 
                            openedContactPoints[openedContactPoints.Count - 1], random, false))
                        {
                            print("Valid level generation.");

                            int indexOfContactPoint = openedContactPoints.Count - 1;
                            ValidatePiece(
                                bossRoomPiece, bossRoomContactPoint, true, 
                                openedContactPoints, ref indexOfContactPoint, levelParent, random);

                            bossRoomPiece.ContactPoints[0].OriginatedRoom = ghostRoom;

                            bossRoomSpawned = true;

                            break;
                        }

                        // Fails boss generation
                        print("Invalid level generation. Attempting another time.");

                        if (bossRoomPiece != null) Destroy(bossRoomPiece);

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
                generateLevelCoroutine = FinalAdjustements(levelParent, openedContactPoints, yi);
                yield return generateLevelCoroutine;
            }
            else
            {
                if (bossRoomPiece != null) Destroy(bossRoomPiece);
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
    /// <param name="wfef">Wait for fixed update.</param>
    /// <returns>Null.</returns>
    private IEnumerator FinalAdjustements(GameObject levelParent, IList<ContactPoint> openedContactPoints,
        YieldInstruction yi)
    {
        print("Generating walls on exits...");
        // Close every opened exit with a wall
        while (openedContactPoints.Count > 0)
        {
            for (int i = openedContactPoints.Count - 1; i >= 0; i--)
            {
                yield return yi;

                if (openedContactPoints[i].ParentRoom.Type == PieceType.Corridor)
                {
                    // Gets the connectected contact point of this piece and
                    // activates its wall
                    openedContactPoints[i].ParentRoom.ContactPointOfCreation.
                        GetComponentInChildren<ContactPointWall>(true).gameObject.SetActive(true);

                    // Destroys the door that was on top of this point (only the wall will remain)
                    Destroy(openedContactPoints[i].ParentRoom.ContactPointOfCreation.
                        GetComponentInChildren<ContactPointDoor>(true).gameObject);

                    // Destroys that corridor/stairs and removes its contact point from the list
                    Destroy(openedContactPoints[i].ParentRoom.gameObject);
                    openedContactPoints.Remove(openedContactPoints[i]);
                }
                else if (openedContactPoints[i].ParentRoom.Type == PieceType.Room)
                {
                    // Destroys the door that was on top of this point (only the wall will remain)
                    Destroy(openedContactPoints[i].
                        GetComponentInChildren<ContactPointDoor>(true).gameObject);

                    openedContactPoints[i].Close();
                    openedContactPoints.Remove(openedContactPoints[i]);
                }
            }
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
                Destroy(lvlParent);
            }
        }

        // Deactivates RoomCollision collider
        AllGeneratedLevelPieces = FindObjectsOfType<LevelPiece>();
        foreach (LevelPiece piece in AllGeneratedLevelPieces)
        {
            if (piece.BoxCollidersParent != null)
            {
                piece.BoxCollidersParent.SetActive(false);
            }
        }

        Debug.Log("Took " + (Time.time - timeOfTotalGeneration) + " seconds to generate, with seed " + seed +
            " and number of loops of " + numberOfLoops);

        if (occludeAndSpawnPlayer)
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

        if (generateLevelCoroutine != null) StopCoroutine(generateLevelCoroutine);
        print(message);

        yield return new WaitForSeconds(0.1f);

        // Destroys every piece previously generated
        GameObject[] levelParents = GameObject.FindGameObjectsWithTag("LevelParent");
        foreach (GameObject lvlParent in levelParents)
            Destroy(lvlParent);

        DestroyEveryPiece();

        yield return new WaitForSeconds(1);
 
        // Generates another level
        generateLevelCoroutine = GenerateLevel(random, false);
        StartCoroutine(generateLevelCoroutine);
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
                Physics.OverlapBox(
                    levelPiece.transform.position + 
                        levelPiece.transform.GetChild(0).localPosition +
                        boxCollider.gameObject.transform.parent.localPosition +
                        boxCollider.gameObject.transform.localPosition + 
                        boxCollider.center, 
                    boxCollider.size / 2,
                    levelPiece.transform.rotation *
                        boxCollider.gameObject.transform.parent.localRotation *
                        boxCollider.gameObject.transform.localRotation, 
                    roomColliderLayer);

            if (roomCollider.Length > 1)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Checks if a piece is valid. If it's not valid, it destroys it.
    /// </summary>
    /// <param name="levelPiece">Piece to check.</param>
    /// <param name="contactPointToSetPiece">Contact point to set piece.</param>
    /// <param name="random">Instance of random.</param>
    /// <param name="creatingLevelBase">True if it's not creating 
    /// walls or boss room (meaning it's still creating the base structure).</param>
    /// <returns>Returns true if the piece is valid, else it returns false.</returns>
    private bool IsPieceValid(LevelPiece levelPiece, ContactPoint contactPointToSetPiece, 
        System.Random random, bool creatingLevelBase = true)
    {
        if (creatingLevelBase)
        {
            // Horizontal limit
            if (levelPiece != null)
            {
                if (levelPiece.transform.position.z > forwardMaximumLevelSize ||
                levelPiece.transform.position.z < -forwardMaximumLevelSize)
                {
                    Destroy(levelPiece.gameObject);
                    return false;
                }

                // Forward limit
                if (levelPiece.transform.position.x > horizontalMaximumLevelSize ||
                    levelPiece.transform.position.x < -horizontalMaximumLevelSize)
                {
                    Destroy(levelPiece.gameObject);
                    return false;
                }
            }
        }

        if (PieceIntersection(levelPiece))
        {
            if (creatingLevelBase)
            {
                contactPointToSetPiece.IncompatiblePieces.Add(levelPiece.ConcreteType);
                Destroy(levelPiece.gameObject);
            } 
            return false;
        }

        return true;
    }

    /// <summary>
    /// Rotates a piece until it matches a contact point. The logic applied is that rooms
    /// point outside, while corridors point inside. This way, the room/corridor will keep
    /// rotating until they match the desired direction, depending on the other piece type.
    /// </summary>
    /// <param name="levelPieceContact">Contact point of the piece.</param>
    /// <param name="connectionContactPoint">Contact point to match.</param>
    private IEnumerator RotatePiece(ContactPoint levelPieceContact,
        ContactPoint connectionContactPoint)
    {
        // Rotatesa piece towards a contact point
        levelPieceContact.ParentRoom.transform.rotation =
            Quaternion.LookRotation(connectionContactPoint.transform.forward, 
            connectionContactPoint.transform.up);

        //// While both points don't have the same direction
        while (levelPieceContact.transform.SameDirectionAs(
                connectionContactPoint.transform) == false)
        {
            // It will keep rotating parent room
            levelPieceContact.ParentRoom.transform.rotation *= 
                levelPieceContact.transform.localRotation;
        }
        yield return yi;
    }

    /// <summary>
    /// Places a piece in a contact point.
    /// </summary>
    /// <param name="levelPiece">Piece to place.</param>
    /// <param name="levelPieceContact">Contact point of the piece.</param>
    /// <param name="connectionContactPoint">Contact point to place the piece.</param>
    private void SetPiece(LevelPiece levelPiece, ContactPoint levelPieceContact, 
        ContactPoint connectionContactPoint)
    {
        levelPiece.transform.position = Vector3.zero;

        Vector3 dir = 
            levelPieceContact.transform.Direction(connectionContactPoint.transform);

        float dist = Vector3.Distance(
            levelPieceContact.transform.position, connectionContactPoint.transform.position);

        levelPiece.transform.position += dir * dist;
    }

    /// <summary>
    /// Rotates and sets a piece in the level.
    /// </summary>
    /// <param name="pieceToPlace">Piece to place.</param>
    /// <param name="contactPoint">Piece contact point.</param>
    /// <param name="openedContactPoint">Opened contact point to place the piece.</param>
    /// <param name="random">Instance of Random.</param>
    private IEnumerator RotateAndSetPiece(LevelPiece pieceToPlace, 
        ContactPoint contactPoint, ContactPoint openedContactPoint)
    {
        // Rotates piece to match contact point rotation
        yield return RotatePiece(contactPoint, openedContactPoint);

        // Sets a piece in a contact point
        SetPiece(pieceToPlace, contactPoint, openedContactPoint);
    }

    /// <summary>
    /// Checks if the last piece is valid.
    /// If it's valid it will add its contact points to a list.
    /// This method also deactivates pre activated walls that are on top of contact points.
    /// <param name="pieceToPlace">Piece to place.</param>
    /// <param name="pieceContactPoint">Piece contact point.</param>
    /// <param name="isRoomPiece">Bool that determines if this piece should be add to all rooms list.</param>
    /// <param name="openedContactPoints">List with all opened contact points.</param>
    /// <param name="index">Index of opened contact points list.</param>
    /// <param name="levelParent">Gameobject parent of all level pieces.</param>
    /// <param name="random">Instance of Random.</param>
    private void ValidatePiece(LevelPiece pieceToPlace, ContactPoint pieceContactPoint, bool isRoomPiece,
        IList<ContactPoint> openedContactPoints, ref int index, GameObject levelParent, System.Random random)
    {
        // Checks if the last piece is valid
        // If it's not valid it will destroy it, else it will add its contact points to a list
        if (IsPieceValid(pieceToPlace, openedContactPoints[index], random))
        {
            // Closes this piece contact point
            pieceContactPoint.Close();

            // For rooms only
            if (isRoomPiece)
            {
                // A variable to have control of all rooms (contains all spawned rooms)
                allRooms.Add(pieceToPlace);

                // Gets the wall on top of current piece to place contact point and deactivates it
                // Only needs this for rooms, because corridors will always destroy walls
                pieceContactPoint.GetComponentInChildren<ContactPointWall>().
                    gameObject.SetActive(false);
            }
            // For corridors only
            else
            {
                // If it's a corridor, destroys its walls and doors on contact points (they're not needed)

                // Gets the wall on top of this open contact point and deactivates it
                // Sets active to false because the contact point if it's a room, it can set it to true later
                openedContactPoints[index].GetComponentInChildren<ContactPointWall>(true).
                    gameObject.SetActive(false);

                foreach (ContactPoint cp in pieceToPlace.ContactPoints)
                {
                    // Destroys the wall that's on top of this point
                    Destroy(cp.GetComponentInChildren<ContactPointWall>(true).gameObject);

                    // Destroys the door that's on top of this point
                    Destroy(cp.GetComponentInChildren<ContactPointDoor>(true).gameObject);
                }
            }

            // Closes point (gizmos to red)
            openedContactPoints[index].Close();
            // Sets this piece connected contact point to the point that it just connected
            pieceToPlace.ContactPointOfCreation = openedContactPoints[index];
            // Sets the opened contact point originated room as this piece
            openedContactPoints[index].OriginatedRoom = pieceToPlace;
            // Adds this piece to the connected point parent room connected pieces
            openedContactPoints[index].ParentRoom.ConnectedPieces.Add(pieceToPlace);
            // Adds the connected point parent room to this piece connected pieces
            pieceToPlace.ConnectedPieces.Add(openedContactPoints[index].ParentRoom);
            // Removes the contact point that the piece just connected to from opened contact points. 
            openedContactPoints.Remove(openedContactPoints[index]);
            // Pushes the currrent index one time back (since it removed it previously)
            index--;
            // Sets transform parent (to be organized)
            pieceToPlace.transform.parent = levelParent.transform;   

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

            yield return StartGeneration(true);
        }
        yield return null;
    }

    protected virtual void OnEndedGeneration() => EndedGeneration?.Invoke();
    public event Action EndedGeneration;

    private enum YieldType { FixedUpdate, Update, WaitForFrame, WaitForSeconds }
}
