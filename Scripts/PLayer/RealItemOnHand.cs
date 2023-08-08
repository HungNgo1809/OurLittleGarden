using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealItemOnHand : MonoBehaviour
{
    public GameObject[] itemOnHand;

    GameObject curObjTool;

    /*
    public void ChangeTool()
    {
        for (int i = 0; i < itemOnHand.Length; i++)
        {
            if (itemOnHand[i].name == HandButtonManager.Instance.curTool)
            {
                curObjTool.SetActive(false);

                itemOnHand[i].SetActive(true);
                curObjTool = itemOnHand[i];
            }
        }
    }*/    
}
