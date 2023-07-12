using UnityEngine.AI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEditor;
using System.Collections.Generic;

public class Shoper : MonoBehaviour
{
    public Transform Spawnpoint;
    public Transform Spawnpoint_Seller;
    public Transform Spawnpoint_Fisher;
    public Transform Sellpoint;
    public Transform QuestPoint;
    public Transform Nor1Point;
    public Transform CenterPoint;
    public Transform Destination_Fisher;
    public Vector3 RandomPosition;

    public Transform NPC1Point;
    public Transform NPC2Point;
    public Transform NPC3Point;
    public Transform NPC4Point;


    public GameObject shopper;
    public GameObject questNPC;
    public GameObject NPCNor1;
    public GameObject Fisher;

    public GameObject NPC1;
    public GameObject NPC2;
    public GameObject NPC3;
    public GameObject NPC4;

    public bool sellerSpawned = false;
    public bool questSpawned = false;
    public bool NPCNor1Spawned = false;
    public bool FisherSpawned = false;

    public bool NPC1Spawned = false;
    public bool NPC2Spawned = false;
    public bool NPC3Spawned = false;
    public bool NPC4Spawned = false;



    public bool SellerComingHome = false;
    public bool QuestComingHome = false;
    public bool NPCNor1ComingHome = false;
    public bool FisherComingHome = false;


    // Neu khoong di dem diem trong vaof X time thi NPC mua ban do se huy
    public float pathResetTimer_seller = 10f;
    public float currentTimer_seller = 0f;
    public bool isMoving_seller = false;
    // Neu khoong di dem diem trong vaof X time thi NPC giao nhiem vu se huy

    public float pathResetTimer_quest = 10f;
    public float currentTimer_quest = 0f;
    public bool isMoving_quest = false;

    public float pathResetTimer_NPCNor1 = 20f;
    public float currentTimer_NPCNor1 = 0f;
    public bool isMoving_NPCNor1 = false;

    public float pathResetTimer_Fisher = 10f;
    public float currentTimer_Fisher = 0f;
    public bool isMoving_Fisher = false;



    public SellerDailyTask sellerTask;

    
    private void Start()
    {
        //shopper.SetActive(false);
        // questNPC.SetActive(false);
     
    }

