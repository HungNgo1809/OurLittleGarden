using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTable : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            UiManager.Instance.ActiveCraftPanel(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            UiManager.Instance.ActiveCraftPanel(false);
        }
    }
}
