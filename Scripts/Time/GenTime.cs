using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class GenTime : MonoBehaviour
{
    public DataManager dataManager;
    public int TimePass = 0;
    // Start is called before the first frame update
    void Start()
    {
        TimePass = dataManager.timePassed;
        for(int i =0; i < TimePass; i++)
        {
            TimeManager.Instance.Tick();

        }
        dataManager.timePassed = 0;
        TimePass = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator TimeUpdate()
    {
        while (true)
        {

            TimePass++;
            dataManager.timePassed = TimePass;
            yield return new WaitForSeconds(1 / 1);

        }

    }

}
