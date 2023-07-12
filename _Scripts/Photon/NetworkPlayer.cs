using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class NetworkPlayer : MonoBehaviour
{
    DisplayUserName disPlayUserName; 
    CharacterController charControl;
    PhotonView photonView;

    public GameObject addFriendUi;
    // Start is called before the first frame update
    void Start()
    {
        charControl = GetComponent<CharacterController>();
        photonView = GetComponent<PhotonView>();
        disPlayUserName = GetComponent<DisplayUserName>();

        if(charControl != null && photonView!= null)
        {
            if(photonView.IsMine == false)
            {
                GetComponent<PlayerControl>().enabled = false;
                GetComponent<UseItem>().enabled = false;
                GetComponent<PlayerExpoit>().enabled = false;
                GetComponent<HandButtonManager>().enabled = false;
                GetComponent<CharacterController>().enabled = false;
                GetComponent<FriendManager>().enabled = false;

                //GetComponent<MeshCollider>().enabled = true;
            }
            GetComponentInChildren<Interactor>().enabled = false;
        }    
    }

    public void Click()
    {
        if (photonView.IsMine == false)
        {
            // Hiển thị nút add friend
            if(addFriendUi.activeSelf)
            {
                addFriendUi.SetActive(false);
            }
            else
            {
                addFriendUi.SetActive(true);
            }    
        }
    }

    public void ConfirmSendFriendRequest()
    {
        //FriendManager.Instance.UpdateFriendRequest();
        addFriendUi.SetActive(false);
        FriendManager.Instance.OnClickSendFriendRequest(disPlayUserName.playerId, disPlayUserName.playerNameText.text);
    }    
}