    void Update()
    {
        if (!sellerSpawned && TimeManager.Instance.timestamp.hour >= 6
               && TimeManager.Instance.timestamp.hour < 18 && TimeManager.Instance.timestamp.day != sellerTask.SpecialDay) // khonng di ban vao ngay dac biet
        {
            SpawnSeller();
            NavMeshAgent nav = shopper.GetComponent<NavMeshAgent>();
            float distance = Vector3.Distance(shopper.transform.position, Sellpoint.position);
            if (distance < 1.5f)
            {

                SellerBehavior behavior = shopper.GetComponent<SellerBehavior>();
                behavior.SwitchState(SellerBehavior.NPCState.Idle);
                sellerSpawned = true;
                SellerComingHome = false;
                nav.isStopped = true;
                nav.ResetPath();

  
            }

        }

        if (!questSpawned && TimeManager.Instance.timestamp.hour >= 6 
            && TimeManager.Instance.timestamp.hour < 18)
        {
            SpawnQuest();

            NavMeshAgent nav = questNPC.GetComponent<NavMeshAgent>();
            Animator animator = questNPC.GetComponent<Animator>();
            animator.SetBool("isDoneWalking", false);
            float distance = Vector3.Distance(questNPC.transform.position, QuestPoint.position);
            QuestBehavior questBehavior = questNPC.GetComponent<QuestBehavior>();
            //float distanceToDestination_ = nav.remainingDistance - nav.stoppingDistance;

            if (distance < 1.5f)
            {
                questBehavior.SwitchStateAction(QuestBehavior.NPCAction.DoNothing);

                 questSpawned = true;

                nav.isStopped = true;
                nav.ResetPath();
                //Debug.Log("here");
            }

        }

        if (TimeManager.Instance.timestamp.hour == 5 && TimeManager.Instance.timestamp.minute == 10)
        {
            RandomPosition = GetRandomPosition(CenterPoint.transform.position, 20f);
        }

        if (!NPCNor1Spawned && TimeManager.Instance.timestamp.hour >= 8 
          && TimeManager.Instance.timestamp.hour < 18)
        {
            SpawnNPCNor1();
            
            NavMeshAgent nav = NPCNor1.GetComponent<NavMeshAgent>();
            Animator animator = NPCNor1.GetComponent<Animator>();
            NPCBehavior behavior = NPCNor1.GetComponent<NPCBehavior>();
            if (nav.isOnNavMesh)
            {
                animator.SetBool("isDoneWalking", false);
                float distance = Vector3.Distance(NPCNor1.transform.position, Nor1Point.transform.position);
                nav.SetDestination(Nor1Point.transform.position);
                //float distanceToDestination_ = nav.remainingDistance - nav.stoppingDistance;

                if (distance < 1.5f)
                {
                    behavior.SwitchStateAction(NPCBehavior.NPCAction.DoNothing);
                    //Debug.Log("here" + distance);
                    NPCNor1Spawned = true;

                    nav.isStopped = true;
                    nav.ResetPath();
                    //Debug.Log("here");
                }
            }
           

        }

        if (!FisherSpawned && TimeManager.Instance.timestamp.hour >= 8
          && TimeManager.Instance.timestamp.hour < 18)
        {
            SpawnFisher();
          
            NavMeshAgent nav = Fisher.GetComponent<NavMeshAgent>();
            Animator animator = Fisher.GetComponent<Animator>();
            FisherBehavior behavior = Fisher.GetComponent<FisherBehavior>();

            if (nav.isOnNavMesh)
            {
                animator.SetBool("isDoneWalking", false);
                float distance = Vector3.Distance(Fisher.transform.position, Destination_Fisher.position);
                nav.SetDestination(Destination_Fisher.position);
                //float distanceToDestination_ = nav.remainingDistance - nav.stoppingDistance;

                if (distance < 1.5f)
                {
                    animator.SetBool("isDoneWalking", true);
                    behavior.SwitchStateAction(FisherBehavior.NPCAction.Fishing);
                    FisherSpawned = true;

                    nav.isStopped = true;
                    nav.ResetPath();
                    //Debug.Log("here");
                }
            }


        }


        if (!NPC1Spawned&& TimeManager.Instance.timestamp.hour >= 8
          && TimeManager.Instance.timestamp.hour < 18)
        {
            SpawnNPC1();

            NavMeshAgent nav = NPC1.GetComponent<NavMeshAgent>();
            GenNPCBehavior behavior = NPC1.GetComponent<GenNPCBehavior>();

            if (nav.isOnNavMesh)
            {
              
                float distance = Vector3.Distance(NPC1.transform.position, NPC1Point.position);
                nav.SetDestination(NPC1Point.position);
                //float distanceToDestination_ = nav.remainingDistance - nav.stoppingDistance;

                if (distance < 1.5f)
                {
                    behavior.SwitchStateAction(GenNPCBehavior.NPCAction.DoNothing);
                    NPC1Spawned = true;

                    
                    //Debug.Log("here");
                }
            }


        }

        if (!NPC2Spawned && TimeManager.Instance.timestamp.hour >= 7 && TimeManager.Instance.timestamp.minute >= 10
          && TimeManager.Instance.timestamp.hour < 18)
        {
            SpawnNPC2();

            NavMeshAgent nav = NPC2.GetComponent<NavMeshAgent>();
            GenNPCBehavior behavior = NPC2.GetComponent<GenNPCBehavior>();

            if (nav.isOnNavMesh)
            {

                float distance = Vector3.Distance(NPC2.transform.position, NPC2Point.position);
                nav.SetDestination(NPC2Point.position);
                //float distanceToDestination_ = nav.remainingDistance - nav.stoppingDistance;

                if (distance < 1.5f)
                {
                    behavior.SwitchStateAction(GenNPCBehavior.NPCAction.DoNothing);
                    NPC2Spawned = true;

                   
                    //Debug.Log("here");
                }
            }


        }

        if (!NPC3Spawned && TimeManager.Instance.timestamp.hour >= 7 && TimeManager.Instance.timestamp.minute >= 22
         && TimeManager.Instance.timestamp.hour < 18)
        {
            SpawnNPC3();

            NavMeshAgent nav = NPC3.GetComponent<NavMeshAgent>();
            GenNPCBehavior behavior = NPC3.GetComponent<GenNPCBehavior>();
         
        
            if (nav.isOnNavMesh)
            {

                float distance = Vector3.Distance(NPC3.transform.position, NPC3Point.position);
                nav.SetDestination(NPC3Point.position);
                if (nav.isStopped == true)
                {
                    nav.isStopped = false;
                }
                if (behavior.state != GenNPCBehavior.NPCState.Walking)
                {
                    behavior.state = GenNPCBehavior.NPCState.Walking;
                }

               
                if(distance <= 1.5)
                {
                    behavior.SwitchStateAction(GenNPCBehavior.NPCAction.DoNothing);
                    NPC3Spawned = true;
                }
             

               
            }
         

        }
        if (!NPC4Spawned && TimeManager.Instance.timestamp.hour >= 8 && TimeManager.Instance.timestamp.minute >= 20
         && TimeManager.Instance.timestamp.hour < 18)
        {
            SpawnNPC4();

            NavMeshAgent nav = NPC4.GetComponent<NavMeshAgent>();
            GenNPCBehavior behavior = NPC4.GetComponent<GenNPCBehavior>();

            if (nav.isOnNavMesh)
            {

                float distance = Vector3.Distance(NPC4.transform.position, NPC4Point.position);
                nav.SetDestination(NPC4Point.position);
                //float distanceToDestination_ = nav.remainingDistance - nav.stoppingDistance;

                if (distance < 1.5f)
                {
                    behavior.SwitchStateAction(GenNPCBehavior.NPCAction.DoNothing);
                    NPC4Spawned = true;

                  
                    //Debug.Log("here");
                }
            }


        }

        if (TimeManager.Instance.timestamp.hour >= 0 && TimeManager.Instance.timestamp.hour <= 5)
        {
            NPC1Spawned = false;
            NPC2Spawned = false;
            NPC3Spawned = false;
            NPC4Spawned = false; 

        }

        //--------------------------

        if (isMoving_NPCNor1) // check tgian di chuyen qua lau_Seller
        {
            currentTimer_NPCNor1 += Time.deltaTime;


            if (currentTimer_NPCNor1 >= pathResetTimer_NPCNor1)
            {
                if (NPCNor1.activeSelf != false)
                {
                    NavMeshAgent nav = NPCNor1.GetComponent<NavMeshAgent>();
                    NPCBehavior behav = NPCNor1.GetComponent<NPCBehavior>();
                    Animator animator = NPCNor1.GetComponent<Animator>();
                    if (nav.isOnNavMesh)
                    {
                        nav.isStopped = true;
                        nav.ResetPath();
                    }
                    behav.SwitchState(NPCBehavior.NPCState.Idle);
                }


                isMoving_NPCNor1 = false;
            }
        }
        
        if (isMoving_seller) // check tgian di chuyen qua lau_Seller
        {
            currentTimer_seller += Time.deltaTime;


            if (currentTimer_seller >= pathResetTimer_seller)
            {
                if (shopper.activeSelf != false)
                {
                    NavMeshAgent nav = shopper.GetComponent<NavMeshAgent>();
                    Animator animator = shopper.GetComponent<Animator>();
                    if (nav.isOnNavMesh)
                    {
                        nav.isStopped = true;
                        nav.ResetPath();
                    }
                    animator.Play("idle");
                }
              

                isMoving_seller = false;
            }
        }


        if (isMoving_quest)// check tgian di chuyen qua lau_Quest
        {
            currentTimer_quest += Time.deltaTime;


            if (currentTimer_quest >= pathResetTimer_quest)
            {
                if (questNPC.activeSelf != false)
                {
     
                    NavMeshAgent nav = shopper.GetComponent<NavMeshAgent>();
                    Animator animator = shopper.GetComponent<Animator>();
                    if(nav.isOnNavMesh)
                    {
                        nav.isStopped = true;
                        nav.ResetPath();
                    }
             
                    animator.Play("idle");
                }


                isMoving_quest = false;
            }


        }

        if (isMoving_Fisher)// check tgian di chuyen qua lau_Quest
        {
            currentTimer_Fisher += Time.deltaTime;


            if (currentTimer_Fisher >= pathResetTimer_Fisher)
            {
                if (Fisher.activeSelf != false)
                {

                    NavMeshAgent nav = shopper.GetComponent<NavMeshAgent>();
                    Animator animator = shopper.GetComponent<Animator>();
                    if (nav.isOnNavMesh)
                    {
                        nav.isStopped = true;
                        nav.ResetPath();
                    }

                    animator.Play("idle");
                }


                isMoving_Fisher = false;
            }


        }
        /*

            if (!dancingLaydy && TimeManager.Instance.timestamp.hour == 15
               )
            {
                DancingLady();
            }



       
        
            else if (sellerSpawned && TimeManager.Instance.timestamp.hour >= 18 && shopper != null)
            {
                    MoveSellerBack();
                    NavMeshAgent nav = shopper.GetComponent<NavMeshAgent>();
                    Animator animator = shopper.GetComponent<Animator>();

                //  animator.SetBool("isDoneWalking", false);
                //float distance = Vector3.Distance(shopper.transform.position, Spawnpoint.position);
                    float distanceToDestination = nav.remainingDistance - nav.stoppingDistance;
                //Debug.Log(distance + "spawnsler");
                if (distanceToDestination < 1f)
                    {
                        // animator.SetBool("isDoneWalking", true);
                        NPCInteract shoperInteract = shopper.GetComponent<NPCInteract>();
                        shoperInteract.SwitchState(NPCInteract.SellerState.Idle);
                        sellerSpawned = false;
                        buyorNot.TalkBackToQuest = false;
                        nav.isStopped = true;
                        nav.ResetPath();
                        shopper.SetActive(false);

                        //Debug.Log("here");
                    }
            }

          
        /*
                   else if (questSpawned && TimeManager.Instance.timestamp.hour >= 18 && questNPC != null)
                   {

                       MoveQuestNPCBack();
                       NavMeshAgent nav = questNPC.GetComponent<NavMeshAgent>();
                       Animator animator = questNPC.GetComponent<Animator>();
                       float distance = Vector3.Distance(questNPC.transform.position, Spawnpoint.position);
                       animator.SetBool("isDoneWalking", false);
                       //float distanceToDestination = nav.remainingDistance - nav.stoppingDistance;
                        //Debug.Log(distance + "spawnsler");
                       if (distance < 1f)
                           {

                               animator.SetBool("isDoneWalking", true);
                               questSpawned = false;
                               nav.isStopped = true;
                               nav.ResetPath();
                               questNPC.SetActive(false);
                               //Debug.Log("here");
                           }
                   }

               if (!talking && TimeManager.Instance.timestamp.hour >= 17f && TimeManager.Instance.timestamp.hour < 18 && questNPC != null && shopper != null)
                   {
                       TalkingToShoper();
                       NavMeshAgent nav_quest = questNPC.GetComponent<NavMeshAgent>();
                       Animator animator_quest = questNPC.GetComponent<Animator>();
                       //Animator animator = shopper.GetComponent<Animator>();
                   // float distance = Vector3.Distance(questNPC.transform.position, shopper.transform.position);
                      float distanceToDestination = nav_quest.remainingDistance - nav_quest.stoppingDistance;
                      animator_quest.SetBool("isDoneWalking", false);
                         Debug.Log("here");
                       //Debug.Log(distance + "spawnsler");
                       if (distanceToDestination < 2f)
                       {
                           Debug.Log("here222");
                           talking = true;

                           //animator.SetBool("isDoneTalking", false);

                           NPCInteract shoperInteract = shopper.GetComponent<NPCInteract>();
                           shoperInteract.SwitchState(NPCInteract.SellerState.Talking);

                           animator_quest.SetBool("isDoneWalking", true);
                           shopper.transform.LookAt(questNPC.transform.position);
                           buyorNot.TalkBackToQuest = true;
                           //    animator.Play("talking");
                           animator_quest.Play("talking");
                           //  questSpawned = false;

                           nav_quest.isStopped = true;
                           nav_quest.ResetPath();
                           //Debug.Log("here");
                       }
                   }
               */

    }


