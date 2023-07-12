using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static PlantState;

public class PlantUI : MonoBehaviour
{
    public DataManager dataManager;
    public PlantState state;
    public GameObject parent;
    public Text timegrowth_text;
    public GameObject wateringIcon;
    public int remainingTimeMinutes;
    public Canvas can ;
    public Box collider;
    public Vector3 canvas_positionHarvest;
    private Vector3 endScale = new Vector3(3,3,3);

    // Start is called before the first frame update
    void Awake()
    {
    
        can.worldCamera= Camera.main;
       
    }

    // Update is called once per frame
    void Update()
    {
        // Get the direction to the main camera
        // thieu quay mat vao cam
        if(gameObject.activeSelf == true)
        {
            displayTimeGrowth();
            RotateTowardsTarget();

        }
        // RotateTowardsTarget();
        

    }
 
  

    public void displayTimeGrowth()
    {
      
        int timegrowth_int = state.growth;
        int timegrowthmax_int = state.growthMax;
        DataManager.CropData cropData = dataManager.SearchCropDataByPrefabId(parent.name);

        wateringIcon.SetActive(false);
        timegrowth_text.gameObject.SetActive(false);


        if (cropData.isHaveWater && state.cropstate != CropState.Harvestable) // dang co nuoc + chua thu hoach dc
        {
            if(wateringIcon.transform.localScale != new Vector3(1,1,1))
            {
                startScaleBack(wateringIcon.transform); //scale Watering icooon back t oo111
            }
         
          
            wateringIcon.SetActive(false);
            timegrowth_text.gameObject.SetActive(true);

            remainingTimeMinutes = timegrowthmax_int - timegrowth_int;

            // Convert remaining time in minutes to 0:00:00 format
            string remainingTime = string.Format("{0:D2}:{1:D2}", remainingTimeMinutes / 60, remainingTimeMinutes % 60);

            // Set text component to display remaining time
            timegrowth_text.text = remainingTime;
            
        }
        else if (state.cropstate == CropState.Harvestable)
        {
          
            timegrowth_text.gameObject.SetActive(false);
            wateringIcon.SetActive(false);
          

        }
        else if(!cropData.isHaveWater)
        {
            timegrowth_text.gameObject.SetActive(false);
           
            wateringIcon.SetActive(true);
            startChangeWaterIcon(); 
        }
     
    }
 
    public void RotateTowardsTarget()
    {

        // Get the direction to the main camera
        can.transform.LookAt(can.transform.position + Camera.main.transform.rotation * -Vector3.back, Camera.main.transform.rotation * Vector3.up);
        
        /*
         *
        // Calculate the direction to the target
        Vector3 directionToTarget = targetPosition - objectToRotate.position;
        directionToTarget.y = 0f; // Set the Y-axis component to zero to ignore rotation along the X-axis

        // Rotate the object to face the target
        objectToRotate.rotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
        */
    }


   

    public void startChangeWaterIcon() // Thieu scale cua Harvest
    {
        StartCoroutine(ScaleIcon(wateringIcon.transform, endScale, 10));
    }

    IEnumerator ScaleIcon(Transform iconTransform, Vector3 endScale, float duration)
    {
        yield return new WaitForSeconds(0);

        float time = 0;
        while (time < duration)
        {
            iconTransform.localScale = Vector3.Lerp(iconTransform.localScale, endScale, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
      
    }
    public void startScaleBack(Transform iconTransform)
    {
        iconTransform.localScale = new Vector3(1,1,1);
      
    }
   

}
