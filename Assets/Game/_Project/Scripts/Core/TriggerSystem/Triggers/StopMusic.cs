using UnityEngine;

[CreateAssetMenu(menuName = "Triggers/Stop Music")]
public class StopMusic : TriggerActionSO
{
    public override void Execute()
    {
        AudioManager.Instance.StopMusic();
    }
}