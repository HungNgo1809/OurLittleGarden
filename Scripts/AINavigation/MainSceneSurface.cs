using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DataManager;
using UnityEngine.AI;

public class MainSceneSurface : MonoBehaviour
{
  


    public GameObject surafce;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BakeDelay());
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public IEnumerator BakeDelay()
    {
        yield return new WaitForSeconds(0.02f);

        if (surafce.GetComponent<NavMeshSurface>() != null && surafce != null)
        {
            NavMeshSurface surface = surafce.GetComponent<NavMeshSurface>();
            surface.BuildNavMesh();
            Debug.Log(surface.gameObject);
        }

    }
    

}
