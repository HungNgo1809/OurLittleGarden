using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class textAnimation : MonoBehaviour
{
    // Start is called before the first frame update

    public TextMeshProUGUI dot;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void changeDotToThree()
    {
        dot.text = ". . .";
        Debug.Log("3");
    }

    public void changeDotToTwo()
    {
        dot.text = ". .";
        Debug.Log("2");
    }


    public void changeDotToOne()
    {
        dot.text = ".";
        Debug.Log("1");
    }
    public void changeDotToZero()
    {
        dot.text = "";
    }
}
