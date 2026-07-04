using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHUD : MonoBehaviour
{
    [SerializeField] private ThemesSo _themesSo;
    
    [SerializeField] private GameObject baseSelectionPanel;

    [SerializeField] private GameObject profileSelectionPanel;
    
    public static bool IsLoadingFromSave { get; private set; }

    public void OpenProfileSelection()
    {
        if (profileSelectionPanel != null)
        {
            profileSelectionPanel.SetActive(true);
            baseSelectionPanel.SetActive(false);
        }
    }
    
    public void CloseProfileSelection()
    {
        if (profileSelectionPanel != null)
        {
            profileSelectionPanel.SetActive(false);
        }
    }
    
    public void StartNewGame()
    {
        IsLoadingFromSave = false;

        // Create new seed

        LoadGameplayScene();
    }

    public void StartLoadedGame()
    {
        LoadGameplayScene();
    }    
    
    public void LoadGameplayScene()
    {
        int sceneIndex = SceneUtility.GetBuildIndexByScenePath("Game");

        if (sceneIndex != -1)
        {
            SceneManager.LoadScene("Game");
        }
        else
        {
            Debug.LogError("Game Scene not found in Build Settings!");
        }
    }
    
    public void SelectTheme()
    {
        _themesSo.currentThemeIndex++;
        if (_themesSo.currentThemeIndex >= _themesSo.skyThemes.Count)
            _themesSo.currentThemeIndex = 0;
        RenderSettings.skybox = _themesSo.skyThemes[_themesSo.currentThemeIndex];
    }
    
    public void ExitApp()
    {
        Application.Quit();
    }
}
