using UnityEngine;

public class CoinItem : ItemLogic
{
    [SerializeField] private int scoreValue = 1;

    protected override void OnPlayerHit(GameObject player)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(scoreValue);
        }
        
        base.OnPlayerHit(player); // calls the original function from parent and despawn the coin
    }
}
