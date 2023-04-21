using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        if(craftable)
        {
            DeleteMaterialInInventory(tool);
            dataManager.AddItemToInventoryData(tool.toolID, "tool", tool.speType);
        }    
    }
    
    public void DeleteMaterialInInventory(Tool tool)
    {
        foreach (Material mat in tool.materials)
        {
            for(int i = 0; i < mat.number; i++)
            {
                dataManager.RemoveItemInDataByPrefabId(mat.prefabItemId);
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
        public string speType;
        public float durability;

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
