using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using static Lands;
using JetBrains.Annotations;
using static DataManager;
using UnityEngine.Audio;
using TMPro;
//using UnityEditor.Animations;

public class UiManager : MonoBehaviour, ITimeTracker
{
    public static UiManager Instance { get; set; }
    public static DestroyPanel InstanceDestroyPanel { get; set; }

    public List<Button> buttons = new List<Button>();

    public TextMeshProUGUI timeText;
    public TextMeshProUGUI dateText;
    public animationPlay animationPlay;

    public GameObject caftPanel;
    public GameObject openScene;
    public GameObject storagePanel;
    public GameObject ideaPanel;
    public GameObject stovePanel;
    public GameObject furnacePanel;
    public GameObject ovenPanel;
    public GameObject windMilPanel;
    public GameObject animalHousePanel;
    public GameObject interiorPanel;
    public GameObject PausePanel;

    public GameObject shopBtn;
    public GameObject inventoryBtn;

    public GameObject totalMoneySell;
    public GameObject inventory;
    public GameObject seller_Chat;
    public GameObject fullInvenPanel;
    public GameObject lackOfMoneyPanel;
    public Text money_text;
    public Text reputation_text;
    public Text numberMoneyOfSell_text;
    public GameObject GlobalMoney;
    public InputField GlobalMoney_Text;
    public InputField GlobalPackName_Text;
    public string curStorage;
    public DataManager dataManager;
    public LargeMapData largeMapData;
    public QuestManager questManager;

    public DestroyPanel destroyPanel;

    public SellOnlineManager sellOnlineManager;

    public GameObject GlobalChat;
    public Transform GlobalScroll;

    public PlayerControl control;

    public GenTime genTime;

    public AudioSource openInventorySound;
    public AudioSource closeInventorySound;

    public AudioMixer audioMixer;

    public List<NPCChat> chatList;
    public NPCChat closestNPC;

    #region upgrade
    public List<GameObject> campFireLv1;
    public List<GameObject> campFireLv2;

    public List<GameObject> toolTablelv1;

    public List<GameObject> ovenLv1;
    public List<GameObject> ovenLv2;

    public List<GameObject> furnaceLv1;
    #endregion
    //public AudioSource openBookSound;
    //public Button homeBtn;
    // Start is called before the first frame update

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (Instance == null)
        {
            Instance = this;
            InstanceDestroyPanel = destroyPanel;
        }
        else
        {
            Destroy(this.gameObject);
        }

