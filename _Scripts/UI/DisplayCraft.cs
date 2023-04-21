using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayCraft : MonoBehaviour
{
    public CraftManager craftManager;

    public Transform materialPanel;
    public GameObject craftBtn;

    public List<GameObject> materialUi;
    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        ClearAll();
    }

    public void CraftMaterialUI(CraftManager.Tool tool)
    {
        ClearAll();
        if (tool != null)
        {
            foreach (CraftManager.Material mat in tool.materials)
            {
                if (mat.isEnough)
                {
                    GameObject objUI = Instantiate(mat.materialUi);
                    materialUi.Add(objUI);
                    objUI.transform.parent = materialPanel;
                }
            }
        }
    }

    public void OnClickTool(string toolId)
    {
        CraftManager.Tool tool = craftManager.SearchToolByToolId(toolId);
        craftManager.OnClickSearchMaterial(tool);

        if (craftManager.craftable && !craftBtn.activeSelf)
        {
            craftBtn.SetActive(true);
        }
        if (!craftManager.craftable && craftBtn.activeSelf)
        {
            craftBtn.SetActive(false);
        }

        if (craftManager.curTool != null)
        {
            CraftMaterialUI(craftManager.curTool);
        }
    }
    public void OnClickCraft(string toolId)
    {
        CraftManager.Tool tool = craftManager.SearchToolByToolId(toolId);
        craftManager.OnClickCraft(tool);

        OnClickTool(toolId);
    }
    public void ClearAll()
    {
        foreach(GameObject ui in materialUi)
        {
            Destroy(ui);
        }
        materialUi.Clear();
    }
}
