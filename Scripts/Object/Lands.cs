using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditor.Animations;
using UnityEngine;
using static DataManager;


public class Lands : MonoBehaviour, ITimeTracker
{

    public DataManager dataManager;
    public Interactor interactor;
    public enum LandStatus
    {
        FarmLand ,  Watered
    }

    public LandStatus landStatus;

    GameObject select;
    public GameTimestamp timeWatered ;

    public PlantState plantState;


    public bool isWater ;

    public bool isLoad ;

    public int timeSinceWatered;
    // THIEU CHECK ISWATERED LUC DAU BOI VI CO THE TRONG CAY TRUOC KHI TUOI -> THIEU CHECK RENDER COLOR OF CURRENT LAND -> UPDATE DATA ISWATERED TRUE/FAlSE
    //color
    Color farmLandColor = new Color(142 / 255.0f, 78 / 255.0f, 0 / 255.0f, 1);
    Color wateredColor = new Color(51 / 255.0f, 38 / 255.0f, 24 / 255.0f, 1);
    void Update()
    {
       
        
    }
    void Start( )
    {
        select = gameObject;
      //  landStatus = LandStatus.FarmLand;
        //interactor = GameObject.Find("Interactor").GetComponent<Interactor>();
        
        //add listener
        if(TimeManager.Instance != null)
        {
            TimeManager.Instance.RegisterTracker(this);
        }    

        /*
        foreach(var child in TimeManager.Instance.listeners )
        {
            //Debug.Log(child + "11");
        }*/
    }
  
   
    public void SwitchState(LandStatus landStatus_)
    {
        landStatus = landStatus_;
       
     
        switch (landStatus_)
        {   
            case LandStatus.FarmLand:
                startChangeWF();
                ChangeDataWateredToFarm();
                break;
            case LandStatus.Watered:
         
                startChangeFW();
               
                timeWatered = TimeManager.Instance.GetGameTimestamp();
                isLoad = false;
                
                break;

           
        }

        //cropstate = stateToSwitch;
    }

    public void ReloadWateredLand()
    {
 
        StartCoroutine(ColorFarmToWatered(wateredColor, 1));
    }


    public void takePlantsInChild()
    {
        plantState = gameObject.GetComponentInChildren<PlantState>();

    }

    public void ClockUpdate(GameTimestamp timestamp)
    {
       
        if (landStatus == LandStatus.Watered)
        {

          
           timeSinceWatered = GameTimestamp.CompareTimestamps(timeWatered, timestamp);
           
            //Debug.Log(timeSinceWatered + gameObject.name + "sdfdsfdsf");
            if (plantState == null)
            {
                if (timeSinceWatered > 24 * 60 )
                {
                    SwitchState(LandStatus.FarmLand);
                    timeSinceWatered = 0;
                   
                }
               
            }
            if (plantState != null )
            {
              
                if (timeSinceWatered > 24 * 60 * plantState.waterBoostTime) 
                {
                    SwitchState(LandStatus.FarmLand);
                    timeSinceWatered = 0;
                   
                }
                if(plantState.cropstate != PlantState.CropState.Harvestable)
                {

                    plantState.Grow();
                }

            }


            // dang set la hour co the luu load ra sai so lon




        }
    }



    public void startChangeFW()
    {
        StartCoroutine(ColorFarmToWatered(wateredColor, 3));
        isWater = true;
        if (gameObject.transform.childCount > 1)
        {
            Transform takePlant = gameObject.transform.GetChild(1);

            DataManager.CropData cropData = dataManager.SearchCropDataByPrefabId(takePlant.gameObject.name);
            cropData.isHaveWater = true;
          
        }

    }
    IEnumerator ColorFarmToWatered(Color endValue, float duration)
    {

        yield return new WaitForSeconds(3f);

        Renderer gameObjectMat = select.GetComponent<Renderer>();
        Color startValue = gameObjectMat.material.color;
     //   gameObjectMat.material.color = Color.Lerp(wateredColor, farmLandColor, Time.deltaTime * 0.2f);
        float time = 0;
        while (time < duration)
        {
            gameObjectMat.material.color = Color.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        gameObjectMat.material.color = endValue;

       
    }
    public void startChangeWF()
    {
        StartCoroutine(ColorWateredToFarm(farmLandColor, 3));

        isWater = false;
        if (gameObject.transform.childCount > 1)
        {
            Transform takePlant = gameObject.transform.GetChild(1);

            DataManager.CropData cropData = dataManager.SearchCropDataByPrefabId(takePlant.gameObject.name);
            cropData.isHaveWater = false;
            Debug.Log("IShave = false");
        }
    }
    IEnumerator ColorWateredToFarm(Color endValue, float duration)
    {

        yield return new WaitForSeconds(3f);

        Renderer gameObjectMat = select.GetComponent<Renderer>();
        Color startValue = gameObjectMat.material.color;
      //  gameObjectMat.material.color = Color.Lerp(wateredColor, farmLandColor, Time.deltaTime * 0.2f);
        float time = 0;
        while (time < duration)
        {
            gameObjectMat.material.color = Color.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        gameObjectMat.material.color = endValue;

       
    }

    public void ChangeDataWateredToFarm() // Doi lai data ->>
    {

        DataManager.TerrainData thisTer = dataManager.SearchTerrainInDataByObjectId(gameObject.name);

 
        thisTer.objectID = thisTer.itemID + gameObject.name.Substring(gameObject.name.IndexOf("_"));


        gameObject.name = thisTer.objectID;



    }

    
}
