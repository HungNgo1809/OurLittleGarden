using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTable : MonoBehaviour
{
    bool isTrigger;
    bool isUsing;

    //public DestroyPanel confirmDestroyPanel;
    Collider player;
    //bool isEdit;
    //public GameObject UiResponse;

    public ParticleSystem destroyVfx;

    public GameObject mesh;

    public AudioSource destroySound;
    public enum TecType
    {
        CraftTable,
        Stove,
        Furnace,
        Oven,
        WindMil
    }

    [SerializeField]
    private TecType Machine;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            player = other;
            isTrigger = true;

            ActiveInteractUi(other, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isTrigger = false;

            ActiveInteractUi(other, false);

            CloseCraft();
        }
    }

    public void ActiveInteractUi(Collider other, bool active)
    {
        if (other.gameObject.GetComponentInChildren<playerFloatingUi>() != null)
        {
            other.gameObject.GetComponentInChildren<playerFloatingUi>().transform.GetChild(0).gameObject.SetActive(active);
            PlayerControl control = other.gameObject.GetComponent<PlayerControl>();
            control.bubbleAnimation.Play("startBubble");

        }
    }


    private void Update()
    {
        if (isTrigger)
        {
            if (Input.GetButtonDown("Use") && !isUsing)
            {
                //UiManager.Instance.ActiveCraftPanel(true);
                switch (Machine)
                {
                    case TecType.CraftTable:
                        UiManager.Instance.ActiveCraftPanel(true);
                        break;
                    case TecType.Stove:
                        UiManager.Instance.ActiveStovePanel(true);
                        break;
                    case TecType.Furnace:
                        UiManager.Instance.ActiveFurnacePanel(true);
                        break;
                    case TecType.Oven:
                        UiManager.Instance.ActiveOvenPanel(true);
                        break;
                    case TecType.WindMil:
                        UiManager.Instance.ActiveWindMilPanel(true);
                        break;
                }
                isUsing = true;
            } else if (Input.GetKeyDown(KeyCode.L) && !isUsing)
            {
                UiManager.InstanceDestroyPanel.gameObject.SetActive(true);
                UiManager.InstanceDestroyPanel.buildingToDestroy = this.gameObject;
            }    
            else if (Input.GetButtonDown("EditPosition") && !Builder.Instance.isEdit && !Builder.Instance.isBuliding && !isUsing)
            {
                if(player != null)
                {
                    ActiveInteractUi(player, false);
                }
                EditPosition();
            }
            else if (Input.GetButtonDown("Use") && isUsing)
            {
                CloseCraft();
            }
        }
        /*
        if(!isTrigger && Input.GetButtonDown("Use"))
        {
            UiManager.Instance.ActiveCraftPanel(false);
        }*/    
    }

    public void CloseCraft()
    {
        switch (Machine)
        {
            case TecType.CraftTable:
                if(UiManager.Instance.caftPanel.activeSelf)
                {
                    UiManager.Instance.ActiveCraftPanel(false);
                    isUsing = false;
                }
                break;
            case TecType.Stove:
                if (UiManager.Instance.stovePanel.activeSelf)
                {
                    UiManager.Instance.ActiveStovePanel(false);
                    isUsing = false;
                }
                break;
            case TecType.Furnace:
                if (UiManager.Instance.furnacePanel.activeSelf)
                {
                    UiManager.Instance.ActiveFurnacePanel(false);
                    isUsing = false;
                }
                break;
            case TecType.Oven:
                if (UiManager.Instance.ovenPanel.activeSelf)
                {
                    UiManager.Instance.ActiveOvenPanel(false);
                    isUsing = false;
                }
                break;
            case TecType.WindMil:
                if (UiManager.Instance.windMilPanel.activeSelf)
                {
                    UiManager.Instance.ActiveWindMilPanel(false);
                    isUsing = false;
                }
                break;
        }   
    }
    public void EditPosition()
    {
        switch (Machine)
        {
            case TecType.CraftTable:
                Builder.Instance.StartEditPos(transform.gameObject, PrefabBuild.Instance.ReQuestItem("toolTable").Mesh_GameObject, new Vector3(0, 1, 0));
                break;
            case TecType.Stove:
                Builder.Instance.StartEditPos(transform.gameObject, PrefabBuild.Instance.ReQuestItem("campFire").Mesh_GameObject, new Vector3(0, 1.5f, 0));
                break;
            case TecType.Furnace:
                Builder.Instance.StartEditPos(transform.gameObject, PrefabBuild.Instance.ReQuestItem("furnace").Mesh_GameObject, new Vector3(0, 1.5f, 0));
                break;
            case TecType.Oven:
                Builder.Instance.StartEditPos(transform.gameObject, PrefabBuild.Instance.ReQuestItem("oven").Mesh_GameObject, new Vector3(0, 1.5f, 0));
                break;
            case TecType.WindMil:
                Builder.Instance.StartEditPos(transform.gameObject, PrefabBuild.Instance.ReQuestItem("windmil").Mesh_GameObject, new Vector3(0, 1.5f, 0));
                break;
        }            
    }

    public void playDestroyVfx()
    {
        destroySound.Play();
        destroyVfx.Play();
    }

    public void StartDestroy()
    {
        StartCoroutine(DestroyBuilding());
    }
    IEnumerator DestroyBuilding()
    {
        yield return new WaitForSeconds(150.0f * Time.deltaTime);
        Destroy(this.gameObject);
    }
}
