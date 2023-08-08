using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using Newtonsoft.Json;
using System.Linq;
using ExitGames.Client.Photon;
using System;
using PlayFab.ClientModels;

public class FriendManager : MonoBehaviour
{
    public static FriendManager Instance { get; set; }
    public DataManager dataManager;
    public PhotonView photonView;

    public List<DataManager.FriendRequest> tmpFriendList;

    bool isLoaded;

    string acceptedFriendId;

    //public bool checkInChat;
    private void Start()
    {
        if (photonView.IsMine)
        {
            Instance = this;

            //dataManager.LoadSpeKeyPlayFab(dataManager.userId, "friendRequest");
        }
        else
        {
            this.enabled = false;
        }
    }
    public void OnClickSendFriendRequest(string friendId, string friendName)
    {
        int resultCheck = CheckDuplicateFriend(friendId);

        switch(resultCheck)
        {
            case 0:
                {
                    DataManager.FriendInfo friend = new DataManager.FriendInfo();
                    friend.friendId = dataManager.userId;
                    friend.friendName = dataManager.userName;

                    dataManager.friendRequestSendList.Add(new DataManager.FriendRequest { friendId = friendId, friendName = friendName, accepted = 0 });

                    //Luu danh sach friend request da gui
                    dataManager.SaveDataPlayfab("friendRequestSend", JsonConvert.SerializeObject(dataManager.friendRequestSendList));

                    //Load danh sach request cua doi phuong
                    LoadOtherFriendRequestListPlayFab(friendId);

                    //Them ban vao list cua doi phuong
                    StartCoroutine(AddYourselftToOtherFriendRequestList(friend, friendId));
                }
                break;
            case 1:
                // đã là bạn bè
                Debug.Log("Hai bạn đã là bạn bè");
                break;
            case 2:
                // đã gửi rồi
                Debug.Log("Đã gửi request trước đó");
                break;
            case 3:
                // người đó đã gửi request cho bạn
                Debug.Log("Bạn của bạn đã gửi request trước rồi, hãy chấp nhận");
                break;
        }
        // Cap nhat ngay bang punRPC

        /*
        Photon.Realtime.Player targetPlayer = PhotonNetwork.PlayerListOthers.FirstOrDefault(p => p.NickName == friendId);

        if(targetPlayer != null)
        {
            photonView.RPC("UpdateFriendRequestRPC", targetPlayer, friend.friendId, friend.friendName);
        }*/

        // Cap nhat ngay danh sach request duoc nhan

    }

    /*
    [PunRPC]
    void UpdateFriendRequestRPC(string id, string name)
    {
        FriendInfo fr = new FriendInfo();
        fr.friendId = id;
        fr.friendName = name;

        AddFriendRequestReceived(fr);
    }*/
    public void AddFriendRequestReceived(DataManager.FriendRequest fr)
    {
        dataManager.friendRequestReceivedList.Add(fr);
    }
    

    public void UpdateFriendRequest()
    {
        dataManager.LoadSpeKeyPlayFab(dataManager.userId, "friendRequest");
        dataManager.LoadSpeKeyPlayFab(dataManager.userId, "friendRequestSend");

        //checkInChat = true;
        //dataManager.CheckFriendRequest();
    } 
    
    public int CheckDuplicateFriend(string id)
    {
        if(dataManager.SearchFriendById(id, dataManager.friendsList) != null)
        {
            return 1;
        }
        if (dataManager.SearchFriendRequestById(id, dataManager.friendRequestSendList) != null)
        {
            return 2;
        }
        if (dataManager.SearchFriendRequestById(id, dataManager.friendRequestReceivedList) != null)
        {
            return 3;
        }

        return 0;
    }
    public void LoadOtherFriendRequestListPlayFab(string id)
    {
        var request = new GetUserDataRequest
        {
            PlayFabId = id,
            Keys = new List<string> { "friendRequest" }
        };

        acceptedFriendId = id;
        PlayFabClientAPI.GetUserData(request, LoadDataSuccess, Err);
    }
    public void LoadOtherFriendSendRequestListPlayFab(string id)
    {
        var request = new PlayFab.ClientModels.GetUserDataRequest
        {
            PlayFabId = id,
            Keys = new List<string> { "friendRequestSend" }
        };
        PlayFabClientAPI.GetUserData(request, LoadDataSendSuccess, Err);
    }

