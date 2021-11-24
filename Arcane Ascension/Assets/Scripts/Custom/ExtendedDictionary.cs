using System;
using System.Collections.Generic;

/// <summary>
/// Extended dictionary that can have events.
/// </summary>
/// <typeparam name="key">Dictionary key.</typeparam>
/// <typeparam name="value">Dictionary value.</typeparam>
public class ExtendedDictionary<key, value> : Dictionary<key, value>
{
    public Dictionary<key, value> Items { get; }

    public ExtendedDictionary() : base() 
    { Items = new Dictionary<key, value>(); }

    public ExtendedDictionary(int capacity) : base(capacity) 
    { Items = new Dictionary<key, value>(); }

    /// <summary>
    /// Adds an item to the dictionary.
    /// </summary>
    /// <param name="key">Key to add.</param>
    /// <param name="value">Value of the key.</param>
    public void AddItem(key key, value value)
    {
        try
        {
            Items.Add(key, value);
            OnValueChanged(key, value);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// Event triggered when an item is added.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected virtual void OnValueChanged(key key, value value) =>
        ValueChanged?.Invoke(key, value);

    public event Action<key, value> ValueChanged;
}
