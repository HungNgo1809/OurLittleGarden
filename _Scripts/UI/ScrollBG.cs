using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBG : MonoBehaviour
{
    public float aboveY = 100f; // Vị trí Y đích
    public float topY = 0f; // Vị trí Y ban đầu
    public float stepSize = 1f; // Kích thước bước di chuyển

    private RectTransform rectTransform;

    public int state = 0;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (rectTransform.anchoredPosition.y >= topY)
        {
            state = 1;
        }

        else if (rectTransform.anchoredPosition.y <= aboveY)
        {
            state = 0;
        }

        if(state == 0)
        {
            Vector2 newPosition = rectTransform.anchoredPosition;
            newPosition.y += stepSize;
            rectTransform.anchoredPosition = newPosition;
        }else if(state == 1)
        {
            Vector2 newPosition = rectTransform.anchoredPosition;
            newPosition.y -= stepSize;
            rectTransform.anchoredPosition = newPosition;
        }    
    }
}