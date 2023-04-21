using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    public Transform parentAfterDrag;

    public Transform lastParent;

    public DataManager dataManager;
    public void OnBeginDrag(PointerEventData eventData)
    {
        lastParent = parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);

        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if(lastParent != parentAfterDrag)
        {
            Transform item = Instantiate(transform);
            //item.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
            item.name = transform.name;
            item.SetParent(parentAfterDrag);
        }

        transform.SetParent(lastParent);
        image.raycastTarget = true;
    }
}
