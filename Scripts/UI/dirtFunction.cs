using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class dirtFunction : MonoBehaviour
{
    // Start is called before the first frame update
    public Canvas cans;

    void Start()
    {
       cans.worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        
    }

    // Update is called once per frame
    void Update()
    {
     
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            cans.gameObject.SetActive(true);
            //   cans.transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.back);
            /*
               Vector3 targetPosition = Camera.main.transform.position;
               targetPosition.z = cans.transform.position.z; // Keep the z position unchanged
               Vector3 direction = targetPosition - cans.transform.position;
               Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
               Debug.Log(rotation.y);
               if(rotation.y > 0)
               {
                   cans.transform.rotation = Quaternion.Euler(0, -rotation.eulerAngles.y + 90, 0);
               }
               else if(rotation.y < 0) { cans.transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y + 90, 0); }
            */
            cans.transform.rotation = Quaternion.LookRotation(cans.transform.position - Camera.main.transform.position);
        }
   
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            cans.gameObject.SetActive(false);
        }
      
    }  
}
