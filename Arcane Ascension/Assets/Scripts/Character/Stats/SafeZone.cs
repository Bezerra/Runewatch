using UnityEngine;

/// <summary>
/// Class responsible for safe zones logic.
/// </summary>
public class SafeZone : MonoBehaviour, IReset
{
    private Stats stats;

    private void OnTriggerStay(Collider other)
    {
        if (stats == null)
        {
            if (other.TryGetComponent(out Stats stats))
            {
                stats.Immune = true;
                this.stats = stats;
            }
        }
        else
        {
            stats.Immune = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (stats == null)
        {
            if (other.TryGetComponent(out Stats stats))
            {
                stats.Immune = false;
                this.stats = null;
            }
        }
        else
        {
            stats.Immune = false;
            this.stats = null;
        }
    }

    public void ResetAfterPoolDisable()
    {
        this.stats = null;
    }
}
