using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHUD : MonoBehaviour
{
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
    
    public void ExitApp()
    {
        Application.Quit();
    }
}
