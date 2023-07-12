
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
}
