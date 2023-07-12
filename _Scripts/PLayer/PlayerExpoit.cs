using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class PlayerExpoit : MonoBehaviour
{
    public bool trigger;

    public Exploit CurExploit;
    public ExploitPhoton CurExploitPhoton;

    public LargeMapData largeMapData;
    public enum type
    {
        local,
        photon
    }

    [SerializeField]
    private type playerType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "exploit" && GetComponent<PhotonView>().IsMine)
        {
            Debug.Log("call");
            if (playerType == type.local)
            {
                if (other.GetComponent<Exploit>().CheckCurTool(GetComponent<HandButtonManager>().curToolSpeType))
                {
                    trigger = true;
                    if(CurExploit != null)
                    {
                        CurExploit.hpSlider.transform.parent.parent.gameObject.SetActive(false);
                    }

                    CurExploit = other.GetComponent<Exploit>();

                    CurExploit.hpSlider.transform.parent.parent.rotation = Quaternion.Euler(Quaternion.LookRotation(CurExploit.hpSlider.transform.parent.parent.position - Camera.main.transform.position).eulerAngles.x, CurExploit.hpSlider.transform.parent.parent.rotation.y, CurExploit.hpSlider.transform.parent.parent.rotation.z);
                    CurExploit.hpSlider.transform.parent.parent.gameObject.SetActive(true);
                }
            }
            else if (playerType == type.photon && (SceneManager.GetActiveScene().name == "Social"))
            {
                if (other.GetComponent<ExploitPhoton>().CheckCurTool(GetComponent<HandButtonManager>().curToolSpeType))
                {
                    trigger = true;
                    if (CurExploitPhoton != null)
                    {
                        CurExploitPhoton.hpSlider.transform.parent.parent.gameObject.SetActive(false);
                    }

                    CurExploitPhoton = other.GetComponent<ExploitPhoton>();

                    CurExploitPhoton.hpSlider.transform.parent.parent.rotation = Quaternion.Euler(Quaternion.LookRotation(CurExploitPhoton.hpSlider.transform.parent.parent.position - Camera.main.transform.position).eulerAngles.x, CurExploitPhoton.hpSlider.transform.parent.parent.rotation.y, CurExploitPhoton.hpSlider.transform.parent.parent.rotation.z);
                    CurExploitPhoton.hpSlider.transform.parent.parent.gameObject.SetActive(true);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "exploit" && GetComponent<PhotonView>().IsMine)
        {
            if (playerType == type.local)
            {
                trigger = false;

                if (CurExploit != null)
                {
                    CurExploit.hpSlider.transform.parent.parent.gameObject.SetActive(false);
                }
            }
            else if (playerType == type.photon)
            {
                trigger = false;

                if (CurExploitPhoton != null)
                {
                    CurExploitPhoton.hpSlider.transform.parent.parent.gameObject.SetActive(false);
                }
            }
        }
    }

    private void Update()
    {
        if (!GetComponent<PhotonView>().IsMine) return;
        
        if(CurExploit != null || CurExploitPhoton != null)
        {
            if (playerType == type.local)
            {
                if (trigger)
                {
                    if (Input.GetButtonDown("Interact") && CurExploit.curDamage > 0f)
                    {
                        Vector3 direction = new Vector3(CurExploit.transform.position.x, transform.position.y, CurExploit.transform.position.z) - transform.position;
                        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

                        //other.transform.LookAt(this.transform);
                        GetComponent<Animator>().SetTrigger("Exploit");

                        CurExploit.ExpoitAction(CurExploit.curDamage);
                    }
                }

                if (CurExploit.hp <= 0f && GetComponent<PhotonView>().IsMine)
                {
                    CurExploit.ExploitComplete();
                }
            }
            else if (playerType == type.photon)
            {
                if (trigger)
                {
                    if (Input.GetButtonDown("Interact") && CurExploitPhoton.curDamage > 0f)
                    {
                        Vector3 direction = new Vector3(CurExploitPhoton.transform.position.x, transform.position.y, CurExploitPhoton.transform.position.z) - transform.position;
                        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

                        //other.transform.LookAt(this.transform);
                        GetComponent<Animator>().SetTrigger("Exploit");

                        CurExploitPhoton.ExpoitAction(CurExploitPhoton.curDamage);
                    }
                }

                if (CurExploitPhoton.hp <= 0f && GetComponent<PhotonView>().IsMine)
                {
                    GetComponent<PhotonView>().RPC("DestroyObject", RpcTarget.OthersBuffered, CurExploitPhoton.transform.name);
                    CurExploitPhoton.ExploitComplete();
                }
            }
        }    
    }
    [PunRPC]
    private void DestroyObject(string objectName)
    {
        StartCoroutine(DestroyOnline(objectName));
    }
    IEnumerator DestroyOnline(string objectName)
    {
        //Debug.Log(objectName);
        GameObject delete = GameObject.Find(objectName);
        yield return new WaitUntil(() => delete!=null);
        // Xóa object trên các client
        Destroy(delete);
        largeMapData.RemoveObjFromData(objectName);
    }    
    /*
    private void FixedUpdate()
    {
        if (CurExploit != null || CurExploitPhoton != null)
        {
            if (playerType == type.local)
            {
                if (trigger)
                {
                    if (Input.GetButtonDown("Interact") && CurExploit.curDamage > 0f)
                    {
                        Vector3 direction = new Vector3(CurExploit.transform.position.x, transform.position.y, CurExploit.transform.position.z) - transform.position;
                        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

                        //other.transform.LookAt(this.transform);
                        GetComponent<Animator>().SetTrigger("Exploit");

                        CurExploit.ExpoitAction(CurExploit.curDamage);
                    }
                }

                if (CurExploit.hp <= 0f && GetComponent<PhotonView>().IsMine)
                {
                    CurExploit.ExploitComplete();
                }
            }
            else if (playerType == type.photon)
            {
                if (trigger)
                {
                    if (Input.GetButtonDown("Interact") && CurExploitPhoton.curDamage > 0f)
                    {
                        Vector3 direction = new Vector3(CurExploitPhoton.transform.position.x, transform.position.y, CurExploitPhoton.transform.position.z) - transform.position;
                        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

                        //other.transform.LookAt(this.transform);
                        GetComponent<Animator>().SetTrigger("Exploit");

                        CurExploitPhoton.ExpoitAction(CurExploitPhoton.curDamage);
                    }
                }

                if (CurExploitPhoton.hp <= 0f && GetComponent<PhotonView>().IsMine)
                {
                    CurExploitPhoton.ExploitComplete();
                }
            }
        }
    }*/

    /*
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && GetComponent<PhotonView>().IsMine)
        {
            trigger = true;
        }

        if (playerType == type.local)
        {
            if (trigger)
            {
                if (Input.GetButtonDown("Interact") && CurExploit.curDamage > 0f)
                {
                    Vector3 direction = new Vector3(CurExploit.transform.position.x, transform.position.y, CurExploit.transform.position.z) - transform.position;
                    transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

                    //other.transform.LookAt(this.transform);
                    GetComponent<Animator>().SetTrigger("Exploit");

                    CurExploit.ExpoitAction(CurExploit.curDamage);
                }
            }

            if (CurExploit.hp <= 0f && GetComponent<PhotonView>().IsMine)
            {
                CurExploit.ExploitComplete();
            }
        }
        else if (playerType == type.photon)
        {
            if (trigger)
            {
                if (Input.GetButtonDown("Interact") && CurExploitPhoton.curDamage > 0f)
                {
                    Vector3 direction = new Vector3(CurExploitPhoton.transform.position.x, transform.position.y, CurExploitPhoton.transform.position.z) - transform.position;
                    transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

                    //other.transform.LookAt(this.transform);
                    GetComponent<Animator>().SetTrigger("Exploit");

                    CurExploitPhoton.ExpoitAction(CurExploitPhoton.curDamage);
                }
            }

            if (CurExploitPhoton.hp <= 0f && GetComponent<PhotonView>().IsMine)
            {
                CurExploitPhoton.ExploitComplete();
            }
        }
    }*/

}