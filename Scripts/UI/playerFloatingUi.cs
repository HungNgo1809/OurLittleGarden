using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerFloatingUi : MonoBehaviour
{
    public bool isLookFull;
    // Update is called once per frame
    void Update()
    {
        if(!isLookFull)
        {
            transform.rotation = Quaternion.Euler(Quaternion.LookRotation(transform.position - Camera.main.transform.position).eulerAngles.x, transform.rotation.y, transform.rotation.z);
        }
        else
        {
            //transform.rotation = Quaternion.Euler(Quaternion.LookRotation(transform.position - Camera.main.transform.position).eulerAngles.x, Quaternion.LookRotation(transform.position - Camera.main.transform.position).eulerAngles.y, Quaternion.LookRotation(transform.position - Camera.main.transform.position).eulerAngles.z);
            transform.rotation = Quaternion.Euler(Quaternion.LookRotation(transform.position - Camera.main.transform.position).eulerAngles.x, Quaternion.LookRotation(transform.position - Camera.main.transform.position).eulerAngles.y - 13.0f, 0);
            //transform.LookAt(Camera.main.transform.position);
        }    

    }
}
