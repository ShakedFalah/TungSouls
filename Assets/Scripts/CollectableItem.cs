using UnityEngine;

public class CollectableItem : MovingItemLogic
{
    [Header("Collectable")]
    public CollectableSettings collectableSettings;
    
    protected override void OnPlayerHit(GameObject player)
    {
        PlayerController playerScript = player.GetComponent<PlayerController>();
        if (playerScript != null)
        {
            collectableSettings.OnCollect(playerScript);
        }

        if (collectableSettings.collectSound != null)
        {
            AudioManager.Instance.PlaySound(collectableSettings.collectSound);
        }

        base.OnPlayerHit(player); // calls the original function from parent and despawn the item
    }
}
