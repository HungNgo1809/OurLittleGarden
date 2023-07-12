using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class PhotonConnector : MonoBehaviourPunCallbacks
{
    public string PlayerPhotonPrefabName = "PhotonPlayer2";
    public string PlayerLocalPrefabName = "PLayerLocal";

    public DataManager dataManager;

    bool createdLocalRoom;
    public bool onLobby;
    public bool changedToLargeRoom;

    public GameObject localPlayer;

    public LargeMapData largeMapData;

    public CameraFollow cameraFollow;
    //bool isSpawnLocal;
    //public bool joinedLargeRoom;
    //public static Action GetPhotonFriends = delegate { };
    #region Unity Method
    private void Start()
    {
        largeMapData.ClearLargeMapData();

        StartCoroutine(StartSpawn()); ;
        string name = dataManager.userId;

        if(!dataManager.firstConnect)
        {
            //Debug.Log("err");
            ConnectToPhoton(name);
        }
    }

    
    private void Update()
    {
       // Debug.Log(PhotonNetwork.CurrentRoom.Name);
    }
    #endregion

    #region Private Method

    #endregion

    #region Public Method
    IEnumerator StartSpawn()
    {
        yield return new WaitUntil(() => (PhotonNetwork.NetworkClientState == ClientState.Joined) && dataManager.isLoadedData);
        SpawnPlayer();
    }    
    public void SpawnPlayer()
    {
        if (PhotonNetwork.NetworkClientState == ClientState.Joined)
        {
            SpawnRemotePlayer();
          
            return;
        }  
        /*
        GameObject obj = Resources.Load(this.PlayerPhotonPrefabName) as GameObject;
        Instantiate(obj, new Vector3(32, 1.5f, 50.46f), Quaternion.Euler(new Vector3(0, 180, 0)));*/
    }
    private PhotonView FindIsMinePhotonView()
    {
        PhotonView[] photonViews = FindObjectsOfType<PhotonView>();

        foreach (PhotonView photonView in photonViews)
        {
            if (photonView.IsMine)
            {
                return photonView;
            }
        }

        return null;
    }

    public void SpawnAllOtherPlayer()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.NickName != PhotonNetwork.NickName)
            {
                GameObject remotePlayer = PhotonNetwork.InstantiateSceneObject(this.PlayerPhotonPrefabName, new Vector3(32, 1.5f, 50.46f), Quaternion.identity);
            }
        }
    }    
    public void SpawnRemotePlayer()
    {
        if(PhotonNetwork.CurrentRoom.Name == dataManager.userId)
        {
            GameObject localPlayer = PhotonNetwork.Instantiate(this.PlayerLocalPrefabName, new Vector3(32, 1.5f, 50.46f), Quaternion.Euler(new Vector3(0, 180, 0)));
            this.localPlayer = localPlayer;
            cameraFollow.target = localPlayer.transform;
        }
        else
        {
            GameObject remotePlayer = PhotonNetwork.Instantiate(this.PlayerPhotonPrefabName, new Vector3(32, 1.5f, 50.46f), Quaternion.Euler(new Vector3(0, 180, 0)));

            if (remotePlayer.GetComponent<PhotonView>().IsMine)
            {
                localPlayer = remotePlayer;
                cameraFollow.target = remotePlayer.transform;
            }
        }
        LoadingScreen.Instance.isReleaseLoading = false;
    }
    public void ConnectToPhoton(string nickName)
    {
        //Debug.Log($"Connect to Photon as {nickName}");
        PhotonNetwork.AuthValues = new Photon.Realtime.AuthenticationValues(nickName);
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();

        PhotonNetwork.NickName = nickName;
    }
    public void CreatePhotonRoom(string roomName)
    {
        //Debug.Log("call");

        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 20;
        PhotonNetwork.JoinOrCreateRoom(roomName, ro, TypedLobby.Default);
    }
    public void BackToLocalRoom()
    {
        //CreatePhotonRoom(dataManager.userId);
    }    
    #endregion

    #region PhotonCallbacks
    public override void OnConnectedToMaster()
    {
        //Debug.Log($"You have connected to the Photon Master Server");
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
    }
    public override void OnJoinedLobby()
    {
        //Debug.Log("You have connected to a Photon Lobby");
        //CreatePhotonRoom("TestRoom");
        //GetPhotonFriends?.Invoke();
        if(!createdLocalRoom && !dataManager.firstConnect)
        {
            //CreatePhotonRoom(dataManager.userId);
            CreatePhotonRoom(dataManager.userId);
            createdLocalRoom = true;
        }
        onLobby = true;
    }
    public override void OnCreatedRoom()
    {
        //Debug.Log($"You have created a Photon Room named {PhotonNetwork.CurrentRoom.Name}");
        //PhotonNetwork.LoadLevel("Social");
    }
    public override void OnJoinedRoom()
    {
        //Debug.Log($"You have joined a Photon Room named {PhotonNetwork.CurrentRoom.Name}");

        if(PhotonNetwork.CurrentRoom.Name == "room")
        {
            //Debug.Log("joined larrge");
            changedToLargeRoom = true;
        }
        else if (!dataManager.firstConnect)
        {
            dataManager.firstConnect = true;
        }
        else
        {
            SpawnPlayer();
        }
    }

    public void LeaveAndJoinLargeRoom()
    {
        PhotonNetwork.LeaveRoom();

        StartCoroutine(ReConnect());

        StartCoroutine(JoinLargeRoom());
    }    
    IEnumerator ReConnect()
    {
        yield return new WaitUntil(() => (PhotonNetwork.NetworkClientState == ClientState.Disconnected));

        Debug.Log("reConnect");
        ConnectToPhoton(dataManager.userId);
    }

    IEnumerator JoinLargeRoom()
    {
        yield return new WaitUntil(() => (PhotonNetwork.NetworkClientState == ClientState.JoinedLobby));

        Debug.Log("lobby");
        CreatePhotonRoom("room");
    }
    public override void OnLeftRoom()
    {
        //Debug.Log("You have left a Photon Room");
        onLobby = false;
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        //Debug.Log($"Failde to join a Photon room: {message}");
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //Debug.Log($"Another player has joined the room: {newPlayer.UserId}");
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //Debug.Log($"Player has left room {otherPlayer.UserId}");
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        //Debug.Log($"New master client {newMasterClient.UserId}");
    }
    #endregion
}