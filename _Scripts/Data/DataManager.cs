using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewData", menuName = "DataManager")]
public class DataManager : ScriptableObject
{
    public List<TerrainData> terData;
    public List<Building> buildData;
    public List<CropData> cropData;
    public List<InventoryData> inventoryData;

    #region userData
    public string userEmail;
    public string userPassword;
    #endregion

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
    public void AddTerrainToData(GameObject obj, string type, string itemID)
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

        terData.Add(ter);

        obj.name = ter.objectID;
    }
    public void RemoveTerrainFromData(string itemId)
    {
        TerrainData ter = SearchTerrainInDataByItemId(itemId);
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
    //inventory
    public InventoryData SearchItemInDataByPrefabId(string id)
    {
        return inventoryData.Where(p => p.prefabItemID == id).FirstOrDefault();
    }
    public InventoryData SearchItemInDataByObjectId(string id)
    {
        return inventoryData.Where(p => p.objectItemID == id).FirstOrDefault();
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
    public int CountNumberItemWithSamePrefabId(string id)
    {
        IEnumerable<InventoryData> filteredItems = inventoryData.Where(x => x.prefabItemID == id);

        return filteredItems.Count();
    }
    public void AddItemToInventoryData( string itemID, string type, string speType)
    {
        InventoryData inv = new InventoryData();

        inv.prefabItemID = itemID;
        inv.objectItemID = itemID + "_" + inventoryData.Count();

        inv.type = type;
        inv.speType = type;

        inventoryData.Add(inv);
    }

    public void AddPlant(GameObject obj, string seedID , string seedObjectID , float timeToGrow , int InCurrentMode)
    {
        CropData crop = new CropData();

        crop.prefabID = seedID;
        crop.objectID = seedObjectID + "_" + cropData.Count();

        crop.PosX = obj.transform.position.x;
        crop.PosY = obj.transform.position.y;
        crop.PosZ = obj.transform.position.z;

        crop.growTime = timeToGrow;

        crop.curMode = InCurrentMode;

        cropData.Add(crop);

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
    }
    #endregion

    #region CropsData
    [System.Serializable]
    public class CropData
    {
        public string prefabID;
        public string objectID;

        public string terObjectID;

        public float PosX;
        public float PosY;
        public float PosZ;

        public float growTime;

        public float curTime;
        public int curMode;
        public int isHaveWater;

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
    }
    #endregion

}
