using UnityEngine;

[CreateAssetMenu(fileName = "OnCollectMultiplier", menuName = "Scriptable Objects/OnCollect/OnCollectMultiplier")]
public class OnCollectMultiplier : OnCollectSO
{
    public override void OnCollect(PlayerController player)
    {
        player.StartMultiplier();
    }
}
