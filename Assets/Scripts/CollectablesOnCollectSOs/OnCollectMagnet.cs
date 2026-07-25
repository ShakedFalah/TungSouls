using UnityEngine;

[CreateAssetMenu(fileName = "OnCollectMagnet", menuName = "Scriptable Objects/OnCollect/OnCollectMagnet")]
public class OnCollectMagnet : OnCollectSO
{
    public override void OnCollect(PlayerController player)
    {
        player.StartMagnet();
    }
}
