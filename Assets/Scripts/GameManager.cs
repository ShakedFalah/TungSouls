using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // to restart the game

public class GameManager : SingletonPersistent<GameManager> // making it a singleton
{

    public int currentDifficultyIndex;
    public float currentTime { get; private set; } = 0f;
    public float currentDistance { get; private set; } = 0f;
    public int currentScore { get; private set; } = 0;
    private PlayerController playerController;

    [Header("Themes and Profiles")]
    [SerializeField] ThemesSo _themesSo;
    [SerializeField] ProfileSO _profileSO;


    public event Action<DifficultySettings> onDifficultyChange;
    public event Action onGameOver;
    public event Action<float> onDistanceChanged;
    public event Action<int> onScoreChanged;
    public DifficultySettings CurrentDifficulty => difficultyManager.difficultyList[currentDifficultyIndex];

    [Header("Game State")]
    [SerializeField] private bool isGameOver = false;
    public bool IsGameOver => isGameOver;
    private Coroutine difficultyCoroutine;

    [Header("Difficulty")]
    [SerializeField] private DifficultyManager difficultyManager;

    [Header("Upgrades")]
    [SerializeField] private MetaUpgradesSO metaUpgrades;
    [SerializeField] private IsMagnetableSettingsSO multiplierMagnetableSettings;

    private Dictionary<string, int> upgradeIndexes = new Dictionary<string, int>();
    private HUDManager hudManager;
    private Canvas hud;
    private Canvas pauseMenu;

    private void Start()
    {
        if (_profileSO.iSProfileLoaded)
        {
            LoadGame(_profileSO.profileName);
        }
        else
        {
            ApplySeededRandomness(UnityEngine.Random.Range(100000, 999999).ToString());
        }
    }
    void Update()
    {
        if (isGameOver) return;

        currentTime += Time.deltaTime; // time

        currentDistance += CurrentDifficulty.movementSpeed * Time.deltaTime; // distance
        onDistanceChanged?.Invoke(currentDistance);

        if (hudManager != null)
        {
            hudManager.UpdateHUDFields(currentTime, currentDistance, currentDifficultyIndex + 1, playerController.scoreMultiplier);
        }

    }

    public void AddScore(int amount)
    {
        currentScore += amount;

        if (hudManager != null)
        {
            hudManager.UpdateScoreDisplay(currentScore); // displays the score
        }

        onScoreChanged?.Invoke(currentScore);
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
        onGameOver.Invoke();

        Time.timeScale = 0;


        StopAllCoroutines();
        Debug.Log("Game Over");

        StartCoroutine(RestartSequence());
    }

