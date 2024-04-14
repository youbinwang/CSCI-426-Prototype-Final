using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct DifficultyLevel
{
    public string name;
    public Color uiTextColor;
    public float scoreThrehold;

    public DifficultyLevel(string name, Color uiTextColor, float scoreThrehold)
    {
        this.name = name;
        this.uiTextColor = uiTextColor;
        this.scoreThrehold = scoreThrehold;
        
    }
}

public class GradeSystem : MonoBehaviour
{
    public DifficultyLevel[] levels;
    public int currentLevelIndex = 0;
    public float score;

    public static event Action OnEnemyDestroy;

    [Header ("DynamicParameters")]
    public Text uiText;
    void Start()
    {
        UpdateDifficulty();
    }

    void Update()
    {
        uiText.text = $"Score: {score}";
        CheckDifficulty();
    }

    //------------------------------------------- ReceiveScore ----------------------------------
    void OnEnable()
    {
        OnEnemyDestroy += AddScore;
    }

    private void OnDisable()
    {
        OnEnemyDestroy -= AddScore;
    }


    public static void TriggerEnemDestroyed()
    {
        OnEnemyDestroy?.Invoke();
    }

    public void AddScore()
    {
        score++;


    }
    //------------------------------------------- CheckDifficulty ----------------------------------
    private void CheckDifficulty()
    {
        for(int i = 0; i < levels.Length; i++)
        {
            if(score >= levels[i].scoreThrehold)
            {
                currentLevelIndex = i;
            }
        }

        UpdateDifficulty();

    }

    private void UpdateDifficulty()
    {
        DifficultyLevel currentLevel = levels[currentLevelIndex];
        uiText.color = currentLevel.uiTextColor;

    }
  
}
