using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ColorTimeEffect : MonoBehaviour
{
    public PostProcessVolume postProcessVolume;
    public Color startColor = new Color(255f,255f,255f);
    public Color targetColor = new Color(25f / 255f, 25f / 255f, 25f / 255f);

    private ColorGrading colorGradingLayer;
 
    private void Start()
    {
        postProcessVolume.profile.TryGetSettings(out colorGradingLayer);
    }

    public void UpdateColorLerp(float t)
    {
        // Lerp the color between the start and target colors
       
        colorGradingLayer.colorFilter.value = Color.Lerp(startColor, targetColor, t);
        Debug.Log(colorGradingLayer.colorFilter.value); // kha nang lam t= 300 tu 19h toi 24 h con tu 24- 5h  sang thi nguoc lai
    }
}
