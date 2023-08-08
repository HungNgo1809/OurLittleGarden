using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerFoatingUiPhoton : MonoBehaviour
{
    void Update()
    {
        transform.rotation = Quaternion.Euler(Quaternion.LookRotation(transform.position - Camera.main.transform.position).eulerAngles.x, Quaternion.LookRotation(transform.position - Camera.main.transform.position).eulerAngles.y, transform.rotation.z);
    }
}
