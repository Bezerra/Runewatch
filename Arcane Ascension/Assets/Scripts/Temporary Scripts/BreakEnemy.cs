using UnityEngine;

public class BreakEnemy : MonoBehaviour
{
    [SerializeField] private GameObject brokenEnemy;

    private SkinnedMeshRenderer meshRender;

    private void Awake()
    {
        meshRender = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    public void Break()
    {
        meshRender.enabled = false;
        brokenEnemy.SetActive(true);
        brokenEnemy.transform.parent = null;
    }
}
