using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayInventory : MonoBehaviour
{
    public static DisplayInventory Instance { get; set; }

    public DataManager dataManager;

    public GameObject[] UiSlot;

    //public GameObject InventoryPanel;

    bool isClear;
    //public GameObject craftPanel;
    //public GameObject[] CraftUiSlot;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        Invoke("UpdateUI", 0.1f); 
    }
    public void UpdateUI()
    {
        //ClearAll();
      
        foreach (DataManager.InventoryData item in dataManager.inventoryData)
        {
            if (item.invPos>0 && item.button == 0)
            {
                if(UiSlot[item.invPos - 1].transform.childCount == 0) 
                {
                    Debug.Log(item.prefabItemID);
                    GameObject objUI = Instantiate(InventoryItem.Instance.SearchItemByID(item.prefabItemID).objUI);

                    objUI.name = item.objectItemID;
                    objUI.transform.parent = UiSlot[item.invPos - 1].transform;

                    objUI.transform.position = UiSlot[item.invPos - 1].transform.position;

                    objUI.GetComponent<RectTransform>().localScale = new Vector3(0.8f, 0.8f, 0.8f);
                 
                }
            }
        }
    }
  
    public void ClearAll()
    {
        for(int i=0; i < UiSlot.Length; i++)
        {
            foreach(Transform child in UiSlot[i].transform)
            {
                Destroy(child.gameObject);
            }
        }
    }

    public GameObject SearchFirstEmptyUiSlot()
    {
        foreach(GameObject uiSlot in UiSlot)
        {
            if(uiSlot.transform.childCount == 0)
            {
                //Debug.Log(uiSlot);
                return uiSlot;
            }    
        }
        return null;
    }
    public bool SearchNumberEmptyUiSlot(int numberIsNeed)
    {
        int countslotnumber = 0;
        foreach (GameObject uiSlot in UiSlot)
        {
            if (uiSlot.transform.childCount == 0)
            {
                //Debug.Log(uiSlot);
                countslotnumber++;
            }
        }
        if(countslotnumber >= numberIsNeed)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }
    /*
    private void OnApplicationQuit()
    {
        dataManager.terData.Clear();
    }*/
}
