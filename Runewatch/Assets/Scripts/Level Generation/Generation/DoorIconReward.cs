using UnityEngine;

/// <summary>
/// Script responsible for updating the texture of a door, 
/// depending on the loot inside a room.
/// </summary>
public class DoorIconReward : MonoBehaviour
{
    [SerializeField] private Texture spellTexture;
    [SerializeField] private Texture passiveTexture;

    public void UpdateIcon(LootType lootType)
    {
        Renderer renderer = GetComponent<Renderer>();

        if (lootType == LootType.UnknownSpell)
        {
            renderer.material.SetTexture("_MainTexture", spellTexture); 
        }
        else
        {
            renderer.material.SetTexture("_MainTexture", passiveTexture);
        }
    }

    /// <summary>
    /// Disables sprite..
    /// </summary>
    public void DisableObject()
    {
        gameObject.SetActive(false);
    }
}
