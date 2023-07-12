using UnityEngine;
using System;
using Photon.Chat;
using Photon.Pun;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using PlayFab.ClientModels;
using Photon.Chat.Demo;
using System.Collections;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PlayFab;
using Newtonsoft.Json;

public class PhotonChatController : MonoBehaviourPunCallbacks, IChatClientListener
{
    //[SerializeField] private string nickName;
    public static PhotonChatController Instance { get; set; }
    private ChatClient chatClient;

    public DataManager dataManager;

    public GameObject InvitePanel;
    public Text inviteText;

    public string curSender;
    public string curRoomInvited;
    public Button homeBtn;

    int check;
    public bool isLoadRemoveFriendList;

    public List<photonStatus> friendsStatus;
    public List<String> tmpRemoveFriendList;
    #region Unity Methods

    private void Awake()
    {
        //nickName = PlayerPrefs.GetString("USERNAME");
        //UIFriend.OnInviteFriend += HandleFriendInvite;
    }
    private void OnDestroy()
    {
        //UIFriend.OnInviteFriend -= HandleFriendInvite;
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
        if(Instance == null)
        {
            Instance = this;

            chatClient = new ChatClient(this);
            ConnectoToPhotonChat();
        }
        else
        {
            Destroy(this.gameObject);
        }

        StartCoroutine(RemoveAllOldFriend());
    }

    private void Update()
    {
        chatClient.Service();

        //Debug.Log(chatClient.State);
        //Debug.Log(PhotonNetwork.NetworkClientState);
    }

    #endregion

    #region  Private Methods
    public void CheckFriendStatus(string friendId)
    {
        
    }    
    private void ConnectoToPhotonChat()
    {
        //Debug.Log("Connecting to Photon Chat");
        chatClient.AuthValues = new Photon.Chat.AuthenticationValues(dataManager.userId);
        ChatAppSettings chatSettings = PhotonNetwork.PhotonServerSettings.AppSettings.GetChatSettings();
        chatClient.ConnectUsingSettings(chatSettings);
    }

    #endregion

    #region  Public Methods

    public void OnClickInvite(string recipient)
    {
        if (!PhotonNetwork.InRoom) return;

        dataManager.SaveFarmTerData();
        dataManager.TriggerSave();

        chatClient.SendPrivateMessage(recipient, PhotonNetwork.CurrentRoom.Name);
    }

    public void OnClickRemove(string friendId)
    {
        //Xoa ban
        RemoveChooseFriend(friendId);
        //Load list xóa fiends của đối tượng
        isLoadRemoveFriendList = false;
        LoadRemovedFriendList(friendId, "removeFriend");
        //Them minh vao list do
        StartCoroutine(AddYouToRemoveList(friendId));
    }

    IEnumerator AddYouToRemoveList(string friendId)
    {
        yield return new WaitUntil(() => isLoadRemoveFriendList);

        //isLoadRemoveFriendList = false;
        tmpRemoveFriendList.Add(dataManager.userId);

        //Debug.Log(tmpRemoveFriendList);
        //Gui len lai
        dataManager.SaveDataToOtherPlayer(friendId, "removeFriend", JsonConvert.SerializeObject(tmpRemoveFriendList));
    }
    IEnumerator RemoveAllOldFriend()
    {
        yield return new WaitForSeconds(0.1f);

        isLoadRemoveFriendList = false;
        LoadRemovedFriendList(dataManager.userId, "removeFriend");

        yield return new WaitUntil(() => isLoadRemoveFriendList);
        isLoadRemoveFriendList = false;

        if(tmpRemoveFriendList.Count > 0)
        {
            foreach (string friendId in tmpRemoveFriendList)
            {
                RemoveChooseFriend(friendId);
            }
            tmpRemoveFriendList.Clear();

            dataManager.SaveDataPlayfab("removeFriend", JsonConvert.SerializeObject(tmpRemoveFriendList));
        }
    }
    public void RemoveChooseFriend(string friendId)
    {
        var request = new RemoveFriendRequest
        {
            FriendPlayFabId = friendId,
        };

        PlayFabClientAPI.RemoveFriend(request, OnFriendRemoved, OnError);
    }    
    private void OnError(PlayFabError err)
    {
        Debug.Log(err);
    }

    private void OnFriendRemoved(RemoveFriendResult obj)
    {
        dataManager.friendsList.Clear();

        dataManager.LoadFriendList();
    }
    public void LoadRemovedFriendList(string friendId, string key)
    {
        //Debug.Log("load");
        var request = new GetUserDataRequest
        {
            PlayFabId = friendId,
            Keys = new List<string> { key }
        };
        PlayFabClientAPI.GetUserData(request, LoadDataSuccess, OnError);
    }

