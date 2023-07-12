using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearData : MonoBehaviour
{
    // Dat object nay o scene dang nhap dau tien va khong cho quay ra, hoac destroy khi co vat the tuong tu
    public DataManager dataManager;
    public LargeMapData largeMapData;
    public SellOnlineManager sellOnlineManager;
    private void Awake()
    {
        StartCoroutine(ClearOldData());
    }
    /*
    private void OnApplicationQuit()
    {
        //Clear old data
        StartCoroutine(ClearOldData());
    }*/

    IEnumerator ClearOldData()
    {
        //Clear old data
        dataManager.terData.Clear();
        dataManager.buildData.Clear();
        dataManager.cropData.Clear();
        dataManager.inventoryData.Clear();
        dataManager.storageData.Clear();
        dataManager.animalHouseData.Clear();
        dataManager.affections.Clear();

        dataManager.curSpecialQuest.items.Clear();
        dataManager.curSpecialQuest.moneyReward = 0;
        dataManager.curSpecialQuest.reputationReward = 0;

        dataManager.friendsList.Clear();
        dataManager.friendRequestReceivedList.Clear();
        dataManager.friendRequestSendList.Clear();

        dataManager.hp = 100;
        dataManager.food = 100;
        dataManager.stamina = 100;
        dataManager.sleep = 100;

        dataManager.coins = 400;
        dataManager.reputation = 0;

        dataManager.userEmail = "";
        dataManager.userPassword = "";
        dataManager.userId = "";
        dataManager.displayName = "";
        dataManager.userName = "";

        dataManager.isLoadedData = false;
        dataManager.isSavedDataBeforeQuit = 0;

        dataManager.firstConnect = false;
        dataManager.isOldbie = 0;
        dataManager.checkLoad = 0;

        dataManager.timeData = new DataManager.TimeData();
        dataManager.playerQuestIDData = new DataManager.CurrentQuest();
        dataManager.playerSideQuestIDData = new DataManager.CurrentSideQuest();
        dataManager.dataManagerMainQuest.Clear();
        dataManager.dataManagerSideQuest.Clear();

        largeMapData.loadSuccess = false;
        largeMapData.isCreateNewMap = false;
        largeMapData.terData.Clear();
        largeMapData.objData.Clear();
        largeMapData.createMapDay = new System.DateTime();
        largeMapData.createObjDay = new System.DateTime();
        largeMapData.spawnPosition = new Vector3();
        largeMapData.saveCount = 0;

        sellOnlineManager.curPack = new SellOnlineManager.itemPack();
        sellOnlineManager.serVerSellingPack.Clear();
        sellOnlineManager.yourSellingPack.Clear();
        sellOnlineManager.curPackName = "";



        yield return null;
    }
}
