using UnityEngine;

public class HUD : MonoBehaviour
{
    public void PauseGame()
    {
        GameManager.Instance.PauseGame();
    }
}
