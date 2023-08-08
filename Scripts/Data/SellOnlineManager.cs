using Newtonsoft.Json;
using PlayFab;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewData", menuName = "SellOnlineManager")]
public class SellOnlineManager : ScriptableObject
{
    public DataManager dataManager;

    public List<itemPack> serVerSellingPack;
    public List<itemPack> yourSellingPack;

    public itemPack curPack;

    public string curPackName;

    int tmpCoin;
    int plusCoin;
    string tmpId;
    public void SetCurPackName(string text)
    {
        curPackName = text;
    }    
    public void PushSellItem()
    {
        dataManager.SaveDataToOtherPlayer("E5637C85D555F071", "sellingPack", JsonConvert.SerializeObject(serVerSellingPack));
    } 
    
    public void LoadServerSellingPack()
    {
        LoadDataFromPlayFab("sellingPack");
    }
    public void LoadYourSellingPack()
    {
        yourSellingPack.Clear();
        foreach(itemPack pack in serVerSellingPack)
        {
            if(pack.senderId == dataManager.userId)
            {
                yourSellingPack.Add(pack);
            }    
        }    
    }    
    public void LoadDataFromPlayFab(string key)
    {
        var request = new PlayFab.AdminModels.GetUserDataRequest
        {
            PlayFabId = "E5637C85D555F071",
            Keys = new List<string> { key }
        };
        PlayFabAdminAPI.GetUserData(request, LoadDataSuccess, Err);
    }

    private void Err(PlayFabError obj)
    {
        Debug.Log(obj);
    }
    private void LoadDataSuccess(PlayFab.AdminModels.GetUserDataResult result)
    {
        if (result.Data == null)
        {
            return;
        }
        if (result.Data.ContainsKey("sellingPack"))
        {
            serVerSellingPack = JsonConvert.DeserializeObject<List<itemPack>>(result.Data["sellingPack"].Value);
        }
    }
    public void SelectItem(DataManager.InventoryData item)
    {
        if(curPack.items != null)
        {
            curPack.items.Add(item);
        }
        else
        {
            curPack.items = new List<DataManager.InventoryData>();

            curPack.items.Add(item);
        }    

    }
    public void DeSelectItem(DataManager.InventoryData item)
    {
        curPack.items.Remove(item);
    }
    public void ConfirmSell(InputField price)
    {
        //add item pack
        curPack.id = dataManager.userId + serVerSellingPack.Count + UnityEngine.Random.Range(0f, 1000.0f).ToString();
        curPack.senderId = dataManager.userId;
        curPack.name = curPackName;
      
        if(price.text == "")
        {
            Text placeholder_SellingMoney = price.placeholder.GetComponent<Text>();
            curPack.price = int.Parse(placeholder_SellingMoney.text);
        }
        else
        {
            curPack.price = int.Parse(price.text);
        }
       

        serVerSellingPack.Add(curPack);

        //Đẩy lên db
        PushSellItem();

        //reset pack
        curPack = new itemPack();
        UiManager.Instance.RemoveSoldItem();
        //Thông báo thành công
    }
    public void ConfirmBuy()
    {
        if(dataManager.coins >= curPack.price)
        {
            //tru tien
            dataManager.coins = dataManager.coins - curPack.price;

            //add item
            foreach (DataManager.InventoryData item in curPack.items)
            {
                dataManager.AddItemToInventoryData(item.prefabItemID, item.type, item.speType, item.durability, int.Parse(DisplayInventory.Instance.SearchFirstEmptyUiSlot().name), item.money);
                DisplayInventory.Instance.UpdateUI();
            }

            //thông báo thành công
            //thêm tiền cho chủ pack
            AddMoneyForSeller(curPack);
            //Xóa pack
            serVerSellingPack.Remove(curPack);

            //Cập nhật data
            PushSellItem();

            //Ui
            SellOnlineUi.Instance.DisplayBuyList();
        }
        else
        {
            //Báo k đủ tiền
        }    

    }
    public void AddMoneyForSeller(itemPack buyPack)
    {
        tmpId = buyPack.senderId;
        plusCoin = buyPack.price;
        LoadMoneyPlayFab(buyPack.senderId, "coins");
    }
    public void LoadMoneyPlayFab(string id, string key)
    {
        var request = new PlayFab.ClientModels.GetUserDataRequest
        {
            PlayFabId = id,
            Keys = new List<string> { key }
        };
        PlayFabClientAPI.GetUserData(request, LoadMoneySuccess, Err);
    }

    private void LoadMoneySuccess(PlayFab.ClientModels.GetUserDataResult result)
    {
        if (result.Data == null)
        {
            return;
        }
        if (result.Data.ContainsKey("coins"))
        {
            if (result.Data.TryGetValue("coins", out var intDataValue))
            {
                if (int.TryParse(intDataValue.Value, out var intValue))
                {
                    tmpCoin = intValue + plusCoin;
                    dataManager.SaveDataToOtherPlayer(tmpId, "coins", JsonConvert.ToString(tmpCoin));

                    tmpCoin = 0;
                    plusCoin = 0;
                    tmpId = "";
                }
            }
        }
    }

    [System.Serializable]
    public class itemPack
    {
        public string id;

        public string senderId;
        public string name;

        public int price;

        public List<DataManager.InventoryData> items;
    }    
}
