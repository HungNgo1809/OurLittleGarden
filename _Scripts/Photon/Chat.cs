using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Chat : MonoBehaviour
{
    public InputField inputField;
    public GameObject MessPrefab;
    public GameObject Content;

    public TextTyping textTyping;
    public bool valueChanged = false;

    public string curText;

    public DataManager dataManager;
    public static Chat Instance { get; set; }
    private void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        inputField.onValueChanged.AddListener(OnInputFieldValueChange);
    }
    private void Update()
    {
        if(valueChanged)
        {
            textTyping.isTypingText = true;

            if(Input.GetKeyDown(KeyCode.Return))
            {
                //Debug.Log("enter");
                ChatBubbleHandle();
                SendMessage();
            }    
        }    
    }
    public void ChatBubbleHandle()
    {
        ChatBubble[] bubbleList = FindObjectsOfType<ChatBubble>();

        foreach(ChatBubble bubble in bubbleList)
        {
            bubble.Action();
        }
    }
    public void SendMessage()
    {
        //Debug.Log(PhotonNetwork.NickName);
        GetComponent<PhotonView>().RPC("GetMessage", RpcTarget.All, (dataManager.userName + ": " + inputField.text));

        curText = inputField.text;
        inputField.text = ""; 
        textTyping.isTypingText = valueChanged = false;
    }

    public void OnClickUpdateFriend()
    {
        if(FriendManager.Instance != null)
        {
            dataManager.checkLoad = 0;
            FriendManager.Instance.UpdateFriendRequest();
            StartCoroutine(CheckRequestAfterUpdate());
        }    
    }
    IEnumerator CheckRequestAfterUpdate()
    {
        yield return new WaitUntil(() => (dataManager.checkLoad >= 2));

        dataManager.CheckFriendRequest();

        yield return new WaitUntil(() => dataManager.isCheckFriendRequest);
        dataManager.isCheckFriendRequest = false;

        dataManager.friendsList.Clear();
        dataManager.LoadFriendList();
    }    
    private void OnApplicationQuit()
    {
        textTyping.isTypingText = valueChanged = false;
    }

    [PunRPC]
    public void GetMessage(string ReceivedMess)
    {
        GameObject M = Instantiate(MessPrefab, Vector3.zero, Quaternion.identity, Content.transform);

        //Debug.Log(ReceivedMess);
        M.GetComponent<Text>().text = ReceivedMess;
        M.transform.SetAsFirstSibling();
    }
    public void OnInputFieldValueChange(string value)
    {
        valueChanged = true;
    }  
}
