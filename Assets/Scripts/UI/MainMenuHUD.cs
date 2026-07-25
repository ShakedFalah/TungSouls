using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHUD : MonoBehaviour
{
    [SerializeField] private int totalTungs;
    
    [SerializeField] private ThemesSo _themesSo;
    
    [SerializeField] private GameObject baseSelectionPanel;

    [SerializeField] private GameObject profileSelectionPanel;
    
    [SerializeField] private GameObject finalSelectionPanel;
    
    [SerializeField] private GameObject saveStats;
    [SerializeField] private TextMeshProUGUI saveNameText;
    [SerializeField] private TextMeshProUGUI currencyText;
    
    [SerializeField] private GameObject upgradeSelectionPanel;
    
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

    public void OpenFinalMenu()
    {
        string selectedProfile = PlayerPrefs.GetString("LastSelectedProfile", "");
        saveNameText.text = selectedProfile;
        
        if (!string.IsNullOrEmpty(selectedProfile))
        {
            SaveData data = SaveHandler.ReadFromJson(selectedProfile);
            currencyText.text = $"Tungs: {data.totalTungs}$";
        }
        else
        {
            currencyText.text = "Tungs: 0$";
        }
        
        saveStats.SetActive(true);
        finalSelectionPanel.SetActive(true);
        profileSelectionPanel.SetActive(false);
        upgradeSelectionPanel.SetActive(false);
    }

    public void OpenUpgradeMenu()
    {
        upgradeSelectionPanel.SetActive(true);
        finalSelectionPanel.SetActive(false);
    }
    
    
    public void ExitApp()
    {
        Application.Quit();
    }
}
