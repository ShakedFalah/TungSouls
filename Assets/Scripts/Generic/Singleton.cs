using UnityEngine;

/// <summary>
/// Abstract base class for creating a Singleton pattern in Unity.
/// Ensures that only one instance of the derived class exists throughout the game.
/// </summary>
/// <typeparam name="T">The type of MonoBehaviour to be made into a singleton.</typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                // Find existing instance in the scene
                _instance = FindFirstObjectByType<T>();

                if (_instance == null)
                {
                    // Optional: Automatically create a new GameObject if none exists
                    GameObject singletonObject = new GameObject(typeof(T).Name);
                    _instance = singletonObject.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
        }
        else if (_instance != this)
        {
            // Destroy duplicate instances to maintain the singleton
            Destroy(gameObject);
        }
    }
}

// Singleton that stays between scene loads
public class SingletonPersistent<T> : MonoBehaviour
    where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    public virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}