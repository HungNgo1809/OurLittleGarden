using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public DataManager dataManager;
    public DataManager.InventoryData curItem;

    public Transform curUi;
    //public DisplayInventory display;
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
        if(!draggableItem.isSellingMode && draggableItem != null && dropped != null) // tao moi the khac nul o day
        {
            if (transform.childCount > 0)
            {
                //UpdateHandButton(transform.GetComponentInChildren<DraggableItem>().gameObject);
                SwapUi(transform.GetComponentInChildren<DraggableItem>(), draggableItem);
            }
            else
            {
                // set transform only if slot empty
                draggableItem.parentAfterDrag = transform;
            }


            //update to data
            UpdateInventoryPosition(draggableItem);
            // change curUi
            StartCoroutine(SetCurUi());
        }
        //Swap if it already have item in slot
       
    }

    IEnumerator SetCurUi()
    {
        yield return new WaitForSeconds(0.15f);
        if(transform.childCount > 0)
        {
            curUi = transform.GetChild(0);
        }    
    }

    public void SwapUi(DraggableItem prev, DraggableItem cur)
    {
        prev.parentAfterDrag = cur.lastParent;
        cur.parentAfterDrag = transform;

        prev.ChangeParent(prev.transform, prev.parentAfterDrag);
        UpdateInventoryPosition(prev);
        //cur.ChangeParent(cur.transform, cur.parentAfterDrag);
    }

    public void UpdateInventoryPosition(DraggableItem dropped)
    {
        if (dropped.name != null)
        {
            curItem = dataManager.SearchItemInDataByObjectId(dropped.name);

            curItem.invPos = int.Parse(gameObject.name);
            curItem.button = 0;
        }
    }
}
