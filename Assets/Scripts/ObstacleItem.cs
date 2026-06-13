using UnityEngine;

public class ObstacleItem : MovingItemLogic
{
    protected override void OnPlayerHit(GameObject player)
    {
        PlayerController playerScript = player.GetComponent<PlayerController>();

        // if player is invincible
        if (playerScript != null && playerScript.isInvincible)
        {
            Debug.Log("Ignore Hit!!!");
            
            base.OnPlayerHit(player);
            return;
        }
        
        // if player is NOT invincible
        if (GameManager.Instance != null)
        {
            GameManager.Instance.TriggerGameOver(); // calls GameManager TriggerGameOver sequence
        }

        base.OnPlayerHit(player);
    }
}
