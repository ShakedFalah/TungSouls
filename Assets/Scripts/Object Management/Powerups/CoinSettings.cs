using UnityEngine;

[CreateAssetMenu(fileName = "CoinSettings", menuName = "Scriptable Objects/Collectable/CoinSettings")]
public class CoinSettings : CollectableSettings
{
    public int scoreValue;
    public override void OnCollect(PlayerController player)
    {
        if (GameManager.Instance != null)
        {
            int recievedScore =
                Mathf.RoundToInt(scoreValue *
                                 player.scoreMultiplier); // extra value is float so we gotta convert
            GameManager.Instance.AddScore(recievedScore);
        }
    }
}