    IEnumerator RestartSequence()
    {
        yield return new WaitForSecondsRealtime(2f);

        // disabling any active item
        MovingItemLogic[] activeItems = FindObjectsByType<MovingItemLogic>(FindObjectsSortMode.None);
        foreach (MovingItemLogic item in activeItems)
        {
            item.gameObject.SetActive(false);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // reload level
        Time.timeScale = 1;
    }

    IEnumerator ChangeDifficulty()
    {
        yield return new WaitForEndOfFrame();
        while (currentDifficultyIndex < difficultyManager.difficultyList.Count - 1)
        {
            yield return new WaitForSeconds(difficultyManager.difficultyChangeTime * (currentDifficultyIndex + 1) - currentTime);

            if (isGameOver) yield break;

            if (currentDifficultyIndex < difficultyManager.difficultyList.Count - 1)
            {
                currentDifficultyIndex++;
                if (onDifficultyChange != null)
                {
                    onDifficultyChange.Invoke(CurrentDifficulty);
                }
            }
        }
    }

    void UpdateUpgradeStatus() // power up statuses
    {
        if (upgradeIndexes.TryGetValue("Magnet", out int magnetIndex))
        {
            playerController.magnetSettings =
                GetAndApplyPowerUpUpgrade("Magnet", magnetIndex) as MagnetSettings;
        }

        if (upgradeIndexes.TryGetValue("Invincibility", out int invincibilityIndex))
        {
            playerController.invincibilitySettings =
                GetAndApplyPowerUpUpgrade("Invincibility", invincibilityIndex) as InvincibilitySettings;
        }

        if (upgradeIndexes.TryGetValue("Multiplier", out int multiplierIndex))
        {
            playerController.multiplierSettings =
                GetAndApplyPowerUpUpgrade("Multiplier", multiplierIndex) as MultiplierSettings;
        }
    }

    void Restart()
    {
        RenderSettings.skybox = _themesSo.skyThemes[_themesSo.currentThemeIndex];
        
        // Reset our variables for the fresh scene
        isGameOver = false;
        currentDifficultyIndex = 0;
        currentTime = 0f;
        currentDistance = 0f;
        currentScore = 0;
        Time.timeScale = 1f;

        AssignReferences();

        UpdateUpgradeStatus();

        // Restart the difficulty loop safely
        if (difficultyCoroutine != null)
        {
            StopCoroutine(difficultyCoroutine);
        }
        
        difficultyCoroutine = StartCoroutine(ChangeDifficulty());
    }

    private void AssignReferences()
    {
        hudManager = GameObject.FindGameObjectWithTag("HUDManager").GetComponent<HUDManager>();
        hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<Canvas>();
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu").GetComponent<Canvas>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public override void Awake()
    {
        base.Awake();

        if (Instance != this) return;

        // Listen for scene loads
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    void OnDestroy()
    {
        // Unsubscribe to clean up memory
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Instance.SaveGame(_profileSO.profileName);
        if (scene.name != "Game")
        {
            Instance = null;
            Destroy(gameObject);
        } else
        {
            Restart();
        }

    }

    public void PauseGame()
    {
        pauseMenu.enabled = true;
        hud.enabled = false;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        pauseMenu.enabled = false;
        hud.enabled = true;
        Time.timeScale = 1;
    }

    private void OnApplicationQuit()
    {
        SaveGame(_profileSO.profileName);
    }

    private void SaveGame(string saveName)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerController playerController = player.GetComponent<PlayerController>();
        
        SaveData data = SaveHandler.ReadFromJson(saveName);

        data.score = currentScore;
        data.distance = currentDistance;
        data.time = currentTime;
        data.playerPositionX = player.transform.position.x;
        data.obstaclesData = SaveHandler.GetObstacleDataList();
        data.magnetDuration = playerController.magnetDuration;
        data.multiplierDuration = playerController.multiplierDuration;
        data.invincibilityDuration = playerController.invincibleDuration;
        data.multiplierValue = playerController.scoreMultiplier;
        data.randomState = JsonUtility.ToJson(UnityEngine.Random.state);
        data.totalTungs += this.currentScore;

        SaveHandler.SaveToJson(data, saveName);

        CameraCaptureToTexture cameraCapture = GetComponent<CameraCaptureToTexture>();
        Texture2D thumbnail = cameraCapture.Capture();
        SaveHandler.SaveThumbnail(thumbnail, saveName);
    }

    public void LoadGame(string fileName)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerController playerController = player.GetComponent<PlayerController>();
        Vector3 playerPosition = player.transform.position;
        SaveData data = SaveHandler.ReadFromJson(fileName);
        string[] saves = SaveHandler.GetSaveNames();

        if(data == null) return;
        
        AddScore(data.score);
        currentDistance = data.distance;
        currentTime = data.time;
        currentDifficultyIndex = data.difficultyIndex;

        if (!string.IsNullOrEmpty(data.randomState))
        {
            UnityEngine.Random.state = JsonUtility.FromJson<UnityEngine.Random.State>(data.randomState);
        }
        
        player.transform.position = new Vector3(data.playerPositionX, playerPosition.y, playerPosition.z);
        playerController.magnetDuration = data.magnetDuration;
        playerController.multiplierDuration = data.multiplierDuration;
        playerController.invincibleDuration = data.invincibilityDuration;
        playerController.scoreMultiplier = data.multiplierValue;

        // Load meta upgrades
        // Multiplier
        if (data.purchasedUpgrades.Contains("Mult_3"))
        {
            upgradeIndexes["Multiplier"] = 3;
        } else if (data.purchasedUpgrades.Contains("Mult_2"))
        {
            upgradeIndexes["Multiplier"] = 2;
        } else if (data.purchasedUpgrades.Contains("Mult_1"))
        {
            upgradeIndexes["Multiplier"] = 1;
        } else
        {
            upgradeIndexes["Multiplier"] = 0;
        }

        // Magnet
        if (data.purchasedUpgrades.Contains("Mag_3"))
        {
            upgradeIndexes["Magnet"] = 3;
        }
        else if (data.purchasedUpgrades.Contains("Mag_2"))
        {
            upgradeIndexes["Magnet"] = 2;
        }
        else if (data.purchasedUpgrades.Contains("Mag_1"))
        {
            upgradeIndexes["Magnet"] = 1;
        }
        else
        {
            upgradeIndexes["Magnet"] = 0;
        }

        // Invincibility
        if (data.purchasedUpgrades.Contains("Inv_3"))
        {
            upgradeIndexes["Invincibility"] = 3;
        }
        else if (data.purchasedUpgrades.Contains("Inv_2"))
        {
            upgradeIndexes["Invincibility"] = 2;
        }
        else if (data.purchasedUpgrades.Contains("Inv_1"))
        {
            upgradeIndexes["Invincibility"] = 1;
        }
        else
        {
            upgradeIndexes["Invincibility"] = 0;
        }

        UpdateUpgradeStatus();
        LoadObstacles(data.obstaclesData);
    }

    private void LoadObstacles(ObstacleData[] obstaclesData)
    {
        if (obstaclesData == null) return;
        
        foreach (ObstacleData data in obstaclesData)
        {
            GameObject spawnedObject = TaggedObjectPooler.Instance.GetPooledObject(data.tag);
            if (spawnedObject != null) // safety checks
            {
                spawnedObject.transform.position = new Vector3(data.positionX, data.positionY, data.positionZ);
            }
        }
    }

    private void ApplySeededRandomness(string seedString)
    {
        if (string.IsNullOrEmpty(seedString))
        {
            seedString = "DefaultSeedValue";
        }

        int seedHash = seedString.GetHashCode();
        UnityEngine.Random.InitState(seedHash);

        Debug.Log($"Pseudorandom State Lock Initiated. Seed Content: '{seedString}' -> State Key: {seedHash}");
    }

    private PowerUpSettings GetAndApplyPowerUpUpgrade(string name, int index)
    {
        PowerupUpgradeSO upgrade = metaUpgrades.GetPowerupUpgradeByIndex(name, upgradeIndexes[name]);

        foreach (var effect in upgrade.effects)
        {
            effect.Apply(this);
        }

        return upgrade.newSettings;
    }

    public void AddPlayerLife()
    {
        playerController.AddLife();
    }

    public void SetMultiplierMagnetable(bool magnetable)
    {
        multiplierMagnetableSettings.isMagnetable = magnetable;
    }
}

