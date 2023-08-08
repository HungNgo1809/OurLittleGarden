using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BuildTriggerCanvas : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject UIPanel;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            UIPanel.gameObject.SetActive(true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            UIPanel.gameObject.SetActive(false);
        }
    }
}
