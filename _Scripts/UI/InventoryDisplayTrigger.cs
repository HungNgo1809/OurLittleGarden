using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDisplayTrigger : MonoBehaviour
{
    private void OnEnable()
    {
        //DisplayInventory.Instance.ClearAll();
        DisplayInventory.Instance.UpdateUI();
    }
}
