using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class HandSlot : MonoBehaviour, IDropHandler
{
    public DataManager dataManager;
    public DataManager.InventoryData curItem;
    public bool isAnimation;
    public Transform curUi;
    //public DestroyHandUi destroyHand;
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();

        //Swap if it already have item in slot
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

        // change curUi
        StartCoroutine(SetCurUi());
        //update to data
        UpdateHandButton(dropped);

    }

    IEnumerator SetCurUi()
    {
        yield return new WaitForSeconds(0.15f);
        if (transform.childCount > 0)
            curUi = transform.GetChild(0);
    }
    
    public void SwapUi(DraggableItem prev, DraggableItem cur)
    {
        prev.parentAfterDrag = cur.lastParent;
        cur.parentAfterDrag = transform;

        prev.ChangeParent(prev.transform, prev.parentAfterDrag);
        UpdateHandButton(prev.gameObject);
        //cur.ChangeParent(cur.transform, cur.parentAfterDrag);
    }

    public void UpdateHandButton(GameObject dropped)
    {
        //Update hand button for item
        if (dropped.name != null)
        {
            curItem = dataManager.SearchItemInDataByObjectId(dropped.name);

            curItem.button = int.Parse(gameObject.name);
            curItem.invPos = 0;
        }
    }
}