    public void SpawnSeller()
    {

        if(shopper.activeSelf == false)
        {
            shopper.SetActive(true);
           
        }
        NavMeshAgent nav = shopper.GetComponent<NavMeshAgent>();
        SellerBehavior behavior = shopper.GetComponent<SellerBehavior>();
        if(nav.isOnNavMesh)
        {
            behavior.SwitchState(SellerBehavior.NPCState.Walking);
            nav.ResetPath();

            // animator.Play("walking");
            // animator.SetBool("isDoneTalking", true);
            nav.SetDestination(Sellpoint.transform.position);
            isMoving_seller = true;
            currentTimer_seller = 0f; // Reset the timer
        }
        
      

     
    }

    /*
    public void MoveSellerBack()
    {
        NavMeshAgent nav = shopper.GetComponent<NavMeshAgent>();
        nav.ResetPath();
        nav.SetDestination(Spawnpoint.transform.position);
        Animator animator = shopper.GetComponent<Animator>();
        NPCBehavior behavior = shopper.GetComponent<NPCBehavior>();
        behavior.SwitchState(NPCBehavior.NPCState.Walking);
        // animator.SetBool("isDoneTalking", true);
        // animator.Play("walking");
        shopercominghome = true;
        dancingLaydy = false; // reset


    }
    */
    public void SpawnQuest() // chua co huy Task neu qua gan
    {
        if (questNPC.activeSelf == false)
        {
            questNPC.SetActive(true);
          

        }

        NavMeshAgent nav_ = questNPC.GetComponent<NavMeshAgent>();

        Animator animator = questNPC.GetComponent<Animator>();

        

        if(nav_.isOnNavMesh)
        {
            nav_.ResetPath();
            animator.Play("walking");
            animator.SetBool("isDoneTalking", true);
            nav_.SetDestination(QuestPoint.transform.position);
            isMoving_quest = true;
            currentTimer_quest = 0f; // Reset the timer
        }
       
    }

