using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearData : MonoBehaviour
{
    // Dat object nay o scene dang nhap dau tien va khong cho quay ra, hoac destroy khi co vat the tuong tu
    public DataManager dataManager;

    private void Awake()
    {
        StartCoroutine(ClearOldData());
    }
    /*
    private void OnApplicationQuit()
    {
        //Clear old data
        StartCoroutine(ClearOldData());
    }*/

    IEnumerator ClearOldData()
    {
        //Clear old data
        dataManager.terData.Clear();
        dataManager.buildData.Clear();
        dataManager.cropData.Clear();
        dataManager.inventoryData.Clear();

        yield return null;
    }
}
