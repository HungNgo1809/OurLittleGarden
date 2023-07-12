using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ChatMess : MonoBehaviour
{
    public RectTransform cover;
    // Update is called once per frame
    void Update()
    {
        Vector2 anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        anchoredPosition.y = - (cover.rect.height/2);
        GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
    }
}