    private void LoadDataSendSuccess(GetUserDataResult result)
    {
        if (result.Data == null)
        {
            dataManager.SaveDataPlayfab("friendRequestSend", JsonConvert.ToString(tmpFriendList));
            isLoaded = true;
        }
        if (result.Data.ContainsKey("friendRequestSend"))
        {
            List<DataManager.FriendRequest> fr = JsonConvert.DeserializeObject<List<DataManager.FriendRequest>>(result.Data["friendRequestSend"].Value);
            tmpFriendList.AddRange(fr);

            isLoaded = true;
        }
    }

    private void Err(PlayFabError err)
    {
        Debug.Log(err);
    }

    private void LoadDataSuccess(PlayFab.ClientModels.GetUserDataResult result)
    {
        Debug.Log(result.Data.Values);
        if(result.Data == null)
        {
            dataManager.SaveDataToOtherPlayer(acceptedFriendId, "friendRequest", JsonConvert.ToString(tmpFriendList));
            isLoaded = true;
        }    
        if(result.Data.ContainsKey("friendRequest"))
        {
            List<DataManager.FriendRequest> fr = JsonConvert.DeserializeObject<List<DataManager.FriendRequest>>(result.Data["friendRequest"].Value);
            tmpFriendList.AddRange(fr);

            isLoaded = true;
        }
    }

    IEnumerator AddYourselftToOtherFriendRequestList(DataManager.FriendInfo info, string otherId)
    {
        yield return new WaitUntil(() => (isLoaded));

        isLoaded = false;
        DataManager.FriendRequest req = new DataManager.FriendRequest();
        req.friendId = info.friendId;
        req.friendName = info.friendName;
        req.accepted = 0;

        tmpFriendList.Add(req);
        isLoaded = true;

        yield return new WaitUntil(() => (isLoaded));

        Debug.Log("Luu sang nguoi choi khac"); 
        dataManager.SaveDataToOtherPlayer(otherId, "friendRequest", JsonConvert.SerializeObject(tmpFriendList));
        isLoaded = false;
    }

    // add friend (chấp nhận)
    public void AcceptFriendRequest(string id)
    {
        // Xóa request và ui
        if(dataManager.SearchFriendRequestById(id, dataManager.friendRequestReceivedList) != null)
        {
            dataManager.friendRequestReceivedList.Remove(dataManager.SearchFriendRequestById(id, dataManager.friendRequestReceivedList));
            dataManager.SaveDataPlayfab("friendRequest", JsonConvert.SerializeObject(dataManager.friendRequestReceivedList));
        }    
        // add bạn lên playfab
        AddFriend(id);
    }    

    public void RejectFriendRequest(string id)
    {
        // Xóa Ui
        if (dataManager.SearchFriendRequestById(id, dataManager.friendRequestReceivedList) != null)
        {
            dataManager.friendRequestReceivedList.Remove(dataManager.SearchFriendRequestById(id, dataManager.friendRequestReceivedList));
            dataManager.SaveDataPlayfab("friendRequest", JsonConvert.SerializeObject(dataManager.friendRequestReceivedList));
        }
    }    
    public void AddFriend(string friendPlayFabId)
    {
        var request = new AddFriendRequest
        {
            FriendPlayFabId = friendPlayFabId
        };

        acceptedFriendId = friendPlayFabId; // Lưu FriendPlayFabId vào biến tạm

        PlayFabClientAPI.AddFriend(request, OnAcceptFriendRequestSuccess, OnFailure);
    }

    private void OnFailure(PlayFabError obj)
    {
        Debug.Log(obj);
    }

    private void OnAcceptFriendRequestSuccess(AddFriendResult obj)
    {
        //Cập nhật danh sách bạn bè
        dataManager.LoadFriendList();

        //Load dữ liệu friend request send của đối tượng
        LoadOtherFriendSendRequestListPlayFab(acceptedFriendId);

        //Thực hiện sửa và đẩy lại
        StartCoroutine(ChangeOtherFriendSendRequest());
    }

    IEnumerator ChangeOtherFriendSendRequest()
    {
        yield return new WaitUntil(() => isLoaded);

        DataManager.FriendRequest fr_rq = FindOtherFriendRequestToMe(dataManager.userId);

        if(fr_rq != null)
        {
            fr_rq.accepted = 1;
            dataManager.SaveDataToOtherPlayer(acceptedFriendId, "friendRequestSend", JsonConvert.SerializeObject(tmpFriendList));
        }    
    }    
    public DataManager.FriendRequest FindOtherFriendRequestToMe(string id)
    {
        return tmpFriendList.Where(p => p.friendId == id).FirstOrDefault();
    }    
}
