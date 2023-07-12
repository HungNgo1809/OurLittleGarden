using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendsUi : MonoBehaviour
{
    public static FriendsUi Instance { get; set; }
    public GameObject friendsPrefab;
    public GameObject SendedPrefab;
    public GameObject ReceivedPrefab;

    public Transform display;
    public Transform displaySended;
    public Transform displayReceived;

    public DataManager dataManager;
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnEnable()
    {
        OnClickFriends();
    }
    public void OnClickFriends()
    {
        Clear();
        foreach(DataManager.FriendInfo fr in dataManager.friendsList)
        {
            //Debug.Log(fr.friendId);
            GameObject spawn = Instantiate(friendsPrefab);
            spawn.transform.SetParent(display);
            spawn.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

            if (spawn.GetComponent<FriendsUiComponent>() != null)
            {
                spawn.GetComponent<FriendsUiComponent>().id = fr.friendId;
                spawn.GetComponent<FriendsUiComponent>().name = fr.friendName;
                spawn.GetComponent<FriendsUiComponent>().nameDisplay.text = fr.friendName;

                if(PhotonChatController.Instance != null)
                {
                    spawn.GetComponent<FriendsUiComponent>().status = PhotonChatController.Instance.GetStatus(fr.friendId);
                }
            }    
        }    
    }
    public void OnClickSended()
    {
        Clear();
        foreach (DataManager.FriendRequest fr in dataManager.friendRequestSendList)
        {
            GameObject spawn = Instantiate(SendedPrefab);
            spawn.transform.SetParent(displaySended);
            spawn.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

            if (spawn.GetComponent<FriendsUiComponent>() != null)
            {
                spawn.GetComponent<FriendsUiComponent>().id = fr.friendId;
                spawn.GetComponent<FriendsUiComponent>().name = fr.friendName;
                spawn.GetComponent<FriendsUiComponent>().nameDisplay.text = fr.friendName;
            }
        }
    }
    public void OnClickReceived()
    {
        Clear();
        foreach (DataManager.FriendRequest fr in dataManager.friendRequestReceivedList)
        {
            GameObject spawn = Instantiate(ReceivedPrefab);
            spawn.transform.SetParent(displayReceived);
            spawn.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

            if (spawn.GetComponent<FriendsUiComponent>() != null)
            {
                spawn.GetComponent<FriendsUiComponent>().id = fr.friendId;
                spawn.GetComponent<FriendsUiComponent>().name = fr.friendName;
                spawn.GetComponent<FriendsUiComponent>().nameDisplay.text = fr.friendName;
            }
        }
    }
    
    public void Clear()
    {
        foreach(Transform child in display)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in displaySended)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in displayReceived)
        {
            Destroy(child.gameObject);
        }
    }    
}
