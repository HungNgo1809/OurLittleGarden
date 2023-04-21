using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class HandSlot : MonoBehaviour, IDropHandler
{
    public DataManager dataManager;
    public DataManager.InventoryData curItem;

    public Transform curUi;
    public DestroyHandUi destroyHand;
    public void OnDrop(PointerEventData eventData)
    {
        //reset old item
        if((curItem != null) && (transform.childCount != 0))
        {
            curItem.button = 0;
            if(curUi != null)
            {
                Destroy(curUi.gameObject);
            }
        }


        GameObject dropped = eventData.pointerDrag;
        DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
        draggableItem.parentAfterDrag = transform;

        StartCoroutine(SetCurUi());

        if(dropped.name != null)
        {
            curItem = dataManager.SearchItemInDataByObjectId(dropped.name);

            if(curItem.button != 0)
            {
                //Destroy
                destroyHand.DestroyItem(curItem.button - 1);
            }

            curItem.button = int.Parse(gameObject.name);
        }
    }

    IEnumerator SetCurUi()
    {
        yield return new WaitForSeconds(1.0f);
        curUi = transform.GetChild(0);
    }
}
