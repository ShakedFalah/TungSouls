using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHUD : MonoBehaviour
{
    [SerializeField] ThemesSo _themesSo;
    
    public void StartGame()
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
