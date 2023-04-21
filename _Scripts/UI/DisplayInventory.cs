using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayInventory : MonoBehaviour
{
    public DataManager dataManager;

    public GameObject[] UiSlot;
    //public GameObject craftPanel;
    //public GameObject[] CraftUiSlot;
    private void OnEnable()
    {
        UpdateUI();
    }
    public void UpdateUI()
    {
        //ClearAll();
        int i = 0;
        foreach(DataManager.InventoryData item in dataManager.inventoryData)
        {
            if (UiSlot[i].transform.childCount == 0)
            {
                GameObject objUI = Instantiate(InventoryItem.Instance.SearchItemByID(item.prefabItemID).objUI);

                objUI.name = item.objectItemID;
                objUI.transform.parent = UiSlot[i].transform;

                objUI.transform.position = UiSlot[i].transform.position;
            }
            i++;
        }
    }
    
    public void ClearAll()
    {
        for(int i=0; i < UiSlot.Length; i++)
        {
            foreach(Transform child in UiSlot[i].transform)
            {
                Destroy(child);
            }
        }
    }
}
