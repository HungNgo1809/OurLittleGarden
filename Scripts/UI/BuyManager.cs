using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BuyManager : MonoBehaviour
{
    public DataManager dataManager;

    public int number = 0;

    public Text numberUi;

    public int price;

    public Image icon;

    public Image aniamtionIcon;

    public Animator book;

    public InventoryItem.ItemData good;


    public void OnClickBuy()
    {
        if (DisplayInventory.Instance.SearchFirstEmptyUiSlot() != null)
        {
            int number_ = int.Parse(numberUi.text);
            if (price * number <= dataManager.coins && number_ > 0)
            {
                //mua đồ
                Buy();
                aniamtionIcon.sprite = icon.sprite;
                book.Play("BuyItem",0,0);
                numberUi.text = "0";
                // hiển thị mua thành công
            }
            else if (price * number >= dataManager.coins)
            {
                // hiển thị mua thất bại : hien thi thieu tien
                numberUi.text = "0";
                UiManager.Instance.StartCoroutine(UiManager.Instance.SlowOffGameObject(UiManager.Instance.lackOfMoneyPanel));
            }
        }
        else
        {
            UiManager.Instance.StartCoroutine(UiManager.Instance.SlowOffGameObject(UiManager.Instance.fullInvenPanel));
            numberUi.text = "0";
        }

    }
    public void Buy()
    {
        if(number > 0)
        {
            for(int i = number; i > 0; i--)
            {
                dataManager.AddItemToInventoryData(good.prefabItemID, good.type, good.speType, good.durability, int.Parse(DisplayInventory.Instance.SearchFirstEmptyUiSlot().name), good.money);
                DisplayInventory.Instance.UpdateUI();
            }    

            dataManager.coins = dataManager.coins - (price * number);
        }    
    }

    public void OnClickPlus()
    {
        number++;
        numberUi.text = number.ToString();
    }

    public void OnClickMinus()
    {
        if(number >0)
        {
            number--;
            numberUi.text = number.ToString();
        }
        
    }
}
