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

        // �������ʱ�䴰���ڵ�ɱ�м�¼
        while (killTimes.Count > 0 && killTimes.Peek() < currentTime - killTime)
        {
            killTimes.Dequeue();
        }

        // �����Ѷ�
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
        killTimes.Enqueue(Time.time); // ��¼��ǰɱ�е�ʱ���
        //UpdateDifficulty(killTimes.Count); // ÿ��ɱ�к��Ը����Ѷ�
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

        // �رղ�����Ҫ������
        foreach (var source in currentInstruments)
        {
            if (!newInstruments.Contains(source) && source != null)
            {
                source.volume = 0f;
            }
        }

        // �������Ѷ���Ҫ������
        foreach (var source in newInstruments)
        {
            if (source != null && source.volume == 0)
            {
                source.volume = 0.45f;
            }
        }

        // ���µ�ǰ�Ѷ�����
        currentDifficultyIndex = difficultyIndex;
    }
}
