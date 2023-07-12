using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static NPCInteract;

public class QuestBehavior : MonoBehaviour
{
    // Start is called before the first frame update;
    public Transform DefaultPoint;
    public Transform HomePoint;

    public Transform Player;
    public Transform Seller;
    public Transform Nor1NPC;
    public Transform listPS_transform;
    public int TimeWalkup = 0;
    public int TimeGoHome = 6;
    public Shoper shopoer;
    public Animator animator;
    public float distanceMovedThreshold = 1f;
    public GameObject dustEffectPrefab;
    public GameObject NPCGonnaMeet;
    public GameObject NPCTalkWith;
    private float distanceMoved = 1.5f;
    private NavMeshAgent nav;


    public bool isMeeting = false;
    public bool isGoingSomeWhere = false;
    public bool isGoingHome = false;
    public bool isMeetNPC = false;
    public bool isMoving = false;
    public bool isDancing = false;
    public bool isThinking = false;
    public bool isMeetSeller = false;



    private int thatrandoomNPCint;
    private Vector3 randomPositioon;
    private Vector3 previousPosition;
    private float pathResetTimer = 45f;
    private float currentTimer = 0f;

    private QuestNPCInteract interact;

    #region NPCAction
    public enum NPCAction
    {
        DoNothing, MeetSophia, GoingSomeWhere, GoingHome, Dancing, Thinking, MeetSeller , MeetNPC , TalkWithNPC
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
              
                break;
            case NPCAction.MeetSophia:
                PBRInteract interact = Nor1NPC.GetComponent<PBRInteract>();
                if(!interact.isOnWorkingTimes)
                {
                    isMeeting = true;

                    StartCoroutine(MeetSophia());
                }
                else
                {
                    isMeeting = false;
                    SwitchStateAction(NPCAction.Thinking);
                }
                isGoingSomeWhere = false;
                isMeetSeller = false;
                isMeetNPC= false;
                break;
            case NPCAction.GoingSomeWhere:
                isGoingSomeWhere = true;
                isMeetNPC = false;
                isMeeting = false;
                isMeetSeller = false;
                randomPositioon = shopoer.GetRandomPosition(shopoer.CenterPoint.transform.position, 30f);
                Debug.Log("GoingSomeWhere" + gameObject.name);
                StartCoroutine(GoingSomeWhereNPC());
                break;

            case NPCAction.GoingHome:
                isGoingHome = true;
                isMeetNPC = false;
                isGoingSomeWhere = false;
                isMeeting = false;
                isMeetSeller = false;
                StartCoroutine(GoingHomePoint());

                break;
            case NPCAction.Dancing:
                DancingAction();
                isDancing = true;
                break;
            case NPCAction.Thinking:
                ThinkingAction();
                isThinking = true;
                break;   
            case NPCAction.MeetSeller:
                isMeetSeller = true;
                isMeetNPC = false;
                isGoingSomeWhere = false;
                isMeeting = false;
                StartCoroutine(MeetingSeller());
                break;
            case NPCAction.MeetNPC:
                isMeetSeller = false;
                isMeetNPC = true;
                isGoingSomeWhere = false;
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
        Idle, Walking, Talking, Dancing, Thinking
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

        }

