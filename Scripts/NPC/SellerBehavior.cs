using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static NPCInteract;

public class SellerBehavior : MonoBehaviour
{
    // Start is called before the first frame update;

    public Transform HomePoint;

    public int TimeWalkup = 0;
    public int TimeGoHome = 18;
    public Shoper shopoer;
    public Animator animator;
    public GameObject NPCTalkWith;
    private NavMeshAgent nav;

    private bool isGoingHome = false;
    private bool isMoving = false;
    private bool isDancing = false;
    private bool isThinking = false;


    private float pathResetTimer = 45f;
    private float currentTimer = 0f;


   

    #region NPCAction
    public enum NPCAction
    {
        DoNothing, GoingHome, Dancing, Thinking
    }

    public NPCAction action;

    public void SwitchStateAction(NPCAction action_)
    {
        isDancing = false;
        nav.isStopped = true;
        nav.ResetPath();
        switch (action_)
        {
            case NPCAction.DoNothing:
                SwitchState(NPCState.Idle);
                Debug.Log("Donothing" + gameObject.name);
                break;

            case NPCAction.GoingHome:
                isGoingHome = true;
                break;
            case NPCAction.Dancing:
                isDancing = true;
                break;
            case NPCAction.Thinking:
                isThinking = true;
                break;


        }

        action = action_;
    }

    #endregion


    #region NPCState
    public enum NPCState
    {
        Idle, Walking, Talking, Dancing, Thinking , TalkWithNPC
    }

    public NPCState state;


    public void SwitchState(NPCState seller)
    {
        animator.SetBool("isDoneWalking", true);
        animator.SetBool("isDoneTalking", true);
        animator.SetBool("isDoneDancing", true);
        animator.SetBool("isDoneThinking", true);
        switch (seller)
        {
            case NPCState.Idle:
                animator.Play("idle");

                break;
            case NPCState.Walking:
                animator.SetBool("isDoneWalking", false);
                animator.Play("walking");

                break;

            case NPCState.Talking:
                animator.SetBool("isDoneTalking", false);
                animator.Play("talking");

                break;
            case NPCState.Dancing:
                animator.SetBool("isDoneDancing", false);
                animator.Play("dancing");

                break;
            case NPCState.Thinking:
                animator.SetBool("isDoneThinking", false);
                animator.Play("thinking");

                break;
            case NPCState.TalkWithNPC:
                animator.SetBool("isDoneTalking", false);
                animator.Play("talking");
                if (NPCTalkWith != null )
                {
                    gameObject.transform.LookAt(NPCTalkWith.transform);
                }
                break;
        }

        state = seller;
    }
    #endregion




    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();


    }

    // Update is called once per frame
    void Update()
    {
        //Time 

        if (!isGoingHome && TimeManager.Instance.timestamp.hour == TimeGoHome)
        {

            SwitchStateAction(NPCAction.GoingHome);
        }
        //

        if (isMoving) // check tgian di chuyen qua lau_Seller
        {
            currentTimer += Time.deltaTime;


            if (currentTimer >= pathResetTimer)
            {
                if (gameObject.activeSelf != false)
                {
                    NavMeshAgent nav = gameObject.GetComponent<NavMeshAgent>();
                    nav.isStopped = true;
                    nav.ResetPath();
                    SwitchState(NPCState.Idle);
                }


                isMoving = false;
            }
        }


        if (isGoingHome)
        {
            shopoer.SellerComingHome = true;
           
            GoingHomePoint();
            float distance = Vector3.Distance(gameObject.transform.position, HomePoint.position);

            if (distance < 1f)
            {
                isGoingHome = false;
                //  animator.SetBool("isDoneWalking", true);
                SwitchState(NPCState.Idle);
                nav.isStopped = true;
                nav.ResetPath();

                shopoer.SellerComingHome = false;
                gameObject.SetActive(false);

            }
            shopoer.sellerSpawned = false;
        }

        if (isDancing)
        {
            DancingAction();
            isDancing = false;
        }
        if (isThinking)
        {
            ThinkingAction();
            isThinking = false;
        }
    }

    public void GoingHomePoint()
    {


        // animator.SetBool("isDoneWalking", false);
        //  animator.Play("walking");
        SwitchState(NPCState.Walking);
        nav.SetDestination(HomePoint.position);

    }

    public void DancingAction()
    {
        SwitchState(NPCState.Dancing);
    }
    public void ThinkingAction()
    {
        SwitchState(NPCState.Thinking);
    }
}
