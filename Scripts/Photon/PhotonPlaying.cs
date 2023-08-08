using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class PhotonPlaying : MonoBehaviourPunCallbacks
{
    public string PlayerPhotonPrefabName = "PhotonPlayer";
    public List<PLayerProfile> players = new List<PLayerProfile>();

    public GameObject localPlayer;

    public DataManager dataManager;
    public LargeMapData largeMapData;

    public GenerateLargeMap generateLargeMap;

    public bool isJoinedLocalRoom;

    //public Vector3 spawnPos;

    bool left;

    public string roomName;
    private void Awake()
    {
        //spawnPos = largeMapData.spawnPosition;
        //LoadRoomPlayer();
        //SpawnPlayer();
        //dataManager.isLoadedData = false;
        StartCoroutine(StartSpawn());
        //dataManager.isOldbie = 1;
    }
    IEnumerator StartSpawn()
    {
        yield return new WaitUntil(() => ((roomName != null || roomName != "") && generateLargeMap.isDrawed));

        SpawnPlayer();
    }
    public void SpawnPlayer()
    {
        if(PhotonNetwork.NetworkClientState == ClientState.Joined && PhotonNetwork.CurrentRoom.Name == roomName)
        {
            //Khoi tao remote player
            SpawnRemotePlayer();
            return;
        }

        //Khoi tao local player
        /*
        GameObject obj = Resources.Load(this.PlayerPhotonPrefabName) as GameObject;
        Instantiate(obj, spawnPos, Quaternion.identity);
        */
        //AddRoomPLayer(PhotonNetwork.NickName, obj);
    }

    public void SpawnRemotePlayer()
    {
        GameObject remotePlayer =
        PhotonNetwork.Instantiate(this.PlayerPhotonPrefabName, largeMapData.spawnPosition, Quaternion.identity);
        //AddRoomPLayer

        if(remotePlayer.GetComponent<PhotonView>().IsMine)
        {
            localPlayer = remotePlayer;
        }
        remotePlayer.name = remotePlayer.GetComponent<DisplayUserName>().playerNameText.text;

        LoadingScreen.Instance.isReleaseLoading = false;

    
    }

    public void LoadRoomPlayer()
    {
        if (PhotonNetwork.NetworkClientState != ClientState.Joined) return;

        PLayerProfile pLayerProfile;

        foreach(KeyValuePair<int, Player> playerData in PhotonNetwork.CurrentRoom.Players)
        {
            pLayerProfile = new PLayerProfile { playerName = playerData.Value.NickName };
            players.Add(pLayerProfile);
        }    
    
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
    public void ConnectToPhoton(string nickName)
    {
        PhotonNetwork.AuthValues = new Photon.Realtime.AuthenticationValues(nickName);
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();

        PhotonNetwork.NickName = nickName;
    }
    public void LeaveAndJoinLocalRoom()
    {
        largeMapData.SaveAction();

        PhotonNetwork.LeaveRoom();

        StartCoroutine(ReConnect());

        StartCoroutine(JoinLocalRoom());
        StartCoroutine(LoadLocalLevel());
        
    }
    IEnumerator ReConnect()
    {
        yield return new WaitUntil(() => (PhotonNetwork.NetworkClientState == ClientState.Disconnected));

        ConnectToPhoton(dataManager.userName);
    }

    IEnumerator JoinLocalRoom()
    {
        yield return new WaitUntil(() => (PhotonNetwork.NetworkClientState == ClientState.JoinedLobby));

        CreatePhotonRoom(dataManager.userId);


    }
    IEnumerator LoadLocalLevel()
    {
        //UiManager.Instance.UpdateCropData();
        yield return new WaitUntil(() => (isJoinedLocalRoom && largeMapData.saveCount>=43));

        largeMapData.saveCount = 0;
        //yield return new WaitUntil(() => (left));

        if (UiManager.Instance != null)
        {
            Destroy(UiManager.Instance.gameObject);
        }

        //PhotonNetwork.LoadLevelIfSynced("Main");
        //SceneManager.LoadScene("Main");
        LoadingScreen.Instance.FlexLoadingScreenWithoutBool("Main");

    }
    public override void OnJoinedRoom()
    {

        if (PhotonNetwork.CurrentRoom.Name != roomName)
        {
            isJoinedLocalRoom = true;
        }
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(PhotonNetwork.NetworkClientState + "+" + "left");
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log($"You have connected to the Photon Master Server");
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
    }
    public override void OnLeftRoom()
    {
        Debug.Log(PhotonNetwork.NetworkClientState + "+" + "left");
        left = true;
    }
    /*
    public void AddRoomPLayer(string playerName, GameObject playerObj)
    {
        PLayerProfile pLayerProfile = new PLayerProfile();
        pLayerProfile.playerName = playerName;
        pLayerProfile.playerObj = playerObj;

        players.Add(pLayerProfile);
    }*/

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }

    [System.Serializable]
    public class PLayerProfile
    {
        public string playerName;

        public GameObject playerObj;
    }
}