    private void LoadDataSuccess(GetUserDataResult result)
    {
        //Debug.Log("loadSuccess");
        if (result.Data == null)
        {
            isLoadRemoveFriendList = true;
            return;
        }
        if (result.Data.ContainsKey("removeFriend"))
        {
            tmpRemoveFriendList = JsonConvert.DeserializeObject<List<String>>(result.Data["removeFriend"].Value);
            isLoadRemoveFriendList = true;
        }
    }
    public void FindPhotonFriends()
    {
        List<string> tmp1 = new List<string>();

        foreach(DataManager.FriendInfo fr in dataManager.friendsList)
        {
            tmp1.Add(fr.friendId);
        }

        string[] tmp2 = tmp1.ToArray();
        chatClient.AddFriends(tmp2);
    }
    public int GetStatus(string id)
    {
        foreach(photonStatus friend in friendsStatus)
        {
            if(friend.userId == id)
            {
                return friend.status;
            }
        }
        return 0;
    }
    #endregion

    #region Photon Chat Callbacks

    public void DebugReturn(DebugLevel level, string message)
    {
        Debug.Log($"Photon Chat DebugReturn: {message}");
    }

    void IChatClientListener.OnDisconnected()
    {
        Debug.Log("You have disconnected from the Photon Chat");
        chatClient.SetOnlineStatus(ChatUserStatus.Offline);
    }

    void IChatClientListener.OnConnected()
    {
        Debug.Log("You have connected to the Photon Chat");
        //OnChatConnected?.Invoke(chatClient);
        chatClient.SetOnlineStatus(ChatUserStatus.Online);
        FindPhotonFriends();
    }

