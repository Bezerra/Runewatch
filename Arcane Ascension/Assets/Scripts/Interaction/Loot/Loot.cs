using UnityEngine;

/// <summary>
/// Class responsible for triggering potions behaviour.
/// </summary>
public class Loot : MonoBehaviour
{
    [SerializeField] private LootType lootType;
    [Range(1f, 100000f)][SerializeField] private float amount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layers.PlayerLayerNum)
        {
            

            Destroy(gameObject);
        }
    }

    private enum LootType { Gold, ArcanePower }
}
