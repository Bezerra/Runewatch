using UnityEngine;
using UnityEngine.Animations;

/// <summary>
/// Class responsible for applying book constraint to look at the player.
/// </summary>
public class BookFindPlayer : MonoBehaviour, IFindPlayer
{
    private GameObject mainCam;
    private LookAtConstraint lac;
    private ConstraintSource cs;

    private void Awake()
    {
        Camera cam = Camera.main;
        if (cam != null) mainCam = cam.gameObject;
        lac = GetComponent<LookAtConstraint>();
    }

    private void OnEnable() =>
        FindPlayer();

    public void FindPlayer(Player player = null)
    {
        if (lac == null) return;

        if (lac.sourceCount > 0)
        {
            for (int i = 0; i < lac.sourceCount; i++)
            {
                lac.RemoveSource(i);
            }
        }

        Camera cam = Camera.main;

        if (mainCam == null)
        {
            if (cam != null) mainCam = cam.gameObject;
        }

        if (mainCam != null)
        {
            cs.sourceTransform = mainCam.transform;
            cs.weight = 1;
            lac.AddSource(cs);
            lac.constraintActive = true;
        }

    }

    public void PlayerLost(Player mainCam)
    {
        if (lac == null) return;

        if (lac.sourceCount > 0)
        {
            for (int i = 0; i < lac.sourceCount; i++)
            {
                lac.RemoveSource(i);
            }
        }
    }
}
