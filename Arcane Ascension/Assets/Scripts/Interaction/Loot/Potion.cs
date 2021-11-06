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
            if (potionType == PotionType.Health)
            {
                if (other.TryGetComponent<IHealth>(out IHealth iHealable))
                {
                    iHealable.Heal(percentage * iHealable.MaxHealth / 100, StatsType.Health);
                }
            }
            else
            {
                if (other.TryGetComponent<IMana>(out IMana iHealable))
                {
                    iHealable.Heal(percentage * iHealable.MaxMana / 100, StatsType.Mana);
                }
            }

            Destroy(gameObject);
        }
    }

    private enum PotionType { Health, Mana }
}
