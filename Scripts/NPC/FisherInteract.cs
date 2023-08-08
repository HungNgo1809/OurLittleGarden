using Photon.Realtime;
using System;
using System.Collections;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static DataManager;
using Random = UnityEngine.Random;


public class FisherInteract : MonoBehaviour
{
    // Start is called before the first frame update
    public DataManager dataManager;
    public Shoper shop;
    public NavMeshAgent nav;
    public Coroutine lookBackCoroutine;
    public bool TalkBackToQuest;
    public int numberOfFish = 0;
    public int prise = 40;
    public int finalPrise = 0;
    public string Item;
    public GameObject HaveFishPanel;
    public GameObject SuccessOrNoFishPanel;
    public Text SuccessOrNoFishPanel_Text;
    public Text HaveFishPanel_Text;
    public bool isMeet = false;
   
    PlayerControl player;
    private Animator animator;
    private FisherBehavior behavirInteract;
    private bool isInteracted = false;
    private FisherDailyTask fisherDailyTask;


    void Start()
    {

        animator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        behavirInteract = GetComponent<FisherBehavior>();
        numberOfFish = Random.Range(0, 5);
        finalPrise = numberOfFish * prise * Random.Range(0, 9);
    }

    // Update is called once per frame
    void Update()
    {
        if(TimeManager.Instance.timestamp.hour ==0 && isMeet == true)
        {
            isMeet = false;
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && !shop.FisherComingHome)
        {

            if (other.gameObject.GetComponentInChildren<playerFloatingUi>() != null)
            {
                other.gameObject.GetComponentInChildren<playerFloatingUi>().transform.GetChild(0).gameObject.SetActive(true);

            }
            if (Input.GetKey(KeyCode.E))
            {
                UiManager.Instance.FindClosestNPC(other.gameObject);
                if (UiManager.Instance.closestNPC.NPC == gameObject)
                {
                    player = other.GetComponent<PlayerControl>();
                    PlayerControl control = other.GetComponent<PlayerControl>();
                    control.NPCInteract_ = true;
                    if (shop.FisherSpawned == false)
                    {
                        shop.FisherSpawned = true;
                    }
                    if(behavirInteract.state != FisherBehavior.NPCState.Talking)
                    {
                        behavirInteract.SwitchStateAction(FisherBehavior.NPCAction.DoNothing);
                        behavirInteract.SwitchState(FisherBehavior.NPCState.Talking);
                    }
                   
                    gameObject.transform.LookAt(other.transform);
                 
                    if (lookBackCoroutine != null)
                        StopCoroutine(lookBackCoroutine);

                    //HaveFishPanel.SetActive(true);
                    RandomFish();
                    QuestManager.Instance.CheckQuestRequirementMeet(gameObject.name);
                    QuestManager.Instance.CheckSideQuestRequirementMeet(gameObject.name);
                    QuestManager.Instance.CheckSideQuest(gameObject);
                    isInteracted = true;
                }
               

                /*
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
                */

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
        }
        if (other.gameObject.GetComponentInChildren<playerFloatingUi>() != null)
        {
            other.gameObject.GetComponentInChildren<playerFloatingUi>().transform.GetChild(0).gameObject.SetActive(false);
        }
       
    }

    public void RandomFish()
    {
        if (isMeet == false)
        {
            numberOfFish = Random.Range(0, 5);
            finalPrise = numberOfFish * prise + Random.Range(0, 9);
            
            if (dataManager.SearchNPCAffectionByName("Liam") != null)
            {
                Affection npc = dataManager.SearchNPCAffectionByName("Liam");
                finalPrise = finalPrise - Mathf.RoundToInt((finalPrise * npc.level) / 30f);

            }
            isMeet = true;
        }
        if (isMeet == true)
        {

            if(numberOfFish == 0)
            {
                HaveFishPanel.SetActive(false);
                SuccessOrNoFishPanel_Text.text = " Greetings, traveler! Unfortunately, I don't have any fish available at the moment. It seems the catch was quite scarce today.";
                StartCoroutine(SlowOffGameObject(SuccessOrNoFishPanel));
                player.NPCInteract_ = false;
            }
            else
            {
               
                HaveFishPanel_Text.text = "Ah, greetings! I'm pleased to say that I have a fresh catch today. Take a look at these beauties! I have " + numberOfFish + " fishs available, priced at " + finalPrise + " coins.";

            }
        }
    }

    public void BuyFish()
    {
        
        bool isHavervestable = false;
        if (DisplayInventory.Instance.SearchNumberEmptyUiSlot(numberOfFish) == false)
        {
            // Khoohg cho nhặt vì túi đầy, thông báo
            UiManager.Instance.StartCoroutine(UiManager.Instance.SlowOffGameObject(UiManager.Instance.fullInvenPanel));
        }
        else
        {
            isHavervestable = true;

        }
        if (isHavervestable == true)
        {
            if (finalPrise <= dataManager.coins)
            {
                InventoryItem.ItemData obj =
                                           InventoryItem.Instance.SearchItemByID(Item);

                for (int i = 0; i < numberOfFish; i++)
                {

                    dataManager.AddItemToInventoryData(obj.prefabItemID, obj.type, obj.speType, obj.durability, int.Parse(DisplayInventory.Instance.SearchFirstEmptyUiSlot().name), obj.money);
                    DisplayInventory.Instance.UpdateUI();
                }
                if (dataManager.SearchNPCAffectionByName("Liam") != null)
                {
                    Affection npc = dataManager.SearchNPCAffectionByName("Liam");
                    npc.amountOfTrade += finalPrise;

                    dataManager.IncreaseAmount(npc);
                }
                dataManager.coins -= finalPrise;
                numberOfFish = 0;
                SuccessOrNoFishPanel_Text.text = "Here you go. Enjoy your purchase!";
                StartCoroutine(SlowOffGameObject(SuccessOrNoFishPanel));
            }
            else if (finalPrise >= dataManager.coins)
            {
                // hiển thị mua thất bại : hien thi thieu tien
                UiManager.Instance.StartCoroutine(UiManager.Instance.SlowOffGameObject(UiManager.Instance.lackOfMoneyPanel));
            }
       


        }
        player.NPCInteract_ = false;
    }

    IEnumerator Lookback()
    {
        yield return new WaitForSeconds(5f);
        float distance = Vector3.Distance(gameObject.transform.position, shop.Destination_Fisher.position);
        
        if(distance > 1.5f)
        {
            shop.FisherSpawned = false;
            StopAllCoroutines();
            fisherDailyTask.DoCurrentTask();
        }
        else
        {
            gameObject.transform.LookAt(shop.Destination_Fisher.transform.position);
            behavirInteract.SwitchStateAction(FisherBehavior.NPCAction.Fishing);
            /*
            if (!TalkBackToQuest)
            {
                gameObject.transform.LookAt(shop.Destination_Fisher.transform.position);
                behavirInteract.SwitchState(FisherBehavior.NPCState.Idle);
            }
            else if (TalkBackToQuest)
            {
                gameObject.transform.LookAt(shop.Destination_Fisher.transform.position);
                behavirInteract.SwitchState(FisherBehavior.NPCState.Talking);
                animator.Play("talking");
            }
            */
        }
      

    }

    public IEnumerator SlowOffGameObject(GameObject obj)
    {
        obj.SetActive(true);

        yield return new WaitForSeconds(1f);
        obj.SetActive(false);
    }

    
}
