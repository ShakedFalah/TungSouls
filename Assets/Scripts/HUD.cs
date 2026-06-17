using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public void PauseGame()
    {
        GameManager.Instance.PauseGame();
    }
}
