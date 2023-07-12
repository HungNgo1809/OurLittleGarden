using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendPanel : MonoBehaviour
{
    public GameObject[] listPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ActiveAndDeactive(GameObject button)
    {
        if(button.activeSelf == false)
        {
            button.SetActive(true);
        }
        foreach(GameObject go in listPanel)
        {
            if(go.name != button.name && go.activeSelf == true)
            {
                go.SetActive(false);
            }
        }
    }
}
