using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;

public class DisplayUserName : MonoBehaviourPunCallbacks
{
    public DataManager dataManager;
    public Text playerNameText;

    public string playerId;

    void Start()
    {
        if (photonView.IsMine)
        {
            UpdatePlayerName(dataManager.displayName);
            StartCoroutine(SetNameRPC());
        }
    }

    IEnumerator SetNameRPC()
    {
        yield return new WaitUntil(() => PhotonNetwork.IsConnected);

        photonView.RPC("UpdatePlayerNameRPC", RpcTarget.OthersBuffered, dataManager.displayName);
        photonView.RPC("UpdatePlayerIdRPC", RpcTarget.OthersBuffered, dataManager.userId);
    } 
    
    [PunRPC]
    void UpdatePlayerNameRPC(string name)
    {
        UpdatePlayerName(name);
    }

    [PunRPC]
    void UpdatePlayerIdRPC(string id)
    {
        UpdatePlayerId(id);
    }

    void UpdatePlayerName(string name)
    {
        if (playerNameText != null)
        {
            playerNameText.text = name;
        }
        else
        {
            Debug.LogError("PlayerNameText is null. Make sure the Text component is assigned to the script.");
        }
    }

    void UpdatePlayerId(string id)
    {
        playerId = id;
    }    
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        if (photonView.Owner != null && targetPlayer == photonView.Owner && changedProps.ContainsKey("userName"))
        {
            string newName = (string)changedProps["userName"];
            UpdatePlayerName(newName);
        }
    }
}