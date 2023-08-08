using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogCanvas : MonoBehaviour
{
    public static LogCanvas Instance { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
