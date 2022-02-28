using UnityEngine;

[RequireComponent(typeof(Stats))]
/// <summary>
/// Class for all characters.
/// </summary>
public abstract class Character : MonoBehaviour
{
    // Scriptable object with general values
    [SerializeField] protected CharacterSO allValues;

    /// <summary>
    /// Gets common CharacterSO Values to all type of character classes.
    /// </summary>
    public CharacterSO CommonValues { get => allValues; set => allValues = value; }

    // Transform variables
    [SerializeField] private Transform hand;
    [SerializeField] private Transform eyes;

    // Transform getters
    public Transform Hand => hand;
    public Transform Eyes => eyes;
}
