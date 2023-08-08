using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerIdeaMachine : MonoBehaviour
{
    bool isTrigger;
    bool isUsing;

    Collider player;

    public ParticleSystem destroyVfx;
    public GameObject mesh;
    //bool isEdit;
    //public GameObject UiResponse;

    public AudioSource destroySound;
    public DataManager dataManager;
    public enum type
    {
        IdeaMachine,
        AnimalHouse
    }

    [SerializeField]
    private type Type;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && PhotonNetwork.CurrentRoom.Name == dataManager.userId)
        {
            player = other;
            isTrigger = true;

            ActiveInteractUi(other, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && PhotonNetwork.CurrentRoom.Name == dataManager.userId)
        {
            isTrigger = false;

            ActiveInteractUi(other, false);

            CloseIdea();
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
                switch (Type)
                {
                    case type.IdeaMachine:
                        UiManager.Instance.ActiveIdeaPanel(true);
                        isUsing = true;
                        break;
                    case type.AnimalHouse:
                        UiManager.Instance.ActiveAnimalHousePanel(true);
                        isUsing = true;
                        break;
                }
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
                    ActiveInteractUi(player, false);
                }
                EditPosition();
            }
            else if (Input.GetButtonDown("Use") && isUsing)
            {
                CloseIdea();
            }
        }
        /*
        if(!isTrigger && Input.GetButtonDown("Use"))
        {
            UiManager.Instance.ActiveIdeaPanel(false);
        }*/
    }

    public void CloseIdea()
    {
        switch(Type)
        {
            case type.IdeaMachine:
                if (UiManager.Instance.ideaPanel.activeSelf)
                {
                    UiManager.Instance.ActiveIdeaPanel(false);
                }
                isUsing = false;
                break;
            case type.AnimalHouse:
                if (UiManager.Instance.animalHousePanel.activeSelf)
                {
                    UiManager.Instance.ActiveAnimalHousePanel(false);
                }
                isUsing = false;
                break;
        }
    }
    public void EditPosition()
    {
        Builder.Instance.StartEditPos(transform.gameObject, PrefabBuild.Instance.ReQuestItem("ideaMachine").Mesh_GameObject, new Vector3(0, 1, 0));
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
