using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendsUiComponent : MonoBehaviour
{
    // Start is called before the first frame update
    public string id;
    public string name;

    public int status = 6;
    public bool isFriendList;

    public GameObject onlineStatus;
    public GameObject offlineStatus;
    public GameObject inviteButton;

    public Text nameDisplay;
    private void Start()
    {
        if(isFriendList)
        {
            StartCoroutine(DisPlayStatus());
        }
    }
    IEnumerator DisPlayStatus()
    {
        yield return new WaitUntil(() => (status < 6));

        if(status == 2)
        {
            onlineStatus.SetActive(true);
            inviteButton.SetActive(true);

            offlineStatus.SetActive(false);
        }
        else
        {
            onlineStatus.SetActive(false);
            inviteButton.SetActive(false);

            offlineStatus.SetActive(true);
        }
    }

    public void Invite()
    {
        PhotonChatController.Instance.OnClickInvite(id);
    }    
    public void Remove()
    {
        PhotonChatController.Instance.OnClickRemove(id);

        Destroy(this.gameObject);
    }
    public void OnClickAccept()
    {
        FriendManager.Instance.AcceptFriendRequest(id);
        FriendsUi.Instance.OnClickReceived();
    }
    public void OnClickReject()
    {
        FriendManager.Instance.RejectFriendRequest(id);
        FriendsUi.Instance.OnClickReceived();
    }   
}
