using UnityEngine;

[CreateAssetMenu(menuName = "Triggers/Play SFX")]
public class PlaySFX : TriggerActionSO
{
    public AudioClip Clip;

    public override void Execute()
    {
        AudioManager.Instance.PlaySFX(Clip);
    }
}