using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static DataManager;
using static InventoryItem;
using static Lands;

public class PBRInteract : MonoBehaviour
{
    public DataManager dataManager;
    public List<GameObject> terList; // List of ter objects
    public float wateringTime = 2f; // Adjust the duration of the watering animation
    public GameObject talkPanel;
    public List<CropData> plantsToWater;
    public Shoper shop;
    public Coroutine lookBackCoroutine;
    public bool TalkBackToQuest;
    public InventoryItem inventoryItem;
    public DailyTask dailyTask;
    public bool isOnWorkingTimes = false;
    public Text requiremnt_Text;
    public GameObject lackOfObjectRequirement;
    public GameObject allWatered;
    public ItemData ObjectRequire = null;
    public GameObject watering;
    public ParticleSystem watering_PS;
 
    public GameObject Icon;
    
    public Canvas can;

    private NavMeshAgent nav;
    private Animator animator;
    private int currentPlantIndex;
    private bool isWatering;
    private NPCBehavior behavior;
    public bool isIntereacted = false;
    private void Start()
    {
        can.worldCamera = Camera.main;
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        behavior = GetComponent<NPCBehavior>();
        int random = Random.Range(0, inventoryItem.items.Count);
        ObjectRequire = inventoryItem.items[random];
    }

    public void TakeRequireItems()
    {

        requiremnt_Text.text = " If you need help with watering, I'll be glad to lend a hand. Just make sure you have " + ObjectRequire.speType + " to offer in return. " ;

    }

    public void StartWateringPlants() // phai check xem it nhat co hon 1 cai chua duoc tuoi nuoc -> neu khong co thi se phai hien 1 panel khong co
    {
       
        if(dataManager.SearchItemInDataByPrefabId(ObjectRequire.prefabItemID) != null)
        {
            if (isWatering) return;

            plantsToWater = new List<CropData>();

            // Find plants that need watering
            foreach (CropData cropData in dataManager.cropData)
            {
                if (!cropData.isHaveWater && cropData.curMode != 2)
                {
                    plantsToWater.Add(cropData);
                }
            }

            // Start watering plants
            if (plantsToWater.Count > 0)
            {
                Debug.Log("hee");
                behavior.SwitchStateAction(NPCBehavior.NPCAction.DoNothing);
                dailyTask.GetNextTask(60); // tam thoi < 1 tieng thi huy? task day de uu tien nhiem vu tuoi cay
                currentPlantIndex = 0;
                isWatering = true;
                StartCoroutine(WaterPlantsCoroutine());
                isOnWorkingTimes = true;
                shop.isMoving_NPCNor1 = false;
                //  ----> Xoa do trong inventory
            }
            else
            {
                // tat ca deu da duoc tuoi nuoc -> panel nua o day
                StartCoroutine(SlowOffGameObject(allWatered)) ;
            }
        }
        else
        {
            StartCoroutine(SlowOffGameObject(lackOfObjectRequirement));

        }

      
    }

    private IEnumerator WaterPlantsCoroutine()
    {
      
        while (currentPlantIndex < plantsToWater.Count)
        {
            CropData currentPlant = plantsToWater[currentPlantIndex];
            GameObject targetTer = FindTerObject(currentPlant.terObjectID);
           // Debug.Log(targetTer + " Targetpositon");
            //Debug.Log("currentPlantIndex" + currentPlantIndex + "-00-" + "plantsToWater" + plantsToWater.Count);
            if (targetTer != null)
            {
                Lands lands = targetTer.GetComponent<Lands>();
                Vector3 targetPosition = targetTer.transform.position;

                nav.isStopped = false;
                float distance_ = Vector3.Distance(gameObject.transform.position, targetPosition);
                if(distance_ > 2.3f )
                {
               
                    behavior.SwitchState(NPCBehavior.NPCState.Walking);
                    nav.SetDestination(targetPosition);
                    Debug.Log(targetPosition);
                    if(distance_ < 3.5f)
                    {
                        nav.speed = 1f;
                    }
                    while (distance_ > 2.3f)
                    {
                       
                        float distance = Vector3.Distance(gameObject.transform.position, targetPosition);
                        distance_ = distance;
                        yield return null;
                    }
                    nav.isStopped = true;
                    behavior.SwitchState(NPCBehavior.NPCState.Watering);
                    
                    yield return new WaitForSeconds(1f);
                    //gameObject.transform.LookAt(targetPosition);

                  
                  
                }
                else
                {
                 
                    yield return new WaitForSeconds(0.5f);
                    gameObject.transform.LookAt(targetPosition);
                 
                    behavior.SwitchStateAction(NPCBehavior.NPCAction.Watering);
                  

                }

                if (lands != null)
                {
                    watering.SetActive(true);
                    watering_PS.Play();
                    lands.SwitchState(LandStatus.Watered);
                
                }
                // Wait for the watering animation to finish
                yield return new WaitForSeconds(wateringTime);

                // Increment the current plant index to move to the next plant
                currentPlantIndex++;
                nav.speed = 1f; 
                // Delay before moving to the next plant (optional)
                yield return new WaitForSeconds(2f);
                watering.SetActive(false);
            }
            else
            {
               
                break;
            }
           
        }
        Debug.LogWarning("Done all" );
        // Finish watering plants
        nav.speed = 2f;
        // dang test nen de tam sau nay thanh random
        int random = Random.Range(0, inventoryItem.items.Count);
        ObjectRequire = inventoryItem.items[random];
        isWatering = false;
        plantsToWater.Clear();
        isOnWorkingTimes = false;
        dailyTask.DoCurrentTask();
    }

