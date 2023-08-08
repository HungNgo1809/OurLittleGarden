using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class PlantState : MonoBehaviour
{
    public DataManager dataManager;
  
    public GameObject curObject;
    public GameObject seed;
    public GameObject seeding;
    public GameObject harvestable;
    public PlantUI UIPanel;
    public BoxCollider box;
    public List<float> seasonTimeExtendList = new List<float>();
    public float waterBoostTime;
    public int growth;
    public float seasonTimeExtend;
    public int growthMax;
    public int numberTimesHarvest;
    public int numberOfHarvest;
    public ParticleSystem harvest;
    public enum CropState
    {
        Seed , Seeding , Harvestable
    }
    // Start is called before the first frame update
    void Start()
    {
        if (TimeManager.Instance.timestamp.season == GameTimestamp.Season.Spring)
        {
            seasonTimeExtend = seasonTimeExtendList[0];
        
        }
        else if (TimeManager.Instance.timestamp.season == GameTimestamp.Season.Summer)
        {
            seasonTimeExtend = seasonTimeExtendList[1];

        }
        else if (TimeManager.Instance.timestamp.season == GameTimestamp.Season.Fall)
        {
            seasonTimeExtend = seasonTimeExtendList[2];

        }
        else if (TimeManager.Instance.timestamp.season == GameTimestamp.Season.Winter)
        {
            seasonTimeExtend = seasonTimeExtendList[3];
           
        }
        harvest.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public CropState cropstate;

  
    public void Plant(Transform dirtPosition, Lands land )
    {
        // hien tai fix cung them data - so gio lon de thu hoach - fix cung = 1
        GameObject obj = Instantiate(curObject, dirtPosition.transform);
        
        obj.name = curObject.name + "_" +dataManager.cropData.Count;
        obj.transform.position = dirtPosition.transform.position + new Vector3(0,1.01f,0);
        
        Vector3 pos = new Vector3(0, 1.01f, 0);
       // GrowTime(2);
        SwitchState(CropState.Seed);
        dataManager.AddPlant(pos, obj.name, dirtPosition.gameObject.name, growth, 0 , numberTimesHarvest, land.isWater);
     /*
        if (!ListObjectManager.Instance.CheckForDuplicatePlantObject(obj.name.Split('_')[1]))
        {
            ListObjectManager.Instance.plantObject.Add(obj);
            Debug.Log(obj.name + "check");
        }
        if (!ListObjectManager.Instance.CheckForDuplicateTerObject(dirtPosition.gameObject.name.Split('_')[1]))
        {
            ListObjectManager.Instance.TerObject.Add(dirtPosition.gameObject);
        }
     */
        if (!ListObjectManager.Instance.CheckForDuplicatePlantObject(obj.name))
        {
            ListObjectManager.Instance.plantObject.Add(obj);
           
        }
        if (!ListObjectManager.Instance.CheckForDuplicateTerObject(dirtPosition.gameObject.name))
        {
            ListObjectManager.Instance.TerObject.Add(dirtPosition.gameObject);
        }

        //sau khi trong thi cho UI no quay mat vao Main camera
        //UIPanel.RotateTowardsTarget();


    }


    public void GrowTime(int hourToGrowDays)
    {
        int hourToGrow = GameTimestamp.DaysToHours(hourToGrowDays);
       
        // chuyen ve minute
        growthMax = GameTimestamp.HoursToMinutes(hourToGrow);
      
    }

    /// <summary>
    ///  Hien tai de Grow() moi khi dat dc tuoi nuoc - neu chua tuoi thi khong lon  (test thay kha lau)- thieu data 
    /// </summary>
    public void Grow( ) // goi theo tung phut
    {
        growth++;
       
        // neu khong update tren Planta() - fix cung 1 ngay
        if (growthMax == 0)
        {
           
           /*---> trung oo day
            int hourToGrow = GameTimestamp.DaysToHours(2);
            // chuyen ve minute
            growthMax = GameTimestamp.HoursToMinutes(hourToGrow);
            Debug.Log(hourToGrow + "grow");
           */
        }

        if (TimeManager.Instance.timestamp.season == GameTimestamp.Season.Spring && seasonTimeExtend != seasonTimeExtendList[0])
        {
            seasonTimeExtend = seasonTimeExtendList[0];

        }
        else if (TimeManager.Instance.timestamp.season == GameTimestamp.Season.Summer && seasonTimeExtend != seasonTimeExtendList[1] )
        {
            seasonTimeExtend = seasonTimeExtendList[1];

        }
        else if (TimeManager.Instance.timestamp.season == GameTimestamp.Season.Fall && seasonTimeExtend != seasonTimeExtendList[2])
        {
            seasonTimeExtend = seasonTimeExtendList[2];

        }
        else if (TimeManager.Instance.timestamp.season == GameTimestamp.Season.Winter && seasonTimeExtend != seasonTimeExtendList[3])
        {
            seasonTimeExtend = seasonTimeExtendList[3];

        }

        if (growth >= (growthMax* seasonTimeExtend) / 2 && cropstate == CropState.Seed)
        {
            
            SwitchState(CropState.Seeding);
          
        
        }


        if (growth >= growthMax* seasonTimeExtend && cropstate == CropState.Seeding)
        {
            SwitchState(CropState.Harvestable);
            harvest.Play();
        }
      

    }
    
   
    public void SwitchState(CropState stateToSwitch)
    {
        seed.gameObject.SetActive(false);
        seeding.gameObject.SetActive(false);
        harvestable.gameObject.SetActive(false);
        DataManager.CropData cropOne = dataManager.SearchCropDataByPrefabId(curObject.name);
      
        switch (stateToSwitch)
        {
            case CropState.Seed :
                seed.gameObject.SetActive(true);
                growth = 0;
                box.enabled = false;
                // growth hien tai chua doi sang thoi gian khac - de hoi kho doc
                if (curObject.tag == "Harvest")
                {
                    curObject.tag = "Untagged";
                }
                break;
            case CropState.Seeding :
                seeding.gameObject.SetActive(true);
                // thieu  udpate Data --->
                // cropOne.growTime = growth;
                box.enabled = false;
                cropOne.curMode = 1;


                break;

            case CropState.Harvestable :
                harvestable.gameObject.SetActive(true);
                // thieu  udpate Data --->
                
               // cropOne.growTime = growth;
                cropOne.curMode = 2;
                curObject.tag = "Harvest";
                box.enabled = true;
              
                break;
        }

        cropstate = stateToSwitch;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            UIPanel.gameObject.SetActive(true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            UIPanel.gameObject.SetActive(false);
        }
    }
}
