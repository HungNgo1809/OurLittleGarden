using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageInventory : MonoBehaviour
{
    public DataManager dataManager;

    public Transform[] Inventory;

    public Transform[] Storage;
    // Start is called before the first frame update
    private void OnEnable()
    {
        for(int i = 0; i < DisplayInventory.Instance.UiSlot.Length; i++)
        {
            if(DisplayInventory.Instance.UiSlot[i].transform.childCount > 0)
            {
                Transform tmp =
                Instantiate(DisplayInventory.Instance.UiSlot[i].transform.GetChild(0), new Vector3(0, 0, 0), Quaternion.identity);

                tmp.name = DisplayInventory.Instance.UiSlot[i].transform.GetChild(0).name;
                tmp.parent = Inventory[i];
                tmp.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                if(tmp.GetComponent<DraggableItem>() != null)
                {
                    Destroy(tmp.GetComponent<DraggableItem>());
                }
            }
        }
    }

    private void OnDisable()
    {
        ClearAll();
        ClearAllStorage();
    }

    public void ClearAll()
    {
        foreach(Transform slot in Inventory)
        {
            if(slot.childCount > 0)
            {
                Destroy(slot.GetChild(0).gameObject);
            }
        }
    }
    public void ClearAllStorage()
    {
        foreach (Transform slot in Storage)
        {
            if (slot.childCount > 0)
            {
                Destroy(slot.GetChild(0).gameObject);
            }
        }
    }

    public Transform SearchFirstEmptyStorageSlot()
    {
        foreach(Transform slot in Storage)
        {
            if(slot.childCount == 0)
            {
                return slot;
            }    
        }
        return null;
    }
    public Transform SearchFirstEmptyInventorySlot()
    {
        foreach (Transform slot in Inventory)
        {
            if (slot.childCount == 0)
            {
                return slot;
            }
        }
        return null;
    }
    public void DisplayStorage(string storageId)
    {
        if(dataManager.SearchStorageById(storageId) != null)
        {
            var sto = dataManager.SearchStorageById(storageId);

            int i = 0;
            foreach(DataManager.InventoryData item in sto.items)
            {
                GameObject objUI = Instantiate(InventoryItem.Instance.SearchItemByID(item.prefabItemID).objUI);

                objUI.name = item.objectItemID;
                objUI.transform.parent = Storage[i].transform;

                objUI.transform.position = Storage[i].transform.position;

                objUI.GetComponent<RectTransform>().localScale = new Vector3(0.8f, 0.8f, 0.8f);

                i++;
            }
        }
    }
}
