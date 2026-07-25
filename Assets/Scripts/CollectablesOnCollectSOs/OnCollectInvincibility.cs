using UnityEngine;

[CreateAssetMenu(fileName = "OnCollectInvincibility", menuName = "Scriptable Objects/OnCollect/OnCollectInvincibility")]
public class OnCollectInvincibility : OnCollectSO
{
    public override void OnCollect(PlayerController player)
    {
        player.StartInvinciblity();
    }
}
