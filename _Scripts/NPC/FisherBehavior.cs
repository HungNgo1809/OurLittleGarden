using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FisherBehavior : MonoBehaviour
{
    // Start is called before the first frame update;

    public Transform HomePoint;
    public Transform FishingPoint;
    public int TimeGoHome = 18;
    public Shoper shopoer;
    public Animator animator;
    public GameObject Seller;
    public GameObject FishingRod;
    private NavMeshAgent nav;

    private bool isGoingHome = false;
    private bool isMoving = false;
    private bool isFishing = false;
    private bool isMeetSeller = false;
    private bool isBackToFishing = false;
    private bool EndTalkWithSeller = false;

    private float pathResetTimer = 15f;
    private float currentTimer = 0f;


    #region NPCAction
    public enum NPCAction
    {
        DoNothing, GoingHome, Fishing , MeetSeller , BackToFishing
    }

    public NPCAction action;

    public void SwitchStateAction(NPCAction action_)
    {
        isFishing = false;
        isGoingHome = false;
        nav.isStopped = true;
        nav.ResetPath();
        FishingRod.SetActive(false);
        switch (action_)
        {
            case NPCAction.DoNothing:
                SwitchState(NPCState.Idle);
                Debug.Log("Donothing" + gameObject.name);
                break;

            case NPCAction.GoingHome:
                isGoingHome = true;
                break;
            case NPCAction.Fishing:
                isFishing = true;
                FishingRod.SetActive(true);
                break;
            case NPCAction.MeetSeller:
                isMeetSeller = true;
                break;
            case NPCAction.BackToFishing:
                isBackToFishing = true;
                break;
        }

        action = action_;
    }

    #endregion


    #region NPCState
    public enum NPCState
    {
        Idle, Walking, Talking, Fishing
    }

    public NPCState state;


    public void SwitchState(NPCState fisher)
    {
        animator.SetBool("isDoneWalking", true);
        animator.SetBool("isDoneTalking", true);
        animator.SetBool("isDoneFishing", true);
        switch (fisher)
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
            case NPCState.Fishing:
                animator.SetBool("isDoneFishing", false);
                animator.Play("fishingCast");
                break;


        }

        state = fisher;
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
            shopoer.FisherComingHome = true;

            GoingHomePoint();
            float distance = Vector3.Distance(gameObject.transform.position, HomePoint.position);

            if (distance < 1f)
            {
                isGoingHome = false;
                //  animator.SetBool("isDoneWalking", true);
                SwitchStateAction(NPCAction.DoNothing);
                nav.isStopped = true;
                nav.ResetPath();

                shopoer.FisherComingHome = false;
                gameObject.SetActive(false);

            }
            shopoer.FisherSpawned = false;
        }

        if (isFishing)
        {
            FishingAction();
            isFishing = false;
        }

        if (isMeetSeller)
        {
            if (Seller.gameObject
                .activeSelf)
            {
                MeetingSeller();
                float distance = Vector3.Distance(gameObject.transform.position, Seller.transform.position);
                if (distance < 1.5f)
                {
                    isMeetSeller = false;
                    SellerBehavior sellerBehavior = Seller.GetComponent<SellerBehavior>();
                    Seller.transform.LookAt(gameObject.transform.position);
                    sellerBehavior.SwitchState(SellerBehavior.NPCState.Talking);
                    //    animator.SetBool("isDoneWalking", true);
                    SwitchState(NPCState.Talking);
                    nav.isStopped = true;
                    // nav.ResetPath();
                }
            }
            else
            {
                SwitchStateAction(NPCAction.DoNothing);
                isMeetSeller = false;
            }


        }

        if (EndTalkWithSeller)
        {
            SellerBehavior sellerBehavior = Seller.GetComponent<SellerBehavior>();
            if (state != NPCState.Talking || sellerBehavior.state != SellerBehavior.NPCState.Talking)
            {
                sellerBehavior.SwitchState(SellerBehavior.NPCState.Idle);
                EndTalkWithSeller = false;
            }
        }


        if (isBackToFishing)
        {
            GoToFishing();
            float distance = Vector3.Distance(gameObject.transform.position, FishingPoint.position);

            if (distance < 1f)
            {
                isBackToFishing = false;
                SwitchState(NPCState.Fishing);
                nav.isStopped = true;
                //  nav.ResetPath();
            }

        }
    }

    public void GoingHomePoint()
    {


        // animator.SetBool("isDoneWalking", false);
        //  animator.Play("walking");
        SwitchState(NPCState.Walking);
        nav.SetDestination(HomePoint.position);

    }

    public void FishingAction()
    {
        SwitchState(NPCState.Fishing);
    }

    public void MeetingSeller()
    {

        SwitchState(NPCState.Walking);
        nav.SetDestination(Seller.transform.position);
        EndTalkWithSeller = true;
        isMoving = true;
        currentTimer = 0f; // Reset the timer

    }
    public void GoToFishing()
    {

        SwitchState(NPCState.Walking);
        nav.SetDestination(FishingPoint.position);

        isMoving = true;
        currentTimer = 0f; // Reset the timer

    }
}
