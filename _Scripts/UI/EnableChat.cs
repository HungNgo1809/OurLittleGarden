using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableChat : MonoBehaviour
{
    public GameObject chatPanel;
    public TextTyping textTyping;
    public void OnClickMess()
    {
        if(!chatPanel.activeSelf)
        {
            chatPanel.SetActive(true);
        }
        else
        {
            chatPanel.SetActive(false);
            textTyping.isTypingText = false;
        }    
    }    
}
