using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandButtonManager : MonoBehaviour
{
    public static HandButtonManager Instance { get; set; }

    public string curTool = "none";

    public DataManager dataManager;

    //add
    public GameObject[] itemOnHand;

    GameObject curObjTool;
    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(this);

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            if(DestroyHandUi.Instance.hands[0].transform.childCount != 0)
            {
                if(dataManager.SearchItemInDataByObjectId(DestroyHandUi.Instance.hands[0].transform.GetChild(0).name) != null)
                {
                    DataManager.InventoryData inv = dataManager.SearchItemInDataByObjectId(DestroyHandUi.Instance.hands[0].transform.GetChild(0).name);

                    curTool = inv.prefabItemID;
                }
            }
            else
            {
                curTool = "none";
            }
            ChangeTool();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (DestroyHandUi.Instance.hands[1].transform.childCount != 0)
            {
                if (dataManager.SearchItemInDataByObjectId(DestroyHandUi.Instance.hands[0].transform.GetChild(0).name) != null)
                {
                    DataManager.InventoryData inv = dataManager.SearchItemInDataByObjectId(DestroyHandUi.Instance.hands[0].transform.GetChild(0).name);

                    curTool = inv.prefabItemID;
                }
            }
            else
            {
                curTool = "none";
            }
            ChangeTool();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (DestroyHandUi.Instance.hands[2].transform.childCount != 0)
            {
                if (dataManager.SearchItemInDataByObjectId(DestroyHandUi.Instance.hands[0].transform.GetChild(0).name) != null)
                {
                    DataManager.InventoryData inv = dataManager.SearchItemInDataByObjectId(DestroyHandUi.Instance.hands[0].transform.GetChild(0).name);

                    curTool = inv.prefabItemID;
                }
            }
            else
            {
                curTool = "none";
            }
            ChangeTool();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (DestroyHandUi.Instance.hands[3].transform.childCount != 0)
            {
                if (dataManager.SearchItemInDataByObjectId(DestroyHandUi.Instance.hands[0].transform.GetChild(0).name) != null)
                {
                    DataManager.InventoryData inv = dataManager.SearchItemInDataByObjectId(DestroyHandUi.Instance.hands[0].transform.GetChild(0).name);

                    curTool = inv.prefabItemID;
                }
            }
            else
            {
                curTool = "none";
            }
            ChangeTool();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (DestroyHandUi.Instance.hands[4].transform.childCount != 0)
            {
                if (dataManager.SearchItemInDataByObjectId(DestroyHandUi.Instance.hands[0].transform.GetChild(0).name) != null)
                {
                    DataManager.InventoryData inv = dataManager.SearchItemInDataByObjectId(DestroyHandUi.Instance.hands[0].transform.GetChild(0).name);

                    curTool = inv.prefabItemID;
                }
            }
            else
            {
                curTool = "none";
            }
            ChangeTool();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            if (DestroyHandUi.Instance.hands[5].transform.childCount != 0)
            {
                if (dataManager.SearchItemInDataByObjectId(DestroyHandUi.Instance.hands[0].transform.GetChild(0).name) != null)
                {
                    DataManager.InventoryData inv = dataManager.SearchItemInDataByObjectId(DestroyHandUi.Instance.hands[0].transform.GetChild(0).name);

                    curTool = inv.prefabItemID;
                }
            }
            else
            {
                curTool = "none";
            }
            ChangeTool();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            if (DestroyHandUi.Instance.hands[6].transform.childCount != 0)
            {
                if (dataManager.SearchItemInDataByObjectId(DestroyHandUi.Instance.hands[0].transform.GetChild(0).name) != null)
                {
                    DataManager.InventoryData inv = dataManager.SearchItemInDataByObjectId(DestroyHandUi.Instance.hands[0].transform.GetChild(0).name);

                    curTool = inv.prefabItemID;
                }
            }
            else
            {
                curTool = "none";
            }
            ChangeTool();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            if (DestroyHandUi.Instance.hands[7].transform.childCount != 0)
            {
                if (dataManager.SearchItemInDataByObjectId(DestroyHandUi.Instance.hands[0].transform.GetChild(0).name) != null)
                {
                    DataManager.InventoryData inv = dataManager.SearchItemInDataByObjectId(DestroyHandUi.Instance.hands[0].transform.GetChild(0).name);

                    curTool = inv.prefabItemID;
                }
            }
            else
            {
                curTool = "none";
            }
            ChangeTool();
        }
    }
    public void ChangeTool()
    {
        if(curTool == "none" && curObjTool != null)
        {
            curObjTool.SetActive(false);
            return;
        }  
        
        for (int i = 0; i < itemOnHand.Length; i++)
        {
            Debug.Log(itemOnHand[i].name);
            if (itemOnHand[i].name == curTool)
            {
                if(curObjTool != null)
                {
                    curObjTool.SetActive(false);
                }    

                itemOnHand[i].SetActive(true);
                curObjTool = itemOnHand[i];
            }
        }
    }
}
