using System.Collections;
using UnityEngine;

/// <summary>
/// Class responsible for containing information about every room occlusion.
/// </summary>
public class RoomOcclusionController : MonoBehaviour
{
    // Components
    private LevelGenerator levelGenerator;
    private GameObject startRoom;

    /// <summary>
    /// Current level piece the player is in.
    /// </summary>
    public LevelPiece CurrentLevelPieceCollision { get; set; }

    /// <summary>
    /// Current room occlusion the player is in.
    /// </summary>
    public RoomOcclusion CurrentRoomOcclusion { get; set; }

    private void Awake() =>
        levelGenerator = GetComponent<LevelGenerator>();

    private void OnEnable() =>
        levelGenerator.EndedGeneration += EndedGeneration;

    private void OnDisable() =>
        levelGenerator.EndedGeneration -= EndedGeneration;

    /// <summary>
    /// Method called after the generation is over.
    /// Starts occlusion from start piece.
    /// </summary>
    private void EndedGeneration()
    {
        startRoom = GameObject.FindGameObjectWithTag("StartRoom");
        RoomOcclusion startRoomOcclusion = startRoom.GetComponentInChildren<RoomOcclusion>();
        LevelPiece startRoomLevelPiece = startRoom.GetComponent<LevelPiece>();

        CurrentLevelPieceCollision = startRoomLevelPiece;

        StartCoroutine(EndedGenerationCoroutine(startRoomOcclusion, startRoomLevelPiece));
    }

    /// <summary>
    /// Starts first room occlusion and spawns player.
    /// </summary>
    /// <param name="startRoomOcclusion">First room occlusion.</param>
    /// <returns>.</returns>
    private IEnumerator EndedGenerationCoroutine(RoomOcclusion startRoomOcclusion, LevelPiece startRoomLevelPiece)
    {
        // Waits for occlusion to end, to spawn the player after
        yield return startRoomOcclusion.ControlChildOccludeesCoroutine();
        // Needed to enable first door
        StartCoroutine(startRoomLevelPiece.EnableChildOccludeesCoroutine());

        startRoom.TryGetComponent(out PlayerSpawnLevelPiece playerSpawnLevelPiece);
        //Instantiate(INPUTTEMP);
        Instantiate(PLAYERTEMP, playerSpawnLevelPiece.PlayerSpawnTransform.position,
            playerSpawnLevelPiece.PlayerSpawnTransform.rotation);
    }

    [SerializeField] private GameObject PLAYERTEMP;
    [SerializeField] private GameObject INPUTTEMP;
}
