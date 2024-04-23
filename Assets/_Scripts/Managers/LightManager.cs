using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;

public class LightManager : MonoBehaviour
{
    [Header("Beat Track")]
    [SerializeField] private KoreographyTrack trackName;

    private Light pointLight;

    void Start()
    {
        pointLight = GetComponent<Light>();
        if (Koreographer.Instance != null && trackName != null) {
            Koreographer.Instance.RegisterForEvents(trackName.EventID, ChangeColor);
        }
    }
    
    void ChangeColor(KoreographyEvent koreoEvent)
    {
        pointLight.color = Random.ColorHSV();
    }
}
