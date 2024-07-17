using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Security.Cryptography;

public class TextGlowController : MonoBehaviour
{
    public Color glowColor = Color.yellow; // 自发光颜色
    public float glowPower = 0.5f; // 自发光强度
    public float glowOffset = 0.5f; // 自发光偏移量

    public TMP_Text textMeshPro;
    public Material originalMaterial;
    private Material glowMaterial;

    private void Awake()
    {
        // 获取 TextMeshPro 组件
        if (textMeshPro == null)
        {
            textMeshPro = GetComponent<TMP_Text>();
            if (textMeshPro == null)
            {
                textMeshPro = GetComponentInChildren<TMP_Text>();
            }
        }

        if (textMeshPro == null)
        {
            Debug.LogError("TextMeshPro component not assigned!");
            return;
        }
    }


    private void Start()
    {
      

        // 获取并复制原始材质
        originalMaterial = textMeshPro.fontMaterial;
        if (originalMaterial == null)
        {
            Debug.LogError("Original material not found!");
            return;
        }

        glowMaterial = new Material(originalMaterial);
        glowMaterial.EnableKeyword("GLOW_ON");
        glowMaterial.SetColor(ShaderUtilities.ID_GlowColor, glowColor);
        glowMaterial.SetFloat(ShaderUtilities.ID_GlowPower, glowPower);
        glowMaterial.SetFloat(ShaderUtilities.ID_GlowOffset, glowOffset);
    }

   
   
public void OnPointerEnter()
    {
        if (textMeshPro != null)
        {
            textMeshPro.fontMaterial = glowMaterial;
        }
    }

    public void OnPointerExit()
    {
        if (textMeshPro != null)
        {
            textMeshPro.fontMaterial = originalMaterial;
        }
    }
}


