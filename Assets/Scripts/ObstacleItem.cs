using UnityEngine;

public class ObstacleItem : ItemLogic
{
    protected override void OnPlayerHit(GameObject player)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.TriggerGameOver(); // calls GameManager TriggerGameOver sequence
        }

    }
}
