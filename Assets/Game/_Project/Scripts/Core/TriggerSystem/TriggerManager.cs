using UnityEngine;
using System.Collections.Generic;

public class TriggerManager : MonoBehaviour
{
    public static TriggerManager Instance;

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

    // Executes every action inside the list that's passed through
    internal void Execute(List<TriggerActionSO> TriggerActions)
    {
        foreach (var action in TriggerActions)
        {
            if (action == null)
            {
                Debug.LogWarning("TriggerManager: null action found in list, skipping.");
                continue;
            }
            action.Execute();
        }
    }
}