using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypePlant : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] plant;
    public GameObject[] tree;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TurnOffPLant()
    {

        foreach(var child in plant)
        {
            if(child.activeSelf == true)
            {
                child.gameObject.SetActive(false);
            }
          

        }
        foreach(var child in tree)
        {
            if (child.activeSelf == false)
            {
                child.gameObject.SetActive(true);
            }
        }
    }
    public void TurnOffTree()
    {

        foreach (var child in tree)
        {
            if (child.activeSelf == true)
            {
                child.gameObject.SetActive(false);
            }


        }
        foreach (var child in plant)
        {
            if (child.activeSelf == false)
            {
                child.gameObject.SetActive(true);
            }
        }
    }
}
