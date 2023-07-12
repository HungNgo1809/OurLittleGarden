using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalUI : MonoBehaviour
{
    public animalHouseComponent animalHouse;
    public GameObject parent;
    public Image harvest_icon;
    public Text timegrowth_text;
    public int remainingTimeMinutes;
    public Canvas can;


    // Start is called before the first frame update
    void Awake()
    {
        can.worldCamera = Camera.main;
        timegrowth_text.gameObject.SetActive(false);
        harvest_icon.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Get the direction to the main camera
        // thieu quay mat vao cam
        if(gameObject.activeSelf)
        {
            displayTimeGrowth();
            // RotateTowardsTarget();
            RotateTowardsTarget();
        }
   

    }
    // Thieu chinh cam vao main Cam
    public void displayTimeGrowth()
    {
        if (animalHouse.totalNumberTimeHarvest < animalHouse.totalGrowthMax && animalHouse.listAnimalState.Count > 0)
        {
            timegrowth_text.gameObject.SetActive(true);
           // harvest_icon.gameObject.SetActive(false);
            int timegrowth_int = animalHouse.totalNumberTimeHarvest;
            int timegrowthmax_int = animalHouse.totalGrowthMax;


            remainingTimeMinutes = (timegrowthmax_int - timegrowth_int) / animalHouse.listAnimalState.Count;

            // Convert remaining time in minutes to 0:00:00 format
            string remainingTime = string.Format("{0:D2}:{1:D2}", remainingTimeMinutes / 60, remainingTimeMinutes % 60);

            // Set text component to display remaining time
            timegrowth_text.text = remainingTime;
        }
        else if (animalHouse.totalGrowthMax > 0 && animalHouse.totalNumberTimeHarvest >= animalHouse.totalGrowthMax)
        {
            timegrowth_text.gameObject.SetActive(false);
            //harvest_icon.gameObject.SetActive(true);
            animalHouse.CheckCondition();
        }
        else if (animalHouse.totalGrowthMax == 0 && animalHouse.listAnimalState.Count > 0)
        {
            timegrowth_text.gameObject.SetActive(false);
           // harvest_icon.gameObject.SetActive(false);

        }

    }

    public void RotateTowardsTarget()
    {

        // Get the direction to the main camera
        //can.transform.LookAt(can.transform.position + Camera.main.transform.rotation * -Vector3.back, Camera.main.transform.rotation * Vector3.up);
        can.transform.rotation = Quaternion.Euler(Quaternion.LookRotation(can.transform.position - Camera.main.transform.position).eulerAngles.x, Quaternion.LookRotation(can.transform.position - Camera.main.transform.position).eulerAngles.y - 13.0f, 0);

    }

}
