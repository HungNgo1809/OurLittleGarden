using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DescriptController : MonoBehaviour , IPointerEnterHandler , IPointerExitHandler
{

   // Name of the UI element to activate/deactivate the panel
    public GameObject description; // Reference to the panel game object

    public void OnPointerEnter(PointerEventData eventData)
    {
        
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);

        // Check if there is any other panel behind the current panel
        foreach (RaycastResult result in results)
        {
        
            if(description != null)
            {
                if (result.gameObject.name == description.name)
                {

                    description.SetActive(true);
                }
            }    
        }
    }

  public void OnPointerExit(PointerEventData eventData)
    {
        if(description != null)
        {
            description.SetActive(false);
        }    
    }
}

