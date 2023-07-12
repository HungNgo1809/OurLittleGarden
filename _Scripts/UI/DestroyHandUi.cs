using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyHandUi : MonoBehaviour
{
    public static DestroyHandUi Instance { get; set; }
    public HandSlot[] hands;

    public DataManager dataManager;
    private void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        StartCoroutine(LoadHand());
    }
    public void DestroyItem(int i)
    {
       foreach(Transform item in hands[i].transform)
        {
            Destroy(item.gameObject);
        }
    }
    IEnumerator LoadHand()
    {
        yield return new WaitForSeconds(1f);
        foreach (DataManager.InventoryData inv in dataManager.inventoryData)
        {
            if(inv.button > 0)
            {
                //Debug.Log(inv.prefabItemID);
                if(InventoryItem.Instance != null)
                {
                    Transform ui;
                    ui = Instantiate(InventoryItem.Instance.SearchItemByID(inv.prefabItemID).objUI.transform);
                    ui.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);

                    ui.name = inv.objectItemID;
                    ui.SetParent(hands[inv.button - 1].transform);

                    ui.GetComponent<RectTransform>().localScale = new Vector3(0.8f, 0.8f, 0.8f);
                    //Destroy(ui.GetComponent<DraggableItem>());
                }
            }
        }
    }
}
