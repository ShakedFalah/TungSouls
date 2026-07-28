using UnityEngine;

public class ObstacleItem : MovingItemLogic
{
    protected override void OnPlayerHit(GameObject player)
    {
        PlayerController playerScript = player.GetComponent<PlayerController>();

        playerScript.HitObstacle();
        base.OnPlayerHit(player);
    }
}
