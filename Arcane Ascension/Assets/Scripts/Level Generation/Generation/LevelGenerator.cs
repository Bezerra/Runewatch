using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using ExtensionMethods;

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
    [Range(2, 30)] [SerializeField] private int                      minimumNumberOfRooms;
    [Tooltip("Sets a random maximum number of rooms for each generation(inside its limits)")]
    [SerializeField] private bool                                   randomMaximumNumberOfRooms;
    [Tooltip("Maximum number of rooms on current generation")]
    [Range(5, 75)] [SerializeField] private int                     maximumNumberOfRooms;

    [Header("Level Pieces")]
    [Tooltip("Empty room. Serves as base of creating to starting piece and originated piece of boss room")]
    [SerializeField] private LevelPiece     ghostRoomPiece;
    [Tooltip("Starting piece of generation")]
    [SerializeField] private LevelPiece[]   startingPieces;
    [Tooltip("Final piece of generation")]
    [SerializeField] private LevelPiece     bossRoom;
    [Tooltip("Corridors and stairs only")]
    [SerializeField] private LevelPiece[]   corridors;
    [Header("Rooms with 2+ contact points")]
    [SerializeField] private LevelPiece[]   firstRoomsToSpawn;
    [Tooltip("Rooms only")]
    [SerializeField] private LevelPiece[]   rooms;

    // Rooms lists
    private IList<LevelPiece> allRooms;
    public IList<LevelPiece> AllLevelPiecesGenerated { get; private set; }

    // Generation variables
    private IEnumerator generateLevelCoroutine;
    private System.Random random;
    private YieldInstruction yi;
    private bool pieceIntersected;
    private bool pieceIsValid;

    [Header("Dungeon element")]
    [SerializeField] private ElementType element;
    public ElementType Element => element;
    [SerializeField] private GameObject elementSettings;

    // Starting time of a generation
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
        timeOfTotalGeneration = Time.realtimeSinceStartup;

        // Probability that will determine the number of rooms to break the loop
        float roomProbablity = maximumNumberOfRooms - minimumNumberOfRooms == 0 ? 0 :
            100 / (maximumNumberOfRooms - minimumNumberOfRooms);

        switch (yieldType)
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
            levelParent.transform.parent = this.transform;

            #region Initial Generation
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
                openedContactPoints, ref indexOfOpenContacts, levelParent);

            // Creates a room with more than 2 contact points as first room after first corridor
            // Also prevents a bug that rarely happens when the first room has 2 contact points only
            bool firstRoomCreated = false;
            do
            {
                // Creates rooms depending on their weight
                LevelPiece pieceToPlace = Instantiate(
                    firstRoomsToSpawn[random.Next(0, firstRoomsToSpawn.Length)]);
                ContactPoint pieceContactPoint = pieceToPlace.ContactPoints[
                    random.Next(0, pieceToPlace.ContactPoints.Length)];

                RotateAndSetPiece(
                    pieceToPlace, pieceContactPoint, openedContactPoints[0]);

                yield return IsPieceValid(pieceToPlace, openedContactPoints[0]);

                if (pieceIsValid)
                {
                    int tempIndex = 0;
                    ValidatePiece(pieceToPlace, pieceContactPoint, true,
                        openedContactPoints, ref tempIndex, levelParent);
                    pieceIsValid = false;
                    firstRoomCreated = true;
                }

            } while (firstRoomCreated == false);
            #endregion

            // Creates a list of room weights 
            IList<int> roomWeights = new List<int>();
            for (int z = 0; z < rooms.Length; z++)
            {
                roomWeights.Add(rooms[z].RoomWeight);
            }

            // Main generation loop.
            // Spawns rooms and corridors and tries to connect them while there are opened contact points
            bool forcedBreakLoop;
            do
            {
                pieceIntersected = false;
                pieceIsValid = false;
                forcedBreakLoop = false;

                // For all opened contact points on this loop only < will ignore the ones added inside for now
                for (int i = 0; i < openedContactPoints.Count; i++)
                {
                    // Safety measure to prevent nulls
                    if (i >= openedContactPoints.Count)
                    {
                        forcedBreakLoop = true;
                        break;
                    }
      
                    // If maximum number of rooms exceeds a limit, it breaks the current loop
                    if (allRooms.Count >= maximumNumberOfRooms)
                    {
                        forcedBreakLoop = true;
                        break;
                    }

                    // If minimum number of rooms, checks a probability of breaking the current loop
                    if (allRooms.Count >= minimumNumberOfRooms)
                    {
                        // Probabilty of breaking the loop in this current iteration
                        if (roomProbablity.PercentageCheck(random))
                        {
                            forcedBreakLoop = true;
                            break;
                        }
                    }

                    // Creates a common levelPiece and desired contactPoint to connect
                    LevelPiece pieceToPlace = null;
                    ContactPoint pieceContactPoint = null;
          
                    // If this point is a room
                    if (openedContactPoints[i].ParentRoom.Type == PieceType.Room)
                    {
                        pieceToPlace = Instantiate(corridors[random.Next(0, corridors.Length)]);
                        pieceContactPoint = pieceToPlace.ContactPoints[
                            random.Next(0, pieceToPlace.ContactPoints.Length)];
             
                        // Skips incompatible pieces
                        if (openedContactPoints[i].IncompatiblePieces.Contains(
                            pieceToPlace.ConcreteType))
                        {
                            if (openedContactPoints[i].IncompatiblePieces.Count ==
                                corridors.Length)
                            {
                                forcedBreakLoop = true;
                                break;
                            }

                            Destroy(pieceToPlace.gameObject);
                            continue;
                        }
    
                        // Sets it
                        RotateAndSetPiece(pieceToPlace, pieceContactPoint, openedContactPoints[i]);

                        yield return IsPieceValid(pieceToPlace, openedContactPoints[i]);

                        // Open/Close/Remove/Add points logic happens here
                        if (pieceIsValid)
                        {
                            ValidatePiece(pieceToPlace, pieceContactPoint, false,
                                openedContactPoints, ref i, levelParent);
                            pieceIsValid = false;
                        }
                    }

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
                            if (openedContactPoints[i].IncompatiblePieces.Count ==
                                rooms.Length)
                            {
                                forcedBreakLoop = true;
                                break;
                            }
                            
                            Destroy(pieceToPlace.gameObject);
                            continue;
                        }

                        RotateAndSetPiece(
                            pieceToPlace, pieceContactPoint, openedContactPoints[i]);

                        yield return IsPieceValid(pieceToPlace, openedContactPoints[i]);

                        if (pieceIsValid)
                        {
                            ValidatePiece(
                                pieceToPlace, pieceContactPoint, true, openedContactPoints,
                                ref i, levelParent);
                            pieceIsValid = false;
                        }
                    }
                }

            } while (openedContactPoints.Count > 0 && forcedBreakLoop == false);

            // If number of rooms is lower than the limit, continues loop, this will never happen 99.99%
            if (allRooms.Count < minimumNumberOfRooms)
                continue;

            #region Boss Room
            // Organized a list with contact points by distance
            openedContactPoints = 
                openedContactPoints.OrderByDescending(
                    i => Vector3.Distance(i.transform.position,
                                            startingRoomPiece.transform.position)).ToList();

            // Generate boss room
            LevelPiece bossRoomPiece;
            ContactPoint bossRoomContactPoint;
            LevelPiece corridorToPlace = null;
            bossRoomSpawned = false;

            // For all remaining points by distance (first point is the farthest one)
            for (int i = 0; i < openedContactPoints.Count; i++)
            {
                // If it's a corridor
                if (openedContactPoints[i].ParentRoom.Type == PieceType.Corridor)
                {
                    bossRoomPiece = Instantiate(bossRoom);
                    bossRoomContactPoint = bossRoomPiece.ContactPoints[0];
                    bossRoomPiece.transform.parent = levelParent.transform;

                    // Sets and rotates piece
                    RotateAndSetPiece(bossRoomPiece, bossRoomContactPoint, openedContactPoints[i]);

                    // If everything is valid, ends generation
                    yield return IsPieceValid(bossRoomPiece, openedContactPoints[i], true);
                    if (pieceIsValid)
                    {
                        print("Valid level generation.");

                        ValidatePiece(
                            bossRoomPiece, bossRoomContactPoint, true, 
                            openedContactPoints, ref i, levelParent);

                        // Because this variable will be used later and boss room doesn't count
                        allRooms.Remove(bossRoomPiece);

                        bossRoomPiece.ContactPoints[0].OriginatedRoom = ghostRoom;

                        bossRoomSpawned = true;

                        pieceIsValid = false;

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

                    // For all corridors
                    for (int j = 0; j < corridors.Length; j++)
                    {
                        // Creates a corridor/stairs
                        corridorToPlace = Instantiate(corridors[j]);
                        ContactPoint pieceContactPoint =
                            corridorToPlace.ContactPoints[random.Next(
                                0, corridorToPlace.ContactPoints.Length)];

                        // Skips incompatible pieces
                        if (openedContactPoints[i].IncompatiblePieces.Contains(
                            corridorToPlace.ConcreteType))
                        {
                            if (openedContactPoints[i].IncompatiblePieces.Count ==
                                corridors.Length)
                            {
                                Destroy(corridorToPlace.gameObject);
                                break;
                            }

                            Destroy(corridorToPlace.gameObject);
                            continue;
                        }

                        // Tries to set the room
                        RotateAndSetPiece(corridorToPlace, pieceContactPoint, openedContactPoints[i]);

                        // If it's valid
                        yield return IsPieceValid(corridorToPlace, openedContactPoints[i], false);
                        if (pieceIsValid)
                        {
                            ValidatePiece(
                                corridorToPlace, pieceContactPoint, false, 
                                openedContactPoints, ref i, levelParent);

                            finalCorridorCreated = true;

                            pieceIsValid = false;

                            break;
                        }
                        else
                        {
                            // Else it tries the next corridor
                            Destroy(corridorToPlace.gameObject);
                            continue;
                        }
                    }

                    // Sets and rotates piece
                    if (finalCorridorCreated)
                    {
                        bossRoomPiece = Instantiate(bossRoom);
                        bossRoomContactPoint = bossRoomPiece.ContactPoints[0];
                        bossRoomPiece.transform.parent = levelParent.transform;

                        // From now on, it will be used the last index on opened contact points
                        // because this was the point added on the previous corridor
                        RotateAndSetPiece(bossRoomPiece, bossRoomContactPoint, 
                            openedContactPoints[openedContactPoints.Count - 1]);

                        // If everything is valid, ends generation
                        yield return IsPieceValid(bossRoomPiece, 
                            openedContactPoints[openedContactPoints.Count - 1], true);
                        if (pieceIsValid)
                        {
                            print("Valid level generation.");

                            int indexOfContactPoint = openedContactPoints.Count - 1;
                            ValidatePiece(
                                bossRoomPiece, bossRoomContactPoint, true, 
                                openedContactPoints, ref indexOfContactPoint, levelParent);

                            // Because this variable will be used later and boss room doesn't count
                            allRooms.Remove(bossRoomPiece);

                            bossRoomPiece.ContactPoints[0].OriginatedRoom = ghostRoom;

                            bossRoomSpawned = true;

                            pieceIsValid = false;

                            break;
                        }

                        // Fails boss generation
                        print("Invalid level generation. Attempting another time.");

                        if (bossRoomPiece != null) Destroy(bossRoomPiece);

                        yield return yi;
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
        yield return yi;

        print("Generating walls on exits...");
        // Close every opened exit with a wall
        while (openedContactPoints.Count > 0)
        {
            for (int i = openedContactPoints.Count - 1; i >= 0; i--)
            {
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

        // In the end, checks collisions again as a safety measure
        for (int i = 0; i < allRooms.Count; i++)
        {
            pieceIntersected = false;
            PieceIntersectionCheck[] pieceIntersections = 
                allRooms[i].GetComponentsInChildren<PieceIntersectionCheck>();
            for (int j = 0; j < pieceIntersections.Length; j++)
            {
                pieceIntersections[j].CollisionEvent += CheckIntersection;
                pieceIntersections[j].CanCheckCollision = true;
            }

            // Needs to wait a fixed update, so the piece can check on trigger stay
            yield return yi;

            for (int j = 0; j < pieceIntersections.Length; j++)
            {
                pieceIntersections[j].CollisionEvent -= CheckIntersection;
            }

            if (pieceIntersected)
            {
                break;
            }
        }

        // If there was an intersection after all generation
        // This is a really safety precaution, but it will never happen 99.99%
        if (pieceIntersected)
        {
            Debug.Log("Detected collision after generation. Generating another level.");
            SceneManager.LoadScene("MainMenu");
            // Sucks but it's better than the game getting stuck
        }
        // Else
        else
        {
            // Destroys procedural room colliders
            for (int i = 0; i < allRooms.Count; i++)
            {
                Destroy(allRooms[i].BoxCollidersParent);
            }

            // Finds all pieces generated
            AllLevelPiecesGenerated = FindObjectsOfType<LevelPiece>();

            // Sets random pieces shopkeepers as spawnable
            allRooms.Shuffle(random);
            allRooms[0].GetComponent<LevelPieceGameProgressControlNormalRoom>().RoomSpawnsShopkeeper = true;
            allRooms[1].GetComponent<LevelPieceGameProgressControlNormalRoom>().RoomSpawnsShopkeeper = true;

            // Sets random chests to spawn
            allRooms.Shuffle(random);
            LevelPieceGameProgressControlNormalRoom randRoom1 = allRooms[0].GetComponent<LevelPieceGameProgressControlNormalRoom>();
            LevelPieceGameProgressControlNormalRoom randRoom2 = allRooms[1].GetComponent<LevelPieceGameProgressControlNormalRoom>();
            randRoom1.RoomSpawnsChest = true;
            randRoom1.AbilityType = random.Next(0, 2) == 1 ? AbilityType.Spell : AbilityType.Passive;
            randRoom1.SpawnChestAfterGeneration();
            randRoom2.RoomSpawnsChest = true;
            randRoom2.AbilityType = random.Next(0, 2) == 1 ? AbilityType.Spell : AbilityType.Passive;
            randRoom2.SpawnChestAfterGeneration();

            GameObject thisElementSettings = Instantiate(elementSettings);
            thisElementSettings.transform.parent = GameObject.FindGameObjectWithTag("LevelParent").transform;

            if (occludeAndSpawnPlayer)
                OnEndedGeneration();

            Debug.Log("Took " + (Time.realtimeSinceStartup - timeOfTotalGeneration) + 
                " seconds to generate, with seed " + seed);
        }
    }

    /// <summary>
    /// Checks if a piece intersects with any other piece.
    /// This coroutine will make sure the spawned piece, will check for collisions
    /// with OnTriggerStay for a fixed update frame. If the piece detects a collision
    /// it will set a variable to true.
    /// </summary>
    /// <param name="levelPiece">Piece to check.</param>
    /// <returns>Returns true if a piece intersects with any other piece.</returns>
    private IEnumerator PieceIntersection(LevelPiece levelPiece)
    {
        PieceIntersectionCheck[] pieceIntersections = levelPiece.GetComponentsInChildren<PieceIntersectionCheck>();
        for (int i = 0; i < pieceIntersections.Length; i++)
        {
            pieceIntersections[i].CollisionEvent += CheckIntersection;
            pieceIntersections[i].CanCheckCollision = true;
        }
        
        // Needs to wait for fixed updates, so the piece can check on trigger stay
        yield return yi;

        for (int i = 0; i < pieceIntersections.Length; i++)
        {
            pieceIntersections[i].CollisionEvent -= CheckIntersection;
            pieceIntersections[i].CanCheckCollision = false;
        }
    }

    /// <summary>
    /// Checks if a piece intersected.
    /// </summary>
    /// <param name="didItIntersect">Bool to check if piece intersected.</param>
    public void CheckIntersection(bool didItIntersect) => pieceIntersected = didItIntersect;

    /// <summary>
    /// Checks if a piece is valid. If it's not valid, it destroys it.
    /// </summary>
    /// <param name="levelPiece">Piece to check.</param>
    /// <param name="contactPointToSetPiece">Contact point to set piece.</param>
    /// <param name="creatingLevelBase">True if it's not creating 
    /// walls or boss room (meaning it's still creating the base structure).</param>
    /// <returns>Returns true if the piece is valid, else it returns false.</returns>
    private IEnumerator IsPieceValid(LevelPiece levelPiece, ContactPoint contactPointToSetPiece,
        bool creatingLevelBase = true)
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
                }

                // Forward limit
                if (levelPiece.transform.position.x > horizontalMaximumLevelSize ||
                    levelPiece.transform.position.x < -horizontalMaximumLevelSize)
                {
                    Destroy(levelPiece.gameObject);
                }
            }
        }
    
        // Checks if piece is intersecting 
        yield return PieceIntersection(levelPiece);

        if (pieceIntersected)
        {
            if (creatingLevelBase)
            {
                pieceIsValid = false;
                contactPointToSetPiece.IncompatiblePieces.Add(levelPiece.ConcreteType);
                Destroy(levelPiece.gameObject);
            }
            pieceIntersected = false;
        }
        else
        {
            pieceIsValid = true;
        }
    }

    /// <summary>
    /// Rotates a piece until it matches a contact point. The logic applied is that rooms
    /// point outside, while corridors point inside. This way, the room/corridor will keep
    /// rotating until they match the desired direction, depending on the other piece type.
    /// </summary>
    /// <param name="levelPieceContact">Contact point of the piece.</param>
    /// <param name="connectionContactPoint">Contact point to match.</param>
    private void RotatePiece(ContactPoint levelPieceContact,
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
    private void RotateAndSetPiece(LevelPiece pieceToPlace, 
        ContactPoint contactPoint, ContactPoint openedContactPoint)
    {
        // Rotates piece to match contact point rotation
        RotatePiece(contactPoint, openedContactPoint);

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
    private void ValidatePiece(LevelPiece pieceToPlace, ContactPoint pieceContactPoint, bool isRoomPiece,
        IList<ContactPoint> openedContactPoints, ref int index, GameObject levelParent)
    {
        try
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
                if (pieceContactPoint != null)
                {
                    pieceContactPoint.GetComponentInChildren<ContactPointWall>(true).
                        gameObject.SetActive(false);
                }
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
        catch
        {
            // This will never happen 99.9999% sure, but just as a safety measure
            // It's better than getting the game stuck
            Debug.LogError("Missing reference. Generating another level as a safety measure.");

            // This scene load will depend if it's a new game or loading game
            // Implement in the future
            SceneManager.LoadScene("LoadingScreenToNewGame");
            ////////////////////////////
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
        LevelPiece[] levelPieces = FindObjectsOfType<LevelPiece>();
        foreach (LevelPiece levelPiece in levelPieces)
        {
            if (levelPiece != null)
                Destroy(levelPiece.gameObject);
        }
    }

    /// <summary>
    /// Resets level generation.
    /// </summary>
    /// <param name="message">Message to print.</param>
    /// <param name="random">Instance of random.</param>
    /// <returns>Null.</returns>
    public IEnumerator ResetGeneration(string message, System.Random random)
    {
        if (random == null)
            random = new System.Random();

        if (generateLevelCoroutine != null) StopCoroutine(generateLevelCoroutine);
        print(message);

        yield return new WaitForSeconds(0.25f);

        // Destroys every piece previously generated
        GameObject[] levelParents = GameObject.FindGameObjectsWithTag("LevelParent");
        foreach (GameObject lvlParent in levelParents)
            Destroy(lvlParent);

        DestroyEveryPiece();

        yield return new WaitForSeconds(0.25f);

        // Generates another level
        generateLevelCoroutine = GenerateLevel(random, false);
        StartCoroutine(generateLevelCoroutine);
    }

    protected virtual void OnEndedGeneration() => EndedGeneration?.Invoke();
    public event Action EndedGeneration;

    private enum YieldType { FixedUpdate, Update, WaitForFrame, WaitForSeconds }
}
