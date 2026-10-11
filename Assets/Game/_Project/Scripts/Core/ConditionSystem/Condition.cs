using UnityEngine;
using System;

[Serializable]
public class Condition
{
    // Check if a singular condition is correct

    public string Key;
    public string ExpectedValue; // When being used, this should always be a string. Example "true" or "12"

    public bool Evalutate()
    {
        var value = GameState.Instance.GetFlag<object>(Key);

        if (value == null) return false;

        string StringValue = value.ToString().ToLowerInvariant();
        string Expected = ExpectedValue.ToLowerInvariant();

        return StringValue == Expected;
    }
}
