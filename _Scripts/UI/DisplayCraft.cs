using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class DisplayCraft : MonoBehaviour
{
    public CraftManager craftManager;
    public Transform materialPanel;
    public GameObject craftBtn;

    public GameObject CraftSpePanel;

    public Image toolImage;

    public TextMeshProUGUI ObjectName;

    public List<GameObject> materialUi;

    bool doneLoop;

    //public DataManager dataManager;
    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        ClearAll();
    }

    public void CraftMaterialUI(CraftManager.Tool tool)
    {
        Debug.Log(tool.toolID);
        ClearAll();
        if (tool != null)
        {
            foreach (CraftManager.Material mat in tool.materials)
            {
                GameObject objUI = Instantiate(mat.materialUi);
                int curCount = craftManager.NumberMaterial(mat.prefabItemId);
                //Debug.Log(mat.number);

                objUI.GetComponent<MaterialUi>().needNumber.text = mat.number.ToString();
                objUI.GetComponent<MaterialUi>().haveNumber.text = curCount.ToString() + " /";

                materialUi.Add(objUI);

                objUI.transform.parent = materialPanel;

                objUI.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            }

            doneLoop = true;

            StartCoroutine(ActiveCraftSpePanel());
        }
    }

    IEnumerator ActiveCraftSpePanel()
    {
        yield return new WaitUntil(() => doneLoop);

        CraftSpePanel.SetActive(true);
        doneLoop = false;
    }

    public void OnClickTool(string toolId)
    {
        CraftManager.Tool tool = craftManager.SearchToolByToolId(toolId);
        toolImage.sprite = tool.icon;
        ObjectName.text = ConvertToTitleCase(toolId);

        //craftManager.curTool = tool;
        craftManager.OnClickSearchMaterial(tool);

        if (craftManager.craftable && !craftBtn.activeSelf)
        {
            craftBtn.SetActive(true);
        }
        if (!craftManager.craftable && craftBtn.activeSelf)
        {
            craftBtn.SetActive(false);
        }

        if (craftManager.curTool != null && gameObject.activeSelf)
        {
            CraftMaterialUI(tool);
        }
    }
    public void OnClickCraft()
    {
        CraftManager.Tool tool = craftManager.curTool;
        craftManager.OnClickCraft(tool);

        OnClickTool(craftManager.curTool.toolID);
        QuestManager.Instance.CheckQuestRequirementCraft(tool.toolID);
        QuestManager.Instance.CheckSideQuestRequirementCraft(tool.toolID);
    }
    public void OnClickBuildCraft()
    {
        CraftManager.Tool tool = craftManager.curTool;
        craftManager.OnClickBuildCraft(tool);

        OnClickTool(craftManager.curTool.toolID);
    }    
    public void ClearAll()
    {
        foreach(GameObject ui in materialUi)
        {
            Destroy(ui);
        }
        materialUi.Clear();
    }
    public string ConvertToTitleCase(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        string output = char.ToUpper(input[0]) + input.Substring(1);

        for (int i = 1; i < output.Length; i++)
        {
            if (char.IsUpper(output[i]))
            {
                output = output.Insert(i, " ");
                i++;
            }
        }

        output.ToUpper();
        return output;
    }

    
}
