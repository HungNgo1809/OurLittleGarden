
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;
using Photon.Pun;
using Photon.Realtime;

public class HandButtonManager : MonoBehaviour
{
    //public static HandButtonManager Instance { get; set; }

    public string curTool = "none";
    public string curToolObjId;
    public string curToolSpeType;
    public string curType;
    public DataManager dataManager;

    //add
    public GameObject[] itemOnHand;

    public GameObject curObjTool;

    public int currentHands;

    public PhotonView photonView;

    public AudioSource switchItemSound;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;
        if(Input.GetKeyDown(KeyCode.Alpha1)) // animatioon bi 1 loi la neu bam qua nhanh thi se bi khong kip cho nho? lai
        {
            for (int i = 0; i < DestroyHandUi.Instance.hands.Length; i++)
            {
                if (DestroyHandUi.Instance.hands[i] != DestroyHandUi.Instance.hands[0])
                {
                    
                    GameObject handsslotGO = DestroyHandUi.Instance.hands[i].gameObject;
                    HandSlot handsSlot = handsslotGO.GetComponent<HandSlot>();
                    RectTransform handRectTransform = handsslotGO.GetComponent<RectTransform>();
       
                    if (handRectTransform.sizeDelta.x == 70f && handRectTransform.sizeDelta.y == 70f  )
                    {
                    
                        Vector2 mytarget = new Vector2(60, 60);
                        handRectTransform.sizeDelta = mytarget;
                    }
                    Image handImage = handsslotGO.GetComponent<Image>();
                    handImage.color = Color.white;
                }
            }
        
            GameObject handsslotGO_ = DestroyHandUi.Instance.hands[0].gameObject;
            HandSlot handsSlot_ = handsslotGO_.GetComponent<HandSlot>();
        
            RectTransform handRectTransform_ = handsslotGO_.GetComponent<RectTransform>();
      
            if (handRectTransform_.sizeDelta.x == 60f && handRectTransform_.sizeDelta.y == 60f  )
            {
            
                Vector2 mytarget = new Vector2(70, 70);
                handRectTransform_.sizeDelta = mytarget;
           

            }

            DestroyHandUi.Instance.hands[0].GetComponent<Image>().color = new Color(1f, 0.8117647f, 0.4509804f, 1f);
            if (DestroyHandUi.Instance.hands[0].transform.childCount != 0)
            {
                if (dataManager.SearchItemInDataByObjectId(DestroyHandUi.Instance.hands[0].transform.GetChild(0).name) != null)
                {
                    DataManager.InventoryData inv = dataManager.SearchItemInDataByObjectId(DestroyHandUi.Instance.hands[0].transform.GetChild(0).name);

                    curTool = inv.prefabItemID;
                    curToolObjId = inv.objectItemID;
                    curToolSpeType = inv.speType;
                    curType = inv.type;
                    currentHands = 1;
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

            for (int i = 0; i < DestroyHandUi.Instance.hands.Length; i++)
            {
                if (DestroyHandUi.Instance.hands[i] != DestroyHandUi.Instance.hands[1])
                {

                    GameObject handsslotGO = DestroyHandUi.Instance.hands[i].gameObject;
                    HandSlot handsSlot = handsslotGO.GetComponent<HandSlot>();
                    RectTransform handRectTransform = handsslotGO.GetComponent<RectTransform>();

                    if (handRectTransform.sizeDelta.x == 70f && handRectTransform.sizeDelta.y == 70f  )
                    {

                        Vector2 mytarget = new Vector2(60, 60);
                        handRectTransform.sizeDelta = mytarget;
                    }
                    Image handImage = handsslotGO.GetComponent<Image>();
                    handImage.color = Color.white;
                }
            }

            GameObject handsslotGO_ = DestroyHandUi.Instance.hands[1].gameObject;
            HandSlot handsSlot_ = handsslotGO_.GetComponent<HandSlot>();

            RectTransform handRectTransform_ = handsslotGO_.GetComponent<RectTransform>();

            if (handRectTransform_.sizeDelta.x == 60f && handRectTransform_.sizeDelta.y == 60f  )
            {

                Vector2 mytarget = new Vector2(70, 70);
                handRectTransform_.sizeDelta = mytarget;

            }
            DestroyHandUi.Instance.hands[1].GetComponent<Image>().color = new Color(1f, 0.8117647f, 0.4509804f, 1f);

            if (DestroyHandUi.Instance.hands[1].transform.childCount != 0)
            {
                if (dataManager.SearchItemInDataByObjectId(DestroyHandUi.Instance.hands[1].transform.GetChild(0).name) != null)
                {
                    DataManager.InventoryData inv = dataManager.SearchItemInDataByObjectId(DestroyHandUi.Instance.hands[1].transform.GetChild(0).name);

                    curTool = inv.prefabItemID;
                    curToolObjId = inv.objectItemID;
                    curToolSpeType = inv.speType;
                    curType = inv.type;
                    currentHands = 2;
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

            for (int i = 0; i < DestroyHandUi.Instance.hands.Length; i++)
            {
                if (DestroyHandUi.Instance.hands[i] != DestroyHandUi.Instance.hands[2])
                {

                    GameObject handsslotGO = DestroyHandUi.Instance.hands[i].gameObject;
                    HandSlot handsSlot = handsslotGO.GetComponent<HandSlot>();
                    RectTransform handRectTransform = handsslotGO.GetComponent<RectTransform>();

                    if (handRectTransform.sizeDelta.x == 70f && handRectTransform.sizeDelta.y == 70f  )
                    {

                        Vector2 mytarget = new Vector2(60, 60);
                        handRectTransform.sizeDelta = mytarget;
                    }
                    Image handImage = handsslotGO.GetComponent<Image>();
                    handImage.color = Color.white;
                }
            }

            GameObject handsslotGO_ = DestroyHandUi.Instance.hands[2].gameObject;
            HandSlot handsSlot_ = handsslotGO_.GetComponent<HandSlot>();

            RectTransform handRectTransform_ = handsslotGO_.GetComponent<RectTransform>();

            if (handRectTransform_.sizeDelta.x == 60f && handRectTransform_.sizeDelta.y == 60f  )
            {

                Vector2 mytarget = new Vector2(70, 70);
                handRectTransform_.sizeDelta = mytarget;

            }
            DestroyHandUi.Instance.hands[2].GetComponent<Image>().color = new Color(1f, 0.8117647f, 0.4509804f, 1f);

            if (DestroyHandUi.Instance.hands[2].transform.childCount != 0)
            {
                if (dataManager.SearchItemInDataByObjectId(DestroyHandUi.Instance.hands[2].transform.GetChild(0).name) != null)
                {

                    DataManager.InventoryData inv = dataManager.SearchItemInDataByObjectId(DestroyHandUi.Instance.hands[2].transform.GetChild(0).name);

                    curTool = inv.prefabItemID;
                    curToolObjId = inv.objectItemID;
                    curToolSpeType = inv.speType;
                    curType = inv.type;
                    currentHands = 3;
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

            for (int i = 0; i < DestroyHandUi.Instance.hands.Length; i++)
            {
                if (DestroyHandUi.Instance.hands[i] != DestroyHandUi.Instance.hands[3])
                {

                    GameObject handsslotGO = DestroyHandUi.Instance.hands[i].gameObject;
                    HandSlot handsSlot = handsslotGO.GetComponent<HandSlot>();
                    RectTransform handRectTransform = handsslotGO.GetComponent<RectTransform>();

                    if (handRectTransform.sizeDelta.x == 70f && handRectTransform.sizeDelta.y == 70f  )
                    {

                        Vector2 mytarget = new Vector2(60, 60);
                        handRectTransform.sizeDelta = mytarget;
                    }
                    Image handImage = handsslotGO.GetComponent<Image>();
                    handImage.color = Color.white;
                }
            }

            GameObject handsslotGO_ = DestroyHandUi.Instance.hands[3].gameObject;
            HandSlot handsSlot_ = handsslotGO_.GetComponent<HandSlot>();

            RectTransform handRectTransform_ = handsslotGO_.GetComponent<RectTransform>();

            if (handRectTransform_.sizeDelta.x == 60f && handRectTransform_.sizeDelta.y == 60f  )
            {

                Vector2 mytarget = new Vector2(70, 70);
                handRectTransform_.sizeDelta = mytarget;

            }
            DestroyHandUi.Instance.hands[3].GetComponent<Image>().color = new Color(1f, 0.8117647f, 0.4509804f, 1f);

            if (DestroyHandUi.Instance.hands[3].transform.childCount != 0)
            {
            
                if (dataManager.SearchItemInDataByObjectId(DestroyHandUi.Instance.hands[3].transform.GetChild(0).name) != null)
                {
   
                    DataManager.InventoryData inv = dataManager.SearchItemInDataByObjectId(DestroyHandUi.Instance.hands[3].transform.GetChild(0).name);

                    curTool = inv.prefabItemID;
                    curToolObjId = inv.objectItemID;
                    curToolSpeType = inv.speType;
                    curType = inv.type;
                    currentHands = 4;
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

            for (int i = 0; i < DestroyHandUi.Instance.hands.Length; i++)
            {
                if (DestroyHandUi.Instance.hands[i] != DestroyHandUi.Instance.hands[4])
                {

                    GameObject handsslotGO = DestroyHandUi.Instance.hands[i].gameObject;
                    HandSlot handsSlot = handsslotGO.GetComponent<HandSlot>();
                    RectTransform handRectTransform = handsslotGO.GetComponent<RectTransform>();

                    if (handRectTransform.sizeDelta.x == 70f && handRectTransform.sizeDelta.y == 70f  )
                    {

                        Vector2 mytarget = new Vector2(60, 60);
                        handRectTransform.sizeDelta = mytarget;
                    }
                    Image handImage = handsslotGO.GetComponent<Image>();
                    handImage.color = Color.white;
                }
            }

            GameObject handsslotGO_ = DestroyHandUi.Instance.hands[4].gameObject;
            HandSlot handsSlot_ = handsslotGO_.GetComponent<HandSlot>();

            RectTransform handRectTransform_ = handsslotGO_.GetComponent<RectTransform>();

            if (handRectTransform_.sizeDelta.x == 60f && handRectTransform_.sizeDelta.y == 60f  )
            {

                Vector2 mytarget = new Vector2(70, 70);
                handRectTransform_.sizeDelta = mytarget;

            }
            DestroyHandUi.Instance.hands[4].GetComponent<Image>().color = new Color(1f, 0.8117647f, 0.4509804f, 1f);

            if (DestroyHandUi.Instance.hands[4].transform.childCount != 0)
            {
                
                if (dataManager.SearchItemInDataByObjectId(DestroyHandUi.Instance.hands[4].transform.GetChild(0).name) != null)
                {
                    DataManager.InventoryData inv = dataManager.SearchItemInDataByObjectId(DestroyHandUi.Instance.hands[4].transform.GetChild(0).name);

                    curTool = inv.prefabItemID;
                    curToolObjId = inv.objectItemID;
                    curToolSpeType = inv.speType;
                    curType = inv.type;
                    currentHands = 5;
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

            for (int i = 0; i < DestroyHandUi.Instance.hands.Length; i++)
            {
                if (DestroyHandUi.Instance.hands[i] != DestroyHandUi.Instance.hands[5])
                {

                    GameObject handsslotGO = DestroyHandUi.Instance.hands[i].gameObject;
                    HandSlot handsSlot = handsslotGO.GetComponent<HandSlot>();
                    RectTransform handRectTransform = handsslotGO.GetComponent<RectTransform>();

                    if (handRectTransform.sizeDelta.x == 70f && handRectTransform.sizeDelta.y == 70f  )
                    {

                        Vector2 mytarget = new Vector2(60, 60);
                        handRectTransform.sizeDelta = mytarget;
                    }
                    Image handImage = handsslotGO.GetComponent<Image>();
                    handImage.color = Color.white;
                }
            }

            GameObject handsslotGO_ = DestroyHandUi.Instance.hands[5].gameObject;
            HandSlot handsSlot_ = handsslotGO_.GetComponent<HandSlot>();

            RectTransform handRectTransform_ = handsslotGO_.GetComponent<RectTransform>();

            if (handRectTransform_.sizeDelta.x == 60f && handRectTransform_.sizeDelta.y == 60f  )
            {

                Vector2 mytarget = new Vector2(70, 70);
                handRectTransform_.sizeDelta = mytarget;

            }
            DestroyHandUi.Instance.hands[5].GetComponent<Image>().color = new Color(1f, 0.8117647f, 0.4509804f, 1f);

            if (DestroyHandUi.Instance.hands[5].transform.childCount != 0)
            {
               
                if (dataManager.SearchItemInDataByObjectId(DestroyHandUi.Instance.hands[5].transform.GetChild(0).name) != null)
                {
                    DataManager.InventoryData inv = dataManager.SearchItemInDataByObjectId(DestroyHandUi.Instance.hands[5].transform.GetChild(0).name);

                    curTool = inv.prefabItemID;
                    curToolObjId = inv.objectItemID;
                    curToolSpeType = inv.speType;
                    curType = inv.type;
                    currentHands = 6;
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

            for (int i = 0; i < DestroyHandUi.Instance.hands.Length; i++)
            {
                if (DestroyHandUi.Instance.hands[i] != DestroyHandUi.Instance.hands[6])
                {

                    GameObject handsslotGO = DestroyHandUi.Instance.hands[i].gameObject;
                    HandSlot handsSlot = handsslotGO.GetComponent<HandSlot>();
                    RectTransform handRectTransform = handsslotGO.GetComponent<RectTransform>();

                    if (handRectTransform.sizeDelta.x == 70f && handRectTransform.sizeDelta.y == 70f  )
                    {

                        Vector2 mytarget = new Vector2(60, 60);
                        handRectTransform.sizeDelta = mytarget;
                    }
                    Image handImage = handsslotGO.GetComponent<Image>();
                    handImage.color = Color.white;
                }
            }

            GameObject handsslotGO_ = DestroyHandUi.Instance.hands[6].gameObject;
            HandSlot handsSlot_ = handsslotGO_.GetComponent<HandSlot>();

            RectTransform handRectTransform_ = handsslotGO_.GetComponent<RectTransform>();

            if (handRectTransform_.sizeDelta.x == 60f && handRectTransform_.sizeDelta.y == 60f  )
            {

                Vector2 mytarget = new Vector2(70, 70);
                handRectTransform_.sizeDelta = mytarget;

            }
            DestroyHandUi.Instance.hands[6].GetComponent<Image>().color = new Color(1f, 0.8117647f, 0.4509804f, 1f);

            if (DestroyHandUi.Instance.hands[6].transform.childCount != 0)
            {
              
                if (dataManager.SearchItemInDataByObjectId(DestroyHandUi.Instance.hands[6].transform.GetChild(0).name) != null)
                {    
                    DataManager.InventoryData inv = dataManager.SearchItemInDataByObjectId(DestroyHandUi.Instance.hands[6].transform.GetChild(0).name);

                    curTool = inv.prefabItemID;
                    curToolObjId = inv.objectItemID;
                    curToolSpeType = inv.speType;
                    curType = inv.type;
                    currentHands = 7;
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

            for (int i = 0; i < DestroyHandUi.Instance.hands.Length; i++)
            {
                if (DestroyHandUi.Instance.hands[i] != DestroyHandUi.Instance.hands[7])
                {

                    GameObject handsslotGO = DestroyHandUi.Instance.hands[i].gameObject;
                    HandSlot handsSlot = handsslotGO.GetComponent<HandSlot>();
                    RectTransform handRectTransform = handsslotGO.GetComponent<RectTransform>();

                    if (handRectTransform.sizeDelta.x == 70f && handRectTransform.sizeDelta.y == 70f  )
                    {

                        Vector2 mytarget = new Vector2(60, 60);
                        handRectTransform.sizeDelta = mytarget;
                    }
                    Image handImage = handsslotGO.GetComponent<Image>();
                    handImage.color = Color.white;
                }
            }

            GameObject handsslotGO_ = DestroyHandUi.Instance.hands[7].gameObject;
            HandSlot handsSlot_ = handsslotGO_.GetComponent<HandSlot>();

            RectTransform handRectTransform_ = handsslotGO_.GetComponent<RectTransform>();

            if (handRectTransform_.sizeDelta.x == 60f && handRectTransform_.sizeDelta.y == 60f  )
            {

                Vector2 mytarget = new Vector2(70, 70);
                handRectTransform_.sizeDelta = mytarget;

            }
            DestroyHandUi.Instance.hands[7].GetComponent<Image>().color = new Color(1f, 0.8117647f, 0.4509804f, 1f);

            if (DestroyHandUi.Instance.hands[7].transform.childCount != 0)
            {
                if (dataManager.SearchItemInDataByObjectId(DestroyHandUi.Instance.hands[7].transform.GetChild(0).name) != null)
                {
                    DataManager.InventoryData inv = dataManager.SearchItemInDataByObjectId(DestroyHandUi.Instance.hands[7].transform.GetChild(0).name);

                    curTool = inv.prefabItemID;
                    curToolObjId = inv.objectItemID;
                    curToolSpeType = inv.speType;
                    curType = inv.type;
                    currentHands = 8;
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
        //Debug.Log("ChangeTool");
        switchItemSound.Play();
        if(curTool == "none" && curObjTool != null)
        {
            curObjTool.SetActive(false);
            return;
        }  
        
        for (int i = 0; i < itemOnHand.Length; i++)
        {
            //Debug.Log(itemOnHand[i].name);
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
    /*
    IEnumerator ResizeCellSize(GridLayoutGroup gridLayoutGroup, Vector2 targetCellSize, float duration)
    {
        Vector2 startCellSize = gridLayoutGroup.cellSize;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            // Interpolate the cellSize values
            Vector2 newCellSize = Vector2.Lerp(startCellSize, targetCellSize, t);

            // Assign the new cellSize to the GridLayoutGroup
            gridLayoutGroup.cellSize = newCellSize;

            yield return null;
        }

        // Ensure that the cellSize is set to the target value after the interpolation
        gridLayoutGroup.cellSize = targetCellSize;
    }
    
    IEnumerator ResizeCellSizeBigger(GridLayoutGroup gridLayoutGroup, Vector2 targetCellSize, float duration)
    {
        Vector2 startCellSize = gridLayoutGroup.cellSize;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            // Interpolate the cellSize values
            Vector2 newCellSize = Vector2.Lerp(targetCellSize , startCellSize, t);
         
            // Assign the new cellSize to the GridLayoutGroup
            gridLayoutGroup.cellSize = newCellSize;

            yield return null;
        }

        // Ensure that the cellSize is set to the target value after the interpolation
        gridLayoutGroup.cellSize = targetCellSize;
    }
    */
}
