using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static NPCInteract;

public class NPCBehavior : MonoBehaviour
{

    public Transform HomePoint;

    public Transform Player;
    public Transform Seller;
    public Transform QuestNPC;
    public Transform listPS_transform;
    public int TimeWalkup = 0;
    public int TimeGoHome = 6;
    public Shoper shopoer;
    public Animator animator;
    public float distanceMovedThreshold = 1f; 
    public GameObject dustEffectPrefab;
    public PBRInteract bRInteract;

    public GameObject NPCGonnaMeet;
    public GameObject NPCTalkWith;
    private float distanceMoved = 1.5f; 
    private NavMeshAgent nav;
  

    private GameObject NPCTalkingToMe;
    private GameObject ToNPCTalking;

    public bool isMeeting =false;
    public bool isGoingSomeWhere = false;
    public bool isGoingHome = false;
    public bool isNPCTalkingTo = false;
    public bool isToNPCTalking = false;
    private bool EndTalkWithSeller = false;
    public bool isMoving = false;
    public bool isDancing = false;
    public bool isThinking = false;
    public bool isMeetSeller = false;
    public bool isMeetNPC = false;
    private bool isCallSomeNPCDancing = false;

    private bool hasMetRandomNPC = false;


    private int thatrandoomNPCint;
    private Vector3 randomPositioon;
    private Vector3 previousPosition;
    private float pathResetTimer = 15f;
    private float currentTimer = 0f;


    #region NPCAction
    public enum NPCAction
    {
        DoNothing, MeetEmma, GoingSomeWhere  , GoingHome , Dancing , Thinking , MeetPlayer , MeetSeller , Watering , MeetNPC , TalkWithNPC
    }

    public NPCAction action;

    public void SwitchStateAction(NPCAction action_)
    {
  
        nav.isStopped = true;
        nav.ResetPath();
        switch (action_)
        {
            case NPCAction.DoNothing:
                SwitchState(NPCState.Idle);  
                //Debug.Log("Donothing" + gameObject.name);
                break;
            case NPCAction.MeetEmma:
                isGoingSomeWhere = false;
                isMeetSeller = false;
                isMeeting = true;
                isMeetNPC = false;
                StartCoroutine(MeetEmma());
               
                break;
            case NPCAction.GoingSomeWhere:
                isGoingSomeWhere= true;
                isMeetSeller = false;
                isMeetNPC = false;
                isMeeting = false;
                randomPositioon = shopoer.GetRandomPosition(shopoer.CenterPoint.transform.position, 30f);
                StartCoroutine(GoingSomeWhereNPC());
                
                //Debug.Log("GoingSomeWhere" + gameObject.name);
                break;

            case NPCAction.GoingHome:
                isGoingHome = true;
                isGoingSomeWhere = false;
                isMeetNPC = false;
                isMeetSeller = false;
                isMeeting = false;
                StartCoroutine(GoingHomePoint());
               
                break;

            case NPCAction.Dancing:
                // isDancing = true;
                DancingAction();
                break;
            case NPCAction.Thinking:
                //isThinking = true;
                ThinkingAction();
                break;
            case NPCAction.MeetPlayer:
                gameObject.transform.LookAt(Player);
                break;
            case NPCAction.MeetSeller:
                isMeetSeller = true;
                isGoingSomeWhere = false;
                isMeetNPC = false;
                isMeeting = false;
                StartCoroutine(MeetingSeller());
               
                break;
            case NPCAction.Watering:
                SwitchState(NPCState.Watering);
                break;
            case NPCAction.MeetNPC:
                isGoingHome = false;
                isGoingSomeWhere = false;
                isMeetNPC = transform;
                isMeetSeller = false;
                isMeeting = false;
                if (NPCGonnaMeet != null && NPCGonnaMeet.activeSelf == true)
                {
                    StartCoroutine(MeetNPC());
                }
                else
                {
                    isMeetNPC = false;
                    SwitchStateAction(NPCAction.DoNothing);

                }
                break;
            case NPCAction.TalkWithNPC:
                SwitchState(NPCState.Talking);
                if (NPCTalkWith != null)
                {
                    gameObject.transform.LookAt(NPCTalkWith.transform);
                }
                break;
        }

        action = action_;
    }

 
    #endregion


    #region NPCState
    public enum NPCState
    {
        Idle, Walking, Talking, Dancing , Thinking , Watering
    }

    public NPCState state;


    public void SwitchState(NPCState seller)
    {
        animator.SetBool("isDoneWalking", true);    
        animator.SetBool("isDoneTalking", true);
        animator.SetBool("isDoneDancing", true);
        animator.SetBool("isDoneThinking", true);
        animator.SetBool("isDoneWatering", true);
       
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
            case NPCState.Watering:
                animator.SetBool("isDoneWatering", false);
                animator.Play("watering", 0, 0f);

                break;

        }

