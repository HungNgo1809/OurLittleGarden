
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    public Transform parentAfterDrag;

    public Transform lastParent;

    //public DataManager dataManager;

    public bool isSellingMode = false;
    public void OnBeginDrag(PointerEventData eventData)
    {
       
        
        if(!isSellingMode)
        {
            lastParent = parentAfterDrag = transform.parent;
            transform.SetParent(transform.root);

            transform.SetAsLastSibling();
            image.raycastTarget = false;
        }
        /*
         *  lastParent = parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);

        transform.SetAsLastSibling();
        image.raycastTarget = false;
        */
    }

    public void OnDrag(PointerEventData eventData)
    {
        //  transform.position = Input.mousePosition;
        if (!isSellingMode)
        {
            transform.position = Input.mousePosition;
        }

    }
    public void OnEndDrag(PointerEventData eventData)
    {

        if (!isSellingMode)
        {
            if (lastParent != parentAfterDrag)
            {
                ChangeParent(transform, parentAfterDrag);
            }
            else
            {
                ChangeParent(transform, lastParent);
            }
            image.raycastTarget = true;
        }
        /*
        if (lastParent != parentAfterDrag)
        {
            ChangeParent(transform, parentAfterDrag);
        }
        else
        {
            ChangeParent(transform, lastParent);
        }
        image.raycastTarget = true;
        */
    }

    public void ChangeParent(Transform child, Transform newParrent)
    {
        //Debug.Log(".");
        child.SetParent(newParrent);
    }
}