    public void SpawnNPCNor1()
    {
        if (NPCNor1.activeSelf == false)
        {
            NPCNor1.SetActive(true);


        }

        DailyTask task = NPCNor1.GetComponent<DailyTask>();
        task.GetNextTask(20);

        NavMeshAgent nav_ = NPCNor1.GetComponent<NavMeshAgent>();

        Animator animator = NPCNor1.GetComponent<Animator>();

        if(nav_.isOnNavMesh)
        {
            nav_.ResetPath();
            animator.Play("walking");
            animator.SetBool("isDoneTalking", true);

            isMoving_NPCNor1 = true;
            currentTimer_NPCNor1 = 0f; // Reset the timer
        }
   
    }

    public void SpawnFisher()
    {
        if (Fisher.activeSelf == false)
        {
            Fisher.SetActive(true);


        }
        // questNPC.transform.position = Spawnpoint.transform.position;
        // float range = 1f; // Define the range within which the quest NPC can move randomly

        NavMeshAgent nav_ = Fisher.GetComponent<NavMeshAgent>();

        Animator animator = Fisher.GetComponent<Animator>();
        if (nav_.isOnNavMesh)
        {
            nav_.ResetPath();
            animator.Play("walking");
            animator.SetBool("isDoneTalking", true);

            isMoving_Fisher = true;
            currentTimer_Fisher = 0f; // Reset the timer
        }

    }
    /*
    public void MoveQuestNPCBack()
    {
        NavMeshAgent nav = questNPC.GetComponent<NavMeshAgent>();
        nav.ResetPath();
        nav.SetDestination(Spawnpoint.transform.position);
        Animator animator = questNPC.GetComponent<Animator>();
        animator.Play("walking");
        animator.SetBool("isDoneTalking", true);

    }

    */
    public void TalkingToShoper()
    {
        NavMeshAgent nav = questNPC.GetComponent<NavMeshAgent>();
        if(nav != null)
        {
            nav.ResetPath();
            nav.SetDestination(shopper.transform.position);
            Animator animator = questNPC.GetComponent<Animator>();
            animator.Play("walking");

        }
       
    }
    /*
    public void DancingLady()
    {

        NPCInteract shoperInteract = shopper.GetComponent<NPCInteract>();
        shoperInteract.SwitchState(NPCInteract.SellerState.Dancing);
        dancingLaydy = true;

    }
    */
    public Vector3 GetRandomPosition(Vector3 center, float range)
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomPos = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPos, out hit, range, NavMesh.AllAreas) && hit.position.y <= 2)
            {
                //Debug.Log(hit.position);
                return hit.position;

            }
        }
      
        return center;
    }

    public void SpawnNPC1()
    {
        if (NPC1.activeSelf == false)
        {
            NPC1.SetActive(true);

        }
     
        NavMeshAgent nav = NPC1.GetComponent<NavMeshAgent>();
        GenNPCBehavior behavior = NPC1.GetComponent<GenNPCBehavior>();
        if (nav.isOnNavMesh)
        {
            behavior.SwitchState(GenNPCBehavior.NPCState.Walking);
         
            nav.SetDestination(NPC1Point.transform.position);
        }
    }
    public void SpawnNPC2()
    {
        if (NPC2.activeSelf == false)
        {
            NPC2.SetActive(true);

        }
    
        NavMeshAgent nav = NPC2.GetComponent<NavMeshAgent>();
        GenNPCBehavior behavior = NPC2.GetComponent<GenNPCBehavior>();
        if (nav.isOnNavMesh)
        {
            behavior.SwitchState(GenNPCBehavior.NPCState.Walking);
          
            nav.SetDestination(NPC2Point.transform.position);
        }
    }
    public void SpawnNPC3()
    {
        if (NPC3.activeSelf == false)
        {
            NPC3.SetActive(true);

        }
       
        NavMeshAgent nav = NPC3.GetComponent<NavMeshAgent>();
        GenNPCBehavior behavior = NPC3.GetComponent<GenNPCBehavior>();
        if (nav.isOnNavMesh)
        {
            behavior.SwitchState(GenNPCBehavior.NPCState.Walking);
           
            nav.SetDestination(NPC3Point.position);
        }
    }
    public void SpawnNPC4()
    {
        if (NPC4.activeSelf == false)
        {
            NPC4.SetActive(true);

        }
      
        NavMeshAgent nav = NPC4.GetComponent<NavMeshAgent>();
        GenNPCBehavior behavior = NPC4.GetComponent<GenNPCBehavior>();
        if (nav.isOnNavMesh)
        {
            behavior.SwitchState(GenNPCBehavior.NPCState.Walking);
            
            nav.SetDestination(NPC4Point.transform.position);
        }
    }
}
