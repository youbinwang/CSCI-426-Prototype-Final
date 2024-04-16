using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmController : MonoBehaviour
{
    public Material rhythmMaterial;

    public float bpm = 120.0f; // Beats per minute，私有变量

    void Start()
    {
    }

    // 更新材质的方法
    public void UpdateMaterial()
    {
        float period = 60.0f / bpm;
        rhythmMaterial.SetFloat("_Period", period);
    }

    // 设置BPM值的方法
    public void SetBPM(float newBpm)
    {
        if (bpm != newBpm)
        {
            bpm = newBpm;
            UpdateMaterial(); // 只在BPM改变时更新材质
        }
    }
}
