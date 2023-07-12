using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;



public class QuestNPCInteract : MonoBehaviour
{
    // Start is called before the first frame update

    public Shoper shop;
    public NavMeshAgent nav;
    public Coroutine lookBackCoroutine;
    public bool TalkBackToQuest;
    public QuestManager questManager;
    public ClosetNPCInteract closetNPCInteract;
    public GameObject Icon;
    public Canvas can;
    private Animator animator;
    private QuestBehavior behavirInteract;
    private QuestDailyTask questDailyTask;
    public bool isInteracted = false;
    void Start()
    {
        can.worldCamera = Camera.main;
        animator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        behavirInteract = GetComponent<QuestBehavior>();
        questDailyTask = GetComponent<QuestDailyTask>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && !shop.QuestComingHome)
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
                    PlayerControl control = other.GetComponent<PlayerControl>();
                    control.NPCInteract_ = true;
                    Animator ani = other.GetComponent<Animator>();
                    ani.SetFloat("Speed", 0);
                    ani.SetBool("Run", false);
                    if (shop.questSpawned == false)
                    {
                        shop.questSpawned = true;
                    }
                    NavMeshAgent nav = GetComponent<NavMeshAgent>();
                    
                    if (behavirInteract.state != QuestBehavior.NPCState.Talking)
                    {
                        behavirInteract.SwitchStateAction(QuestBehavior.NPCAction.DoNothing);
                        behavirInteract.SwitchState(QuestBehavior.NPCState.Talking);
                    }

                    nav.isStopped = true;
                    nav.ResetPath();
                    behavirInteract.StopAllCoroutines();
                    // behavirInteract.SwitchStateAction(QuestBehavior.NPCAction.MeetPlayer);
                    gameObject.transform.LookAt(other.transform);
             
                    if (lookBackCoroutine != null)
                        StopCoroutine(lookBackCoroutine);
               
                    QuestManager.Instance.CheckQuestRequirementMeet(gameObject.name);
                    QuestManager.Instance.CheckSideQuestRequirementMeet(gameObject.name);
                    if (questManager.state != QuestManager.QuestState.Accept)
                    {
                        questManager.CheckCurrentQuest();
                        if (questManager.state == QuestManager.QuestState.Available)
                        {

                            questManager.areYouDoneQuestChat.SetActive(false);
                            questManager.questChat.SetActive(true);
                        }
                        else
                        {
                            questManager.noMoreQuestToDo.SetActive(true);
                            StartCoroutine(questManager.SlowOffGameObject(questManager.noMoreQuestToDo));
                            control.NPCiteractoff();
                        }
                    }
                    else
                    {
                        questManager.questChat.SetActive(false);
                        questManager.areYouDoneQuestChat.SetActive(true);


                    }
                    Debug.Log("IsInteracted = true");
                    isInteracted = true;
                }
                Debug.Log("notcalled = true");
           

            }


        }
      

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
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
            if(QuestManager.Instance.state != QuestManager.QuestState.Accept && QuestManager.Instance.state != QuestManager.QuestState.Complete)
            {
                questManager.CheckCurrentQuest();
            }
            if (QuestManager.Instance.state == QuestManager.QuestState.Available)
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
        if (other.tag == "Player" && shop.QuestComingHome == false && isInteracted == true)
        {
            animator.SetBool("isDoneTalking", true);
            StartCoroutine(Lookback());
            lookBackCoroutine = StartCoroutine(Lookback());
            isInteracted = false;
            Debug.Log("QuestLookBack");
        }
        if (other.gameObject.GetComponentInChildren<playerFloatingUi>() != null)
        {
            other.gameObject.GetComponentInChildren<playerFloatingUi>().transform.GetChild(0).gameObject.SetActive(false);
        }
        if(other.tag == "Player")
        {
            if (can.gameObject.activeSelf == true)
            {
                can.gameObject.SetActive(false);
            }
        }
       
      
    }


  
    IEnumerator Lookback()
    {
        yield return new WaitForSeconds(5f);
        float distance = Vector3.Distance(gameObject.transform.position, shop.QuestPoint.position);
        if(distance < 1.5f)
        {
            gameObject.transform.LookAt(shop.QuestPoint.transform.position);
            behavirInteract.SwitchState(QuestBehavior.NPCState.Idle);


        }
        else
        {
            if(behavirInteract.NPCTalkWith != null)
            {
                float distance_ = Vector3.Distance(gameObject.transform.position, behavirInteract.NPCTalkWith.transform.position);
                if (distance_ < 1.5f)

                    if (behavirInteract.isGoingHome == true)
                    {
                        StartCoroutine(behavirInteract.GoingHomePoint());

                    }
                    else if (behavirInteract.isGoingSomeWhere == true)
                    {
                        StartCoroutine(behavirInteract.GoingSomeWhereNPC());
                    }
                    else if (behavirInteract.isMeetSeller == true)
                    {
                        StartCoroutine(behavirInteract.MeetingSeller());
                    }
                    else if (behavirInteract.isMeeting == true)
                    {
                        StartCoroutine(behavirInteract.MeetSophia());
                    }
                    else if (behavirInteract.isMeetNPC == true)
                    {
                        StartCoroutine(behavirInteract.MeetNPC());
                    }
                    else
                    {
                        questDailyTask.DoCurrentTask();
                    }
            }
            else
            {
                if (behavirInteract.isGoingHome == true)
                {
                    StartCoroutine(behavirInteract.GoingHomePoint());

                }
                else if (behavirInteract.isGoingSomeWhere == true)
                {
                    StartCoroutine(behavirInteract.GoingSomeWhereNPC());
                }
                else if (behavirInteract.isMeetSeller == true)
                {
                    StartCoroutine(behavirInteract.MeetingSeller());
                }
                else if (behavirInteract.isMeeting == true)
                {
                    StartCoroutine(behavirInteract.MeetSophia());
                }
                else if (behavirInteract.isMeetNPC == true)
                {
                    StartCoroutine(behavirInteract.MeetNPC());
                }
                else
                {
                    questDailyTask.DoCurrentTask();
                }
            }
            
            
         
        }
       
    }

    public void RotateTowardsTarget()
    {

        // Get the direction to the main camera
        can.transform.LookAt(can.transform.position + Camera.main.transform.rotation * -Vector3.back, Camera.main.transform.rotation * Vector3.up);

    }
}
