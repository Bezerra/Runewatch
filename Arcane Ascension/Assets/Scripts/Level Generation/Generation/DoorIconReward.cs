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
            renderer.material.mainTexture = spellTexture;
        }
        else
        {
            renderer.material.mainTexture = passiveTexture;
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
