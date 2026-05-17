using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private DifficultyManager difficultyManager;

    [SerializeField]
    private float changeDifficultyTime;

    private float nextDifficutlyChange;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(ChangeDifficulty());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ChangeDifficulty()
    {
        while (true)
        {
            yield return new WaitForSeconds(changeDifficultyTime);
            difficultyManager.nextDifficulty();
        }
    }
}
