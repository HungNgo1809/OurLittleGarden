using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickStorageItem : MonoBehaviour
{
    public StorageInventory storageInventory;
    public DataManager dataManager;
   public void OnClickItem()
   {
        if(transform.childCount > 0)
        {
            StorageItem(transform.GetChild(0));
        }    
   }
    public void OnClickItemInStorage()
    {
        if (transform.childCount > 0)
        {
            TakeItem(transform.GetChild(0));
        }
    }
   public void StorageItem(Transform item)
   {
        if(storageInventory.SearchFirstEmptyStorageSlot() != null)
        {
            //ui
            item.parent = storageInventory.SearchFirstEmptyStorageSlot();

            //data
            dataManager.AddItemToStorage(item.name, UiManager.Instance.curStorage);

            DisplayInventory.Instance.ClearAll();
            DisplayInventory.Instance.UpdateUI();
        }    
   }
    
    public void TakeItem(Transform item)
    {
        if (dataManager.SearchItemInStorageByObjectId(item.name, dataManager.SearchStorageById(UiManager.Instance.curStorage)) != null)
        {
            var itemData = dataManager.SearchItemInStorageByObjectId(item.name, dataManager.SearchStorageById(UiManager.Instance.curStorage));
            //ui
            if(DisplayInventory.Instance.UiSlot[itemData.invPos - 1].transform.childCount == 0 && storageInventory.Inventory[itemData.invPos - 1].childCount == 0)
            {
                item.parent = storageInventory.Inventory[itemData.invPos - 1];
            }
            else
            {
                item.parent = storageInventory.SearchFirstEmptyInventorySlot();
                itemData.invPos = int.Parse(DisplayInventory.Instance.SearchFirstEmptyUiSlot().name);
            }

            //data
            dataManager.RemoveItemFromStorage(item.name, UiManager.Instance.curStorage);

            DisplayInventory.Instance.ClearAll();
            DisplayInventory.Instance.UpdateUI();
        }
    }    
}
