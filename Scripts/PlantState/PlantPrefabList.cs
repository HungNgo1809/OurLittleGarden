using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using static PrefabBuild;

public class PlantPrefabList : MonoBehaviour
{
    public List<PrefabCropItem> prefabsItems = new List<PrefabCropItem>();

    public static PlantPrefabList Instance { private set; get; }

    public DataManager dataManager;

    public TileMapGenerator genMap;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        //Debug.Log(gameObject.name);
    }
    private void Start()
    {
       StartCoroutine(ReloadCrop());
    }
    
    public void StartReloadCrop()
    {
        StartCoroutine(ReloadCrop());
    }
    
    IEnumerator ReloadCrop()
    {
        yield return new WaitUntil(() => genMap.isReCreate);
        //yield return new WaitForSeconds(5f);

        foreach (DataManager.CropData crop in dataManager.cropData)
        {
            if (ListObjectManager.Instance.TerObject.Where(obj => obj.name == crop.terObjectID).FirstOrDefault() != null)
            {
                GameObject filteredList = ListObjectManager.Instance.TerObject.Where(obj => obj.name == crop.terObjectID).First();

                string originalString = crop.prefabID;
                int underscoreIndex = originalString.IndexOf('_');
                string extractedString = originalString.Substring(0, underscoreIndex);

                GameObject obj = Instantiate(this.ReQuestItem(extractedString).Prefab_GameObject, filteredList.transform.position, Quaternion.identity);
                ListObjectManager.Instance.plantObject.Add(obj);// chua co checkdup


                obj.transform.SetParent(filteredList.transform);

                obj.transform.name = crop.prefabID;
                Vector3 pos = new Vector3(0, 1.01f, 0);
                obj.transform.position = filteredList.transform.position + pos;

                PlantState plant = obj.GetComponent<PlantState>();
                Lands land = filteredList.GetComponent<Lands>();
              
                //Debug.Log(plant.growth);
                land.takePlantsInChild();

                if(land.landStatus == Lands.LandStatus.FarmLand)
                {
                    crop.isHaveWater = false;
                 
                }
                else if(land.landStatus == Lands.LandStatus.Watered)
                {
                    crop.isHaveWater = true;
                  

                }
                if (crop.curMode == 0)
                {
                    plant.SwitchState(PlantState.CropState.Seed);
                }
                else if (crop.curMode == 1)
                {
                    plant.SwitchState(PlantState.CropState.Seeding);
                }
                else if (crop.curMode == 2)
                {
                    plant.SwitchState(PlantState.CropState.Harvestable);
                }
                plant.growth = crop.growTime;
                if(crop.curNumberHarvest > 0)
                {
                    plant.numberTimesHarvest = crop.curNumberHarvest;
                }
            }
        }           
    }

    public PrefabCropItem ReQuestItem(string Prefab_ID)
    {
        PrefabCropItem prefabItem = prefabsItems.Where(p => p.Prefab_ID == Prefab_ID).First();
        return prefabItem;
    }
    [System.Serializable]
    public class PrefabCropItem
    {
        public string Prefab_ID;

        public GameObject Prefab_GameObject;
        public string Type;

        public GameObject Mesh_GameObject;
    }
}
