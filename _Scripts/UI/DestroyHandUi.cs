using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyHandUi : MonoBehaviour
{
    public static DestroyHandUi Instance { get; set; }
    public HandSlot[] hands;

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
    }
    public void DestroyItem(int i)
    {
       foreach(Transform item in hands[i].transform)
        {
            Destroy(item.gameObject);
        }
    }
}
