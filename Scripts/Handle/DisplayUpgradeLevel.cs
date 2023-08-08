using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayUpgradeLevel : MonoBehaviour
{
    public DataManager dataManager;

    public Text toolTableLevel;
    public Text campFireLevel;
    public Text FurnaceLevel;
    public Text ovenLevel;


    public void OnEnable()
    {
        updateText();
    }

    public void updateText()
    {
        toolTableLevel.text = (dataManager.upgradeData.toolTableLevel + 1).ToString();
        campFireLevel.text = (dataManager.upgradeData.campFireLevel + 1).ToString();
        FurnaceLevel.text = (dataManager.upgradeData.furnaceLevel + 1).ToString();
        ovenLevel.text = (dataManager.upgradeData.ovenLevel + 1).ToString();
    }    
}
