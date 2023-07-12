using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerStore : MonoBehaviour
{
    public DataManager dataManager;

    //public GameObject UiResponse;
    bool isTrigger;
    bool isUsing;

    Collider player;
    //bool isEdit;
    public ParticleSystem destroyVfx;
    public GameObject mesh;

    public AudioSource destroySound;
    private void Start()
    {
        StartCoroutine(AddNewStorageIfNew());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && PhotonNetwork.CurrentRoom.Name == dataManager.userId)
        {
            player = other;

            isTrigger = true;
            //Debug.Log("storage");

            if (other.gameObject.GetComponentInChildren<playerFloatingUi>() != null)
            {
                other.gameObject.GetComponentInChildren<playerFloatingUi>().transform.GetChild(0).gameObject.SetActive(true);
                PlayerControl control = other.gameObject.GetComponent<PlayerControl>();
                control.bubbleAnimation.Play("startBubble");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && PhotonNetwork.CurrentRoom.Name == dataManager.userId)
        {
            isTrigger = false;
            if (other.gameObject.GetComponentInChildren<playerFloatingUi>() != null)
            {
                other.gameObject.GetComponentInChildren<playerFloatingUi>().transform.GetChild(0).gameObject.SetActive(false);
            }

            CloseStorage();
        }
    }

    IEnumerator AddNewStorageIfNew()
    {
        yield return new WaitForSeconds(0.3f);

        if (dataManager.SearchStorageById(transform.name) == null)
        {
            dataManager.AddStorage(transform.name);
        }
    }

    private void Update()
    {
        if (isTrigger)
        {
            if (Input.GetButtonDown("Use") && !isUsing)
            {
                UseStorage();
            }
            else if (Input.GetKeyDown(KeyCode.L) && !isUsing)
            {
                UiManager.InstanceDestroyPanel.gameObject.SetActive(true);
                UiManager.InstanceDestroyPanel.buildingToDestroy = this.gameObject;
            }
            else if (Input.GetButtonDown("EditPosition") && !Builder.Instance.isEdit && !Builder.Instance.isBuliding && !isUsing)
            {
                if (player != null)
                {
                    if (player.gameObject.GetComponentInChildren<playerFloatingUi>() != null)
                    {
                        player.gameObject.GetComponentInChildren<playerFloatingUi>().transform.GetChild(0).gameObject.SetActive(false);
                    }
                }
                EditPosition();
            }
            else if (Input.GetButtonDown("Use") && isUsing)
            {
                CloseStorage();
            }
        }

        /*
        if (isEdit)
        {
            EditPosition();
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                isEdit = false;
            }
        }*/
    }

    public void UseStorage()
    {
        DisplayInventory.Instance.UpdateUI();

        UiManager.Instance.ActiveStoragePanel(true);
        UiManager.Instance.curStorage = transform.name;

        UiManager.Instance.DisplayStorageItem(transform.name);

        isUsing = true;
    }
    public void CloseStorage()
    {
        if (UiManager.Instance.storagePanel.activeSelf)
        {
            UiManager.Instance.ActiveStoragePanel(false);
            UiManager.Instance.curStorage = "0";

            DisplayInventory.Instance.UpdateUI();
            isUsing = false;
        }
    }

    public void EditPosition()
    {
        Builder.Instance.StartEditPos(transform.gameObject, PrefabBuild.Instance.ReQuestItem("storage").Mesh_GameObject, new Vector3(0, 1, 0));
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
