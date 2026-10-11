using UnityEngine;
using System.Collections.Generic;

public class GameState : MonoBehaviour
{
    public static GameState Instance;

    private Dictionary<string, object> flags = new Dictionary<string, object>();

    void Awake()
    {
        // Singleton Setup

        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }

    // Set a value
    public void SetFlag(string key, object value)
    {
        Debug.Log($"GameState: {key} = {value}");
        flags[key] = value;
    }

    // Get a value (Generic)
    public T GetFlag<T>(string key)
    {
        if (flags.TryGetValue(key, out var value) && value is T typedValue)
        {
            return typedValue;
        }
        return default;
    }

    // Check if flag exists
    public bool HasFlag(string key)
    {
        return flags.ContainsKey(key);
    }
}
