using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class OnClick : MonoBehaviour
{
    public List<TriggerActionSO> TriggerActions = new List<TriggerActionSO>();
    public Button ClickableButton;

    private void Start()
    {
        ClickableButton.onClick.AddListener(Execute);
    }

    void Execute()
    {
        TriggerManager.Instance.Execute(TriggerActions);
    }
}
