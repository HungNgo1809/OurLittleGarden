using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathManager : MonoBehaviour
{
    public DataManager dataManager;

    bool trigger = false;
    // Start is called before the first frame update
    void Start()
    {
        if(!GetComponent<PhotonView>().IsMine)
        {
            this.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(dataManager.hp <= 0 && GetComponent<PhotonView>().IsMine && !trigger)
        {
            trigger = true;
            if(GetComponent<PlayerControl>() != null)
            GetComponent<PlayerControl>().isDead = true;

            if(GetComponent<Animator>() != null)
            GetComponent<Animator>().Play("death");

            StartCoroutine(Death());
        }
    }
    IEnumerator Death()
    {

        yield return new WaitForSeconds(2.0f);

        //chạy load panel
        LoadingScreen.Instance.StartLoadingScreenDie(3.5f);

        yield return new WaitForSeconds(2.5f);

        dataManager.coins = dataManager.coins - 100;

        dataManager.hp = 30.0f;
        dataManager.stamina = 0f;

        if(dataManager.food <= 0)
        {
            dataManager.food = 30.0f;
        }
        if (dataManager.sleep <= 0)
        {
            dataManager.sleep = 30.0f;
        }

        if(PhotonNetwork.CurrentRoom.Name == dataManager.userId)
        {
            StartCoroutine(TeleToSpawnPos());
        }else if(PhotonNetwork.CurrentRoom.Name == "room")
        {
            ReturnHomeFromLargeMap();
        }
        else
        {
            PhotonChatController.Instance.BackHome();
        }    
    }

    public void ReturnHomeFromLargeMap()
    {
        StartCoroutine(BackFromLargeMap());
    }
    IEnumerator BackFromLargeMap()
    {
        PhotonPlaying photon = FindObjectOfType<PhotonPlaying>();

        yield return new WaitUntil(() => photon!=null);
        photon.LeaveAndJoinLocalRoom();
    }
    IEnumerator TeleToSpawnPos()
    {
        transform.position = new Vector3(32, 1.5f, 50.46f);
        yield return new WaitForSeconds(1.0f);

        GetComponent<Animator>().Play("Idle");
        GetComponent<PlayerControl>().isDead = false;
        trigger = false;
    }      
}
