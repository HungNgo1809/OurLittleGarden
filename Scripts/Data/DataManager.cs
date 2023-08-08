using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PlayFab;
using Newtonsoft.Json;
using static DataManager;
using System.IO;
using System.Xml.Serialization;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using PlayFab.ClientModels;
using static QuestManager;

[CreateAssetMenu(fileName = "NewData", menuName = "DataManager")]
public class DataManager : ScriptableObject
{
    public List<TerrainData> terData;
    public List<Building> buildData;
    public List<CropData> cropData;
    public List<InventoryData> inventoryData;
    public List<StorageData> storageData;
    public List<AnimalHouse> animalHouseData;
    public List<Affection> affections;

    public Upgrade upgradeData;

    public SpecialQuestManager.SpecialQuest curSpecialQuest;

    public CurrentQuest playerQuestIDData;
    public CurrentSideQuest playerSideQuestIDData;
    [SerializeField]
    public List<Quest> dataManagerMainQuest = new List<Quest>();
    [SerializeField]
    public List<SideQuest> dataManagerSideQuest = new List<SideQuest>();

    public TimeData timeData;
    public int timePassed;

    public bool firstConnect;
    #region userData
    public string userEmail;
    public string userPassword;
    public string userId;
    public string userName;
    public string displayName;

    public string version;
    public int isOldbie;

    public List<FriendInfo> friendsList;

    public List<FriendRequest> friendRequestSendList;
    public List<FriendRequest> friendRequestReceivedList;
    #endregion

    #region playerData
    public float hp;
    public float food;
    public float stamina;
    public float sleep;

    public int coins;
    public int reputation;
    #endregion

    public bool isLoadedData;
    public int isSavedDataBeforeQuit = 0;

    public bool isCheckFriendRequest;

    public int checkLoad = 0;

