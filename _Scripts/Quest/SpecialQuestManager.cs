using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialQuestManager : MonoBehaviour
{
    public List<Sprite> iconList;

    public SpecialQuest curItems;

    public GameObject SpecialQuestPanel;

    public GameObject specialQuest;
    public Transform requestView;
    public GameObject confirmBtn;

    public GameObject coinRanking;
    public GameObject reputationRanking;

    public Transform coinView;
    public Transform reputationView;

    public Text coinText;
    public Text reputationText;

    public GameObject itemUi;

    public DataManager dataManager;

    public LeaderboardManager leaderboardManager;
    private void Start()
    {
        StartCoroutine(TakePicture());
        StartCoroutine(ReLoadCurSpecialQuest());
    }
    IEnumerator TakePicture()
    {
        yield return new WaitUntil(() => InventoryItem.Instance != null);
        foreach(InventoryItem.ItemData item in InventoryItem.Instance.items)
        {
            iconList.Add(item.objUI.GetComponent<Image>().sprite);
        }
    }
    IEnumerator ReLoadCurSpecialQuest()
    {
        yield return new WaitUntil(() => dataManager.isLoadedData);

        if(dataManager.curSpecialQuest != null)
        {
            curItems = dataManager.curSpecialQuest;
        }
    }    
    public void CreateNewQuest()
    {
        ClearSpecialQuest();

        int numberTypeItem = Random.Range(1, 4);

        for(int i = 0; i < numberTypeItem; i++)
        {
            if(InventoryItem.Instance != null)
            {
                int itemIndex = Random.Range(0, InventoryItem.Instance.items.Count);

                InventoryItem.ItemData tmpItem = InventoryItem.Instance.items[itemIndex];

                Item item = new Item();
                item.prefabItemID = tmpItem.prefabItemID;
                item.imageIndex = itemIndex;
                item.money = tmpItem.money;

                ItemNeed itemNeed = new ItemNeed();
                itemNeed.item = item;
                itemNeed.number = Random.Range(1,6);

                curItems.items.Add(itemNeed);
                curItems.moneyReward = curItems.moneyReward + InventoryItem.Instance.items[itemIndex].money * 2 * itemNeed.number;
            }
        }

        curItems.reputationReward = (curItems.moneyReward / 100) + 1;
        updateUiMaterial();

        OnClickCheckComplete();
        dataManager.curSpecialQuest = curItems;
    }    
    public void ChangeQuest()
    {
        ClearSpecialQuest();
        //Clear Ui
        //Gọi tạo quest mới
        CreateNewQuest();
    }
    public void CompleteQuest()
    {
        //Neu dung thi thuc hien

        //Tắt Ui và chạy VFX/animation thể hiện reward đã về
        CloseSpecialQuestPanel();
        //Cong tien vao dataManager
        dataManager.coins = dataManager.coins + curItems.moneyReward;
        //Cong uy tin vao dataManager
        dataManager.reputation = dataManager.reputation + curItems.reputationReward;
        //Xoa do
        DeleteGoodsInInventory();
        //Đổi quest mới
        ChangeQuest();

        QuestManager.Instance.CheckQuestRequirementSpecialQuest();
        QuestManager.Instance.CheckSideQuestRequirementSpecialQuest();
    }
    public void DeleteGoodsInInventory()
    {
        foreach (ItemNeed itemsNeed in curItems.items)
        {
            for (int i = 0; i < itemsNeed.number; i++)
            {
                //Debug.Log(mat.prefabItemId);
                dataManager.RemoveItemInDataByPrefabId(itemsNeed.item.prefabItemID);

                DisplayInventory.Instance.ClearAll();
                DisplayInventory.Instance.UpdateUI();
            }
        }
    }    
    public bool CheckCompleteQuest()
    {
        foreach (ItemNeed itemsNeed in curItems.items)
        {
            if (itemsNeed.number > CountItem(itemsNeed.item.prefabItemID))
            {
                return false;
            }
        }

        return true;

    }
    public void OnClickCheckComplete()
    {
        if(CheckCompleteQuest())
        {
            if(!confirmBtn.activeSelf)
            {
                confirmBtn.SetActive(true);
            }
        }
        else
        {
            if(confirmBtn.activeSelf)
            {
                confirmBtn.SetActive(false);
            }
        }
    }
    public void updateUiMaterial()
    {
        ClearUi();
        foreach(ItemNeed itemNeed in curItems.items)
        {
            GameObject ui = Instantiate(itemUi);

            ui.GetComponent<Image>().sprite = iconList[itemNeed.item.imageIndex];
            ui.GetComponentInChildren<Text>().text = CountItem(itemNeed.item.prefabItemID) + " / " + itemNeed.number;

            ui.transform.SetParent(requestView);
            ui.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
        coinText.text = curItems.moneyReward.ToString();
        reputationText.text = curItems.reputationReward.ToString();
    }
    public void ClearUi()
    {
        foreach(Transform child in requestView)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in coinView)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in reputationView)
        {
            Destroy(child.gameObject);
        }
    }
    public void OnClickSpecialQuest()
    {
        if(curItems.items.Count == 0)
        {
            CreateNewQuest();
        }
        else
        {
            updateUiMaterial();
        }

        specialQuest.SetActive(true);
        coinRanking.SetActive(false);
        reputationRanking.SetActive(false);
    }
    public void OnClickCoinRanking()
    {
        ClearUi();

        leaderboardManager.GetLeaderboard(100, "coins");
        specialQuest.SetActive(false);
        coinRanking.SetActive(true);
        reputationRanking.SetActive(false);
    }
    public void OnClickReputationRanking()
    {
        ClearUi();

        leaderboardManager.GetLeaderboard(100, "reputation");
        specialQuest.SetActive(false);
        coinRanking.SetActive(false);
        reputationRanking.SetActive(true);
    }
        
    public int CountItem(string prefabId)
    {
        return dataManager.CountNumberItemWithSamePrefabId(prefabId);
    }    
    public void CloseSpecialQuestPanel()
    {
        SpecialQuestPanel.SetActive(false);
    }
    public void ClearSpecialQuest()
    {
        curItems.items.Clear();
        curItems.moneyReward = 0;
        curItems.reputationReward = 0;
    }    
    [System.Serializable]
    public class ItemNeed
    {
        public Item item;
        public int number;
    }

    [System.Serializable]
    public class SpecialQuest
    {
        public List<ItemNeed> items;

        public int moneyReward;
        public int reputationReward;
    }
    [System.Serializable]
    public class Item
    {
        public string prefabItemID;

        //public GameObject realObj;
        public int imageIndex;

        public int money;
    }
}
