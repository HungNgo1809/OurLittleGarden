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

    private int growth;

    private int growthMax;
    public enum CropState
    {
        Seed , Seeding , Harvestable
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public CropState cropstate;

  
    public void Plant(Transform dirtPosition )
    {
        // hien tai fix cung them data - so gio lon de thu hoach - fix cung = 1
        GameObject obj = Instantiate(curObject, dirtPosition.transform);
        dataManager.AddPlant(obj, obj.name, obj.name, 1, ((int)cropstate));
        obj.transform.position = dirtPosition.transform.position + new Vector3(0,1.01f,0);

        int hourToGrow = GameTimestamp.DaysToHours(1);
        // chuyen ve minute
        growthMax = GameTimestamp.HoursToMinutes(hourToGrow);
 
        Debug.Log(growthMax + "growthMaxup");

        SwitchState(CropState.Seed);
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
           
            int hourToGrow = GameTimestamp.DaysToHours(1);
            // chuyen ve minute
            growthMax = GameTimestamp.HoursToMinutes(hourToGrow);
        }
        Debug.Log(growth + "a");

        if (growth >= growthMax / 2 && cropstate == CropState.Seed)
        {
            SwitchState(CropState.Seeding);
        }


        if (growth >= growthMax && cropstate == CropState.Seeding)
        {
            SwitchState(CropState.Harvestable);
        }
        
    }
    public void SwitchState(CropState stateToSwitch)
    {
        seed.gameObject.SetActive(false);
        seeding.gameObject.SetActive(false);
        harvestable.gameObject.SetActive(false);
        switch (stateToSwitch)
        {
            case CropState.Seed :
                seed.gameObject.SetActive(true);
                break;
            case CropState.Seeding :
                seeding.gameObject.SetActive(true);
                break;

            case CropState.Harvestable :
                harvestable.gameObject.SetActive(true);
                break;
        }

        cropstate = stateToSwitch;
    }
}
