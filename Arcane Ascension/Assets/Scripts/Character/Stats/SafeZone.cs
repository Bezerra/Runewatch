using UnityEngine;

/// <summary>
/// Class responsible for safe zones logic.
/// </summary>
public class SafeZone : MonoBehaviour, IReset
{
    private Stats stats;
    public EnemyBoss Boss { get; set; }

    private float enemyDefaultDamageResistance;
    private readonly float EXTRARESISTANCEWHILECASTING = -0.6f;

    private void OnEnable()
    {
        if (Boss != null)
        {
            enemyDefaultDamageResistance =
                Boss.EnemyStats.CommonAttributes.DamageResistanceStatusEffectMultiplier;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Boss.EnemyStats.CommonAttributes.DamageResistanceStatusEffectMultiplier = 
            EXTRARESISTANCEWHILECASTING;

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
        Boss.EnemyStats.CommonAttributes.DamageResistanceStatusEffectMultiplier =
            enemyDefaultDamageResistance;

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

    private void OnDisable()
    {
        if (Boss != null)
        {
            Boss.EnemyStats.CommonAttributes.DamageResistanceStatusEffectMultiplier =
            enemyDefaultDamageResistance;
        }

        if (stats != null)
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
