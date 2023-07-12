using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiFunction : MonoBehaviour
{
    public void OpenUi(GameObject ui)
    {
        ui.SetActive(true);
    }
    public void CloseUi(GameObject ui)
    {
        ui.SetActive(false);
    }
}