    public bool isJoinedFriend;
    // Start is called before the first frame update
    #region UnityMethod
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }
    #endregion

    #region CodeMethor
    //terrain
    public void AddTerrainToData(GameObject obj, string type, string itemID, string objType)
    {
        TerrainData ter = new TerrainData();

        //ter.block = obj.gameObject;

        ter.PosX = obj.transform.position.x;
        ter.PosY = obj.transform.position.y;
        ter.PosZ = obj.transform.position.z;

        ter.Rotation = obj.transform.rotation.y;

        ter.type = type;
        ter.itemID = itemID;
        ter.objectID = itemID + "_" + terData.Count;

        ter.objType = objType;
        terData.Add(ter);

        obj.name = ter.objectID;
    }
    public void RemoveTerrainFromData(string objId)
    {
        TerrainData ter = SearchTerrainInDataByObjectId(objId);
        terData.Remove(ter);
    }
    public TerrainData SearchTerrainInDataByObjectId(string id)
    {
        return terData.Where(p => p.objectID == id).FirstOrDefault();
    }
    public TerrainData SearchTerrainInDataByItemId(string id)
    {
        return terData.Where(p => p.itemID == id).FirstOrDefault();
    }

    public FriendInfo SearchFriendById(string id, List<FriendInfo> fr_List)
    {
        return fr_List.Where(p => p.friendId == id).FirstOrDefault();
    }
    public FriendRequest SearchFriendRequestById(string id, List<FriendRequest> fr_List)
    {
        return fr_List.Where(p => p.friendId == id).FirstOrDefault();
    }
    //inventory
    public InventoryData SearchItemInDataByPrefabId(string id)
    {
        return inventoryData.Where(p => p.prefabItemID == id).FirstOrDefault();
    }

    public InventoryData SearchItemInDataByButton(int id)
    {
        return inventoryData.Where(p => p.button == id).FirstOrDefault();
    }

    public InventoryData SearchItemInDataByObjectId(string id)
    {
        return inventoryData.Where(p => p.objectItemID == id).FirstOrDefault();
    }
    public InventoryData SearchItemInStorageByObjectId(string id, StorageData storage)
    {
        return storage.items.Where(p => p.objectItemID == id).FirstOrDefault();
    }
    public void RemoveItemInDataByPrefabId(string id)
    {
        InventoryData inv = SearchItemInDataByPrefabId(id);

        if (inv.button != 0)
        {
            DestroyHandUi.Instance.DestroyItem(inv.button - 1);
        }

        inventoryData.Remove(inv);

    }
    public void RemoveItemInDataByObjectId(string id)
    {
        InventoryData inv = SearchItemInDataByObjectId(id);

        if (inv.button != 0)
        {
            DestroyHandUi.Instance.DestroyItem(inv.button - 1);
        }

        inventoryData.Remove(inv);

    }
    public void RemoveItemInDataByObjectIdButton0(string id)
    {
        InventoryData inv = SearchItemInDataByObjectId(id);

        if (inv.button == 0)
        {
            DestroyHandUi.Instance.DestroyItem(inv.button - 1);
        }

        inventoryData.Remove(inv);

    }
    public int CountNumberItemWithSamePrefabId(string id)
    {
        IEnumerable<InventoryData> filteredItems = inventoryData.Where(x => x.prefabItemID == id);

        return filteredItems.Count();
    }
    public void AddItemToInventoryData( string itemID, string type, string speType, float durability, int pos, int quickSellMoney )
    {
        InventoryData inv = new InventoryData();

        inv.prefabItemID = itemID;
        inv.objectItemID = itemID + "_" + inventoryData.Count();

        inv.type = type;
        inv.speType = speType;

        inv.button = 0;
        inv.invPos = pos;

        inv.durability = durability;
        inv.money = quickSellMoney;
       
        inventoryData.Add(inv);
    }

    public void AddPlant(Vector3 obj, string seedID , string dirtObjectID , int timeToGrow , int InCurrentMode , int currentTimeHarvest ,bool WaterdOrNot)
    {
        CropData crop = new CropData();

        crop.prefabID = seedID;

        crop.PosX = obj.x;
        crop.PosY = obj.y;
        crop.PosZ = obj.z;

        crop.isHaveWater = WaterdOrNot;

        crop.terObjectID = dirtObjectID;

        crop.growTime = timeToGrow;

        crop.curMode = InCurrentMode;

        crop.curNumberHarvest = currentTimeHarvest;

        cropData.Add(crop);

    }

    public void AddBuild(string prefabId, Vector3 pos, float rotation, string type)
    {
        Building build = new Building();

        build.prefabID = prefabId;
        build.objectID = prefabId + "_" + buildData.Count();

        build.PosX = pos.x;
        build.PosY = pos.y;
        build.PosZ = pos.z;

        build.Rotation = rotation;

        build.type = type;

        buildData.Add(build);
    }
    public CropData SearchCropDataByPrefabId(string id)
    {
        return cropData.Where(p => p.prefabID == id).FirstOrDefault();
    }


    public void RemoveCropDataByPrefabID(string prefabId)
    {
        CropData crop = SearchCropDataByPrefabId(prefabId);
        cropData.Remove(crop);
    }
    public void AddStorage(string storageId)
    {
        StorageData storage = new StorageData();

        storage.storageId = storageId;
        storage.items = new List<InventoryData>();

        storageData.Add(storage);
    }
    public void AddAnimalHouse(string animalHouseId, string type  , int Time)
    {
        AnimalHouse animalHouse = new AnimalHouse();

        animalHouse.objectId = animalHouseId;
        animalHouse.type = type;
        animalHouse.currentTime = Time;
        animalHouse.animals = new List<Animal>();

        animalHouseData.Add(animalHouse);
    }
    public StorageData SearchStorageById(string id)
    {
        return storageData.Where(p => p.storageId == id).FirstOrDefault();
    }
    public AnimalHouse SearchAnimalHouseById(string id)
    {
        return animalHouseData.Where(p => p.objectId == id).FirstOrDefault();
    }
    public void AddItemToStorage(string itemId, string storageId)
    {
        var sto = SearchStorageById(storageId);
        var item = SearchItemInDataByObjectId(itemId);
        if ((item != null) && (sto != null))
        {
            //Debug.Log(SearchStorageById(storageId).storageId);

            sto.items.Add(item);
            //SearchStorageById(storageId).items.Add(SearchItemInDataByObjectId(itemId));
            RemoveItemInDataByObjectId(itemId);
        }
    }
    public void RemoveItemFromStorage(string itemId, string storageId)
    {
        var sto = SearchStorageById(storageId);
        var item = SearchItemInStorageByObjectId(itemId, sto);

        //item.invPos = pos;
        if ((item != null) && (sto != null))
        {
            //Debug.Log(SearchStorageById(storageId).storageId);

            sto.items.Remove(item);
            inventoryData.Add(item);
            //SearchStorageById(storageId).items.Add(SearchItemInDataByObjectId(itemId));
        }
    }
    public void RemoveBuildingFromDataByObjectId(string id)
    {
        Building build = SearchBuilding(id);
        buildData.Remove(build);
    }      
    public Building SearchBuilding(string id)
    {
        return buildData.Where(p => p.objectID == id).FirstOrDefault();
    }
    public void UpdateBuildingTransformData(string id, Vector3 position, float rotation)
    {
        if(SearchBuilding(id) != null)
        {
            var build = SearchBuilding(id);

            build.PosX = position.x;
            build.PosY = position.y;
            build.PosZ = position.z;

            build.Rotation = rotation;
        }
    }

    public void SaveCurrentQuest(int questID, List<QuestManager.Quest.Requirement> requirements)
    {
        playerQuestIDData.currentID = questID;
        playerQuestIDData.currentQuestRequirements = requirements;
        // Save the currentQuestID and currentQuestRequirements to your desired storage method (e.g., PlayerPrefs, file, database)
    }

    public void ClearCurrentQuest()
    {
        playerQuestIDData.currentID = 0; // Clear the currentQuestID
        playerQuestIDData.currentQuestRequirements = null; // Clear the currentQuestRequirements
        // Clear the saved currentQuestID and currentQuestRequirements from your storage method
    }

    public int GetCurrentQuestID()
    {
        return playerQuestIDData.currentID;
    }

    public List<QuestManager.Quest.Requirement> GetCurrentQuestRequirements()
    {
        return playerQuestIDData.currentQuestRequirements;
    }

    public void SaveCurrenSidetQuest(int questID, List<QuestManager.SideQuest.Requirement> requirements)
    {
        playerSideQuestIDData.currentSideID = questID;
        playerSideQuestIDData.currentSideQuestRequirements = requirements;
        // Save the currentQuestID and currentQuestRequirements to your desired storage method (e.g., PlayerPrefs, file, database)
    }

    public void ClearCurrentSideQuest()
    {
        playerSideQuestIDData.currentSideID = 0; // Clear the currentQuestID
        playerSideQuestIDData.currentSideQuestRequirements = null; // Clear the currentQuestRequirements
        // Clear the saved currentQuestID and currentQuestRequirements from your storage method
    }

    public int GetCurrentSideQuestID()
    {
        return playerSideQuestIDData.currentSideID;
    }

    public List<QuestManager.SideQuest.Requirement> GetCurrentSideQuestRequirements()
    {
        return playerSideQuestIDData.currentSideQuestRequirements;
    }

    public Affection SearchNPCAffectionByName (string name )
    {
        return affections.Find(affection => affection.NPCName == name);
    }
    /*
    public void DeQuyScaleDeTest(Affection npc)
    {
        if (npc.scale >= 5)
        {
            // Increment the level and reset the scale
            npc.level++;
            npc.scale = npc.scale - 5;

            DeQuyScaleDeTest(npc);
        }
        else
        {
            return;
        }
    }
    */

    public void IncreaseAmount(Affection npcAffection )
    {
        while(npcAffection.amountOfTrade >= 1000)
        {
            npcAffection.amountOfTrade -= 1000;
            IncreaseAffectionScale(npcAffection, 20);

        }
    }

    public void IncreaseAffectionScale(Affection npcAffection, int amount)
    {

        if (npcAffection != null)
        {
            // Increase the scale by the specified amount
            npcAffection.scale += amount;

            // Check if the scale has reached the maximum value for a level
            if (npcAffection.scale >= 100)
            {
                // Increment the level and reset the scale
                npcAffection.level++;
                npcAffection.scale = npcAffection.scale - 100;
                //DeQuyScaleDeTest(npcAffection);
                // Ensure that the level doesn't exceed the maximum level (10 in this case)
                if (npcAffection.level > 10)
                {
                    npcAffection.level = 10;
                }
            }
        }
    }


    #endregion

    #region MapTerrainData
    [System.Serializable]
    public class TerrainData
    {
        public string objectID;

        public string itemID;
       
        public float PosX;
        public float PosY;
        public float PosZ;

        public float Rotation;

        public string type;
        public string objType;

        public int wateredTime;
        public int LandsMode;
    }
    #endregion

    #region BuildData
    [System.Serializable]
    public class Building
    {
        public string prefabID;
        public string objectID;

        public float PosX;
        public float PosY;
        public float PosZ;

        public float Rotation;

        public string type;
        public int timeBuild;
        public bool isDoneBuild;
    }
    #endregion

    #region CropsData
    [System.Serializable]
    public class CropData
    {
        public string prefabID;
  

        public string terObjectID;

        public float PosX;
        public float PosY;
        public float PosZ;

        public int growTime;

        public int curTime;
        public int curMode;
        public bool isHaveWater;
        
        public string type;

        public int curNumberHarvest;
        public int LimitNumberHarvest;
    }
    #endregion

    #region AnimalData
    [System.Serializable]
    public class AnimalData
    {
        public string prefabID;
        public float PosX;
        public float PosY;
        public float PosZ;
        public int growTime;
        public int curTime;
        public int curMode;
        public bool isHaveWater;
        public string type;
        public int curNumberHarvest;
        public int LimitNumberHarvest;
    }
    #endregion

    #region InventoryData
    [System.Serializable]
    public class InventoryData
    {
        public string prefabItemID;
        public string objectItemID;

        public string type;
        public string speType;

        public int button;
        public int invPos;

        public float durability;
        public int money;
        
    }
    #endregion

    #region StorageData
    [System.Serializable]
    public class StorageData
    {
        public string storageId;
        public List<InventoryData> items;
    }
    #endregion

    #region TimeData
    [System.Serializable]
    public class TimeData
    {
        public int years;
        public int seasons;
        public int days;
        public int hourrs = 6;
        public int mintute;
    }
    #endregion

    #region PlayerCurrentQuest
    [System.Serializable]
    public class CurrentQuest
    {
        public int currentID;
        public int questMode;
        public int questHaveDone;
        [SerializeField]
        public List<QuestManager.Quest.Requirement> currentQuestRequirements;
    }

    [System.Serializable]
    public class CurrentSideQuest
    {
        public int currentSideID;
        public int sideQuestMode;
        public int sideQuestHaveDone;
        [SerializeField]
        public List<QuestManager.SideQuest.Requirement> currentSideQuestRequirements;
    }
    #endregion

    #region Save and load   
    public void UpdatePlayerStatistic(string statisticName, int value)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
        {
            new StatisticUpdate
            {
                StatisticName = statisticName,
                Value = value
            }
        }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(request, OnUpdatePlayerStatisticsSuccess, OnUpdatePlayerStatisticsFailure);
    }

    private void OnUpdatePlayerStatisticsSuccess(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Player statistics updated successfully!");
    }

    private void OnUpdatePlayerStatisticsFailure(PlayFabError error)
    {
        Debug.LogError(error);
    }
    public void SaveFarmTerData()
    {
        int totalCount = terData.Count;
        int quarterCount = totalCount / 4;

        List<TerrainData> selectedData1 = terData.Take(quarterCount).ToList();
        List<TerrainData> selectedData2 = terData.Skip(quarterCount).Take(quarterCount).ToList();
        List<TerrainData> selectedData3 = terData.Skip(quarterCount * 2).Take(quarterCount).ToList();
        List<TerrainData> selectedData4 = terData.Skip(quarterCount * 3).ToList();

        string json1 = JsonConvert.SerializeObject(selectedData1);
        string json2 = JsonConvert.SerializeObject(selectedData2);
        string json3 = JsonConvert.SerializeObject(selectedData3);
        string json4 = JsonConvert.SerializeObject(selectedData4);

        SaveDataPlayfab("terData1", json1);
        SaveDataPlayfab("terData2", json2);
        SaveDataPlayfab("terData3", json3);
        SaveDataPlayfab("terData4", json4);

        SaveDataPlayfab("isOldbie", JsonConvert.ToString(1));
    }    
    public void TriggerSave()
    {
        SaveDataPlayfab("buildData", JsonConvert.SerializeObject(buildData));
        SaveDataPlayfab("cropData", JsonConvert.SerializeObject(cropData));
        SaveDataPlayfab("inventoryData", JsonConvert.SerializeObject(inventoryData));
        SaveDataPlayfab("storageData", JsonConvert.SerializeObject(storageData));
        SaveDataPlayfab("animalHouseData", JsonConvert.SerializeObject(animalHouseData));
        SaveDataPlayfab("curSpecialQuest", JsonConvert.SerializeObject(curSpecialQuest));
        SaveDataPlayfab("affections", JsonConvert.SerializeObject(affections));

        SaveDataPlayfab("hp", JsonConvert.ToString(hp));
        SaveDataPlayfab("food", JsonConvert.ToString(food));
        SaveDataPlayfab("stamina", JsonConvert.ToString(stamina));
        SaveDataPlayfab("sleep", JsonConvert.ToString(sleep));

        UpdatePlayerStatistic("coins", coins);
        SaveDataPlayfab("coins", JsonConvert.ToString(coins));

        UpdatePlayerStatistic("reputation", reputation);
        SaveDataPlayfab("reputation", JsonConvert.ToString(reputation));

        SaveDataPlayfab("version", JsonConvert.ToString(version));
        SaveDataPlayfab("isOldbie", JsonConvert.ToString(1));

        SaveDataPlayfab("timeData", JsonConvert.SerializeObject(timeData));
        SaveDataPlayfab("playerQuestIDData", JsonConvert.SerializeObject(playerQuestIDData));
        SaveDataPlayfab("playerSideQuestIDData", JsonConvert.SerializeObject(playerSideQuestIDData));
        SaveDataPlayfab("dataManagerMainQuest", JsonConvert.SerializeObject(dataManagerMainQuest));
        SaveDataPlayfab("dataManagerSideQuest", JsonConvert.SerializeObject(dataManagerSideQuest));

        SaveDataPlayfab("upgradeData", JsonConvert.SerializeObject(upgradeData));

    }
    public void SaveStatusDataOnly()
    {
        SaveDataPlayfab("hp", JsonConvert.ToString(hp));
        SaveDataPlayfab("food", JsonConvert.ToString(food));
        SaveDataPlayfab("stamina", JsonConvert.ToString(stamina));
        SaveDataPlayfab("sleep", JsonConvert.ToString(sleep));

        UpdatePlayerStatistic("coins", coins);
        SaveDataPlayfab("coins", JsonConvert.ToString(coins));

        UpdatePlayerStatistic("reputation", reputation);
        SaveDataPlayfab("reputation", JsonConvert.ToString(reputation));
    }    
    public void SaveDataToOtherPlayer(string id, string key, string data)
    {
        var request = new PlayFab.AdminModels.UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { key, data}
            },
            PlayFabId = id,
            Permission = PlayFab.AdminModels.UserDataPermission.Public
        };
        PlayFabAdminAPI.UpdateUserData(request, OnDataSend, Err);
    }    
    public void SaveDataPlayfab(string key, string data)
    {
        var request = new PlayFab.ClientModels.UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { key, data}
            },
            Permission = PlayFab.ClientModels.UserDataPermission.Public
        };
        PlayFabClientAPI.UpdateUserData(request, SaveDataSuccess, Err);
    }

    void SaveDataSuccess(PlayFab.ClientModels.UpdateUserDataResult result)
    {
        isSavedDataBeforeQuit++;
    }
    void Err(PlayFabError error)
    {
        Debug.Log(error);
    }

    public void LoadDataPlayfab(string id)
    {
        var request = new PlayFab.ClientModels.GetUserDataRequest
        {
            PlayFabId = id,
            //Keys = new List<string> { key }
        };
        PlayFabClientAPI.GetUserData(request, LoadDataSuccess, Err);
    }
    public void LoadSpeKeyPlayFab(string id, string key)
    {
        var request = new PlayFab.ClientModels.GetUserDataRequest
        {
            PlayFabId = id,
            Keys = new List<string> { key }
        };
        PlayFabClientAPI.GetUserData(request, LoadDataSuccess, Err);
    }    
    public void LoadVersionData(string fileName)
    {
        if (!File.Exists(Application.persistentDataPath + "/" + fileName)) return;
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(string));
        FileStream stream = new FileStream(Application.persistentDataPath + "/" + fileName, FileMode.Open);

        if(xmlSerializer != null)
        {
            version = xmlSerializer.Deserialize(stream) as string;
        }
        stream.Close();
    }
    void LoadDataSuccess(PlayFab.ClientModels.GetUserDataResult result)
    {
        if(result.Data == null)
        {
            return;
        }

        if (result.Data.ContainsKey("terData1"))
        {
            List<TerrainData> terData1 = JsonConvert.DeserializeObject<List<TerrainData>>(result.Data["terData1"].Value);
            terData.AddRange(terData1);
        }
        if (result.Data.ContainsKey("terData2"))
        {
            List<TerrainData> terData2 = JsonConvert.DeserializeObject<List<TerrainData>>(result.Data["terData2"].Value);
            terData.AddRange(terData2);
        }
        if (result.Data.ContainsKey("terData3"))
        {
            List<TerrainData> terData3 = JsonConvert.DeserializeObject<List<TerrainData>>(result.Data["terData3"].Value);
            terData.AddRange(terData3);
        }
        if (result.Data.ContainsKey("terData4"))
        {
            List<TerrainData> terData4 = JsonConvert.DeserializeObject<List<TerrainData>>(result.Data["terData4"].Value);
            terData.AddRange(terData4);
        }
        if (result.Data.ContainsKey("buildData"))
        {
            buildData = JsonConvert.DeserializeObject<List<Building>>(result.Data["buildData"].Value);
        }
        if (result.Data.ContainsKey("cropData"))
        {
            cropData = JsonConvert.DeserializeObject<List<CropData>>(result.Data["cropData"].Value);
        }
        if (result.Data.ContainsKey("inventoryData"))
        {
            inventoryData = JsonConvert.DeserializeObject<List<InventoryData>>(result.Data["inventoryData"].Value);
        }
        if (result.Data.ContainsKey("storageData"))
        {
            storageData = JsonConvert.DeserializeObject<List<StorageData>>(result.Data["storageData"].Value);
        }
        if (result.Data.ContainsKey("animalHouseData"))
        {
            animalHouseData = JsonConvert.DeserializeObject<List<AnimalHouse>>(result.Data["animalHouseData"].Value);
        }
        if (result.Data.ContainsKey("curSpecialQuest"))
        {
            curSpecialQuest = JsonConvert.DeserializeObject<SpecialQuestManager.SpecialQuest>(result.Data["curSpecialQuest"].Value);
        }
        if (result.Data.ContainsKey("friendRequest"))
        {
            friendRequestReceivedList = JsonConvert.DeserializeObject<List<FriendRequest>>(result.Data["friendRequest"].Value);
        }
        if (result.Data.ContainsKey("friendRequestSend"))
        {
            friendRequestSendList = JsonConvert.DeserializeObject<List<FriendRequest>>(result.Data["friendRequestSend"].Value);
        }
        if (result.Data.ContainsKey("affections"))
        {
            affections = JsonConvert.DeserializeObject<List<Affection>>(result.Data["affections"].Value);
        }
        if (result.Data.ContainsKey("hp"))
        {
            if (result.Data.TryGetValue("hp", out var floatDataValue))
            {
                if (float.TryParse(floatDataValue.Value, out var floatValue))
                {
                    //Debug.Log(floatValue);
                    hp = floatValue;
                }
            }
        }
        if (result.Data.ContainsKey("food"))
        {
            if (result.Data.TryGetValue("food", out var floatDataValue))
            {
                if (float.TryParse(floatDataValue.Value, out var floatValue))
                {
                    food = floatValue;
                }
            }
        }
        if (result.Data.ContainsKey("stamina"))
        {
            if (result.Data.TryGetValue("stamina", out var floatDataValue))
            {
                if (float.TryParse(floatDataValue.Value, out var floatValue))
                {
                    //Debug.Log(floatValue);
                    stamina = floatValue;
                }
            }
        }
        if (result.Data.ContainsKey("sleep"))
        {
            if (result.Data.TryGetValue("sleep", out var floatDataValue))
            {
                if (float.TryParse(floatDataValue.Value, out var floatValue))
                {
                    sleep = floatValue;
                }
            }
        }
        if (result.Data.ContainsKey("coins"))
        {
            if (result.Data.TryGetValue("coins", out var intDataValue))
            {
                if (int.TryParse(intDataValue.Value, out var intValue))
                {
                    coins = intValue;
                }
            }
        }
        if (result.Data.ContainsKey("reputation"))
        {
            if (result.Data.TryGetValue("reputation", out var intDataValue))
            {
                if (int.TryParse(intDataValue.Value, out var intValue))
                {
                    reputation = intValue;
                }
            }
        }
        if (result.Data.ContainsKey("isOldbie")) 
        {
            if (result.Data.TryGetValue("isOldbie", out var intDataValue))
            {
                if (int.TryParse(intDataValue.Value, out var intValue))
                {
                    isOldbie = intValue;
                }
            }
        }
        if (result.Data.ContainsKey("playerQuestIDData"))
        {
            playerQuestIDData = JsonConvert.DeserializeObject<CurrentQuest>(result.Data["playerQuestIDData"].Value);
        }
        if (result.Data.ContainsKey("playerSideQuestIDData"))
        {
            playerSideQuestIDData = JsonConvert.DeserializeObject<CurrentSideQuest>(result.Data["playerSideQuestIDData"].Value);
        }
        if (result.Data.ContainsKey("dataManagerMainQuest"))
        {
            dataManagerMainQuest = JsonConvert.DeserializeObject<List<Quest>>(result.Data["dataManagerMainQuest"].Value);
        }
        if (result.Data.ContainsKey("dataManagerSideQuest"))
        {
            dataManagerSideQuest = JsonConvert.DeserializeObject<List<SideQuest>>(result.Data["dataManagerSideQuest"].Value);
        }
        if (result.Data.ContainsKey("timeData"))
        {
            timeData = JsonConvert.DeserializeObject<TimeData>(result.Data["timeData"].Value);
        }
        if (result.Data.ContainsKey("upgradeData"))
        {
            upgradeData = JsonConvert.DeserializeObject<Upgrade>(result.Data["upgradeData"].Value);
        }
        isLoadedData = true;
        checkLoad++;
    }
    public void OnDataSend(PlayFab.AdminModels.UpdateUserDataResult result)
    {
        Debug.Log("Save to playfab success");
    }

    public void LoadFriendList()
    {
        friendsList.Clear();

        var request = new GetFriendsListRequest {};

        PlayFabClientAPI.GetFriendsList(request, OnFriendsListSuccess, OnFailure);
    }

    public void UpdateFriendsAfterAcceptRequest()
    {
        Chat.Instance.OnClickUpdateFriend();
    }   
    private void OnFriendsListSuccess(GetFriendsListResult result)
    {
        foreach (var friend in result.Friends)
        {
            var friendInfo = new FriendInfo
            {
                friendId = friend.FriendPlayFabId,
                friendName = friend.Username
            };

            friendsList.Add(friendInfo);
        }
        if(PhotonChatController.Instance != null)
        {
            PhotonChatController.Instance.FindPhotonFriends();
        }    
    }
    private void OnFailure(PlayFabError error)
    {
        Debug.Log(error);
    }
    public void CheckFriendRequest()
    {
        List<FriendRequest> requestNeedRemove = new List<FriendRequest>();
        if (friendRequestSendList.Count == 0)
        {
            isCheckFriendRequest = true;
            return;
        }

        foreach (FriendRequest fr in friendRequestSendList)
        {
            Debug.Log(fr.friendId);
            if (fr.accepted == 1)
            {
                //Thêm bạn bè ngược lại
                FriendManager.Instance.AddFriend(fr.friendId);
                requestNeedRemove.Add(fr);
            }
        }

        foreach (FriendRequest fr in requestNeedRemove)
        {
            friendRequestSendList.Remove(fr);
        }

        SaveDataPlayfab("friendRequestSend", JsonConvert.SerializeObject(friendRequestSendList));

        isCheckFriendRequest = true;
    }
    #endregion

    #region FriendData
    [System.Serializable]
    public class FriendInfo
    {
        public string friendId;
        public string friendName;
    }

    [System.Serializable]
    public class FriendRequest
    {
        public string friendId;
        public string friendName;

        public int accepted;
    }
    #endregion

    #region AnimalData
    [System.Serializable]
    public class Animal
    {
        public string animalId;

        public int curLife;

        public int currentTime;
    }
    [System.Serializable]
    public class AnimalHouse
    {
        public string objectId;
        public string type;
        public int currentTime;

        public List<Animal> animals;
    }
    #endregion

    #region AffectionData
    [System.Serializable]
    public class Affection
    {
        public string NPCName;
        public int level;
        public int scale;
        public int amountOfTrade;
    }
    #endregion
    #region UpgradeData
    [System.Serializable]
    public class Upgrade
    {
        public int campFireLevel;
        public int toolTableLevel;
        public int storageLevel;
        public int ovenLevel;
        public int furnaceLevel;
    }
    #endregion
}
