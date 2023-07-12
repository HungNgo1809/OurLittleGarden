using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellOnlineUi : MonoBehaviour
{
    public static SellOnlineUi Instance { get; set; }

    public GameObject buyItemPanel;
    public Text packPrice;
    public Transform parrent;

    public GameObject ownItemPanel;
    public Transform parrentOwn;

    public GameObject itemUi;

    public GameObject buyUiComponent;
    public GameObject ownUiComPonent;
    public Transform parrentComponent;

    public SellOnlineManager sellOnlineManager;
    public DataManager dataManager;
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public void OnClickBuyPack(SellOnlineManager.itemPack pack)
    {
        if(pack.senderId == dataManager.userId)
        {
            OnClickSellPack(pack);
            return;
        }

        Clear(parrent);
        //set giá
        packPrice.text = pack.price.ToString() + "$";
        //active panel
        buyItemPanel.SetActive(true);
        //khởi tạo item
        CreateItemInPackUi(parrent, pack);
        //
        sellOnlineManager.curPack = pack;
    }
    public void OnClickSellPack(SellOnlineManager.itemPack pack)
    {
        Clear(parrentOwn);
        //active panel
        ownItemPanel.SetActive(true);
        //khởi tạo item
        CreateItemInPackUi(parrentOwn, pack);
    }
    public void CreateItemInPackUi(Transform parrent, SellOnlineManager.itemPack pack)
    {
        foreach (DataManager.InventoryData item in pack.items)
        {
            GameObject spawn = Instantiate(itemUi);
            spawn.transform.SetParent(parrent);
            spawn.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

            //spawn.GetComponentInChildren<Text>().text = ConvertToTitleCase(item.prefabItemID);
            spawn.GetComponent<sellOnlineItemUiPrefabComponent>().icon.sprite = InventoryItem.Instance.SearchItemByID(item.prefabItemID).objUI.GetComponent<Image>().sprite;
        }
    }
    public void DisplayBuyList()
    {
        Clear(parrentComponent);

        foreach(SellOnlineManager.itemPack pack in sellOnlineManager.serVerSellingPack)
        {
            GameObject spawn = Instantiate(buyUiComponent);
            spawn.transform.SetParent(parrentComponent);
            spawn.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

            if (spawn.GetComponent<SellOnlineUiComponent>() != null)
            {
                spawn.GetComponent<SellOnlineUiComponent>().pack = pack;

                spawn.GetComponent<SellOnlineUiComponent>().packName.text = pack.name;
                spawn.GetComponent<SellOnlineUiComponent>().price.text = pack.price.ToString();
            }
        }
    }
    public void DisplayOwnList()
    {
        Clear(parrentComponent);

        foreach (SellOnlineManager.itemPack pack in sellOnlineManager.yourSellingPack)
        {
            GameObject spawn = Instantiate(buyUiComponent);
            spawn.transform.SetParent(parrentComponent);
            spawn.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

            if (spawn.GetComponent<SellOnlineUiComponent>() != null)
            {
                spawn.GetComponent<SellOnlineUiComponent>().pack = pack;

                spawn.GetComponent<SellOnlineUiComponent>().packName.text = pack.name;
                spawn.GetComponent<SellOnlineUiComponent>().price.text = pack.price.ToString();
            }
        }
    }
    public void Clear(Transform parrent)
    {
        foreach(Transform child in parrent)
        {
            Destroy(child.gameObject);
        }
    }
    public string ConvertToTitleCase(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        string output = char.ToUpper(input[0]) + input.Substring(1);

        for (int i = 1; i < output.Length; i++)
        {
            if (char.IsUpper(output[i]))
            {
                output = output.Insert(i, " ");
                i++;
            }
        }

        output.ToUpper();
        return output;
    }
}
