using UnityEngine;

[CreateAssetMenu(menuName = "Triggers/Set Flag")]
public class SetFlag : TriggerActionSO
{
    public string Key;
    public string Value;

    public override void Execute()
    {
        Debug.Log($"Setting flag: {Key} = {Value}");
        GameState.Instance.SetFlag(Key, Value);
    }
}