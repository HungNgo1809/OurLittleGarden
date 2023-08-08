using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalInteract : MonoBehaviour
{
    public SellOnlineManager sellOnlineManager;
    public LargeMapData largeMapData;
    // Start is called before the first frame update
    void Start()
    {
        //transform.position = new Vector3(largeMapData.spawnPosition.x + 2, transform.position.y, largeMapData.spawnPosition.z + 2);
    }

    public void SetPos()
    {
        transform.position = new Vector3(largeMapData.spawnPosition.x + 2, transform.position.y, largeMapData.spawnPosition.z + 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && other.GetComponent<PhotonView>().IsMine)
        {
            /*
            if (other.gameObject.GetComponentInChildren<playerFloatingUi>() != null)
            {
                other.gameObject.GetComponentInChildren<playerFloatingUi>().transform.GetChild(0).gameObject.SetActive(true);
          
            }          
            */
        }
    

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.GetComponent<PhotonView>().IsMine)
        {
          
            sellOnlineManager.LoadServerSellingPack();
            UiManager.Instance.GlobalChat.SetActive(true);

            //PlayerControl control = other.gameObject.GetComponent<PlayerControl>();
   
            //control.bubbleAnimation.Play("startBubbleE",0,0);
           
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && other.GetComponent<PhotonView>().IsMine)
        {
            UiManager.Instance.GlobalChat.SetActive(false);
     

        }
        /*
        if (other.gameObject.GetComponentInChildren<playerFloatingUi>() != null)
        {
            other.gameObject.GetComponentInChildren<playerFloatingUi>().transform.GetChild(0).gameObject.SetActive(false);
        }
        */
    }
}
