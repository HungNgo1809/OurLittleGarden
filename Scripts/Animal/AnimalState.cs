
using System.Collections.Generic;
using UnityEngine;



public class AnimalState : MonoBehaviour , ITimeTracker
{
    public List<float> seasonTimeExtendList = new List<float>();
    public BoxCollider box;
    public int growth;
    public float seasonTimeExtend;
    public int growthMax;
    public int animalLife;
    //public int numberItemsHarvest;
    public animalHouseComponent animalHouse;
    public enum AnimalSate
    {
       Growth , Harvestable
    }

    public AnimalSate animalState;


    public void SwitchState(AnimalSate stateToSwitch)
    {
        /*
        seed.gameObject.SetActive(false);
        seeding.gameObject.SetActive(false);
        harvestable.gameObject.SetActive(false);
        DataManager.CropData cropOne = dataManager.SearchCropDataByPrefabId(curObject.name);
        */
        switch (stateToSwitch)
        {
            case AnimalSate.Growth:
                animalLife--;
                growth = 0;
                break;

            case AnimalSate.Harvestable:
                if (animalState != AnimalSate.Harvestable)  // Check if the state is changing to harvestable
                {
                    animalState = stateToSwitch;
                    CheckAllAnimalsHarvestable();
                }
                growth = growthMax;
                break;
        }


        animalState = stateToSwitch;
    }


    // Start is called before the first frame update
    void Start()
    {
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.RegisterTracker(this);
        }
        if (TimeManager.Instance.timestamp.season == GameTimestamp.Season.Spring && seasonTimeExtend != seasonTimeExtendList[0])
        {
            seasonTimeExtend = seasonTimeExtendList[0];

        }
        else if (TimeManager.Instance.timestamp.season == GameTimestamp.Season.Summer && seasonTimeExtend != seasonTimeExtendList[1])
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

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClockUpdate(GameTimestamp timestamp)
    {

        if(animalState != AnimalSate.Harvestable )
        {
            Grow();
        }
     
       
    }

    public void Grow() // goi theo tung phut
    {
        if (TimeManager.Instance.timestamp.season == GameTimestamp.Season.Spring && seasonTimeExtend != seasonTimeExtendList[0])
        {
            seasonTimeExtend = seasonTimeExtendList[0];
            animalHouse.totalGrowthMax = 0;
            foreach(var state in animalHouse.listAnimalState)
            {
               animalHouse.totalGrowthMax += (int)(state.growthMax * state.seasonTimeExtend);
            }
           

        }
        else if (TimeManager.Instance.timestamp.season == GameTimestamp.Season.Summer && seasonTimeExtend != seasonTimeExtendList[1])
        {
            seasonTimeExtend = seasonTimeExtendList[1];
            animalHouse.totalGrowthMax = 0;
            foreach (var state in animalHouse.listAnimalState)
            {
                animalHouse.totalGrowthMax += (int)(state.growthMax * state.seasonTimeExtend);
            }
        }
        else if (TimeManager.Instance.timestamp.season == GameTimestamp.Season.Fall && seasonTimeExtend != seasonTimeExtendList[2])
        {
            seasonTimeExtend = seasonTimeExtendList[2];
            animalHouse.totalGrowthMax = 0;
            foreach (var state in animalHouse.listAnimalState)
            {
                animalHouse.totalGrowthMax += (int)(state.growthMax * state.seasonTimeExtend);
            }
        }
        else if (TimeManager.Instance.timestamp.season == GameTimestamp.Season.Winter && seasonTimeExtend != seasonTimeExtendList[3])
        {
            seasonTimeExtend = seasonTimeExtendList[3];
            animalHouse.totalGrowthMax = 0;
            foreach (var state in animalHouse.listAnimalState)
            {
                animalHouse.totalGrowthMax += (int)(state.growthMax * state.seasonTimeExtend);
            }
        }

        growth++;
        animalHouse.totalNumberTimeHarvest++;
     
        if (growth >= growthMax * seasonTimeExtend )
        {
            SwitchState(AnimalSate.Harvestable);
       
        }


    }
    private void CheckAllAnimalsHarvestable()
    {
        // Check if all animal states are in the harvestable state
        bool allAnimalsHarvestable = true;

        foreach (AnimalState animalState in animalHouse.listAnimalState)
        {
            if (animalState.animalState != AnimalState.AnimalSate.Harvestable)
            {
                allAnimalsHarvestable = false;
                break;
            }
        }

        if (allAnimalsHarvestable)
        {
            // Do something when all animal states are harvestable
            // For example, enable a UI element or trigger a specific event
        }
    }
    public void updateExtendTime()
    {
        if (TimeManager.Instance.timestamp.season == GameTimestamp.Season.Spring && seasonTimeExtend != seasonTimeExtendList[0])
        {
            seasonTimeExtend = seasonTimeExtendList[0];

        }
        else if (TimeManager.Instance.timestamp.season == GameTimestamp.Season.Summer && seasonTimeExtend != seasonTimeExtendList[1])
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

    }
}
