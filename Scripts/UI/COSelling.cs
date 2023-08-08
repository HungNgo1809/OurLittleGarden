using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class COSelling : MonoBehaviour
{
    public DataManager.InventoryData curItem;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*
    public void CheckMoney()
    {
        COSelling[] COselling = gameObject.transform.parent.GetComponentsInChildren<COSelling>();
        moneyText.text = "0";
        int totalMoney = int.Parse(moneyText.text);
        foreach (COSelling co in COselling)
        {
            if(co.inputfield.text == "" || co.inputfield.text == "0")
            {
                totalMoney += co.curItem.money;
            }
            else
            {
                int money = int.Parse(co.inputfield.text);
                totalMoney += money ;
               
            }
            moneyText.text = totalMoney.ToString();
        }

    }
    */
}
