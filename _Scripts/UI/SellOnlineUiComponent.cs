using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellOnlineUiComponent : MonoBehaviour
{
    public SellOnlineManager.itemPack pack;

    public Text packName;
    public Text price;

    public void OnClick()
    {
        SellOnlineUi.Instance.OnClickBuyPack(pack);
    }
    public void OnClickOwn()
    {
        SellOnlineUi.Instance.OnClickSellPack(pack);
    }
}
