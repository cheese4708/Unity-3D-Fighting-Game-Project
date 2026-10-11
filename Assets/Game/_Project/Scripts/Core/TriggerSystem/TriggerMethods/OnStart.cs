using UnityEngine;
using System.Collections.Generic;

public class OnStart : MonoBehaviour
{
    public List<TriggerActionSO> TriggerActions = new List<TriggerActionSO>();

    private void Start()
    {
        TriggerManager.Instance.Execute(TriggerActions);
    }
}
