using Photon.Pun.Simple;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static DataManager;
using static NPCBehavior;
using static QuestManager.Quest;

[System.Serializable]
public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; set; }
    [SerializeField]
    public List<Quest> quests = new List<Quest>();
    [SerializeField]
    public List<SideQuest> sideQuests = new List<SideQuest>();
    public List<GameObject> buttonTakeQuest = new List<GameObject>();
    public List<GameObject> buttonCheckQuest = new List<GameObject>();
    public GameObject questChat;
    public GameObject QuestDescription;
    public GameObject QuestDetail;
    public Transform QuestDetails_viewport; 
    public PlayerControl player;
    public GameObject areYouDoneQuestChat;
    public GameObject noMoreQuestToDo;
    public GameObject youDoneTheQuest;
    public GameObject notDoneYet;
    public Text UI_Descript;
    public Text UI_DescriptID;
    public Text questDescription_text;
    public DataManager dataManager;
    public CraftManager craftManager;

    public Quest currentQuest;

    public SideQuest currentSideQuest;
    public GameObject UI_ButtonMainQuest;
    public Transform UI_ViewportMainQuest;
    public Transform UI_ViewportSideQuest;
    public Text guideText;
    public GameObject questGuide;

    public GameObject turtorial;

    public GameObject questNPC;

    private void Awake()
    {
       
        if (Instance == null)
        {
            Instance = this;
           
        }
        else
        {
            Destroy(this.gameObject);
        }

    }
    
    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(gameObject);
        currentQuest = null;
        currentSideQuest = null;
        LoadCurrentQuestData();
        LoadCurrentSideQuestData();
        if(dataManager.affections.Count == 0)
        {
            DataManager.Affection affection1 = new DataManager.Affection();
            affection1.NPCName = "Isabella";
            dataManager.affections.Add(affection1);

            DataManager.Affection affection2 = new DataManager.Affection();
            affection2.NPCName = "Liam";
            dataManager.affections.Add(affection2);
        }
        if(dataManager.isOldbie == 0)
        {
            turtorial.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    #region MainQuest
    public enum QuestState
    {
        NotAvailable,
        Available,
        Accept,
        Complete
    }

    public QuestState state;


    public void SwitchState(QuestState questState)
    {

        switch (questState)
        {
            case QuestState.NotAvailable:
                // kieu dang khong nhan duoc nhiem vu nua - co the 1 panel nch - Panel khong the nhan nhiem vu

                break;
            case QuestState.Available:
                // panel rieng - panel co the nhan nhiem vu - Panel co the nhan nhiem vu
             
                break;

            case QuestState.Accept:
                // Panel tiep theo cua Avvailable

                break;
            case QuestState.Complete:
                // Sau khi lam xong
                //Check inventory full thi khong duoc
                questChat.SetActive(false);
                areYouDoneQuestChat.SetActive(false);
                noMoreQuestToDo.SetActive(false);
                youDoneTheQuest.SetActive(true);
                StartCoroutine(SlowOffGameObject(youDoneTheQuest));
                if (currentQuest.QuestReward.Count > 0)
                {
                    foreach (var child in currentQuest.QuestReward)
                    {

                        InventoryItem.ItemData obj =
                          InventoryItem.Instance.SearchItemByID(child);

                        dataManager.AddItemToInventoryData(obj.prefabItemID, obj.type, obj.speType, obj.durability, int.Parse(DisplayInventory.Instance.SearchFirstEmptyUiSlot().name), obj.money);
                        DisplayInventory.Instance.UpdateUI();
                    }

                }

                Debug.Log(dataManager.coins);

                dataManager.coins += currentQuest.MoneyQuestReward;
                Debug.Log(dataManager.coins);
                dataManager.playerQuestIDData.currentID = currentQuest.QuestID;
                dataManager.playerQuestIDData.questHaveDone = currentQuest.QuestID;
                //currentQuest.QuestReward Add vao Inventory 

              
                currentQuest = null;

                dataManager.dataManagerMainQuest.Clear();
                foreach(var quest in quests)
                {
                    if(quest.QuestID <= dataManager.playerQuestIDData.questHaveDone)
                    {
                        dataManager.dataManagerMainQuest.Add(quest);
                    }
                }
                CheckCurrentQuest();

                Debug.Log("You done the job");
                break;

        }

        state = questState;
    }

    public void CheckCurrentQuest()
    {
        if (quests.FirstOrDefault(q => q.QuestID == dataManager.playerQuestIDData.questHaveDone + 1) != null)
        {

            Quest quest = quests.FirstOrDefault(q => q.QuestID == dataManager.playerQuestIDData.questHaveDone + 1);

            int time = GameTimestamp.CompareTimestampsWithoutAbs(quest.AvaiableTimeToTakeQuest, TimeManager.Instance.timestamp);
            if (time > 0)
            {
                //currentQuest = quest;
                SwitchState(QuestState.Available);

            }
            else
            {
                SwitchState(QuestState.NotAvailable);

            }


        }
        else
        {
            Debug.Log("No more Quest");
            SwitchState(QuestState.NotAvailable);
        }
        checkMark();

    } // trong button

    public void checkMark()
    {
        if (state == QuestState.Available)
        {

            questNPC.GetComponent<QuestNPCInteract>().Icon.SetActive(true);
        }
        else
        {
            questNPC.GetComponent<QuestNPCInteract>().can.gameObject.SetActive(false);
            questNPC.GetComponent<QuestNPCInteract>().Icon.SetActive(false);

        }
    }

    public void TakeCurrentQuest()
    {
        if (state == QuestState.Available)
        {
            currentQuest = quests.FirstOrDefault(q => q.QuestID == dataManager.playerQuestIDData.questHaveDone + 1);
        
            SwitchState(QuestState.Accept);
            questChat.SetActive(false);
            QuestDescription.SetActive(true);
            StartCoroutine(AppearTextOneByOne(questDescription_text,0.1f, currentQuest.QuestDescription));
            //\questDescription_text.text = currentQuest.QuestDescription;
            VerticalLayoutGroup verticalGroup = QuestDescription.GetComponent<VerticalLayoutGroup>();
            if (verticalGroup != null)
            {
                ContentSizeFitter contentSizeFitter = verticalGroup.GetComponent<ContentSizeFitter>();
                if (contentSizeFitter != null)
                {
                    contentSizeFitter.SetLayoutVertical(); // Trigger vertical layout update
                    contentSizeFitter.SetLayoutHorizontal(); // Trigger horizontal layout update
                }
            }
        }
        else
        {
            Debug.Log("No more Quest");
        }

    } // trong button

    public void CheckRequirement() // chekc if done the quest
    {
        if (currentQuest != null && currentQuest.Requirements.Count > 0)
        {

            bool isOkay = true;
            InventoryData inv = new InventoryData();
            foreach (var child in currentQuest.Requirements)
            {
                if (child.type == "Give") //thieu check number
                {
                    if (dataManager.inventoryData.FirstOrDefault(q => q.prefabItemID.Equals(child.Name )) != null)
                    {
                        InventoryData inv_ = dataManager.inventoryData.FirstOrDefault(q => q.prefabItemID.Equals(child.Name));
                        inv = inv_;
                        
                        Debug.Log("have");
                    }
                    else
                    {
                        isOkay = false;
                        Debug.Log(" Dont have");
                    }

                }
                else if (child.type == "Build")
                {
                    if (child.isBuildSomething == true)
                    {
                        Debug.Log("  build ");
                    }
                    else
                    {
                        isOkay = false;
                        Debug.Log(" not build yet");
                    }

                }
                else if (child.type == "Harvest")
                {
                    if (child.isHarvestSomething == true)
                    {
                        Debug.Log("  Harvest ");
                    }
                    else
                    {
                        isOkay = false;
                        Debug.Log(" not build yet");
                    }

                }
                else if (child.type == "Craft")
                {
                    if (child.isCrateSomething == true)
                    {
                        Debug.Log("  Craft ");
                    }
                    else
                    {
                        isOkay = false;
                        Debug.Log(" not craft yet");
                    }

                }
                else if (child.type == "Meet")
                {
                    if (child.isMeetNPC == true)
                    {
                        Debug.Log("  Meet ");
                    }
                    else
                    {
                        isOkay = false;
                        Debug.Log(" not craft yet");
                    }

                }
                else if (child.type == "Enter")
                {
                    if (child.isEnterGenWorld == true)
                    {
                        Debug.Log("  Enter ");
                    }
                    else
                    {
                        isOkay = false;
                        Debug.Log(" not craft yet");
                    }

                }
                else if (child.type == "Eat")
                {
                    if (child.isEatSomething == true)
                    {
                        Debug.Log("  Enter ");
                    }
                    else
                    {
                        isOkay = false;
                        Debug.Log(" not craft yet");
                    }

                }
                else if (child.type == "Sleep")
                {
                    if (child.isSleep == true)
                    {
                        Debug.Log("  Enter ");
                    }
                    else
                    {
                        isOkay = false;
                        Debug.Log(" not craft yet");
                    }

                }
                else if (child.type == "SpecialQuest")
                {
                    if (child.isDoneSpecialQuest == true)
                    {
                        Debug.Log("  Enter ");
                    }
                    else
                    {
                        isOkay = false;
                        Debug.Log(" not craft yet");
                    }

                }
                else
                {
                    isOkay = false;
                    Debug.Log(" DifferntType");
                }
            }
            if (DisplayInventory.Instance.SearchNumberEmptyUiSlot(currentQuest.QuestReward.Count) == false)
            {
                // Khoohg cho nhặt vì túi đầy, thông báo
                UiManager.Instance.StartCoroutine(UiManager.Instance.SlowOffGameObject(UiManager.Instance.fullInvenPanel));
            }
            else
            {
                if (isOkay)
                {
                    //Destroy trong inventory
                    Debug.Log("Can Done Quest");
                    SwitchState(QuestState.Complete);
                    dataManager.inventoryData.Remove(inv);
                    DisplayInventory.Instance.ClearAll();
                    DisplayInventory.Instance.UpdateUI();
                }
                else
                {
                    areYouDoneQuestChat.SetActive(false);
                    notDoneYet.SetActive(true);
                    StartCoroutine(SlowOffGameObject(notDoneYet));
                }
            }
           

        }


    }
    public void SaveCurrentQuestData()
    {
        if (currentQuest != null)
        {
            dataManager.SaveCurrentQuest(currentQuest.QuestID, currentQuest.Requirements);
            Debug.Log("here");
            Debug.Log(currentQuest.Requirements.Count);
        }
        else
        {
            dataManager.ClearCurrentQuest();
        }
        if (state == QuestState.NotAvailable)
        {
            dataManager.playerQuestIDData.questMode = 0;
        }
        else if (state == QuestState.Available)
        {
            dataManager.playerQuestIDData.questMode = 1;
        }
        else if (state == QuestState.Accept)
        {
            dataManager.playerQuestIDData.questMode = 2;
        }
        else if (state == QuestState.Complete)
        {
            dataManager.playerQuestIDData.questMode = 3;
        }
        dataManager.dataManagerMainQuest.Clear();
        foreach (var quest in quests)
        {
            if (quest.QuestID <= dataManager.playerQuestIDData.questHaveDone + 1)
            {
                dataManager.dataManagerMainQuest.Add(quest);
            }
        }
    }

    public void LoadCurrentQuestData()
    {
        int questID = dataManager.GetCurrentQuestID();
        if (questID != 0)
        {
            currentQuest = quests.FirstOrDefault(q => q.QuestID == questID);
            if (currentQuest != null)
            {
                List<Requirement> requirements = dataManager.GetCurrentQuestRequirements();
                currentQuest.Requirements = requirements;
                if (dataManager.playerQuestIDData.questMode == 0)
                {
                    state = QuestState.NotAvailable;
                }
                else if (dataManager.playerQuestIDData.questMode == 1)
                {
                    state = QuestState.Available;
                }
                else if (dataManager.playerQuestIDData.questMode == 2)
                {
                    state = QuestState.Accept;
                }
                else if (dataManager.playerQuestIDData.questMode == 3)
                {
                    state = QuestState.Complete;
                }
            }
        }

        foreach (var quest in quests)
        {

            foreach (var questData in dataManager.dataManagerMainQuest)
            {
                if (quest.QuestID == questData.QuestID)
                {
                      quest.Requirements = questData.Requirements;
                }
            }
        }

    }
    public IEnumerator SlowOffGameObject(GameObject obj)
    {


        yield return new WaitForSeconds(1f);
        obj.SetActive(false);
    }
    /*
    public void QuestDescriptionUI()
    {
        if(currentQuest != null && currentQuest.QuestID > 0)
        {
            UI_DescriptID.gameObject.SetActive(true);
            UI_Descript.gameObject.SetActive(true);

            UI_DescriptID.text = "Quest " + currentQuest.QuestID;
            UI_Descript.text = currentQuest.QuestDescription;
        }
    }

    public void QuestDetailCheck()
    {
        ClearAllChild();
        QuestDetails_viewport.gameObject.SetActive(true);
        if (currentQuest != null && currentQuest.Requirements.Count > 0)
        {
            foreach (var child in currentQuest.Requirements)
            {
                if (child.type == "Give") //thieu check number
                {
                    GameObject obj = Instantiate(QuestDetail, QuestDetails_viewport);
                 
                    if (dataManager.inventoryData.FirstOrDefault(q => q.prefabItemID.StartsWith(child.Name + "Harvest")) != null)
                    {
                        InventoryData inv = dataManager.inventoryData.FirstOrDefault(q => q.prefabItemID.StartsWith(child.Name + "Harvest"));
                        Text text = obj.transform.GetChild(0).GetComponent<Text>();
                        text.text = "Have " + child.Name + " inventory";
                        Toggle toggle = obj.GetComponentInChildren<Toggle>();
                        toggle.isOn = true;
                        toggle.enabled = false;
                        Debug.Log(text.text);
                    }
                    else
                    {
                        Text text = obj.transform.GetChild(0).GetComponent<Text>();
                        text.text = "Have " + child.Name + " inventory";
                        Debug.Log(text.text);
                        Toggle toggle = obj.GetComponentInChildren<Toggle>();
                        toggle.isOn = false;
                        toggle.enabled = false;
                    }
                }
                else if (child.type == "Build")
                {
                    GameObject obj = Instantiate(QuestDetail, QuestDetails_viewport);

                    if (child.isBuildSomething == true)
                    {
                        Text text = obj.transform.GetChild(0).GetComponent<Text>();
                        text.text = "Build " + child.Name ;
                        Toggle toggle = obj.GetComponentInChildren<Toggle>();
                        toggle.isOn = true;
                        toggle.enabled = false;
                    }
                    else
                    {
                        Text text = obj.transform.GetChild(0).GetComponent<Text>();
                        text.text = "Build " + child.Name;
                        Toggle toggle = obj.GetComponentInChildren<Toggle>();
                        toggle.isOn = false;
                        toggle.enabled = false;
                    }

                }
                else if (child.type == "Harvest")
                {
                    GameObject obj = Instantiate(QuestDetail, QuestDetails_viewport);
                    Debug.Log("asas");
                    if (child.isHarvestSomething == true)
                    {
                        Text text = obj.transform.GetChild(0).GetComponent<Text>();
                        string itemName = child.Name.Replace("Harvest", "");
                        text.text = "Harvest " + itemName;
                        Toggle toggle = obj.GetComponentInChildren<Toggle>();
                        toggle.isOn = true;
                        toggle.enabled = false;
                    }
                    else
                    {
                        Text text = obj.transform.GetChild(0).GetComponent<Text>();
                        string itemName = child.Name.Replace("Harvest", "");
                        text.text = "Harvest " + itemName;
                        Toggle toggle = obj.GetComponentInChildren<Toggle>();
                        toggle.isOn = false;
                        toggle.enabled = false;
                    }

                }
                else if (child.type == "Craft")
                {
                    GameObject obj = Instantiate(QuestDetail, QuestDetails_viewport);

                    if (child.isCrateSomething == true)
                    {
                        Text text = obj.transform.GetChild(0).GetComponent<Text>();
                        text.text = "Craft " + child.Name;
                        Toggle toggle = obj.GetComponentInChildren<Toggle>();
                        toggle.isOn = true;
                        toggle.enabled = false;
                    }
                    else
                    {
                        Text text = obj.transform.GetChild(0).GetComponent<Text>();
                        text.text = "Craft " + child.Name;
                        Toggle toggle = obj.GetComponentInChildren<Toggle>();
                        toggle.isOn = false;
                        toggle.enabled = false;
                    }

                }
                else if (child.type == "Meet")
                {
                    GameObject obj = Instantiate(QuestDetail, QuestDetails_viewport);

                    if (child.isMeetNPC == true)
                    {
                        Text text = obj.transform.GetChild(0).GetComponent<Text>();
                        text.text = "Meet  " + child.Name;
                        Toggle toggle = obj.GetComponentInChildren<Toggle>();
                        toggle.isOn = true;
                        toggle.enabled = false;
                    }
                    else
                    {
                        Text text = obj.transform.GetChild(0).GetComponent<Text>();
                        text.text = "Meet " + child.Name;
                        Toggle toggle = obj.GetComponentInChildren<Toggle>();
                        toggle.isOn = false;
                        toggle.enabled = false;
                    }

                }
                else if (child.type == "Enter")
                {
                    GameObject obj = Instantiate(QuestDetail, QuestDetails_viewport);

                    if (child.isEnterGenWorld == true)
                    {
                        Text text = obj.transform.GetChild(0).GetComponent<Text>();
                        text.text = "Enter  " + child.Name;
                        Toggle toggle = obj.GetComponentInChildren<Toggle>();
                        toggle.isOn = true;
                        toggle.enabled = false;
                    }
                    else
                    {
                        Text text = obj.transform.GetChild(0).GetComponent<Text>();
                        text.text = "Enter " + child.Name;
                        Toggle toggle = obj.GetComponentInChildren<Toggle>();
                        toggle.isOn = false;
                        toggle.enabled = false;
                    }

                }
                else
                {
                    Debug.Log(" DifferntType");
                }
            }
        }
    }

   */

    public void QuestDescriptionUI(Quest q)
    {
        UI_DescriptID.gameObject.SetActive(true);
        UI_Descript.gameObject.SetActive(true);
        UI_DescriptID.text = "Quest " + q.QuestID;
        UI_Descript.text = q.QuestDescription;
        guideText.text = guideText.text + "\n";
        guideText.text = q.Guide;
    
    }
    public void QuestDetailCheck(Quest q)
    {
        ClearAllChild();
        QuestDetails_viewport.gameObject.SetActive(true);
        foreach (var child in q.Requirements)
        {
            if (child.type == "Give") //thieu check number
            {
                GameObject obj = Instantiate(QuestDetail, QuestDetails_viewport);

                if (dataManager.inventoryData.FirstOrDefault(q => q.prefabItemID.Equals(child.Name)) != null)
                {
                    InventoryData inv = dataManager.inventoryData.FirstOrDefault(q => q.prefabItemID.Equals(child.Name));
                    Text text = obj.transform.GetChild(0).GetComponent<Text>();
                    text.text = "Have " + child.AdjustName + " inventory";
                    Toggle toggle = obj.GetComponentInChildren<Toggle>();
                    toggle.isOn = true;
                    toggle.enabled = false;
                    Debug.Log(text.text);
                }
                else
                {
                    Text text = obj.transform.GetChild(0).GetComponent<Text>();
                    text.text = "Have " + child.AdjustName + " inventory";
                    Debug.Log(text.text);
                    Toggle toggle = obj.GetComponentInChildren<Toggle>();
                    toggle.isOn = false;
                    toggle.enabled = false;
                }
            }
            else if (child.type == "Build")
            {
                GameObject obj = Instantiate(QuestDetail, QuestDetails_viewport);

                if (child.isBuildSomething == true)
                {
                    Text text = obj.transform.GetChild(0).GetComponent<Text>();
                    text.text = "Build " + child.AdjustName;
                    Toggle toggle = obj.GetComponentInChildren<Toggle>();
                    toggle.isOn = true;
                    toggle.enabled = false;
                }
                else
                {
                    Text text = obj.transform.GetChild(0).GetComponent<Text>();
                    text.text = "Build " + child.AdjustName;
                    Toggle toggle = obj.GetComponentInChildren<Toggle>();
                    toggle.isOn = false;
                    toggle.enabled = false;
                }

            }
            else if (child.type == "Harvest")
            {
                GameObject obj = Instantiate(QuestDetail, QuestDetails_viewport);
                Debug.Log("asas");
                if (child.isHarvestSomething == true)
                {
                    Text text = obj.transform.GetChild(0).GetComponent<Text>();
                    string itemName = child.Name.Replace("Harvest", "");
                    text.text = "Harvest " + itemName;
                    Toggle toggle = obj.GetComponentInChildren<Toggle>();
                    toggle.isOn = true;
                    toggle.enabled = false;
                }
                else
                {
                    Text text = obj.transform.GetChild(0).GetComponent<Text>();
                    string itemName = child.Name.Replace("Harvest", "");
                    text.text = "Harvest " + itemName;
                    Toggle toggle = obj.GetComponentInChildren<Toggle>();
                    toggle.isOn = false;
                    toggle.enabled = false;
                }

            }
            else if (child.type == "Craft")
            {
                GameObject obj = Instantiate(QuestDetail, QuestDetails_viewport);

                if (child.isCrateSomething == true)
                {
                    Text text = obj.transform.GetChild(0).GetComponent<Text>();
                    text.text = "Craft " + child.AdjustName;
                    Toggle toggle = obj.GetComponentInChildren<Toggle>();
                    toggle.isOn = true;
                    toggle.enabled = false;
                }
                else
                {
                    Text text = obj.transform.GetChild(0).GetComponent<Text>();
                    text.text = "Craft " + child.AdjustName;
                    Toggle toggle = obj.GetComponentInChildren<Toggle>();
                    toggle.isOn = false;
                    toggle.enabled = false;
                }

            }
            else if (child.type == "Meet")
            {
                GameObject obj = Instantiate(QuestDetail, QuestDetails_viewport);

                if (child.isMeetNPC == true)
                {
                    Text text = obj.transform.GetChild(0).GetComponent<Text>();
                    text.text = "Meet  " + child.Name;
                    Toggle toggle = obj.GetComponentInChildren<Toggle>();
                    toggle.isOn = true;
                    toggle.enabled = false;
                }
                else
                {
                    Text text = obj.transform.GetChild(0).GetComponent<Text>();
                    text.text = "Meet " + child.Name;
                    Toggle toggle = obj.GetComponentInChildren<Toggle>();
                    toggle.isOn = false;
                    toggle.enabled = false;
                }

            }
            else if (child.type == "Enter")
            {
                GameObject obj = Instantiate(QuestDetail, QuestDetails_viewport);

                if (child.isEnterGenWorld == true)
                {
                    Text text = obj.transform.GetChild(0).GetComponent<Text>();
                    text.text = "Enter  " + child.Name;
                    Toggle toggle = obj.GetComponentInChildren<Toggle>();
                    toggle.isOn = true;
                    toggle.enabled = false;
                }
                else
                {
                    Text text = obj.transform.GetChild(0).GetComponent<Text>();
                    text.text = "Enter " + child.Name;
                    Toggle toggle = obj.GetComponentInChildren<Toggle>();
                    toggle.isOn = false;
                    toggle.enabled = false;
                }

            }
            else if (child.type == "Eat")
            {
                GameObject obj = Instantiate(QuestDetail, QuestDetails_viewport);

                if (child.isEatSomething == true)
                {
                    Text text = obj.transform.GetChild(0).GetComponent<Text>();
                    text.text = "Eat  " + child.Name;
                    Toggle toggle = obj.GetComponentInChildren<Toggle>();
                    toggle.isOn = true;
                    toggle.enabled = false;
                }
                else
                {
                    Text text = obj.transform.GetChild(0).GetComponent<Text>();
                    text.text = "Eat " + child.Name;
                    Toggle toggle = obj.GetComponentInChildren<Toggle>();
                    toggle.isOn = false;
                    toggle.enabled = false;
                }

            }
            else if (child.type == "Sleep")
            {
                GameObject obj = Instantiate(QuestDetail, QuestDetails_viewport);

                if (child.isSleep == true)
                {
                    Text text = obj.transform.GetChild(0).GetComponent<Text>();
                    text.text = "Go to sleep ";
                    Toggle toggle = obj.GetComponentInChildren<Toggle>();
                    toggle.isOn = true;
                    toggle.enabled = false;
                }
                else
                {
                    Text text = obj.transform.GetChild(0).GetComponent<Text>();
                    text.text = "Go to sleep ";
                    Toggle toggle = obj.GetComponentInChildren<Toggle>();
                    toggle.isOn = false;
                    toggle.enabled = false;
                }

            }
            else if (child.type == "SpecialQuest")
            {
                GameObject obj = Instantiate(QuestDetail, QuestDetails_viewport);

                if (child.isDoneSpecialQuest == true)
                {
                    Text text = obj.transform.GetChild(0).GetComponent<Text>();
                    text.text = "Do Special Quest ";
                    Toggle toggle = obj.GetComponentInChildren<Toggle>();
                    toggle.isOn = true;
                    toggle.enabled = false;
                }
                else
                {
                    Text text = obj.transform.GetChild(0).GetComponent<Text>();
                    text.text = "Do Special Quest ";
                    Toggle toggle = obj.GetComponentInChildren<Toggle>();
                    toggle.isOn = false;
                    toggle.enabled = false;
                }

            }
            else
            {
                Debug.Log(" DifferntType");
            }
        }
    }

    public void CheckQuestRequirementHarvest(string buildObjectName)
    {
        if (currentQuest != null && currentQuest.Requirements.Count > 0)
        {
            foreach (var requirement in currentQuest.Requirements)
            {
                if (requirement.type == "Harvest")
                {
                    // Compare the built object's ID with the requirement's ID

                    if (buildObjectName.Equals(requirement.Name))
                    {
                        // The character has built the right thing
                        // Mark the requirement as completed or update its status
                        requirement.isHarvestSomething = true;

                        Debug.Log("Harvest the right thing!");
                    }
                }
            }
        }
    }

    public void CheckQuestRequirementBuild(string buildObjectName)
    {
        if (currentQuest != null && currentQuest.Requirements.Count > 0)
        {
            foreach (var requirement in currentQuest.Requirements)
            {
                if (requirement.type == "Build")
                {
                    // Compare the built object's ID with the requirement's ID

                    if (buildObjectName.Equals(requirement.Name))
                    {
                        // The character has built the right thing
                        // Mark the requirement as completed or update its status
                        requirement.isBuildSomething = true;

                        Debug.Log("Built the right thing!");
                    }
                }
            }
        }
    }

    public void CheckQuestRequirementCraft(string craftObjectName)
    {
        if (currentQuest != null && currentQuest.Requirements.Count > 0)
        {
            foreach (var requirement in currentQuest.Requirements)
            {
                if (requirement.type == "Craft")
                {
                    // Compare the built object's ID with the requirement's ID

                    if (craftObjectName.Equals(requirement.Name))
                    {
                        // The character has built the right thing
                        // Mark the requirement as completed or update its status
                        requirement.isCrateSomething = true;

                        Debug.Log("Craft the right thing!");
                    }
                }
            }
        }
    }

    public void CheckQuestRequirementMeet(string meetNPCName)
    {
        if (currentQuest != null && currentQuest.Requirements.Count > 0)
        {
            foreach (var requirement in currentQuest.Requirements)
            {
                if (requirement.type == "Meet")
                {

                    if (meetNPCName.Equals(requirement.Name))
                    {
                        requirement.isMeetNPC = true;
                    }
                }
            }
        }
    }

    public void CheckQuestRequirementEnterWorld()
    {
        if (currentQuest != null && currentQuest.Requirements.Count > 0)
        {
            foreach (var requirement in currentQuest.Requirements)
            {
                if (requirement.type == "Enter")
                {
                    requirement.isEnterGenWorld = true;
                }
            }
        }
    }

    public void CheckQuestRequirementEat(string meetNPCName)
    {
        if (currentQuest != null && currentQuest.Requirements.Count > 0)
        {
            foreach (var requirement in currentQuest.Requirements)
            {
                if (requirement.type == "Eat")
                {

                    if (meetNPCName.Equals(requirement.Name))
                    {
                        requirement.isEatSomething = true;
                    }
                }
            }
        }
    }
    public void CheckQuestRequirementSleep()
    {
        if (currentQuest != null && currentQuest.Requirements.Count > 0)
        {
            foreach (var requirement in currentQuest.Requirements)
            {
                if (requirement.type == "Sleep")
                {

                    requirement.isSleep = true;
                }
            }
        }
    }
    public void CheckQuestRequirementSpecialQuest()
    {
        if (currentQuest != null && currentQuest.Requirements.Count > 0)
        {
            foreach (var requirement in currentQuest.Requirements)
            {
                if (requirement.type == "SpecialQuest")
                {

                    requirement.isDoneSpecialQuest = true;
                }
            }
        }
    }

    #endregion

    #region SideQuest

    public enum SideQuestState
    {
        NotAvailable,
        Available,
        Accept,
        Complete
    }

    public SideQuestState sideQueststate;


    public void SwitchSideQuestState(SideQuestState questState)
    {

        switch (questState)
        {
            case SideQuestState.NotAvailable:
                // kieu dang khong nhan duoc nhiem vu nua - co the 1 panel nch - Panel khong the nhan nhiem vu

                break;
            case SideQuestState.Available:
                // panel rieng - panel co the nhan nhiem vu - Panel co the nhan nhiem vu

                break;

            case SideQuestState.Accept:
                // Panel tiep theo cua Avvailable

                break;
            case SideQuestState.Complete:
                // Sau khi lam xong
                //Check inventory full thi khong duoc
                youDoneTheQuest.SetActive(true);
                StartCoroutine(SlowOffGameObject(youDoneTheQuest));
                if (currentSideQuest.QuestReward.Count > 0)
                {
                    foreach (var child in currentSideQuest.QuestReward)
                    {

                        InventoryItem.ItemData obj =
                          InventoryItem.Instance.SearchItemByID(child);

                        dataManager.AddItemToInventoryData(obj.prefabItemID, obj.type, obj.speType, obj.durability, int.Parse(DisplayInventory.Instance.SearchFirstEmptyUiSlot().name), obj.money);
                        DisplayInventory.Instance.UpdateUI();
                    }

                }
                dataManager.coins += currentSideQuest.MoneyQuestReward;
                dataManager.playerSideQuestIDData.currentSideID = currentSideQuest.QuestID;
                dataManager.playerSideQuestIDData.sideQuestHaveDone = currentSideQuest.QuestID;
                //currentQuest.QuestReward Add vao Inventory 
                dataManager.dataManagerSideQuest.Clear();
                foreach (var quest in sideQuests)
                {
                    if (quest.QuestID <= dataManager.playerSideQuestIDData.sideQuestHaveDone)
                    {
                        dataManager.dataManagerSideQuest.Add(quest);
                    }
                }
                CheckCurrentSideQuest();
                currentSideQuest = null;
                Debug.Log("You done the job");
                break;

        }

        sideQueststate = questState;
    }
    public void CheckCurrentSideQuest()
    {
        if (sideQuests.FirstOrDefault(q => q.QuestID == dataManager.playerSideQuestIDData.sideQuestHaveDone + 1) != null)
        {

            SideQuest quest = sideQuests.FirstOrDefault(q => q.QuestID == dataManager.playerSideQuestIDData.sideQuestHaveDone + 1);

            int time = GameTimestamp.CompareTimestampsWithoutAbs(quest.AvaiableTimeToTakeQuest, TimeManager.Instance.timestamp);
            if (time > 0)
            {
                //currentQuest = quest;
                SwitchSideQuestState(SideQuestState.Available);

            }
            else
            {
                SwitchSideQuestState(SideQuestState.NotAvailable);

            }


        }
        else
        {
            Debug.Log("No more Quest");
            SwitchSideQuestState(SideQuestState.NotAvailable);
        }

    } // trong button
    public string TakeNPCSideQuestName()
    {
        SideQuest quest = sideQuests.FirstOrDefault(q => q.QuestID == dataManager.playerSideQuestIDData.sideQuestHaveDone + 1);
        return quest.NPCGive;
    }

    public void CheckSideQuest(GameObject NPCName) // Ban dau active button nhan nihem vu cua NPC do vaf sau khi nhan thi same spot se la cua check nhiem vu xong hay chua
    {
        foreach (var but in buttonCheckQuest)
        {
            if (but.activeSelf == true) 
            {
                but.SetActive(false);
            }
        }
        foreach (var but in buttonTakeQuest)
        {
            if (but.activeSelf == true)
            {
                but.SetActive(false);
            }
        }
        if (sideQueststate != SideQuestState.Accept)
        {
            if (sideQuests.FirstOrDefault(q => q.QuestID == dataManager.playerSideQuestIDData.sideQuestHaveDone + 1) != null)
            {

                SideQuest quest = sideQuests.FirstOrDefault(q => q.QuestID == dataManager.playerSideQuestIDData.sideQuestHaveDone + 1);

                if (quest.NPCGive == NPCName.name)
                {
                    int time = GameTimestamp.CompareTimestampsWithoutAbs(quest.AvaiableTimeToTakeQuest, TimeManager.Instance.timestamp);
                
                    if (time > 0)
                    {
                        SwitchSideQuestState(SideQuestState.Available);

                        foreach (var but in buttonTakeQuest)
                        {
                            if (but.name.StartsWith(NPCName.name))
                            {
                                but.SetActive(true);
                            }
                            else
                            {
                                but.SetActive(false);
                            }
                        }

                    }
                    else
                    {
                        SwitchSideQuestState(SideQuestState.NotAvailable);

                    }
                }
                else
                {
                    Debug.Log("Quest is given by other NPC");
                }

            }
            else
            {
                Debug.Log("No more Quest");
                SwitchSideQuestState(SideQuestState.NotAvailable);
            }
        }
        else
        {
            foreach (var but in buttonCheckQuest)
            {
                if (but.name.StartsWith(currentSideQuest.NPCGive))
                {
                    but.SetActive(true);
                }
                else
                {
                    but.SetActive(false);
                }
            }
        }

    }

    public void TakeCurrentSideQuest()
    {
        if (sideQueststate == SideQuestState.Available)
        {
            currentSideQuest = sideQuests.FirstOrDefault(q => q.QuestID == dataManager.playerSideQuestIDData.sideQuestHaveDone + 1);

            SwitchSideQuestState(SideQuestState.Accept);
            StartCoroutine(AppearTextOneByOne(questDescription_text, 0.1f, currentSideQuest.QuestDescription));
            //questDescription_text.text = currentSideQuest.QuestDescription;
            Debug.Log("Accepted");
        }
        else
        {
            Debug.Log("No more Quest");
        }

    } // trong button

    public void CheckRequirementSideQuest() // chekc if done the quest
    {
        if (currentSideQuest != null && currentSideQuest.Requirements.Count > 0)
        {

            bool isOkay = true;
            InventoryData inv = new InventoryData();
            foreach (var child in currentSideQuest.Requirements)
            {
                if (child.type == "Give") //thieu check number
                {
                    if (dataManager.inventoryData.FirstOrDefault(q => q.prefabItemID.Equals(child.Name)) != null)
                    {
                        InventoryData inv_ = dataManager.inventoryData.FirstOrDefault(q => q.prefabItemID.Equals(child.Name));
                        inv = inv_;
                      
                        Debug.Log("have");
                    }
                    else
                    {
                        isOkay = false;
                        Debug.Log(" Dont have");
                    }

                }
                else if (child.type == "Build")
                {
                    if (child.isBuildSomething == true)
                    {
                        Debug.Log("  build ");
                    }
                    else
                    {
                        isOkay = false;
                        Debug.Log(" not build yet");
                    }

                }
                else if (child.type == "Harvest")
                {
                    if (child.isHarvestSomething == true)
                    {
                        Debug.Log("  Harvest ");
                    }
                    else
                    {
                        isOkay = false;
                        Debug.Log(" not build yet");
                    }

                }
                else if (child.type == "Craft")
                {
                    if (child.isCrateSomething == true)
                    {
                        Debug.Log("  Craft ");
                    }
                    else
                    {
                        isOkay = false;
                        Debug.Log(" not craft yet");
                    }

                }
                else if (child.type == "Meet")
                {
                    if (child.isMeetNPC == true)
                    {
                        Debug.Log("  Meet ");
                    }
                    else
                    {
                        isOkay = false;
                        Debug.Log(" not craft yet");
                    }

                }
                else if (child.type == "Enter")
                {
                    if (child.isEnterGenWorld == true)
                    {
                        Debug.Log("  Enter ");
                    }
                    else
                    {
                        isOkay = false;
                        Debug.Log(" not craft yet");
                    }

                }
                else if (child.type == "Eat")
                {
                    if (child.isEatSomething == true)
                    {
                        Debug.Log("  Enter ");
                    }
                    else
                    {
                        isOkay = false;
                        Debug.Log(" not craft yet");
                    }

                }
                else if (child.type == "Sleep")
                {
                    if (child.isSleep == true)
                    {
                        Debug.Log("  Enter ");
                    }
                    else
                    {
                        isOkay = false;
                        Debug.Log(" not craft yet");
                    }

                }
                else if (child.type == "SpecialQuest")
                {
                    if (child.isDoneSpecialQuest == true)
                    {
                        Debug.Log("  Enter ");
                    }
                    else
                    {
                        isOkay = false;
                        Debug.Log(" not craft yet");
                    }

                }
                else
                {
                    isOkay = false;
                    Debug.Log(" DifferntType");
                }
            }
            if (DisplayInventory.Instance.SearchNumberEmptyUiSlot(currentSideQuest.QuestReward.Count) == false)
            {
                // Khoohg cho nhặt vì túi đầy, thông báo
                UiManager.Instance.StartCoroutine(UiManager.Instance.SlowOffGameObject(UiManager.Instance.fullInvenPanel));
            }
            else
            {
                if (isOkay)
                {
                    //Destroy trong inventory
                    Debug.Log("Can Done Quest");
                    SwitchSideQuestState(SideQuestState.Complete);
                    dataManager.inventoryData.Remove(inv);
                    DisplayInventory.Instance.ClearAll();
                    DisplayInventory.Instance.UpdateUI();
                }
                else
                {
                    notDoneYet.SetActive(true);
                    StartCoroutine(SlowOffGameObject(notDoneYet));
                }
            }
           

        }


    }

    public void CheckSideQuestRequirementHarvest(string buildObjectName)
    {
        if (currentSideQuest != null && currentSideQuest.Requirements.Count > 0)
        {
            foreach (var requirement in currentSideQuest.Requirements)
            {
                if (requirement.type == "Harvest")
                {
                    // Compare the built object's ID with the requirement's ID

                    if (buildObjectName.Equals(requirement.Name))
                    {
                        // The character has built the right thing
                        // Mark the requirement as completed or update its status
                        requirement.isHarvestSomething = true;

                        Debug.Log("Harvest the right thing!");
                    }
                }
            }
        }
    }

    public void CheckSideQuestRequirementBuild(string buildObjectName)
    {
        if (currentSideQuest != null && currentSideQuest.Requirements.Count > 0)
        {
            foreach (var requirement in currentSideQuest.Requirements)
            {
                if (requirement.type == "Build")
                {
                    // Compare the built object's ID with the requirement's ID

                    if (buildObjectName.Equals(requirement.Name))
                    {
                        // The character has built the right thing
                        // Mark the requirement as completed or update its status
                        requirement.isBuildSomething = true;

                        Debug.Log("Built the right thing!");
                    }
                }
            }
        }
    }

    public void CheckSideQuestRequirementCraft(string craftObjectName)
    {
        if (currentSideQuest != null && currentSideQuest.Requirements.Count > 0)
        {
            foreach (var requirement in currentSideQuest.Requirements)
            {
                if (requirement.type == "Craft")
                {
                    // Compare the built object's ID with the requirement's ID

                    if (craftObjectName.Equals(requirement.Name))
                    {
                        // The character has built the right thing
                        // Mark the requirement as completed or update its status
                        requirement.isCrateSomething = true;

                        Debug.Log("Craft the right thing!");
                    }
                }
            }
        }
    }

    public void CheckSideQuestRequirementMeet(string meetNPCName)
    {
        if (currentSideQuest != null && currentSideQuest.Requirements.Count > 0)
        {
            foreach (var requirement in currentSideQuest.Requirements)
            {
                if (requirement.type == "Meet")
                {

                    if (meetNPCName.Equals(requirement.Name))
                    {
                        requirement.isMeetNPC = true;
                    }
                }
            }
        }
    }

    public void CheckSideQuestRequirementEnterWorld()
    {
        if (currentSideQuest != null && currentSideQuest.Requirements.Count > 0)
        {
            foreach (var requirement in currentSideQuest.Requirements)
            {
                if (requirement.type == "Enter")
                {
                    requirement.isEnterGenWorld = true;
                }
            }
        }
    }

    public void CheckSideQuestRequirementEat(string meetNPCName)
    {
        if (currentSideQuest != null && currentSideQuest.Requirements.Count > 0)
        {
            foreach (var requirement in currentSideQuest.Requirements)
            {
                if (requirement.type == "Eat")
                {

                    if (meetNPCName.Equals(requirement.Name))
                    {
                        requirement.isEatSomething = true;
                    }
                }
            }
        }
    }

    public void CheckSideQuestRequirementSleep()
    {
        if (currentSideQuest != null && currentSideQuest.Requirements.Count > 0)
        {
            foreach (var requirement in currentSideQuest.Requirements)
            {
                if (requirement.type == "Sleep")
                {

                    requirement.isSleep = true;
                }
            }
        }
    }

    public void CheckSideQuestRequirementSpecialQuest()
    {
        if (currentSideQuest != null && currentSideQuest.Requirements.Count > 0)
        {
            foreach (var requirement in currentSideQuest.Requirements)
            {
                if (requirement.type == "SpecialQuest")
                {

                    requirement.isDoneSpecialQuest = true;
                }
            }
        }
    }

    public void SaveCurrentSideQuestData()
    {
        if (currentSideQuest!= null)
        {
            dataManager.SaveCurrenSidetQuest(currentSideQuest.QuestID, currentSideQuest.Requirements);
        }
        else
        {
            dataManager.ClearCurrentSideQuest();
        }
        if (sideQueststate == SideQuestState.NotAvailable)
        {
            dataManager.playerSideQuestIDData.sideQuestMode = 0;
        }
        else if (sideQueststate == SideQuestState.Available)
        {
            dataManager.playerSideQuestIDData.sideQuestMode = 1;
        }
        else if (sideQueststate == SideQuestState.Accept)
        {
            dataManager.playerSideQuestIDData.sideQuestMode = 2;
        }
        else if (sideQueststate == SideQuestState.Complete)
        {
            dataManager.playerSideQuestIDData.sideQuestMode = 3;
        }
        dataManager.dataManagerSideQuest.Clear();
        foreach (var quest in sideQuests)
        {
            if (quest.QuestID <= dataManager.playerSideQuestIDData.sideQuestHaveDone + 1)
            {
                dataManager.dataManagerSideQuest.Add(quest);
            }
        }
    }

    public void LoadCurrentSideQuestData()
    {
        int questID = dataManager.GetCurrentSideQuestID();
        if (questID != 0)
        {
            currentSideQuest = sideQuests.FirstOrDefault(q => q.QuestID == questID);
            if (currentSideQuest != null)
            {
                List<SideQuest.Requirement> requirements = dataManager.GetCurrentSideQuestRequirements();
                currentSideQuest.Requirements = requirements;
                if (dataManager.playerSideQuestIDData.sideQuestMode == 0)
                {
                    sideQueststate = SideQuestState.NotAvailable;
                }
                else if (dataManager.playerSideQuestIDData.sideQuestMode == 1)
                {
                    sideQueststate = SideQuestState.Available;
                }
                else if (dataManager.playerSideQuestIDData.sideQuestMode == 2)
                {
                    Debug.Log("heere");
                    sideQueststate = SideQuestState.Accept;
                }
                else if (dataManager.playerSideQuestIDData.sideQuestMode == 3)
                {
                    sideQueststate = SideQuestState.Complete;
                }
            }
        }
        foreach (var quest in sideQuests)
        {

            foreach (var questData in dataManager.dataManagerSideQuest)
            {
                if (quest.QuestID == questData.QuestID)
                {
                    quest.Requirements = questData.Requirements;
                }
            }
        }
    }


    public void QuestDescriptionUISideQuest(SideQuest q)
    {
        if (q != null && q.QuestID > 0)
        {
            UI_DescriptID.gameObject.SetActive(true);
            UI_Descript.gameObject.SetActive(true);
           
            UI_DescriptID.text = "Quest " + q.QuestID;
            UI_Descript.text = q.QuestDescription;
            guideText.text = guideText.text + "\n";
            guideText.text = q.Guide;
        }
    }

    public void QuestDetailCheckSideQuest(SideQuest q)
    {
        ClearAllChild();
        QuestDetails_viewport.gameObject.SetActive(true);
        if (q != null && q.Requirements.Count > 0)
        {
            foreach (var child in q.Requirements)
            {
                if (child.type == "Give") //thieu check number
                {
                    GameObject obj = Instantiate(QuestDetail, QuestDetails_viewport);

                    if (dataManager.inventoryData.FirstOrDefault(q => q.prefabItemID.Equals(child.Name)) != null)
                    {
                        InventoryData inv = dataManager.inventoryData.FirstOrDefault(q => q.prefabItemID.Equals(child.Name));
                        Text text = obj.transform.GetChild(0).GetComponent<Text>();
                        text.text = "Have " + child.AdjustName + " in inventory";
                        Toggle toggle = obj.GetComponentInChildren<Toggle>();
                        toggle.isOn = true;
                        toggle.enabled = false;
                        Debug.Log(text.text);
                    }
                    else
                    {
                        Text text = obj.transform.GetChild(0).GetComponent<Text>();
                        text.text = "Have " + child.AdjustName + " in inventory";
                        Debug.Log(text.text);
                        Toggle toggle = obj.GetComponentInChildren<Toggle>();
                        toggle.isOn = false;
                        toggle.enabled = false;
                    }
                }
                else if (child.type == "Build")
                {
                    GameObject obj = Instantiate(QuestDetail, QuestDetails_viewport);

                    if (child.isBuildSomething == true)
                    {
                        Text text = obj.transform.GetChild(0).GetComponent<Text>();
                        text.text = "Build " + child.AdjustName;
                        Toggle toggle = obj.GetComponentInChildren<Toggle>();
                        toggle.isOn = true;
                        toggle.enabled = false;
                    }
                    else
                    {
                        Text text = obj.transform.GetChild(0).GetComponent<Text>();
                        text.text = "Build " + child.AdjustName;
                        Toggle toggle = obj.GetComponentInChildren<Toggle>();
                        toggle.isOn = false;
                        toggle.enabled = false;
                    }

                }
                else if (child.type == "Harvest")
                {
                    GameObject obj = Instantiate(QuestDetail, QuestDetails_viewport);
                    Debug.Log("asas");
                    if (child.isHarvestSomething == true)
                    {
                        Text text = obj.transform.GetChild(0).GetComponent<Text>();
                        string itemName = child.Name.Replace("Harvest", "");
                        text.text = "Harvest " + itemName;
                        Toggle toggle = obj.GetComponentInChildren<Toggle>();
                        toggle.isOn = true;
                        toggle.enabled = false;
                    }
                    else
                    {
                        Text text = obj.transform.GetChild(0).GetComponent<Text>();
                        string itemName = child.Name.Replace("Harvest", "");
                        text.text = "Harvest " + itemName;
                        Toggle toggle = obj.GetComponentInChildren<Toggle>();
                        toggle.isOn = false;
                        toggle.enabled = false;
                    }

                }
                else if (child.type == "Craft")
                {
                    GameObject obj = Instantiate(QuestDetail, QuestDetails_viewport);

                    if (child.isCrateSomething == true)
                    {
                        Text text = obj.transform.GetChild(0).GetComponent<Text>();
                        text.text = "Craft " + child.AdjustName;
                        Toggle toggle = obj.GetComponentInChildren<Toggle>();
                        toggle.isOn = true;
                        toggle.enabled = false;
                    }
                    else
                    {
                        Text text = obj.transform.GetChild(0).GetComponent<Text>();
                        text.text = "Craft " + child.AdjustName;
                        Toggle toggle = obj.GetComponentInChildren<Toggle>();
                        toggle.isOn = false;
                        toggle.enabled = false;
                    }

                }
                else if (child.type == "Meet")
                {
                    GameObject obj = Instantiate(QuestDetail, QuestDetails_viewport);

                    if (child.isMeetNPC == true)
                    {
                        Text text = obj.transform.GetChild(0).GetComponent<Text>();
                        text.text = "Meet  " + child.Name;
                        Toggle toggle = obj.GetComponentInChildren<Toggle>();
                        toggle.isOn = true;
                        toggle.enabled = false;
                    }
                    else
                    {
                        Text text = obj.transform.GetChild(0).GetComponent<Text>();
                        text.text = "Meet " + child.Name;
                        Toggle toggle = obj.GetComponentInChildren<Toggle>();
                        toggle.isOn = false;
                        toggle.enabled = false;
                    }

                }
                else if (child.type == "Enter")
                {
                    GameObject obj = Instantiate(QuestDetail, QuestDetails_viewport);

                    if (child.isEnterGenWorld == true)
                    {
                        Text text = obj.transform.GetChild(0).GetComponent<Text>();
                        text.text = "Enter  " + child.Name;
                        Toggle toggle = obj.GetComponentInChildren<Toggle>();
                        toggle.isOn = true;
                        toggle.enabled = false;
                    }
                    else
                    {
                        Text text = obj.transform.GetChild(0).GetComponent<Text>();
                        text.text = "Enter " + child.Name;
                        Toggle toggle = obj.GetComponentInChildren<Toggle>();
                        toggle.isOn = false;
                        toggle.enabled = false;
                    }

                }
                else if (child.type == "Eat")
                {
                    GameObject obj = Instantiate(QuestDetail, QuestDetails_viewport);

                    if (child.isEatSomething == true)
                    {
                        Text text = obj.transform.GetChild(0).GetComponent<Text>();
                        text.text = "Eat  " + child.Name;
                        Toggle toggle = obj.GetComponentInChildren<Toggle>();
                        toggle.isOn = true;
                        toggle.enabled = false;
                    }
                    else
                    {
                        Text text = obj.transform.GetChild(0).GetComponent<Text>();
                        text.text = "Eat " + child.Name;
                        Toggle toggle = obj.GetComponentInChildren<Toggle>();
                        toggle.isOn = false;
                        toggle.enabled = false;
                    }

                }
                else if (child.type == "Sleep")
                {
                    GameObject obj = Instantiate(QuestDetail, QuestDetails_viewport);

                    if (child.isSleep == true)
                    {
                        Text text = obj.transform.GetChild(0).GetComponent<Text>();
                        text.text = "Go to sleep ";
                        Toggle toggle = obj.GetComponentInChildren<Toggle>();
                        toggle.isOn = true;
                        toggle.enabled = false;
                    }
                    else
                    {
                        Text text = obj.transform.GetChild(0).GetComponent<Text>();
                        text.text = "Go to sleep " ;
                        Toggle toggle = obj.GetComponentInChildren<Toggle>();
                        toggle.isOn = false;
                        toggle.enabled = false;
                    }

                }
                else if (child.type == "SpecialQuest")
                {
                    GameObject obj = Instantiate(QuestDetail, QuestDetails_viewport);

                    if (child.isDoneSpecialQuest == true)
                    {
                        Text text = obj.transform.GetChild(0).GetComponent<Text>();
                        text.text = "Do Special Quest " ;
                        Toggle toggle = obj.GetComponentInChildren<Toggle>();
                        toggle.isOn = true;
                        toggle.enabled = false;
                    }
                    else
                    {
                        Text text = obj.transform.GetChild(0).GetComponent<Text>();
                        text.text = "Do Special Quest ";
                        Toggle toggle = obj.GetComponentInChildren<Toggle>();
                        toggle.isOn = false;
                        toggle.enabled = false;
                    }

                }
                else
                {
                    Debug.Log(" DifferntType");
                }
            }
        }
    }



    #endregion

    public void QuestUIButton(GameObject button)
    {
        if(button.activeSelf)
        {
            UI_DescriptID.gameObject.SetActive(false);
            UI_Descript.gameObject.SetActive(false);
            QuestDetails_viewport.gameObject.SetActive(false);
            if (UI_ViewportMainQuest.childCount > 0)
            {
                foreach (Transform child in UI_ViewportMainQuest.transform)
                {
                    Destroy(child.gameObject);
                }
            }

            if (UI_ViewportSideQuest.childCount > 0)
            {
                foreach (Transform child in UI_ViewportSideQuest.transform)
                {
                    Destroy(child.gameObject);
                }
            }

           

            if (currentQuest != null)
            {
                for (int i = currentQuest.QuestID; i >= 0; i--)
                {
                    var quest = quests.Find(q => q.QuestID == i);
                    if (quest != null)
                    {
                        GameObject obj = Instantiate(UI_ButtonMainQuest, UI_ViewportMainQuest);
                        Text text = obj.GetComponentInChildren<Text>();
                        text.text = "Quest " + quest.QuestID;
                        Button but = obj.GetComponent<Button>();
                        but.onClick.AddListener(() => { QuestDetailCheck(quest); QuestDescriptionUI(quest); TurnOffGuide(); });
                      
                    }
                }
            }
            else
            {
               
                for (int i = dataManager.playerQuestIDData.questHaveDone; i >= 0; i--)
                {
                    var quest = quests.Find(q => q.QuestID == i);
                    if (quest != null)
                    {
                        GameObject obj = Instantiate(UI_ButtonMainQuest, UI_ViewportMainQuest);
                        Text text = obj.GetComponentInChildren<Text>();
                        text.text = "Quest " + quest.QuestID;
                        Button but = obj.GetComponent<Button>();
                        but.onClick.AddListener(() => { QuestDetailCheck(quest); QuestDescriptionUI(quest); TurnOffGuide(); });
                    
                    }
                }
            }
            
            if(currentSideQuest != null)
            {

                for (int i = currentSideQuest.QuestID; i >= 0; i--)
                {
                    var quest = sideQuests.Find(q => q.QuestID == i);
                    if (quest != null)
                    {
                        GameObject obj = Instantiate(UI_ButtonMainQuest, UI_ViewportSideQuest);
                        Text text = obj.GetComponentInChildren<Text>();
                        text.text = "Quest " + quest.QuestID;
                        Button but = obj.GetComponent<Button>();
                        but.onClick.AddListener(() => { QuestDescriptionUISideQuest(quest); QuestDetailCheckSideQuest(quest); TurnOffGuide(); });
                     
                    }
                }
            }
            else
            {
                for (int i = dataManager.playerSideQuestIDData.sideQuestHaveDone; i >= 0; i--)
                {
                    var quest = sideQuests.Find(q => q.QuestID == i);
                    if (quest != null)
                    {
                        GameObject obj = Instantiate(UI_ButtonMainQuest, UI_ViewportSideQuest);
                        Text text = obj.GetComponentInChildren<Text>();
                        text.text = "Quest " + quest.QuestID;
                        Button but = obj.GetComponent<Button>();
                        but.onClick.AddListener(() => { QuestDescriptionUISideQuest(quest); QuestDetailCheckSideQuest(quest); TurnOffGuide(); });
                        
                    }
                }
            }
           
        }
       
    }

    public void ClearAllChild()
    {
        if (QuestDetails_viewport.transform.childCount > 0)
        {
            foreach (Transform child in QuestDetails_viewport.transform)
            {
                Destroy(child.gameObject);
            }
        }

    }

    public void QuestGuideCheck()
    {
        if(UI_Descript.gameObject.activeSelf)
        {
            questGuide.SetActive(true);
            
        
        }
    }

    public void TurnOffGuide()
    {
        if (questGuide.gameObject.activeSelf)
        {

            questGuide.SetActive(false);


        }
    }

    private IEnumerator AppearTextOneByOne(Text textDisplay, float TimeDelay , string Description)
    {
        
        string originalText = Description;
        textDisplay.text = string.Empty;
        string[] words = originalText.Split(' ');
        

        for (int i = 0; i < words.Length; i++)
        {
            string word = words[i];
            textDisplay.text += word;

            yield return new WaitForSeconds(TimeDelay);

            if (i < words.Length - 1)
            {
                textDisplay.text += " ";
            }
        }
        string Tempt = textDisplay.text;
        textDisplay.text += "\n";
        textDisplay.text = Tempt;
    }


    [System.Serializable]
    public class Quest
    {
        public int QuestID;
        public string QuestDescription;
        public string Guide;
        public List<string> QuestReward = new List<string>();
        public int MoneyQuestReward;
        public List<Requirement> Requirements = new List<Requirement>();
        public GameTimestamp AvaiableTimeToTakeQuest = new GameTimestamp(0,0,0,0,0);
        [System.Serializable]
        public class Requirement
        {
            public string Name;
            public string AdjustName;
            public int Number;
            public string type;
            public bool isCrateSomething = false;
            public bool isBuildSomething = false;
            public bool isHarvestSomething = false;
            public bool isMeetNPC = false;
            public bool isEatSomething = false;
            public bool isSleep = false;
            public bool isDoneSpecialQuest = false;
            public bool isEnterGenWorld = false;
        }
    }

    [System.Serializable]
    public class SideQuest
    {
        public string NPCGive;
        public int QuestID;
        public string QuestDescription;
        public string Guide;
        public List<string> QuestReward = new List<string>();
        public int MoneyQuestReward;
        public List<Requirement> Requirements = new List<Requirement>();
        public GameTimestamp AvaiableTimeToTakeQuest = new GameTimestamp(0, 0, 0, 0, 0);
        [System.Serializable]
        public class Requirement
        {
            public string Name;
            public string AdjustName;
            public int Number;
            public string type;
            public bool isCrateSomething = false;
            public bool isBuildSomething = false;
            public bool isHarvestSomething = false;
            public bool isMeetNPC = false;
            public bool isEatSomething = false;
            public bool isSleep = false;
            public bool isDoneSpecialQuest = false;
            public bool isEnterGenWorld = false;
            
        }
    }
}
