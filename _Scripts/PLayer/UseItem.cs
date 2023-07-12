using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class UseItem : MonoBehaviour
{
    public DataManager dataManager;
    public PhotonView photonView;

    public HandButtonManager handButtonManager;

    public AudioSource eatSound;
    void Update()
    {
        if (Input.GetButtonDown("Interact") && photonView.IsMine)
        {
            if (handButtonManager.curTool != "none")
            {
                Eat();
            }
        }
    }

    public void Eat()
    {
        if (handButtonManager.curType == "food" || handButtonManager.curType == "harvest")
        {
            eatSound.Play();
            float value = dataManager.SearchItemInDataByObjectId(handButtonManager.curToolObjId).durability;
            StatusManager.Instance.ChangeFood(value);

            dataManager.RemoveItemInDataByObjectId(handButtonManager.curToolObjId);
            QuestManager.Instance.CheckQuestRequirementEat(handButtonManager.curToolSpeType);
            QuestManager.Instance.CheckSideQuestRequirementEat(handButtonManager.curToolSpeType);
            //animation
            if (handButtonManager.curObjTool != null)
            {
                handButtonManager.curObjTool.SetActive(false);
            }

            handButtonManager.curTool = "none";
            handButtonManager.curToolObjId = "none";
            handButtonManager.curToolSpeType = "none";
            handButtonManager.curType = "none";
        }
    }    
}
