using UnityEngine;

/// <summary>
/// Class responsible for triggering potions behaviour.
/// </summary>
public class Potion : MonoBehaviour
{
    [SerializeField] private PotionSO potion;
    [SerializeField] private LootAndInteractionSoundType lootType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layers.PlayerLayerNum)
        {
            if (potion.PotionType == PotionType.Health)
            {
                if (other.TryGetComponent<IHealth>(out IHealth iHealable))
                {
                    iHealable.Heal(potion.Percentage * iHealable.MaxHealth / 100, StatsType.Health);
                }
            }
            else
            {
                if (other.TryGetComponent<IMana>(out IMana iHealable))
                {
                    iHealable.Heal(potion.Percentage * iHealable.MaxMana / 100, StatsType.Mana);
                }
            }
            LootSoundPoolCreator.Pool.InstantiateFromPool(lootType.ToString(), transform.position, Quaternion.identity);
            transform.parent.gameObject.SetActive(false);
        }
    }
}