        state = seller;
    }
    #endregion




    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        previousPosition = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        //Time 

        if (!isGoingHome && TimeManager.Instance.timestamp.hour >= TimeGoHome && !bRInteract.isOnWorkingTimes)
        {

            SwitchStateAction(NPCAction.GoingHome);
        }



        //
        if(!bRInteract.isOnWorkingTimes)
        {
            if (isMoving) // check tgian di chuyen qua lau_Seller
            {
                currentTimer += Time.deltaTime;


                if (currentTimer >= pathResetTimer)
                {
                    if (gameObject.activeSelf != false)
                    {
                        NavMeshAgent nav = gameObject.GetComponent<NavMeshAgent>();
                        Animator animator = gameObject.GetComponent<Animator>();
                        nav.isStopped = true;
                        nav.ResetPath();
                        SwitchState(NPCState.Idle);
                    }



                    isMoving = false;
                }
            }

            if (nav.isOnNavMesh && nav.remainingDistance > 0f && nav.hasPath)
            {

                float frameDistance = Vector3.Distance(transform.position, previousPosition);


                distanceMoved += frameDistance;


                previousPosition = transform.position;


                if (distanceMoved >= distanceMovedThreshold)
                {
                    dustEffect();
                    distanceMoved = 0f;
                }
            }
            else
            {

                previousPosition = transform.position;
            }

            //
            /*
            if (isMeeting)
            {
                
            }

            if (isNPCTalkingTo)
            {
                QuestBehavior behavior = NPCTalkingToMe.GetComponent<QuestBehavior>();
                if (behavior.state != QuestBehavior.NPCState.Talking)
                {
                    SwitchState(NPCState.Idle);
                    isNPCTalkingTo = false;
                }
            }

            if (isToNPCTalking)
            {
                QuestBehavior behavior = ToNPCTalking.GetComponent<QuestBehavior>();
                if (state != NPCState.Talking)
                {
                    behavior.SwitchState(QuestBehavior.NPCState.Idle);
                    isToNPCTalking = false;
                }
            }

            if (isGoingSomeWhere)
            {
              
                

            }

            if (isMeetSeller)
            {
                


            }
            /*
            if (EndTalkWithSeller)
            {
                SellerBehavior sellerBehavior = Seller.GetComponent<SellerBehavior>();
                if (state != NPCState.Talking || sellerBehavior.state != SellerBehavior.NPCState.Talking)
                {
                    sellerBehavior.SwitchState(SellerBehavior.NPCState.Idle);
                    EndTalkWithSeller = false;
                }
            }
           
            if (isGoingHome)
            {

               
            }

            if (isDancing)
            {
               
                isDancing = false;
            }
            if (isThinking)
            {
                
                isThinking = false;
            }
             */
        }

    }



    public IEnumerator MeetEmma()
    {
        nav.ResetPath();
        ToNPCTalking = QuestNPC.gameObject;
        SwitchState(NPCState.Walking);
       


        while (Vector3.Distance(gameObject.transform.position, QuestNPC.position) > 1.5 && isMeeting && !bRInteract.isIntereacted)
        {

            if (nav.isStopped == true)
            {
                nav.isStopped = false;
            }
            SwitchState(NPCState.Walking);
            nav.SetDestination(QuestNPC.position);
            yield return null;
        }
        isMeeting = false;
        if (Vector3.Distance(gameObject.transform.position, QuestNPC.position) <= 2.5)
        {
            gameObject.transform.LookAt(QuestNPC.position);

            QuestBehavior behav = QuestNPC.GetComponent<QuestBehavior>();
            behav.NPCTalkWith = gameObject;
            if (behav.state != QuestBehavior.NPCState.Talking)
            {
                behav.SwitchStateAction(QuestBehavior.NPCAction.TalkWithNPC);
            }
            SwitchState(NPCState.Talking);
        }
        else if(bRInteract.isIntereacted)
        {
            SwitchState(NPCState.Talking);
        }
        else
        {
            SwitchState(NPCState.Thinking);
        }
       
        isToNPCTalking = true;
        nav.isStopped = true;

    }

    public IEnumerator GoingSomeWhereNPC()
    {
      
        SwitchState(NPCState.Walking);
        nav.SetDestination(randomPositioon);
        
        isMoving = true;
        currentTimer = 0f; // Reset the timer
        float distance = Vector3.Distance(gameObject.transform.position, randomPositioon);
        while (Vector3.Distance(gameObject.transform.position, randomPositioon) > 1.5 && isGoingSomeWhere && !bRInteract.isIntereacted)
        {
            if (nav.isStopped == true)
            {
                nav.isStopped = false;
            }
          
            nav.SetDestination(randomPositioon);
            SwitchState(NPCState.Walking);
            yield return null;
        }
        if (bRInteract.isIntereacted)
        {
            SwitchState(NPCState.Talking);
        }
        else
        {
            SwitchState(NPCState.Idle);
        }
        isMoving = false;
        isGoingSomeWhere = false;
        //    animator.SetBool("isDoneWalking", true);
       
        nav.isStopped = true;
        //  nav.ResetPath();
    }


  
 


    public IEnumerator GoingHomePoint()
    {
 
       
       // animator.SetBool("isDoneWalking", false);
      //  animator.Play("walking");
        SwitchState(NPCState.Walking);
        nav.SetDestination(HomePoint.position);
        
        shopoer.NPCNor1ComingHome = true;
       
        while (Vector3.Distance(gameObject.transform.position, HomePoint.position) > 1.5)
        {
            if (nav.isStopped == true)
            {
                nav.isStopped = false;
            }
            SwitchState(NPCState.Walking);
            nav.SetDestination(HomePoint.position);
           
            yield return null;
        }
        isGoingHome = false;
        //  animator.SetBool("isDoneWalking", true);
        SwitchState(NPCState.Idle);
        nav.isStopped = true;
        nav.ResetPath();
        shopoer.NPCNor1ComingHome = false;
        gameObject.SetActive(false);



        //

       // shopoer.questSpawned = false;
        shopoer.NPCNor1Spawned = false;
    }

    public void DancingAction()
    {
        SwitchState(NPCState.Dancing);
    }
    public void ThinkingAction()
    {
        SwitchState(NPCState.Thinking);
    }

    public IEnumerator MeetingSeller()
    {
        if (Seller.gameObject
                    .activeSelf)
        {
            SwitchState(NPCState.Walking);
            
          
            EndTalkWithSeller = true;
            isMoving = true;
            currentTimer = 0f; // Reset the timer
            //float distance = Vector3.Distance(gameObject.transform.position, Seller.transform.position);
            while (Vector3.Distance(gameObject.transform.position, Seller.transform.position) > 2f && Seller.gameObject.activeSelf == true && isMeetSeller && !bRInteract.isIntereacted)
            {
                if (nav.isStopped == true)
                {
                    nav.isStopped = false;
                }
                Debug.Log(Vector3.Distance(gameObject.transform.position, Seller.transform.position));
                SwitchState(NPCState.Walking);
                nav.SetDestination(Seller.transform.position);
                yield return null;
            }
            
            if(Vector3.Distance(gameObject.transform.position, Seller.transform.position) <= 2.5f)
            {
                gameObject.transform.LookAt(Seller.transform.position);
                SwitchState(NPCState.Talking);
                SellerBehavior sellerBehavior = Seller.GetComponent<SellerBehavior>();
                //Seller.transform.LookAt(gameObject.transform.position);
                sellerBehavior.NPCTalkWith = gameObject;
                sellerBehavior.SwitchState(SellerBehavior.NPCState.TalkWithNPC);
                //    animator.SetBool("isDoneWalking", true);
            }
            else if (bRInteract.isIntereacted)
            {
                SwitchState(NPCState.Talking);
            }
            else 
            {
                SwitchState(NPCState.Thinking);
            }
            isMoving = false;
            isMeetSeller = false;
          

            nav.ResetPath();
            nav.isStopped = true;
           
            // nav.ResetPath();
        }
        else
        {
            SwitchStateAction(NPCAction.DoNothing);
            isMeetSeller = false;
        }
    }

    public IEnumerator MeetNPC()
    {
        nav.ResetPath();
        SwitchState(NPCState.Walking);
       


        while (Vector3.Distance(gameObject.transform.position, NPCGonnaMeet.transform.position) > 2.5 && isMeetNPC && !bRInteract.isIntereacted)
        {

            if (nav.isStopped == true)
            {
                nav.isStopped = false;
            }
            SwitchState(NPCState.Walking);
            nav.SetDestination(NPCGonnaMeet.transform.position);
            yield return null;
        }
        if(Vector3.Distance(gameObject.transform.position, NPCGonnaMeet.transform.position) <= 2.5)
        {
            gameObject.transform.LookAt(NPCGonnaMeet.transform.position);
            GenNPCBehavior gen = NPCGonnaMeet.GetComponent<GenNPCBehavior>();
            NPCGonnaMeet.transform.LookAt(gameObject.transform);
            if (gen.state != GenNPCBehavior.NPCState.Talking)
            {
                gen.SwitchStateAction(GenNPCBehavior.NPCAction.DoNothing);
                gen.SwitchState(GenNPCBehavior.NPCState.Talking);
            }
            SwitchState(NPCState.Talking);
        }
        else if (bRInteract.isIntereacted)
        {
            SwitchState(NPCState.Talking);
        }
        else
        {
            SwitchState(NPCState.Thinking);
        }
        isMeetNPC = false;
       
        nav.isStopped = true;

    }





    public void dustEffect()
    {
        GameObject obj = Instantiate(dustEffectPrefab, listPS_transform.transform);
        obj.transform.position = gameObject.transform.position;

        ParticleSystem dustPS = obj.GetComponentInChildren<ParticleSystem>();

        dustPS.Play();
        StartCoroutine(disappear(obj, dustPS));
        Destroy(obj, 3f);
    }

    IEnumerator disappear(GameObject obje, ParticleSystem dustSP)
    {

        SpriteRenderer spriteRenderer = obje.GetComponentInChildren<SpriteRenderer>();
        float duration = 2.5f; // Time in seconds to fade out the sprite
        float timer = 0.0f; // Timer to track progress
        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / duration);
            Color color = spriteRenderer.color;
            color.a = Mathf.Lerp(1.0f, 0.0f, t);
            spriteRenderer.color = color;


            yield return null;
        }

    }
}
