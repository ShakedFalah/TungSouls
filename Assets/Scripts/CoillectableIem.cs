using UnityEngine;

public enum ItemType { Coin, Invincibility, ScoreMultiplier, Magnet}

public class CollectableItem : MovingItemLogic
{
    // Dropdown menu for item selection
    [Header("Item Type")]
    [SerializeField] private ItemType itemType;
    
    [Header("Item setting")]
    [SerializeField] private int scoreValue = 1; 
    [SerializeField] float effectDuration = 5f;
    [SerializeField] float extraValue = 2f; // for radius or multiplier
    
    protected override void OnPlayerHit(GameObject player)
    {
        PlayerController playerScript = player.GetComponent<PlayerController>();
        if (playerScript != null)
        {
            switch (itemType)
            {
                case ItemType.Coin:
                    if (GameManager.Instance != null)
                    {
                        int recievedScore =
                            Mathf.RoundToInt(scoreValue *
                                             playerScript.scoreMultiplier); // extra value is float so we gotta convert
                        GameManager.Instance.AddScore(recievedScore);
                    }

                    break;

                case ItemType.Invincibility:
                    playerScript.StartInvinciblity(effectDuration);
                    break;

                case ItemType.ScoreMultiplier:
                    playerScript.StartMultiplier(effectDuration, extraValue);
                    break;

                case ItemType.Magnet:
                    playerScript.StartMagnet(effectDuration, extraValue);
                    break;
            }
        }
        base.OnPlayerHit(player); // calls the original function from parent and despawn the item
    }
}
