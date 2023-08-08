using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;

public class ChatBubble : MonoBehaviourPunCallbacks
{
    public GameObject ChatDisplay;
    public Text Mess;

    public int enterTextCount = 0;

    public void Action()
    {
        if (photonView.IsMine)
        {
            UpdateMess(Chat.Instance.inputField.text);
            StartCoroutine(SetMessRPC(Chat.Instance.inputField.text));
        }
    }

    IEnumerator SetMessRPC(string text)
    {
        yield return new WaitUntil(() => PhotonNetwork.IsConnected);

        photonView.RPC("UpdateMessRPC", RpcTarget.OthersBuffered, text);
    }

    [PunRPC]
    void UpdateMessRPC(string text)
    {
        UpdateMess(text);
    }

    void UpdateMess(string text)
    {
        ChatDisplay.transform.rotation = Quaternion.Euler(Quaternion.LookRotation(ChatDisplay.transform.position - Camera.main.transform.position).eulerAngles.x, Quaternion.LookRotation(ChatDisplay.transform.position - Camera.main.transform.position).eulerAngles.y - 13.0f, 0);
        ChatDisplay.SetActive(true);
        Mess.text = text;

        enterTextCount++;
        StartCoroutine(CloseMessBubble());
    }

    IEnumerator CloseMessBubble()
    {
        yield return new WaitForSeconds(5.0f);

        enterTextCount--;

        if(enterTextCount == 0)
        {
            ChatDisplay.SetActive(false);
        }
    }

    /*
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        if (photonView.Owner != null && targetPlayer == photonView.Owner && changedProps.ContainsKey("chatMessage"))
        {
            string newMessage = (string)changedProps["chatMessage"];
            UpdateMess(newMessage);
        }
    }*/
}
