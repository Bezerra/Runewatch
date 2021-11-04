using UnityEngine;

/// <summary>
/// Class responsible for triggering potions behaviour.
/// </summary>
public class Potion : MonoBehaviour
{
    [SerializeField] private PotionType potionType;
    [Range(0f, 100f)][SerializeField] private float percentage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layers.PlayerLayerNum)
        {
            if (other.TryGetComponent<Stats>(out Stats stats))
            {
                if (potionType == PotionType.Health)
                {
                    stats.Heal(percentage * stats.Attributes.MaxHealth / 100, StatsType.Health);
                }
                else
                {
                    stats.Heal(percentage * stats.Attributes.MaxHealth / 100, StatsType.Mana);
                }
            }
            Destroy(gameObject);
        }
    }

    private enum PotionType { Health, Mana }
}
