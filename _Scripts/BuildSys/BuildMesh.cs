using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMesh : MonoBehaviour
{
    public bool canBuild;

    public GameObject Green;
    public GameObject Red;

    public Transform[] RaycastStart;
    public bool isColide;
    private void OnTriggerEnter(Collider other)
    {
        isColide = true;
    }

    private void OnTriggerStay(Collider other)
    {
        isColide = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isColide = false;
    }
    public bool CheckRayCast()
    {
        if(RaycastStart.Length > 0)
        {
            foreach (Transform start in RaycastStart)
            {
                RaycastHit hit;
                Physics.Raycast(start.position, Vector3.down, out hit);

                if (hit.distance >= 0.2)
                {
                    return false;
                }
            }
        }    
        return true;
    }
    private void Update()
    {
        if(!isColide && CheckRayCast())
        {
            canBuild = true;

            Green.SetActive(true);
            Red.SetActive(false);
        }
        else
        {
            canBuild = false;

            Green.SetActive(false);
            Red.SetActive(true);
        }
    }
}