        TimeManager.Instance.RegisterTracker(this);
        StartCoroutine(StartCheckFriendRequest());
    }

    IEnumerator StartCheckFriendRequest()
    {
        yield return new WaitUntil(() => (FriendManager.Instance != null));

        dataManager.CheckFriendRequest();

        yield return new WaitUntil(() => (dataManager.isCheckFriendRequest));
        dataManager.isCheckFriendRequest = false;
    }

    void Update()
    {
        if (money_text.text != dataManager.coins.ToString())
        {
            money_text.text = dataManager.coins.ToString();
        }
        if (reputation_text.text != dataManager.reputation.ToString())
        {
            reputation_text.text = dataManager.reputation.ToString();
        }

        if (Input.GetButtonDown("Cancel"))
        { 
            if(PausePanel.activeSelf)
            {
                PausePanel.SetActive(false);
            }
            else
            {
                PausePanel.SetActive(true);
            }   
        }

    }

    public void UpdateCraftLevel()
    {
        if (dataManager.upgradeData.campFireLevel >= 1)
        {
            foreach (GameObject locks in campFireLv1)
            {
                locks.SetActive(false);
            }
        }

        if (dataManager.upgradeData.campFireLevel >= 2)
        {
            foreach(GameObject locks in campFireLv2)
            {
                locks.SetActive(false);
            }    
        }

        if (dataManager.upgradeData.ovenLevel >= 1)
        {
            foreach (GameObject locks in ovenLv1)
            {
                locks.SetActive(false);
            }
        }

        if (dataManager.upgradeData.ovenLevel >= 2)
        {
            foreach (GameObject locks in ovenLv2)
            {
                locks.SetActive(false);
            }
        }

        if (dataManager.upgradeData.toolTableLevel >= 1)
        {
            foreach (GameObject locks in toolTablelv1)
            {
                locks.SetActive(false);
            }
        }

        if (dataManager.upgradeData.furnaceLevel >= 1)
        {
            foreach (GameObject locks in furnaceLv1)
            {
                locks.SetActive(false);
            }
        }
    }    
    public void Active(GameObject obj)
    {
        if (!obj.activeSelf)
        {
            obj.SetActive(true);
        }
        else
        {
            obj.SetActive(false);
        }
    }
    public void OpenInventorySound(GameObject inventoryPanel)
    {
        if (!inventoryPanel.activeSelf)
        {
            openInventorySound.Play();
        }
        else
        {
            closeInventorySound.Play();
        }
    }
    public void reverseActive(GameObject obj)
    {
        if (obj.activeSelf && !animationPlay.isProcessing)
        {
            obj.SetActive(false);
        }
        else if(!obj.activeSelf && !animationPlay.isProcessing)
        {
            obj.SetActive(true);
        }
    }    

    public void SetActive(GameObject obj)
    {
        if(!obj.activeSelf)
        {
            obj.SetActive(true);
        }
    }

    public void DeActive(GameObject obj)
    {
        if (obj.activeSelf)
        {
            obj.SetActive(false);
        }
    }

    public void ClockUpdate(GameTimestamp timestamp)
    {
        int hours = timestamp.hour;
        int minutes = timestamp.minute;

        string prefix = "AM ";

        if (hours > 12)
        {
            prefix = "PM ";
            hours -= 12;
        }
        if(timeText != null)
        {
            timeText.text = prefix + hours + ":" + minutes.ToString("00");
        }


        int day = timestamp.day;
        string season = timestamp.season.ToString();
        string dayOfWeek = timestamp.GetDayOfTheWeek().ToString();

        if (dateText != null)
            dateText.text = season + " " + day + "\n" + dayOfWeek ;
    }
    public void ActiveCraftPanel(bool active)
    {
        UpdateCraftLevel();
        caftPanel.SetActive(active);
        
        
        if(caftPanel.activeSelf)
        {
         //   caftPanel.GetComponent<Animator>().Play("openAnimation");
            caftPanel.GetComponent<Animator>().Play("background");
            openScene.GetComponent<Animator>().Play("OpenScene");
        }
        else
        {
            if (caftPanel.transform.GetChild(4).gameObject.activeSelf == true)
            {
                caftPanel.transform.GetChild(4).gameObject.SetActive(false); 
            }
          
        }

    }
    public void ActiveStovePanel(bool active)
    {
        UpdateCraftLevel();
        stovePanel.SetActive(active);


        if (stovePanel.activeSelf)
        {
            //   caftPanel.GetComponent<Animator>().Play("openAnimation");
            stovePanel.GetComponent<Animator>().Play("background");
            openScene.GetComponent<Animator>().Play("OpenScene");
        }
        /*
        if(stovePanel.activeSelf)
        {
            stovePanel.GetComponent<Animator>().Play("openAnimation");
            openScene.GetComponent<Animator>().Play("OpenScene");
        }*/
        else
        {
            if (stovePanel.transform.GetChild(4).gameObject.activeSelf == true)
            {
                stovePanel.transform.GetChild(4).gameObject.SetActive(false);
            }

        }
    }
    public void ActiveStoragePanel(bool active)
    {
        storagePanel.SetActive(active);
    }
    public void ActiveIdeaPanel(bool active)
    {
        ideaPanel.SetActive(active);

        if (ideaPanel.activeSelf)
        {
            //   caftPanel.GetComponent<Animator>().Play("openAnimation");
            ideaPanel.GetComponent<Animator>().Play("background");
            openScene.GetComponent<Animator>().Play("OpenScene");
        }
        else
        {
            if (ideaPanel.transform.GetChild(5).gameObject.activeSelf == true)
            {
                ideaPanel.transform.GetChild(5).gameObject.SetActive(false);
            }

        }
    }
    public void ActiveAnimalHousePanel(bool active)
    {
        animalHousePanel.SetActive(active);

        if (animalHousePanel.activeSelf)
        {
            //   caftPanel.GetComponent<Animator>().Play("openAnimation");
            animalHousePanel.GetComponent<Animator>().Play("background");
            openScene.GetComponent<Animator>().Play("OpenScene");
        }
        else
        {
            if (animalHousePanel.transform.GetChild(4).gameObject.activeSelf == true)
            {
                animalHousePanel.transform.GetChild(4).gameObject.SetActive(false);
            }

        }
    }
    public void ActiveInteriorPanel(bool active)
    {
        interiorPanel.SetActive(active);

        if (interiorPanel.activeSelf)
        {
            //   caftPanel.GetComponent<Animator>().Play("openAnimation");
            interiorPanel.GetComponent<Animator>().Play("background");
            openScene.GetComponent<Animator>().Play("OpenScene");
        }

    }
    public void ActiveFurnacePanel(bool active)
    {
        UpdateCraftLevel();
        furnacePanel.SetActive(active);

        if (furnacePanel.activeSelf)
        {
            //   caftPanel.GetComponent<Animator>().Play("openAnimation");
            furnacePanel.GetComponent<Animator>().Play("background");
            openScene.GetComponent<Animator>().Play("OpenScene");
        }

        else
        {
            if (furnacePanel.transform.GetChild(4).gameObject.activeSelf == true)
            {
                furnacePanel.transform.GetChild(4).gameObject.SetActive(false);
            }

        }
    }
    public void ActiveOvenPanel(bool active)
    {
        UpdateCraftLevel();
        ovenPanel.SetActive(active);

        if (ovenPanel.activeSelf)
        {
            //   caftPanel.GetComponent<Animator>().Play("openAnimation");
            ovenPanel.GetComponent<Animator>().Play("background");
            openScene.GetComponent<Animator>().Play("OpenScene");
        }
        else
        {
            if (ovenPanel.transform.GetChild(4).gameObject.activeSelf == true)
            {
                ovenPanel.transform.GetChild(4).gameObject.SetActive(false);
            }

        }
    }
    public void ActiveWindMilPanel(bool active)
    {
        windMilPanel.SetActive(active);

        if (windMilPanel.activeSelf)
        {
            //   caftPanel.GetComponent<Animator>().Play("openAnimation");
            windMilPanel.GetComponent<Animator>().Play("background");
            openScene.GetComponent<Animator>().Play("OpenScene");
        }
        else
        {
            if (windMilPanel.transform.GetChild(4).gameObject.activeSelf == true)
            {
                windMilPanel.transform.GetChild(4).gameObject.SetActive(false);
            }

        }
    }

    public void DisplayStorageItem(string storageId)
    {
        storagePanel.GetComponent<StorageInventory>().DisplayStorage(storageId);
    }

    public void ActiveShopBtn(bool active)
    {
        shopBtn.SetActive(active);
    }
    public void ActiveInventoryBtn(bool active)
    {
        inventoryBtn.SetActive(active);
    }

    public void UpdateBuildData()
    {
        foreach (DataManager.Building build in dataManager.buildData)
        {
            if (ListObjectManager.Instance.buildObject.Where(obj => obj.name == build.objectID).First() != null)
            {
                GameObject building = ListObjectManager.Instance.buildObject.Where(obj => obj.name == build.objectID).First();
                if (building! != null)
                {
                    BuildTime buildtime = building.GetComponent<BuildTime>();

                    build.timeBuild = buildtime.growth;
                    build.isDoneBuild = buildtime.isDone;
                   
                }
            }
        }
    }

    public void UpdateCropData() // update ca time data
    {
        foreach (DataManager.CropData crop in dataManager.cropData)
        {
            if(ListObjectManager.Instance.plantObject.Where(obj => obj.name == crop.prefabID).First() != null)
            {
                GameObject crop_ = ListObjectManager.Instance.plantObject.Where(obj => obj.name == crop.prefabID).First();
                if (crop_! != null)
                {
                    PlantState plant = crop_.GetComponentInChildren<PlantState>();

                    crop.growTime = plant.growth;
                    //plant.growth = crop.curTime;
                    if(plant.numberTimesHarvest >0)
                    {
                        crop.curNumberHarvest= plant.numberTimesHarvest;
                    }
                    if (plant.cropstate == PlantState.CropState.Seed)
                    {
                        crop.curMode = 0;
                    }
                    else if (plant.cropstate == PlantState.CropState.Seeding)
                    {
                        crop.curMode = 1;
                    }
                    else if (plant.cropstate == PlantState.CropState.Harvestable)
                    {
                        crop.curMode = 2;
                    }

                }
            }
        }
        foreach (GameObject child_ in ListObjectManager.Instance.TerObject)
        {
            DataManager.TerrainData thisTer = dataManager.SearchTerrainInDataByObjectId(child_.name);
            if (thisTer != null)
            {
                Lands land = child_.GetComponent<Lands>();
                thisTer.wateredTime = land.timeSinceWatered;
                if (land.landStatus == LandStatus.FarmLand)
                {
                    thisTer.LandsMode = 0;

                }
                if (land.landStatus == LandStatus.Watered)
                {
                    thisTer.LandsMode = 1;

                }
            }
        }
        DataManager.TimeData time = dataManager.timeData;
        time.years = TimeManager.Instance.timestamp.year;
        if(TimeManager.Instance.timestamp.season == GameTimestamp.Season.Spring)
        {
            time.seasons = 0;
        }
        else if (TimeManager.Instance.timestamp.season == GameTimestamp.Season.Summer)
        {
            time.seasons = 1;
        }
        else if(TimeManager.Instance.timestamp.season == GameTimestamp.Season.Fall)
        {
            time.seasons = 2;
        }
        else if (TimeManager.Instance.timestamp.season == GameTimestamp.Season.Winter)
        {
            time.seasons = 3;
        }
        time.days = TimeManager.Instance.timestamp.day;
        time.hourrs = TimeManager.Instance.timestamp.hour;
        time.mintute = TimeManager.Instance.timestamp.minute;
    }
    public void UpdateAnimalData() // update ca time data
    {
        //Debug.Log("aaaa");
        foreach (DataManager.AnimalHouse animalHouse in dataManager.animalHouseData)
        {
            //Debug.Log("bbb");
            if (ListObjectManager.Instance.buildObject.Where(obj => obj.name == animalHouse.objectId).FirstOrDefault() != null)
            {
                GameObject animalHouse_ = ListObjectManager.Instance.buildObject.Where(obj => obj.name == animalHouse.objectId).First();
                Debug.Log(animalHouse_.name) ;
                if (animalHouse_! != null)
                {
                    animalHouseComponent component = animalHouse_.GetComponent<animalHouseComponent>();

                    animalHouse.currentTime = component.totalNumberTimeHarvest;

                    foreach(var childComponent in component.listAnimalState)
                    {
                        foreach(var animals in animalHouse.animals)
                        {
                            if(animals.animalId == childComponent.gameObject.name)
                            {
                                animals.currentTime = childComponent.growth;
                            }
                        }
                    }
                    Debug.Log(component.totalNumberTimeHarvest);

                }
            }
        }
       
    }
    public void OnClickSave()
    {
        if (SceneManager.GetActiveScene().name == "Social" || (dataManager.userId == PhotonNetwork.CurrentRoom.Name))
        {
            StartCoroutine(SaveDataUByButton());
        }
        else
        {
            StartCoroutine(SaveStatusOnlyByButton());
        }    
    } 
    private void OnApplicationQuit()
    {
        //PhotonNetwork.LeaveRoom();
        if (SceneManager.GetActiveScene().name == "Social" || (dataManager.userId == PhotonNetwork.CurrentRoom.Name))
        {
            //largeMapData.SaveDataToCom("map", largeMapData.terData);
            //largeMapData.SaveDataToCom("obj", largeMapData.objData);
            //largeMapData.saveCount = 0;
            //largeMapData.SaveGridTerData();

            if(SceneManager.GetActiveScene().name == "Social")
            {
                largeMapData.SaveAction();
            }    

            if (dataManager.isSavedDataBeforeQuit < 26 || (largeMapData.saveCount < 43 && SceneManager.GetActiveScene().name == "Social"))
            {
                Application.CancelQuit();
            }

            if (SceneManager.GetActiveScene().name == "Main")
            {
                UpdateCropData();
                UpdateBuildData();
                UpdateAnimalData();
                questManager.SaveCurrentQuestData();
                questManager.SaveCurrentSideQuestData();
            }
         
            StartCoroutine(SaveDataU());
        }
        else
        {
            if(dataManager.isSavedDataBeforeQuit < 6)
            {
                Application.CancelQuit();
            }
            StartCoroutine(SaveStatusOnly());
        }
       
    }
    IEnumerator SaveStatusOnly()
    {
        dataManager.SaveStatusDataOnly();

        yield return new WaitUntil(() => dataManager.isSavedDataBeforeQuit >= 6);
        Application.Quit();
    }
    IEnumerator SaveStatusOnlyByButton()
    {
        dataManager.SaveStatusDataOnly();

        yield return new WaitUntil(() => dataManager.isSavedDataBeforeQuit >= 6);
        dataManager.isSavedDataBeforeQuit = 0;
    }
    IEnumerator SaveDataU()
    {
         dataManager.TriggerSave();
         dataManager.SaveFarmTerData();
         yield return new WaitUntil(() => ((dataManager.isSavedDataBeforeQuit >= 26 && largeMapData.saveCount >= 43 && SceneManager.GetActiveScene().name == "Social") ||
                                          (dataManager.isSavedDataBeforeQuit >= 26 && SceneManager.GetActiveScene().name != "Social")));

         Application.Quit();
    }

    IEnumerator SaveDataUByButton()
    {
        if (SceneManager.GetActiveScene().name == "Main")
        {
            UpdateCropData();
            UpdateBuildData();
            UpdateAnimalData();
            questManager.SaveCurrentQuestData();
            questManager.SaveCurrentSideQuestData();
        }

        dataManager.TriggerSave();
        dataManager.SaveFarmTerData();

        yield return new WaitUntil(() => dataManager.isSavedDataBeforeQuit >= 25);
        dataManager.isSavedDataBeforeQuit = 0;
    }    
    public void StartSelling()
    {
        totalMoneySell.SetActive(true);
        inventory.SetActive(true);
        seller_Chat.SetActive(false);
        foreach (var child in buttons)
        {
            child.enabled = true;
            InventoryButton inventory = child.GetComponent<InventoryButton>();
            inventory.isOnSellingMode= true;
            inventory.isOnUpdatingMode= false;
            if(child.transform.childCount > 0)
            {
                DraggableItem drag = child.GetComponentInChildren<DraggableItem>();
                drag.isSellingMode = true;
            }
        }

    }

    public void EndSelling()
    {
        numberMoneyOfSell_text.text = "0"; // nen them 1 panel cho ba` gia cam on hay gi do
        totalMoneySell.SetActive(false);
        inventory.SetActive(false);
        foreach (var child in buttons)
        {
            child.enabled = false;
            InventoryButton inventory = child.gameObject.GetComponent<InventoryButton>();
            inventory.isOnSellingMode= false;

            //inventory.isOnUpdatingMode = false;
            if (inventory.isClicked)
            {
                inventory.image.color = new Color(238 / 255f, 236 / 255f, 178 / 255f, 1f); // Set the default color
                inventory.isClicked = false;
            }
            if (child.transform.childCount > 0)
            {
                DraggableItem drag = child.GetComponentInChildren<DraggableItem>();
                drag.isSellingMode = false;
            }
        }
        DisplayInventory.Instance.UpdateUI();
    }
    public void ConfirmSell()
    {
        int money =  int.Parse(numberMoneyOfSell_text.text); // day cucng nen the 
        dataManager.coins += money;
        // tang do hao cam
        if(dataManager.SearchNPCAffectionByName("Isabella") != null)
        {
            Affection npc = dataManager.SearchNPCAffectionByName("Isabella");
            npc.amountOfTrade += money;
            dataManager.IncreaseAmount(npc);
        }
      
        numberMoneyOfSell_text.text = "0";
        totalMoneySell.SetActive(false);
        inventory.SetActive(false);

        RemoveSoldItem();
    }

    public void RemoveSoldItem()
    {
        foreach (var child in buttons)
        {
            InventoryButton inventory = child.gameObject.GetComponent<InventoryButton>();
            inventory.isOnSellingMode = false;
            //inventory.isOnUpdatingMode = false;
            InventorySlot slot = child.gameObject.GetComponent<InventorySlot>();

            if (child.transform.childCount > 0)
            {
                DraggableItem drag = child.GetComponentInChildren<DraggableItem>();
                drag.isSellingMode = true;
            }

            if (inventory.isClicked)
            {
                dataManager.RemoveItemInDataByObjectId(inventory.transform.GetChild(0).name);
                Destroy(inventory.transform.GetChild(0).gameObject);
                DisplayInventory.Instance.UpdateUI();
                slot.curItem = null;
                inventory.image.color = new Color(238 / 255f, 236 / 255f, 178 / 255f, 1f); // Set the default color
                inventory.isClicked = false;
            }
            child.enabled = false;
        }
    }

    public void GlobalStartSelling()
    {
        GlobalChat.SetActive(false);
        GlobalMoney.SetActive(true);
        inventory.SetActive(true);
        foreach (var child in buttons)
        {
            child.enabled = true;
            InventoryButton inventory = child.GetComponent<InventoryButton>();
            inventory.isOnUpdatingMode = true;
            if (child.transform.childCount > 0)
            {
                DraggableItem drag = child.GetComponentInChildren<DraggableItem>();
                drag.isSellingMode = true;
            }
        }

    }
    public void GlobalEndSelling()
    {
        GlobalMoney_Text.text = ""; 
        GlobalPackName_Text.text = "";
        Text placeholder_SellingMoney = GlobalMoney_Text.placeholder.GetComponent<Text>();
        placeholder_SellingMoney.text = "0";
        Transform[] children = GlobalScroll.GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            if (child != GlobalScroll.transform)
            {
                Destroy(child.gameObject);
            }
        }
        GlobalChat.SetActive(false);
        GlobalMoney.SetActive(false);
        inventory.SetActive(false);
        foreach (var child in buttons)
        {
            child.enabled = false;
            InventoryButton inventory = child.gameObject.GetComponent<InventoryButton>();
            inventory.isOnUpdatingMode = false;
            //inventory.isOnUpdatingMode = false;
            if (inventory.isClicked)
            {
                inventory.image.color = new Color(238 / 255f, 236 / 255f, 178 / 255f, 1f); // Set the default color
                inventory.isClicked = false;
            }
            if (child.transform.childCount > 0)
            {
                DraggableItem drag = child.GetComponentInChildren<DraggableItem>();
                drag.isSellingMode = false;
            }
        }
        DisplayInventory.Instance.UpdateUI();
    }

    public void GlobalSellConfirmSell()
    {
        GlobalMoney_Text.text = "";
        GlobalPackName_Text.text = "";
        //int money = int.Parse(numberMoneyOfSell_text.text); // day cucng nen the 
        //dataManager.coins += money;
        GlobalChat.SetActive(false);
        GlobalMoney.SetActive(false);
        inventory.SetActive(false);

    }
    public void GlobalMoneyChange()
    {
        int money = int.Parse(GlobalMoney_Text.text);
        if (sellOnlineManager != null && GlobalMoney_Text.text != "")
        {
            sellOnlineManager.curPack.price = money;
        }
    }
    public void GlobalNamePackChange() // pack name
    {
        if (sellOnlineManager != null)
        {
            sellOnlineManager.curPack.name = GlobalPackName_Text.text;
        }
    }

    public PlayerControl ReturnPlayerControl(PlayerControl playerControl)
    {
        return control = playerControl;
    }

    public void TurnOffPlayerInteract()
    {
        if(control != null)
        {
            control.NPCInteract_ = false;
        }
    }

    public IEnumerator SlowOffGameObject(GameObject obj)
    {
        obj.SetActive(true);

        yield return new WaitForSeconds(1f);
        obj.SetActive(false);
    }


    public void startGentTime()
    {
        UpdateCropData();
        UpdateBuildData();
        UpdateAnimalData();
        questManager.SaveCurrentQuestData();
        questManager.SaveCurrentSideQuestData();

        timeText.gameObject.SetActive(false);
       dateText.gameObject.SetActive(false);
       StartCoroutine(genTime.TimeUpdate());
    }
    public void SetMusic(float vol)
    {
        if(vol != 0)
        audioMixer.SetFloat("Music", Mathf.Log10(vol)*20);
    }

    public void SetSound(float vol)
    {
        if (vol != 0)
            audioMixer.SetFloat("SFX", Mathf.Log10(vol) * 20);
    }

    public void ResetCurPackSellOnline()
    {
        sellOnlineManager.curPack = new SellOnlineManager.itemPack();
        sellOnlineManager.curPackName = "";
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }

    public void FindClosestNPC(GameObject player)
    {
        float closestDistance = Mathf.Infinity;
        Debug.Log("1");
        foreach (NPCChat npcChat in chatList)
        {
            float distance = Vector3.Distance(player.transform.position, npcChat.NPC.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestNPC = npcChat;
                Debug.Log(closestNPC.NPC.name);
            }
        }

        if (closestNPC != null && closestNPC.NPC.name != "Emma")
        {
            if (closestNPC.NPC.name == "Isabella" )
            {
                if(inventory.activeSelf != true)
                {
                    closestNPC.chatPanel.SetActive(true);
                }
                
            }
            else
            {
                closestNPC.chatPanel.SetActive(true);
            }

            // You can perform additional actions for the closest NPC here
        }
        Debug.Log("here");
    }


    [System.Serializable]
    public class NPCChat
    {
        public GameObject NPC;
        public GameObject chatPanel;
    }

}
