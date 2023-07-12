using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class TriggerPhoton : MonoBehaviour
{
    public PhotonConnector photon;

    public LargeMapData largeMapData;

    public DataManager dataManager;
    Collider player;

    bool isTrigger;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player = other;

            isTrigger = true;
            //Debug.Log("storage");

            if (other.gameObject.GetComponentInChildren<playerFoatingUiPhoton>() != null)
            {
                other.gameObject.GetComponentInChildren<playerFoatingUiPhoton>().transform.GetChild(0).gameObject.SetActive(true);
                PlayerControl control = other.gameObject.GetComponent<PlayerControl>();
                if(control.bubbleAnimation_Ship != null)
                {
                    control.bubbleAnimation_Ship.Play("startBubbleE");
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isTrigger = false;
            if (other.gameObject.GetComponentInChildren<playerFoatingUiPhoton>() != null)
            {
                other.gameObject.GetComponentInChildren<playerFoatingUiPhoton>().transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
    private void Update()
    {
        if(isTrigger)
        {
            if (Input.GetButtonDown("Use"))
            {
                if(PhotonNetwork.CurrentRoom.Name == dataManager.userId)
                {
                    UiManager.Instance.startGentTime();
                    largeMapData.saveCount = 0;
                    largeMapData.LoadDataFromPlayFab();

                    photon.LeaveAndJoinLargeRoom();
                    QuestManager.Instance.CheckQuestRequirementEnterWorld();
                    QuestManager.Instance.CheckSideQuestRequirementEnterWorld();
                    //Debug.Log("use");
                    StartCoroutine(LoadLargeLevel());
                }
                else
                {
                    PhotonChatController.Instance.BackHome();
                }
            }
        }    
    }

    IEnumerator LoadLargeLevel()
    {
        UiManager.Instance.UpdateCropData();

        Debug.Log("courotine");
        yield return new WaitUntil(() => (photon.changedToLargeRoom && largeMapData.loadSuccess));

        Debug.Log("loadLevel");
        //largeMapData.loadSuccess = false;
        //PhotonNetwork.LoadLevelIfSynced("Social");
        //SceneManager.LoadScene("Social");
        //LoadingScreen.Instance.StartLoadingScreen("Social");
        LoadingScreen.Instance.FlexLoadingScreenWithoutBool("Social");
    }

   
}
