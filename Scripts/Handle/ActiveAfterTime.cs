using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveAfterTime : MonoBehaviour
{
    public float waitTime;
    public GameObject ActiveObject;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("active", waitTime);
    }

    public void active()
    {
        ActiveObject.SetActive(true);
    }    


}
