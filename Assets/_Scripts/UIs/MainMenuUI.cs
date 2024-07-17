using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
    public TMP_Text tmpText; // 你的 TMP 文本对象
    public float bounceDuration = 0.5f; // 律动的持续时间
    public float bounceScale = 1.2f; // 放大倍数

    private void Start()
    {
        if (tmpText == null)
        {
            tmpText = GetComponent<TMP_Text>();
        }

        StartBouncing();
    }

    private void StartBouncing()
    {
        // 律动效果：循环放大和缩小
        tmpText.transform.DOScale(bounceScale, bounceDuration).SetLoops(-1, LoopType.Yoyo);
    }
}
