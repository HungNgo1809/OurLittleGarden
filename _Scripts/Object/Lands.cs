using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Lands : MonoBehaviour, ITimeTracker
{
    public Interactor interactor;
    public enum LandStatus
    {
        FarmLand , Watered
    }

    public LandStatus landStatus;

    GameObject select;
    GameTimestamp timeWatered;

    PlantState plantState;

    public GameObject cropPrefab;
    public bool fromFarmToWatered ;
    public bool fromWateredToFarm;


    //color
    Color farmLandColor = new Color(233 / 255.0f, 200 / 255.0f, 166 / 255.0f, 1);
    Color wateredColor = new Color(51 / 255.0f, 38 / 255.0f, 24 / 255.0f, 1);
    void Update()
    {
       
        
    }
    void Start( )
    {
        select = gameObject;
        landStatus = LandStatus.FarmLand;
        interactor = GameObject.Find("Interactor").GetComponent<Interactor>();
        fromFarmToWatered = false;
        fromWateredToFarm = false;
        //add listener
        TimeManager.Instance.RegisterTracker(this);

    }
  
   
    public void SwitchState(LandStatus landStatus_)
    {
        landStatus = landStatus_;
        switch (landStatus_)
        {
            case LandStatus.FarmLand:
                startChangeTF();

                break;
            case LandStatus.Watered:
         
                startChangeFT();
               
                timeWatered = TimeManager.Instance.GetGameTimestamp();
                Debug.Log(timeWatered);
                break;

           
        }

        //cropstate = stateToSwitch;
    }


    public void takePlantsInChild()
    {
        plantState = gameObject.GetComponentInChildren<PlantState>();
    }

    public void ClockUpdate(GameTimestamp timestamp)
    {
        if(landStatus == LandStatus.Watered)
        {
           int timeSinceWatered = GameTimestamp.CompareTimestamps(timeWatered, timestamp);
            Debug.Log(timeSinceWatered);

            if(timeSinceWatered > 24) 
            {
                SwitchState(LandStatus.FarmLand);
            }
            if(plantState != null )
            {
                plantState.Grow();
                Debug.Log("a");
            }
            
        }
    }



    public void startChangeFT()
    {
        StartCoroutine(ColorFarmToWatered(wateredColor, 3));

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
    public void startChangeTF()
    {
        StartCoroutine(ColorWateredToFarm(farmLandColor, 3));

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
}