    public void OnChatStateChange(ChatState state)
    {
        Debug.Log($"Photon Chat OnChatStateChange: {state.ToString()}");
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        Debug.Log($"Photon Chat OnGetMessages {channelName}");
        for (int i = 0; i < senders.Length; i++)
        {
            Debug.Log($"{senders[i]} messaged: {messages[i]}");
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        if (!string.IsNullOrEmpty(message.ToString()))
        {
            // Channel Name format [Sender : Recipient]
            string[] splitNames = channelName.Split(new char[] { ':' });
            string senderName = splitNames[0];

            if (!sender.Equals(senderName, StringComparison.OrdinalIgnoreCase))
            {
                if(dataManager.SearchFriendById(sender, dataManager.friendsList)!= null)
                {
                    //Debug.Log(dataManager.SearchFriendById(sender, dataManager.friendsList).friendName + ":" + message);
                    inviteText.text = dataManager.SearchFriendById(sender, dataManager.friendsList).friendName + " " + "invite you to join party";

                    string roomName = message.ToString();
                    if (PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom.Name != roomName)
                    {
                        //StartCoroutine(LeaveRoomAndJoinNewRoom(roomName));
                        //Hiện panel xác nhận hoặc từ chối
                        curSender = sender;
                        curRoomInvited = roomName;
                        InvitePanel.SetActive(true);
                    }
                }    
            }
        }
    }
    public void RejectInvite()
    {
        InvitePanel.SetActive(false);
    }    
    public void AcceptInvite()
    {
        InvitePanel.SetActive(false);
        StartCoroutine(JoinWithFriend(curRoomInvited, curSender));
    }
    IEnumerator JoinWithFriend(string room, string id)
    {
        // cùng phòng
        if (PhotonNetwork.CurrentRoom.Name == room)
        {
            // Set vị trí cho cùng vị trí bạn bè
            SetPositionToFriend(id);
        }
        // người mời đang ở thế giới chung nhưng không cùng phòng
        else if (room == "room")
        {
            PhotonNetwork.LeaveRoom();
            yield return new WaitUntil(() => PhotonNetwork.NetworkClientState == ClientState.JoinedLobby);

            PhotonNetwork.JoinRoom(room);
            yield return new WaitUntil(() => PhotonNetwork.NetworkClientState == ClientState.Joined);

            //Lưu data của mình
            UiManager.Instance.UpdateCropData();
            UiManager.Instance.UpdateBuildData();

            dataManager.isSavedDataBeforeQuit = 0;
            dataManager.TriggerSave();
            dataManager.SaveFarmTerData();

            yield return new WaitUntil(() => dataManager.isSavedDataBeforeQuit >= 25);
            dataManager.isSavedDataBeforeQuit = 0;
            //Load scene với data của admin
            SceneManager.LoadScene("Social");
            //Set vị trí cho cùng vị trí bạn bè
            SetPositionToFriend(id);
        }
        // Cả 2 cùng đang ở nhà
        else if(room != "room" && PhotonNetwork.CurrentRoom.Name != "room")
        {
            //lưu data map của mình lên playfab
            UiManager.Instance.UpdateCropData();
            UiManager.Instance.UpdateBuildData();

            dataManager.isSavedDataBeforeQuit = 0;
            dataManager.TriggerSave();
            dataManager.SaveFarmTerData();

            yield return new WaitUntil(() => dataManager.isSavedDataBeforeQuit >= 25);
            dataManager.isSavedDataBeforeQuit = 0;

            //Xóa data map của mình
            dataManager.checkLoad = 0;
            dataManager.terData.Clear();
            dataManager.buildData.Clear();
            dataManager.cropData.Clear();

            TileMapGenerator genMap = FindObjectOfType<TileMapGenerator>();

            //Xóa các object của mình
            check = 0;
            int checkLimit = ListObjectManager.Instance.terObject.Count + ListObjectManager.Instance.buildObject.Count + ListObjectManager.Instance.plantObject.Count;
            ClearOldObject();
            yield return new WaitUntil(() => (check >= checkLimit));
            //Load data của chủ phòng
            LoadMasterClientData(id);

            yield return new WaitUntil(() => (dataManager.checkLoad >= 6));
            dataManager.checkLoad = 0;

            //Doi de tao lai map
            //Time.timeScale = 0.0f;
            genMap.isReCreate = false;
            //Tạo lại map
            genMap.StartReCreateMap();

            yield return new WaitUntil(() => (genMap.isReCreate));
            //tạo lại cây và building
            PlantPrefabList.Instance.StartReloadCrop();
            Builder.Instance.StartReLoadBuild();

            //Time.timeScale = 1.0f;
            //Tham gia phong moi
            PhotonNetwork.LeaveRoom();
            yield return new WaitUntil(() => PhotonNetwork.NetworkClientState == ClientState.JoinedLobby);

            PhotonNetwork.JoinRoom(room);
            yield return new WaitUntil(() => PhotonNetwork.NetworkClientState == ClientState.Joined);

            //active nút home
            homeBtn.gameObject.SetActive(true);
            //UiManager.Instance.homeBtn.gameObject.SetActive(true);
            //UiManager.Instance.homeBtn.onClick.AddListener(BackHome);
        }
        // người mời ở nhà, người đc mời ở thế giới chung
        else if(room != "room" && PhotonNetwork.CurrentRoom.Name == "room")
        {
            PhotonNetwork.LeaveRoom();
            yield return new WaitUntil(() => PhotonNetwork.NetworkClientState == ClientState.JoinedLobby);

            PhotonNetwork.JoinRoom(room);
            yield return new WaitUntil(() => PhotonNetwork.NetworkClientState == ClientState.Joined);

            SceneManager.LoadScene("Main");
        }    
    }

    public void ClearOldObject()
    {
        foreach(GameObject ter in ListObjectManager.Instance.terObject)
        {

            Destroy(ter);
            check++;
        }
        ListObjectManager.Instance.terObject.Clear();
        ListObjectManager.Instance.TerObject.Clear();
        foreach (GameObject build in ListObjectManager.Instance.buildObject)
        {
            Destroy(build);
            check++;
        }
        ListObjectManager.Instance.buildObject.Clear();
        foreach (GameObject crop in ListObjectManager.Instance.plantObject)
        {
            Destroy(crop);
            check++;
        }
        ListObjectManager.Instance.plantObject.Clear();

    }
    public void BackHome()
    {
        StartCoroutine(ReturnHome());
    }    
    IEnumerator ReturnHome()
    {     
        //Clear data chủ phòng
        dataManager.checkLoad = 0;
        dataManager.terData.Clear();
        dataManager.buildData.Clear();
        dataManager.cropData.Clear();

        TileMapGenerator genMap = FindObjectOfType<TileMapGenerator>();

        //Xóa các object của mình
        check = 0;
        int checkLimit = ListObjectManager.Instance.terObject.Count + ListObjectManager.Instance.buildObject.Count + ListObjectManager.Instance.plantObject.Count;
        ClearOldObject();
        yield return new WaitUntil(() => (check >= checkLimit));

        //Load data của mình
        ReloadMyData();

        yield return new WaitUntil(() => (dataManager.checkLoad >= 6));
        dataManager.checkLoad = 0;

        genMap.isReCreate = false;
        //Tạo lại map
        PlantPrefabList.Instance.StartReloadCrop();
        Builder.Instance.StartReLoadBuild();
        genMap.StartReCreateMap();

        yield return new WaitUntil(() => (genMap.isReCreate));

        PhotonNetwork.LeaveRoom();
        yield return new WaitUntil(() => PhotonNetwork.NetworkClientState == ClientState.JoinedLobby);

        /*PhotonNetwork.JoinRoom(dataManager.userId);
        yield return new WaitUntil(() => PhotonNetwork.NetworkClientState == ClientState.Joined);*/

        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 20;
        PhotonNetwork.JoinOrCreateRoom(dataManager.userId, ro, TypedLobby.Default);
        yield return new WaitUntil(() => PhotonNetwork.NetworkClientState == ClientState.Joined);
        //tắt nút home
        homeBtn.gameObject.SetActive(false);
        /*
        // reload lại scene       
        SceneManager.LoadScene("Main");
        yield return null;*/
    }
    private void SetPositionToFriend(string playerID)
    {
        PhotonView you = new PhotonView();
        PhotonView target = new PhotonView();

        PhotonView[] photonViews = FindObjectsOfType<PhotonView>();

        foreach (PhotonView photonView in photonViews)
        {
            // Kiểm tra xem người chơi có trùng khớp với ID được tìm kiếm hay không
            if (photonView.Owner != null && photonView.Owner.UserId == playerID)
            {
                //set
                target = photonView;
            }
            if(photonView.IsMine)
            {
                you = photonView;
            }    
        }

        if (target != null && you != null)
        {
            you.transform.position = target.transform.position;
        }    
    }

    public void LoadMasterClientData(string hostId)
    {
        dataManager.LoadSpeKeyPlayFab(hostId, "terData1");
        dataManager.LoadSpeKeyPlayFab(hostId, "terData2");
        dataManager.LoadSpeKeyPlayFab(hostId, "terData3");
        dataManager.LoadSpeKeyPlayFab(hostId, "terData4");

        dataManager.LoadSpeKeyPlayFab(hostId, "buildData");
        dataManager.LoadSpeKeyPlayFab(hostId, "cropData");
    }
    public void ReloadMyData()
    {
        dataManager.LoadSpeKeyPlayFab(dataManager.userId, "terData1");
        dataManager.LoadSpeKeyPlayFab(dataManager.userId, "terData2");
        dataManager.LoadSpeKeyPlayFab(dataManager.userId, "terData3");
        dataManager.LoadSpeKeyPlayFab(dataManager.userId, "terData4");

        dataManager.LoadSpeKeyPlayFab(dataManager.userId, "buildData");
        dataManager.LoadSpeKeyPlayFab(dataManager.userId, "cropData");
    }    
    public void OnSubscribed(string[] channels, bool[] results)
    {
        Debug.Log($"Photon Chat OnSubscribed");
        for (int i = 0; i < channels.Length; i++)
        {
            Debug.Log($"{channels[i]}");
        }
    }

    public void OnUnsubscribed(string[] channels)
    {
        Debug.Log($"Photon Chat OnUnsubscribed");
        for (int i = 0; i < channels.Length; i++)
        {
            Debug.Log($"{channels[i]}");
        }
    }

    void IChatClientListener.OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        /*
        Debug.Log($"Photon Chat OnStatusUpdate: {user} changed to {status}: {message}");
        //PhotonStatus newStatus = new PhotonStatus(user, status, (string)message);
        Debug.Log($"Status Update for {user} and its now {status}.");
        //OnStatusUpdated?.Invoke(newStatus);*/

        if(FindStatus(user) == null)
        {
            photonStatus friend = new photonStatus();
            friend.userId = user;
            friend.status = status;

            friendsStatus.Add(friend);
        }
        else
        {
            FindStatus(user).status = status;
        }

    }

    public photonStatus FindStatus(string id)
    {
        foreach(photonStatus friend in friendsStatus)
        {
            if(friend.userId == id)
            {
                return friend;
            }
        }
        return null;
    }
    public void OnUserSubscribed(string channel, string user)
    {
        Debug.Log($"Photon Chat OnUserSubscribed: {channel} {user}");
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        Debug.Log($"Photon Chat OnUserUnsubscribed: {channel} {user}");
    }
    /*
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        //Hiển thị panel thông báo

        StartCoroutine(ReturnHome());
    }
    */
    #endregion

    [System.Serializable]
    public class photonStatus
    {
        public string userId;
        public int status;
    }
}
