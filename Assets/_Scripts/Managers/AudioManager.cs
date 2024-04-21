using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DifficultySetting
{
    
    public float killCount; 
    public List<AudioSource> activeInstruments; 
}
public class AudioManager : MonoBehaviour
{
    public List<DifficultySetting> settings;
    private Queue<float> killTimes = new Queue<float>();
    private int currentDifficultyIndex = 0;
    public float killTime;

    public static event Action OnEnemyDestroy;
    public List<AudioSource> currentInstruments;
    public List<AudioSource> newInstruments;

    void Start()
    {
        foreach (var setting in settings)
        {
            foreach (var source in setting.activeInstruments)
            {
                if (source != null) source.volume = 0;
            }
        }
    }

    private void Update()
    {
        float currentTime = Time.time;

        // 清除不在时间窗口内的杀敌记录
        while (killTimes.Count > 0 && killTimes.Peek() < currentTime - killTime)
        {
            killTimes.Dequeue();
        }

        // 更新难度
        UpdateDifficulty(killTimes.Count);
    }

    //------------------------------------------- ReceiveScore ----------------------------------
    void OnEnable()
    {
        OnEnemyDestroy += RegisterKill;
    }

    private void OnDisable()
    {
        OnEnemyDestroy -= RegisterKill;
    }


    public static void TriggerEnemDestroyed()
    {
        OnEnemyDestroy?.Invoke();
    }



    public void RegisterKill()
    {
        killTimes.Enqueue(Time.time); // 记录当前杀敌的时间戳
        //UpdateDifficulty(killTimes.Count); // 每次杀敌后尝试更新难度
    }

    //------------------------------------------- UpdateMusic ----------------------------------
    private void UpdateDifficulty(int currentKillCount)
    {
       
        
        for (int i = settings.Count - 1; i >= 0; i--)
        {
            if (currentKillCount >= settings[i].killCount)
            {
                SetInstruments(i);
                break;
            }
        }
    }


    private void SetInstruments(int difficultyIndex)
    {
        
        if (difficultyIndex == currentDifficultyIndex) return;

        currentInstruments = settings[currentDifficultyIndex].activeInstruments;
        newInstruments = settings[difficultyIndex].activeInstruments;

        // 关闭不再需要的乐器
        foreach (var source in currentInstruments)
        {
            if (!newInstruments.Contains(source) && source != null)
            {
                source.volume = 0f;
            }
        }

        // 开启新难度需要的乐器
        foreach (var source in newInstruments)
        {
            if (source != null && source.volume == 0)
            {
                source.volume = 0.45f;
            }
        }

        // 更新当前难度索引
        currentDifficultyIndex = difficultyIndex;
    }
}
