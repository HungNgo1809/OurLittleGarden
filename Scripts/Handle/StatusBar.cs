using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    public Slider slider;
    public float maxValue;

    public int Type;
    private void Start()
    {
        SetMaxStatus(maxValue);
    }
    public void SetMaxStatus(float status)
    {
        slider.maxValue = maxValue;
        slider.value = status;
    }

    public void SetStatus(float status)
    {
        slider.value = status;
    }
}