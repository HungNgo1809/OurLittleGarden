using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHouse : MonoBehaviour
{
    public Color normalColor;
    public Color blackColor;

    public GameObject interactPanel;
    public GameObject editHouseBtn;

    public Transform door;

    bool isTrigger;

    public bool inHouse;
    Transform player;

    public Light sun;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            player = other.transform;
            isTrigger = true;

            //interactPanel.SetActive(true);
        }    
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isTrigger = false;

            //interactPanel.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(isTrigger)
        {
            if (Input.GetButtonDown("Use") && !inHouse)
            {
                EnterHouse();
            }else if(Input.GetButtonDown("Use") && inHouse)
            {
                LeaveHouse();
            }
        }    
    }

    public void EnterHouse()
    {
        isTrigger = false;

        sun.gameObject.SetActive(false);

        player.gameObject.SetActive(false);

        player.position = door.transform.position;
        player.rotation = Quaternion.Euler(0, -90, 0);

        player.gameObject.SetActive(true);

        Camera.main.backgroundColor = blackColor;
        if (PhotonNetwork.IsMasterClient)
        {
            editHouseBtn.SetActive(true);
        }    
    }
    public void LeaveHouse()
    {
        isTrigger = false;
        sun.gameObject.SetActive(true);

        player.gameObject.SetActive(false);
        player.rotation = Quaternion.Euler(0, 180, 0);
        player.position = new Vector3(32, 1.5f, 50.46f);
        player.gameObject.SetActive(true);

        editHouseBtn.SetActive(false);
        Camera.main.backgroundColor = normalColor;
    }
}