    void OnAnimatorIK(int layerIndex)
    {
        Animator animator = behavior.animator;

        if (!animator.GetBool("isDoneWatering") == false)
        {

            animator.SetBool("isDoneWalking", true);
            animator.SetBool("isDoneWatering", true);
        }
    }

    private GameObject FindTerObject(string terObjectID)
    {

        return ListObjectManager.Instance.TerObject.FirstOrDefault(terObject => terObject.name == terObjectID);
    }


    void Update()
    {

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && !shop.NPCNor1ComingHome)
        {

            RotateTowardsTarget();
            if (other.gameObject.GetComponentInChildren<playerFloatingUi>() != null)
            {
                other.gameObject.GetComponentInChildren<playerFloatingUi>().transform.GetChild(0).gameObject.SetActive(true);

            }
            if (Input.GetKey(KeyCode.E))
            {

                UiManager.Instance.FindClosestNPC(other.gameObject);
                if(UiManager.Instance.closestNPC.NPC == gameObject)
                {
                    PlayerControl control = other.GetComponent<PlayerControl>();
                    control.NPCInteract_ = true;
                    Animator ani = other.GetComponent<Animator>();
                    ani.SetFloat("Speed", 0);
                    ani.SetBool("Run", false);
                    if (shop.NPCNor1Spawned == false)
                    {
                        shop.NPCNor1Spawned = true;
                    }
                    behavior.StopAllCoroutines();
                    QuestManager.Instance.CheckQuestRequirementMeet(gameObject.name);
                    QuestManager.Instance.CheckSideQuestRequirementMeet(gameObject.name);
                    QuestManager.Instance.CheckSideQuest(gameObject);
                    if(behavior.state != NPCBehavior.NPCState.Talking)
                    {
                        behavior.SwitchStateAction(NPCBehavior.NPCAction.DoNothing);
                        behavior.SwitchState(NPCBehavior.NPCState.Talking);
                    }
                   
                    gameObject.transform.LookAt(other.gameObject.transform);
                 
                    if (lookBackCoroutine != null)
                        StopCoroutine(lookBackCoroutine);
                    isIntereacted = true;
                }

                //talkPanel.SetActive(true);
                // panel - bat hien tai dang khoa' player vao r --> 2 panel 2 button -> 1 panal ve lua chon 2 button -> 1 panel neu dong y se hien  la trao doi? cong lao = 1 list Do
                // neu nhan tuoi cay thi se phai? huy next task de dam bao thoi gian - hoac them 1 bool vao task de kiem tra neu khong lam nv thi moi thuc hien task



                // sau khi xong -> xoa ObjectRequire


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
        if (other.tag == "Player" && shop.NPCNor1ComingHome == false && isIntereacted == true)
        {
            animator.SetBool("isDoneTalking", true);
            if(!isOnWorkingTimes)
            {
                StartCoroutine(Lookback());
                lookBackCoroutine = StartCoroutine(Lookback());

            }
        
            isIntereacted = false;
          
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



    IEnumerator Lookback()
    {
        yield return new WaitForSeconds(5f);
        float distance = Vector3.Distance(gameObject.transform.position, shop.Nor1Point.position);
        if(distance < 1.5f)
        {
            gameObject.transform.LookAt(shop.Nor1Point.transform.position);
            behavior.SwitchState(NPCBehavior.NPCState.Idle);
        }
        else
        {
            if(behavior.NPCTalkWith != null)
            {
                float distance_ = Vector3.Distance(gameObject.transform.position, behavior.NPCTalkWith.transform.position);
                if (distance_ < 1.5f)
                {
                    behavior.SwitchStateAction(NPCBehavior.NPCAction.TalkWithNPC);
                }
                else
                {
                    if (behavior.isGoingHome == true)
                    {
                        StartCoroutine(behavior.GoingHomePoint());

                    }
                    else if (behavior.isGoingSomeWhere == true)
                    {
                        StartCoroutine(behavior.GoingSomeWhereNPC());

                    }
                    else if (behavior.isMeetSeller == true)
                    {
                        StartCoroutine(behavior.MeetingSeller());
                    }
                    else if (behavior.isMeeting)
                    {
                        StartCoroutine(behavior.MeetEmma());
                    }
                    else if (behavior.isMeetNPC)
                    {
                        StartCoroutine(behavior.MeetNPC());
                    }
                    else
                    {
                        dailyTask.DoCurrentTask();
                    }

                }
            }
            else
            {
                if (behavior.isGoingHome == true)
                {
                    StartCoroutine(behavior.GoingHomePoint());

                }
                else if (behavior.isGoingSomeWhere == true)
                {
                    StartCoroutine(behavior.GoingSomeWhereNPC());

                }
                else if (behavior.isMeetSeller == true)
                {
                    StartCoroutine(behavior.MeetingSeller());
                }
                else if (behavior.isMeeting)
                {
                    StartCoroutine(behavior.MeetEmma());
                }
                else if (behavior.isMeetNPC)
                {
                    StartCoroutine(behavior.MeetNPC());
                }
                else
                {
                    dailyTask.DoCurrentTask();
                }
            }
          
        }

    }

    public IEnumerator SlowOffGameObject(GameObject obj)
    {
        obj.SetActive(true);

        yield return new WaitForSeconds(1f);
        obj.SetActive(false);
    }

    public void RotateTowardsTarget()
    {

        // Get the direction to the main camera
        can.transform.LookAt(can.transform.position + Camera.main.transform.rotation * -Vector3.back, Camera.main.transform.rotation * Vector3.up);

    }
}