        state = seller;
    }
    #endregion




    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        previousPosition = transform.position;
        interact = GetComponent<QuestNPCInteract>();
    }

    // Update is called once per frame
    void Update()
    {
        //Time 

        if (!isGoingHome && TimeManager.Instance.timestamp.hour >= TimeGoHome)
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

        /*
        if (isMeeting)
        {
         
        }
       
        if (isNPCTalkingTo)
        {
            NPCBehavior behavior = NPCTalkingToMe.GetComponent<NPCBehavior>();
            if (behavior.state != NPCBehavior.NPCState.Talking)
            {
                SwitchState(NPCState.Idle);
                isNPCTalkingTo = false;
            }
        }
       
        if (isToNPCTalking)
        {
            NPCBehavior behavior = ToNPCTalking.GetComponent<NPCBehavior>();
            if (state != NPCState.Talking)
            {
                behavior.SwitchState(NPCBehavior.NPCState.Idle);
                isToNPCTalking = false;
            }
        }
         
        if (isGoingSomeWhere)
        {
          
            

        }

        if (isMeetSeller)
        {
           


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



    public IEnumerator MeetSophia()
    {
        nav.ResetPath();
        SwitchState(NPCState.Walking);

     
      
        PBRInteract pBRInteract = Nor1NPC.GetComponent<PBRInteract>();


        while (Vector3.Distance(gameObject.transform.position, Nor1NPC.position) > 2 && isMeeting && !pBRInteract.isOnWorkingTimes && !interact.isInteracted)
        {
           
            if (nav.isStopped == true)
            {
                nav.isStopped = false;
            }
            SwitchState(NPCState.Walking);
            nav.SetDestination(Nor1NPC.position);
            yield return null;
        }
        if(!pBRInteract.isOnWorkingTimes && Vector3.Distance(gameObject.transform.position, Nor1NPC.position) <= 2)
        {
            NPCBehavior behav = Nor1NPC.GetComponent<NPCBehavior>();
            behav.NPCTalkWith = gameObject;
            if (behav.state != NPCBehavior.NPCState.Talking)
            {
                behav.SwitchStateAction(NPCBehavior.NPCAction.TalkWithNPC);
            }
            gameObject.transform.LookAt(Nor1NPC.position);
            SwitchState(NPCState.Talking);

        }
        else if (interact.isInteracted)
        {
            SwitchState(NPCState.Talking);
        }
        else
        {
            SwitchState(NPCState.Thinking);
        }
        isMeeting = false;
       
     
        nav.isStopped = true;


    }

    public IEnumerator GoingSomeWhereNPC()
    {
        SwitchState(NPCState.Walking);
        nav.SetDestination(randomPositioon);
     
        isMoving = true;
        currentTimer = 0f; // Reset the timer

 
        while (Vector3.Distance(gameObject.transform.position, randomPositioon) > 2 && isGoingSomeWhere && !interact.isInteracted)
        {
           
            if (nav.isStopped == true)
            {
                nav.isStopped = false;
            }
            nav.SetDestination(randomPositioon);
            SwitchState(NPCState.Walking);
            yield return null;
        }
        if (interact.isInteracted)
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
        shopoer.QuestComingHome = true;
       

        while (Vector3.Distance(gameObject.transform.position, HomePoint.position) > 1)
        {
            if (nav.isStopped == true)
            {
                nav.isStopped = false;
            }
            nav.SetDestination(HomePoint.position);
            SwitchState(NPCState.Walking);
            yield return null;
        }
        isGoingHome = false;
        //  animator.SetBool("isDoneWalking", true);
        SwitchState(NPCState.Idle);
        nav.isStopped = true;
        nav.ResetPath();

        shopoer.QuestComingHome = false;

        gameObject.SetActive(false);
        shopoer.questSpawned = false;
     

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
           
           
            //EndTalkWithSeller = true;
            isMoving = true;
            currentTimer = 0f; // Reset the timer
            float distance = Vector3.Distance(gameObject.transform.position, Seller.transform.position);
            
            while (Vector3.Distance(gameObject.transform.position, Seller.transform.position) > 2 && Seller.gameObject.activeSelf == true && isMeetSeller && !interact.isInteracted)
            {
                if (nav.isStopped == true)
                {
                    nav.isStopped = false;
                }  
                SwitchState(NPCState.Walking);
                nav.SetDestination(Seller.transform.position);
                yield return null;
            }
            

            isMoving = false;
            isMeetSeller = false;
            nav.ResetPath();
            nav.isStopped = true;
            if (Vector3.Distance(gameObject.transform.position, Seller.transform.position) <= 2)
            {
                SellerBehavior sellerBehavior = Seller.GetComponent<SellerBehavior>();
                //Seller.transform.LookAt(gameObject.transform.position);
                sellerBehavior.NPCTalkWith = gameObject;
                sellerBehavior.SwitchState(SellerBehavior.NPCState.TalkWithNPC);
                gameObject.transform.LookAt(Seller.position);

               
                SwitchState(NPCState.Talking);
                Debug.Log(state);

            }
            else if (interact.isInteracted)
            {
                SwitchState(NPCState.Talking);
            }
            else
            {
                SwitchState(NPCState.Thinking);
            }
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


        

        while (Vector3.Distance(gameObject.transform.position, NPCGonnaMeet.transform.position) > 2 && isMeetNPC && !interact.isInteracted) 
        {

            if (nav.isStopped == true)
            {
                nav.isStopped = false;
            }
            SwitchState(NPCState.Walking);
            nav.SetDestination(NPCGonnaMeet.transform.position);
            yield return null;
        }
        isMeetNPC = false;
        if(Vector3.Distance(gameObject.transform.position, NPCGonnaMeet.transform.position) <= 2)
        {
            NPCGonnaMeet.transform.LookAt(gameObject.transform.position);
            GenNPCBehavior behav = NPCGonnaMeet.GetComponent<GenNPCBehavior>();
            if (behav.state != GenNPCBehavior.NPCState.Talking)
            {
                behav.SwitchState(GenNPCBehavior.NPCState.Talking);
            }
            gameObject.transform.LookAt(NPCGonnaMeet.transform.position);
            SwitchState(NPCState.Talking);
        }
        else if(interact.isInteracted)
        {
            SwitchState(NPCState.Talking);
        }
        else 
        {
            SwitchState(NPCState.Thinking);
        }

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
