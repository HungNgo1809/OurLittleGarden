using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static PlantState;


public class NPCInteract : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject chat;
    public Image chat_img;
    public Shoper shop;
    public animationPlay openbook;
    private Animator animator;
    public NavMeshAgent nav;
    public bool isface;
    public Coroutine lookBackCoroutine;
    public bool isAskingTo;
    public bool TalkBackToQuest;
    public ClosetNPCInteract closetNPCInteract;
    public GameObject Icon;
    public Canvas can;


    private SellerBehavior behavirInteract;
    private SellerDailyTask sellerDailyTask;
    private bool isInteracted = false;
    void Start()
    {
        can.worldCamera = Camera.main;
        animator = GetComponent<Animator>();   
        nav = GetComponent<NavMeshAgent>();
        behavirInteract = GetComponent<SellerBehavior>();
        sellerDailyTask = GetComponent<SellerDailyTask>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && !shop.SellerComingHome)
        {
            RotateTowardsTarget();
            if (other.gameObject.GetComponentInChildren<playerFloatingUi>() != null)
            {
                other.gameObject.GetComponentInChildren<playerFloatingUi>().transform.GetChild(0).gameObject.SetActive(true);

            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                closetNPCInteract.FindClosestNPC(other.gameObject);


                if(closetNPCInteract.closestNPC.NPC == gameObject)
                {
                    //chat.SetActive(true);
                    QuestManager.Instance.CheckQuestRequirementMeet(gameObject.name);
                    QuestManager.Instance.CheckSideQuestRequirementMeet(gameObject.name);
                    PlayerControl control = other.GetComponent<PlayerControl>();
                    control.NPCInteract_ = true;
                    Animator ani = other.GetComponent<Animator>();
                    ani.SetFloat("Speed", 0);
                    ani.SetBool("Run", false);
                    QuestManager.Instance.CheckSideQuest(gameObject);
                    if (shop.sellerSpawned == false)
                    {
                        shop.sellerSpawned = true;
                    }
                    if(behavirInteract.state != SellerBehavior.NPCState.Talking)
                    {
                        behavirInteract.SwitchStateAction(SellerBehavior.NPCAction.DoNothing);
                        behavirInteract.SwitchState(SellerBehavior.NPCState.Talking);

                    }
                  
                    gameObject.transform.LookAt(other.transform.position);
                    if (lookBackCoroutine != null)
                        StopCoroutine(lookBackCoroutine);
                    isInteracted = true;
                }
               
            }


        }
      
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" )
        {
            if (other.gameObject.GetComponentInChildren<playerFloatingUi>() != null)
            {
                other.gameObject.GetComponentInChildren<playerFloatingUi>().transform.GetChild(0).gameObject.SetActive(true);
                PlayerControl control = other.gameObject.GetComponent<PlayerControl>();
                control.bubbleAnimation.Play("startBubbleE");
                UiManager.Instance.ReturnPlayerControl(control);
                //  buble.Play("loopAnimation");
            }
            can.gameObject.SetActive(true);
            if (QuestManager.Instance.sideQueststate != QuestManager.SideQuestState.Accept && QuestManager.Instance.sideQueststate != QuestManager.SideQuestState.Complete)
            {
                QuestManager.Instance.CheckCurrentSideQuest();
            }

            if (QuestManager.Instance.sideQueststate == QuestManager.SideQuestState.Available && gameObject.name == QuestManager.Instance.TakeNPCSideQuestName())
            {
                Icon.SetActive(true);
            }
            else
            {
                //can.gameObject.SetActive(false);
                Icon.SetActive(false);

            }
        }
      
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && shop.SellerComingHome == false  && isInteracted == true)
        {
            animator.SetBool("isDoneTalking", true);
            StartCoroutine(Lookback());
            lookBackCoroutine = StartCoroutine(Lookback());
            isInteracted = false;
           
        }
        if (other.gameObject.GetComponentInChildren<playerFloatingUi>() != null)
        {
            other.gameObject.GetComponentInChildren<playerFloatingUi>().transform.GetChild(0).gameObject.SetActive(false);
        }
        if (other.tag == "Player")
        {
            if (can.gameObject.activeSelf == true)
            {
                can.gameObject.SetActive(false);
            }
        }
    }


    public void ImageBlur()
    {
        chat_img = chat.GetComponent<Image>();

        chat_img.color = new Color(0, 0, 0, 0);
        StartCoroutine(DelayChatActive());
    }

    IEnumerator DelayChatActive()
    {
        yield return new WaitForSeconds(2.5f);
        Debug.Log("aaa");
        Image im = chat.GetComponent<Image>();
        im.color = new Color(1, 1, 1, 1);
        chat.SetActive(false);
    }
    IEnumerator Lookback()
    {
        yield return new WaitForSeconds(5f);
        
        if(behavirInteract.NPCTalkWith != null)
        {
            float distance = Vector3.Distance(gameObject.transform.position, behavirInteract.NPCTalkWith.transform.position);
            if (distance < 2.5f)
            {
                behavirInteract.SwitchState(SellerBehavior.NPCState.TalkWithNPC);
                animator.Play("talking");
            }
            else
            {
                behavirInteract.NPCTalkWith = null;
                sellerDailyTask.DoCurrentTask();
            }
        }
        else
        {
            sellerDailyTask.DoCurrentTask();
        }
    }

    public void RotateTowardsTarget()
    {

        // Get the direction to the main camera
        can.transform.LookAt(can.transform.position + Camera.main.transform.rotation * -Vector3.back, Camera.main.transform.rotation * Vector3.up);

    }
}
