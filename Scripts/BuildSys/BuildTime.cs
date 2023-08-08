using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static Lands;
using UnityEngine.Playables;


public class BuildTime : MonoBehaviour, ITimeTracker
{
    public DataManager dataManager;
    public BuildUI UIPanel;
    public BoxCollider box;
    public int growth;

    public int growthMax;

    public bool isDone = false;
    void Start()
    {
        if (growth >= growthMax)
        {
            box.enabled = true;
            growth = growthMax;
        }

        //add listener
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.RegisterTracker(this);
        }


    }
    /// <summary>
    ///  Hien tai de Grow() moi khi dat dc tuoi nuoc - neu chua tuoi thi khong lon  (test thay kha lau)- thieu data 
    /// </summary>
    public void Grow() // goi theo tung phut
    {
        growth++;
        if (growth < growthMax)
        {
            box.enabled = false;

           
        }

        if (growth >= growthMax)
        {
            box.enabled = true;

            isDone = true;
            growth = growthMax;
        }


    }
    public void ClockUpdate(GameTimestamp timestamp)
    {
        if(!isDone)
        {
            Grow();
        }
      
    }
}