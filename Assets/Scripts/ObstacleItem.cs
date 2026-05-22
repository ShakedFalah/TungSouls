using UnityEngine;

public class ObstacleItem : ItemLogic
{
    protected override void OnPlayerHit(GameObject player)
    {
        Debug.Log("Player hit");
        
        // instadeath? player has hp?
    }
}
