using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewData", menuName = "CraftManager")]
public class CraftManager : ScriptableObject
{
    public List<Tool> toolsList;

    public DataManager dataManager;

    public bool craftable;

    public Tool curTool;
    public void OnClickSearchMaterial(Tool tool)
    {
        curTool = tool;
        int count = 0;
        foreach(Material mat in tool.materials)
        {
            int num = NumberMaterial(mat.prefabItemId);
            if(num >= mat.number)
            {
                count++;
                mat.isEnough = true;
            }
            else
            {
                mat.isEnough = false;
            }
        }

        if(count >= tool.materials.Count())
        {
            // đủ điều kiện craft
            craftable = true;
        }
        else
        {
            craftable = false;
        }    
    }

    public void OnClickCraft(Tool tool)
    {
        if(DisplayInventory.Instance.SearchFirstEmptyUiSlot() == null)
        {
            // thông báo túi đồ full
        }else    
        if(craftable)
        {
            DeleteMaterialInInventory(tool);

            dataManager.AddItemToInventoryData(tool.toolID, tool.type, tool.speType, tool.durability, int.Parse(DisplayInventory.Instance.SearchFirstEmptyUiSlot().name), tool.quickSellMoney);

            DisplayInventory.Instance.ClearAll();
            DisplayInventory.Instance.UpdateUI();
        }    
    }
    public void OnClickBuildCraft(Tool tool)
    {
        DeleteMaterialInInventory(tool);

        //Build
        if(tool.toolID == "campFire")
        {
            Builder.Instance.StartBuild(tool.toolID, new Vector3(0, 1.5f, 0), 0);
        }
        else
        {
            Builder.Instance.StartBuild(tool.toolID, new Vector3(0, 1, 0), 0);
        }    
    }    
    
    public void DeleteMaterialInInventory(Tool tool)
    {
        foreach (Material mat in tool.materials)
        {
            for(int i = 0; i < mat.number; i++)
            {
                //Debug.Log(mat.prefabItemId);
                dataManager.RemoveItemInDataByPrefabId(mat.prefabItemId);


                DisplayInventory.Instance.ClearAll();
                DisplayInventory.Instance.UpdateUI();
            }
        }
    }
    public int NumberMaterial(string prefabId)
    {
        return dataManager.CountNumberItemWithSamePrefabId(prefabId);
    }
    public Tool SearchToolByToolId(string toolId)
    {
        return toolsList.Where(p => p.toolID == toolId).FirstOrDefault();
    }

    [System.Serializable]
    public class Tool
    {
        public string toolID;
        public Sprite icon;

        public string type;
        public string speType;

        public float durability;
        public int quickSellMoney;

        public List<Material> materials;
    }

    [System.Serializable]
    public class Material
    {
        public string prefabItemId;
        public int number;

        public bool isEnough;
        public GameObject materialUi;
    }
}
