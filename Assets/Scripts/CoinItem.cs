using UnityEngine;

public class CoinItem : ItemLogic
{
    [SerializeField] private int scoreValue = 1;

    protected override void OnPlayerHit(GameObject player)
    {
        Debug.Log("Coin Collected"); // add score logic later !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        
        base.OnPlayerHit(player); // calls the original function from parent and despawn the coin
    }
}
