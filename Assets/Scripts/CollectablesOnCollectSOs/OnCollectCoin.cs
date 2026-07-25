using UnityEngine;

[CreateAssetMenu(fileName = "OnCollectCoin", menuName = "Scriptable Objects/OnCollect/OnCollectCoin")]
public class OnCollectCoin : OnCollectSO
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
