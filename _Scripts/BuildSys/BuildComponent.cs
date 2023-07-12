using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildComponent : MonoBehaviour
{
    public string selectItem;
    public DataManager dataManager;

    public Vector3 offset;
    public void OnClickBuild(int coins)
    {
        if (dataManager.coins >= coins)
        {
            //dataManager.coins = dataManager.coins - coins;

            Builder.Instance.StartBuild(selectItem, offset, coins);
            UiManager.Instance.TurnOffPlayerInteract();
            //Debug.Log("on click" + selectItem);
        }
    }
}
