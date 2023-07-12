using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static DataManager;

public class InventoryButton : MonoBehaviour
{
    public Text plusMoney;
    public DataManager dataManager;
    public Transform scrollPosition;
    public GameObject prefab;
    // Start is called before the first frame update\
    public bool isClicked = false;
    public Image image;
    public InputField sellingMoney;
    public bool isOnSellingMode = false;
    public bool isOnUpdatingMode = false;
    private Button button;
    
    private InventorySlot slot;

   

    public SellOnlineManager sellOnlineManager;
    void Start()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        slot = GetComponent<InventorySlot>();
   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    
    public void OnButtonClick()
    {
        if(isOnSellingMode)
        {
            if (button.enabled && transform.childCount > 0)
            {
                isClicked = !isClicked; //


                // hoi thua khong nhi
                slot.curItem = dataManager.SearchItemInDataByObjectId(transform.GetChild(0).name);
                // slot.UpdateInventoryPosition();
                if (isClicked)
                {
                    if (slot.curItem.money != 0)
                    {

                        image.color = new Color(1f, 0.8117647f, 0.4509804f, 1f); // Set the clicked color
                        int money = int.Parse(plusMoney.text);
                        if (dataManager.SearchNPCAffectionByName("Isabella") != null)
                        {
                            Affection npc = dataManager.SearchNPCAffectionByName("Isabella");
                            money += slot.curItem.money + Mathf.RoundToInt((slot.curItem.money * npc.level) / 30f);
                         
                        }
                        else
                        {
                            //khong tim thay NPC trong data
                            money += slot.curItem.money;
                        }
                      

                        plusMoney.text = money.ToString();
                    }
                }
                else
                {
                    if (slot.curItem.money != 0)
                    {
                        image.color = new Color(238 / 255f, 236 / 255f, 178 / 255f, 1f); // Set the default color
                        int money = int.Parse(plusMoney.text);
                        if (dataManager.SearchNPCAffectionByName("Isabella") != null)
                        {
                            Affection npc = dataManager.SearchNPCAffectionByName("Isabella");
                            money -= slot.curItem.money + Mathf.RoundToInt((slot.curItem.money * npc.level) / 30f);
                        }
                        else
                        {
                            //khong tim thay NPC trong data
                            money -= slot.curItem.money;
                        }

                        plusMoney.text = money.ToString();
                    }
                }
            }
        }
        if(isOnUpdatingMode)
        {
            if (button.enabled && transform.childCount > 0)
            {
                isClicked = !isClicked; //


                // hoi thua khong nhi
                slot.curItem = dataManager.SearchItemInDataByObjectId(transform.GetChild(0).name);
                // slot.UpdateInventoryPosition();
                if (isClicked)
                {

                    if (slot.curItem.money != 0)
                    {
                        if (sellOnlineManager != null)
                        {
                            sellOnlineManager.SelectItem(slot.curItem);
                        }

                        image.color = new Color(1f, 0.8117647f, 0.4509804f, 1f); // Set the clicked color
                        GameObject clone = Instantiate(prefab , scrollPosition);
                        clone.transform.SetParent(scrollPosition); // Set the same parent as the original object
                        COSelling cOSelling = clone.GetComponent<COSelling>();
                        cOSelling.curItem = slot.curItem;
                        GameObject child = Instantiate(gameObject.transform.GetChild(0).gameObject, clone.transform);
                        child.transform.SetParent(clone.transform);
                        DraggableItem draggableItem = child.GetComponent<DraggableItem>();
                        draggableItem.isSellingMode = true;
                        clone.name = cOSelling.curItem.prefabItemID;
                        child.name = cOSelling.curItem.objectItemID;
                        Text input = clone.GetComponentInChildren<Text>();
                        input.text = cOSelling.curItem.money.ToString();

                        //cOSelling.moneyText = sellingMoney;
                        //cOSelling.inputfield = input;
                        //int sellingMoney_int = int.Parse(sellingMoney.text);
                        //sellingMoney_int += cOSelling.curItem.money;
                        //int initialSellingMoney_int = sellingMoney_int;
                        //cOSelling.firstMoney = initialSellingMoney_int;
                        if (sellingMoney.text == "")
                        {
                            
                            Text placeholder_SellingMoney = sellingMoney.placeholder.GetComponent<Text>();
                            int totalMoney = 0;
                            COSelling[] COselling = scrollPosition.GetComponentsInChildren<COSelling>();
                            foreach (COSelling co in COselling)
                            {
                                totalMoney += co.curItem.money;
                                placeholder_SellingMoney.text = totalMoney.ToString();
                            }
                        }
                   

                    }

                }
                else
                {
                    if (slot.curItem.money != 0)
                    {
                        if (sellOnlineManager != null)
                        {
                            sellOnlineManager.DeSelectItem(slot.curItem);
                        }

                        image.color = new Color(238 / 255f, 236 / 255f, 178 / 255f, 1f); // Set the default color
                        COSelling[] COselling = scrollPosition.GetComponentsInChildren<COSelling>();
                        COSelling existingSlot = COselling.FirstOrDefault(p => p.curItem.objectItemID == slot.curItem.objectItemID);
                        if (existingSlot != null)
                        {
                            // Destroy the existing slot
                            Destroy(existingSlot.gameObject);
                        }
                        /*
                        image.color = new Color(238 / 255f, 236 / 255f, 178 / 255f, 1f); // Set the default color
                        COSelling[] COselling = scrollPosition.GetComponentsInChildren<COSelling>();
                        COSelling existingSlot = COselling.FirstOrDefault(p => p.curItem.objectItemID == slot.curItem.objectItemID);
                        if(COselling.Length == 0)
                        {
                            sellingMoney.text = "0";
                        }
                     
                        if(existingSlot.inputfield.text == "" || existingSlot.inputfield.text == "0")
                        {
                            int sellingMoney_int = int.Parse(sellingMoney.text);
                            sellingMoney_int -= existingSlot.curItem.money;
                            sellingMoney.text = sellingMoney_int.ToString();
                        }
                        else
                        {
                            int sellingMoney_int = int.Parse(sellingMoney.text);
                            int existingMoney = int.Parse(existingSlot.inputfield.text);
                            sellingMoney_int -= existingMoney;
                            sellingMoney.text = sellingMoney_int.ToString();


                            if (sellOnlineManager != null)
                            {
                                sellOnlineManager.DeSelectItem(slot.curItem);
                                sellOnlineManager.curPack.price = sellingMoney_int;
                            }
                        }
                       
                        if (existingSlot != null)
                        {
                            // Destroy the existing slot
                            Destroy(existingSlot.gameObject);
                        }
                         */

                    }


                }
            }
        }
    }
}
