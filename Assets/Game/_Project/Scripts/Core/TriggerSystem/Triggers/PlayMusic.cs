using UnityEngine;

[CreateAssetMenu(menuName = "Triggers/Play Music")]
public class PlayMusic : TriggerActionSO
{
    public AudioClip Clip;
    public bool Loop;

    public override void Execute()
    {
        AudioManager.Instance.PlayMusic(Clip, Loop);
    }
}