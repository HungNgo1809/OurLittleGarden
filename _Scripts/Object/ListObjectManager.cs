using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListObjectManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static ListObjectManager Instance { get; set; }

    public List<GameObject> plantObject;
    public List<GameObject> TerObject;
    public List<GameObject> buildObject;
    public List<GameObject> terObject;
    //public List<GameObject> interiorObject;
    void Start()
    {
        Instance = this; // Set the instance to this script instance
        plantObject = new List<GameObject>(); // Initialize the list
        TerObject = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool CheckForDuplicatePlantObject(string uniqueNumber)
    {
        foreach (GameObject obj in plantObject)
        {
            if (obj.name.Contains(uniqueNumber))
            {
                return true; // Duplicate found
            }
        }
        return false; // No duplicates found
    }
    public bool CheckForDuplicateTerObject(string uniqueNumber)
    {
        foreach (GameObject obj in TerObject)
        {
            if (obj.name.Contains(uniqueNumber))
            {
                return true; // Duplicate found
            }
        }
        return false; // No duplicates found
    }

    public void RemoveBuild(GameObject obj)
    {
        buildObject.Remove(obj);
    }
}
