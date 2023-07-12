using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPanel : MonoBehaviour
{
    public GameObject buildingToDestroy;

    public DataManager dataManager;

    //public ParticleSystem destroyVfx;
    private void OnEnable()
    {
        //destroyVfx.transform.position = buildingToDestroy.transform.position;
    }
    public void ConfirmDestroy()
    {
        //destroyVfx.Play();
        if(buildingToDestroy.GetComponent<ToolTable>() != null)
        {
            buildingToDestroy.GetComponent<ToolTable>().mesh.SetActive(false);
            buildingToDestroy.GetComponent<ToolTable>().playDestroyVfx();
            buildingToDestroy.GetComponent<ToolTable>().StartDestroy();
        }
        else if(buildingToDestroy.GetComponent<TriggerIdeaMachine>() != null)
        {
            buildingToDestroy.GetComponent<TriggerIdeaMachine>().mesh.SetActive(false);
            buildingToDestroy.GetComponent<TriggerIdeaMachine>().playDestroyVfx();
            buildingToDestroy.GetComponent<TriggerIdeaMachine>().StartDestroy();
        }
        else if (buildingToDestroy.GetComponent<TriggerStore>() != null)
        {
            buildingToDestroy.GetComponent<TriggerStore>().mesh.SetActive(false);
            buildingToDestroy.GetComponent<TriggerStore>().playDestroyVfx();
            buildingToDestroy.GetComponent<TriggerStore>().StartDestroy();
        }    
        dataManager.RemoveBuildingFromDataByObjectId(buildingToDestroy.name);
        ListObjectManager.Instance.RemoveBuild(buildingToDestroy);
        this.gameObject.SetActive(false);

        //StartCoroutine(DestroyBuilding());
    }  
    public void CancleDestroy()
    {
        this.gameObject.SetActive(false);
    }    
}
