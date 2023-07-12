using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Water;
using static DataManager;
using static Lands;

public class Interactor : MonoBehaviour
{
    public PhotonView photonView;
    public DataManager dataManager;
    //public HandButtonManager handButtonManager;
    public PlantState[] plantState;
    public PlayerControl playerControl;

    public Animator animator;
    public ParticleSystem wateringPS;

    public GameObject Dirt;
    public GameObject WaterredDirt;

    public AudioSource pickSound;
    public AudioSource harvestSound;
    public AudioSource digSound;
    public AudioSource wateringSound;

    public GameObject InteractableTer;
    public GameObject testDigPS;

    
    public Material digDirt_material;
    public Material wateringDirt_material;
    public float time;
    //UI list
    public GameObject fixTerUI;
    public GameObject farmTerUI;
    public GameObject pickUpUI;
    
    public HandButtonManager handButtonManager;
    //current UI
    GameObject curUI;
    Lands lands;
    public GameObject InteractCube;

    public PlantState takePlantState ;

    // Update is called once per frame
    private void Start()
    {
     
    }
    void Update()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position, Vector3.down, out hit) && transform.position.y > -500 && photonView.IsMine)
        {
            //string typeObj = dataManager.SearchTerrainInDataByItemId(hit.collider.gameObject.name.Substring(0, hit.collider.gameObject.name.IndexOf("_"))).type;
            if (SceneManager.GetActiveScene().name != "Main") return;
            if (hit.collider.gameObject != null)
            {
                Interactable(hit.collider.gameObject);
            }

            if (Input.GetButtonDown("Interact"))
            {
                if (handButtonManager.curTool != "none")
                {
                    //dig
                    if (handButtonManager.curToolSpeType == "sholve")
                    {
                        //Look at interact obj
                        Vector3 direction = hit.transform.position - transform.parent.position;
                        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                        transform.parent.rotation = Quaternion.Euler(transform.parent.rotation.x, angle, transform.parent.rotation.y);

                        //Interact
                        if (hit.transform.tag == "Plane")
                        {
                            string typeObj = dataManager.SearchTerrainInDataByItemId(hit.collider.gameObject.name.Substring(0, hit.collider.gameObject.name.IndexOf("_"))).type;
                            //Debug.Log(typeObj);
                            if (typeObj == "fixTer")
                            {
                                Vector3 plushitpoint = new Vector3(0, 1.3f, 0);
                                testDigPS.transform.position = hit.transform.position + plushitpoint;
                                //de digps1-2 cho khac sau
                                testDigPS.gameObject.SetActive(true);
                                ParticleSystem digps1 = testDigPS.transform.GetChild(0).GetComponent<ParticleSystem>();
                                ParticleSystem digps2 = testDigPS.transform.GetChild(1).GetComponent<ParticleSystem>();
                                ParticleSystem digps3 = testDigPS.transform.GetChild(2).GetComponent<ParticleSystem>();
                                digps1.Play();
                                digps2.Play();
                                digps3.Play();
                                //Cần kiểm tra cả công cụ khi có inventory
                                Debug.Log("Xúc đất");
                                Dig(hit);

                                //StartCoroutine(CheckAnim());
                            }
                            else { Debug.Log("not type of fixTer"); }
                        }
                    }
                    else if (handButtonManager.curToolSpeType == "watering")
                    {
                        if (hit.transform.tag == "Plane")
                        {
                            // string typeObj = dataManager.SearchTerrainInDataByItemId(hit.collider.gameObject.name.Substring(0, hit.collider.gameObject.name.IndexOf("_"))).type;
                            //Debug.Log(typeObj);
                            lands = hit.collider.gameObject.GetComponent<Lands>();
                            if (lands != null)
                            {
                                Debug.Log("Watering");
                                wateringPS.Play();
                                Watering(hit);

                                lands.SwitchState(LandStatus.Watered);
                                Debug.Log("fromFarmToWatered = true roi");
                            }
                            /*
                            if (typeObj == "farmingTer" || typeObj == "farmTer")
                            {
                                Debug.Log("Watering");
                                wateringPS.Play();
                                Watering(hit);
                               
                                lands.SwitchState(LandStatus.Watered);
                                Debug.Log("fromFarmToWatered = true roi");
                            }
                            */
                            else { Debug.Log("not type farmTer"); }
                        }
                    }
                    if (handButtonManager.curType == "seed")
                    {
                        Vector3 direction = hit.transform.position - transform.parent.position;
                        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                        transform.parent.rotation = Quaternion.Euler(transform.parent.rotation.x, angle, transform.parent.rotation.y);

                     
                        //Interact
                        if (hit.transform.tag == "Plane") // thieu dieu kien chuyen doi data type = farming thi khong dc trong nua - phai chuyen tu farm sang farming va nguoc lai
                        {
                            string typeObj = dataManager.SearchTerrainInDataByItemId(hit.collider.gameObject.name.Substring(0, hit.collider.gameObject.name.IndexOf("_"))).type;
                            //Debug.Log(typeObj);
                            lands = hit.collider.gameObject.GetComponent<Lands>();
                     
                            DataManager.TerrainData thisTer = dataManager.SearchTerrainInDataByObjectId(InteractableTer.name);
               
                            if (lands != null && thisTer.type != "farmingTer") // der tam
                            {
                                handReturnState();

                                takePlantState.Plant(hit.transform, lands);
                                
                                if (takePlantState != null)
                                {
                                    PlantTree(hit);


                                    lands.takePlantsInChild();
                                    RemoveInventoryData(); // ---> chua co so luong nen xoa tam thoi -- xoa inventory xong deactive tren tay
                                }



                                handButtonManager.curObjTool.SetActive(false);
                            }
                            /*
                            if (typeObj == "farmTer")
                            {
                                //plant tree 
                                handReturnState();
                            
                                takePlantState.Plant(hit.transform, lands);
                                
                                if (takePlantState != null)
                                {
                                    PlantTree(hit);


                                    lands.takePlantsInChild();
                                    RemoveInventoryData(); // ---> chua co so luong nen xoa tam thoi -- xoa inventory xong deactive tren tay
                                }



                                handButtonManager.curObjTool.SetActive(false);


                            }
                            */
                            else { Debug.Log("not type farmTer"); }
                        }
                    }
                }
            }
            /*
            if(Input.GetKeyDown(KeyCode.K))
            {
                if(handButtonManager.curTool != "none")
                {
                    if (handButtonManager.curType == "seed")
                    {
                        Vector3 direction = hit.transform.position - transform.parent.position;
                        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                        transform.parent.rotation = Quaternion.Euler(transform.parent.rotation.x, angle, transform.parent.rotation.y);

                        if (Input.GetKeyDown(KeyCode.K))
                        //if(true)
                        {
                        
                            //Interact
                            if (hit.transform.tag == "Plane")
                            {
                                string typeObj = dataManager.SearchTerrainInDataByItemId(hit.collider.gameObject.name.Substring(0, hit.collider.gameObject.name.IndexOf("_"))).type;
                                //Debug.Log(typeObj);
                                lands = hit.collider.gameObject.GetComponent<Lands>();
                                if (typeObj == "farmTer")
                                {
                                    //plant tree 
                                    handReturnState();
                                
                                    takePlantState.Plant(hit.transform , lands);
                                    if (takePlantState != null)
                                    {
                                        PlantTree(hit);

                                        lands.takePlantsInChild();
                                        RemoveInventoryData(); // ---> chua co so luong nen xoa tam thoi -- xoa inventory xong deactive tren tay
                                    }



                                    handButtonManager.curObjTool.SetActive(false);


                                }
                                else { Debug.Log("not type farmTer"); }
                            }

                        }
                    }
                }
               
            }
            */
     

            if (Input.GetButtonDown("Pick"))
            {
                if (hit.transform.tag == "PickUp")
                {
                    if (DisplayInventory.Instance.SearchFirstEmptyUiSlot() == null)
                    {
                        // Khoohg cho nhặt vì túi đầy, thông báo
                        UiManager.Instance.StartCoroutine(UiManager.Instance.SlowOffGameObject(UiManager.Instance.fullInvenPanel));
                    }
                    else
                    {
                        animator.SetTrigger("PickUp");
                        pickSound.Play();
                        //Them vao tui
                        string name = hit.transform.name;
                        InventoryItem.ItemData obj =
                        InventoryItem.Instance.SearchItemByID(name.Substring(0, name.IndexOf("_")));

                        dataManager.AddItemToInventoryData(obj.prefabItemID, obj.type, obj.speType, obj.durability, int.Parse(DisplayInventory.Instance.SearchFirstEmptyUiSlot().name), obj.money);
                        DisplayInventory.Instance.UpdateUI();

                        //Xoa vat the da nhat
                        dataManager.RemoveTerrainFromData(name);
                        Destroy(hit.transform.gameObject);
                    }
                }
                if (hit.transform.tag == "Harvest")
                {

                    if (hit.collider.GetComponent<PlantState>() != null)
                    {
                        PlantState plant = hit.collider.GetComponent<PlantState>();

                        if (plant.numberTimesHarvest == 0)
                        {
                            bool isHavervestable = false;
                            if (DisplayInventory.Instance.SearchFirstEmptyUiSlot() == null)
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
                                animator.SetTrigger("PickUp");
                                harvestSound.Play();

                                string name = hit.transform.name;
                                DataManager.CropData cropdata = dataManager.SearchCropDataByPrefabId(InteractableTer.name);
                                DataManager.TerrainData thisTer = dataManager.SearchTerrainInDataByObjectId(cropdata.terObjectID);

                                thisTer.type = "farmTer";
                                string harvest_name = name.Replace("Seed", "Harvest");
                                string desiredName = harvest_name.Substring(0, harvest_name.IndexOf('_'));

                                QuestManager.Instance.CheckQuestRequirementHarvest(desiredName);
                                QuestManager.Instance.CheckSideQuestRequirementHarvest(desiredName);
                                Debug.Log(desiredName);
                                InventoryItem.ItemData obj =
                                InventoryItem.Instance.SearchItemByID(harvest_name.Substring(0, harvest_name.IndexOf("_")));
                                dataManager.AddItemToInventoryData(obj.prefabItemID, obj.type, obj.speType, obj.durability, int.Parse(DisplayInventory.Instance.SearchFirstEmptyUiSlot().name), obj.money);
                                DisplayInventory.Instance.UpdateUI();
                                ListObjectManager.Instance.plantObject.Remove(hit.collider.gameObject); // neu ma thu hoach roi thi xoa trong list di
                                dataManager.RemoveCropDataByPrefabID(name);       // chua display UI
                                Destroy(hit.transform.gameObject);
                            }
                        }
                        else if (plant.numberTimesHarvest <= 9 && plant.numberTimesHarvest > 0)
                        {
                            bool isHavervestable = false;
                            if (DisplayInventory.Instance.SearchNumberEmptyUiSlot(plant.numberOfHarvest) == false)
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
                                //chua co update datamanager
                                animator.SetTrigger("PickUp");
                                string name = hit.transform.name;
                                string harvest_name = name.Replace("Seed", "Harvest");
                                string desiredName = harvest_name.Substring(0, harvest_name.IndexOf('_'));
                                QuestManager.Instance.CheckQuestRequirementHarvest(desiredName);
                                QuestManager.Instance.CheckSideQuestRequirementHarvest(desiredName);
                                InventoryItem.ItemData obj =
                                InventoryItem.Instance.SearchItemByID(harvest_name.Substring(0, harvest_name.IndexOf("_")));

                                for (int i = 0; i < plant.numberOfHarvest; i++)
                                {
   
                                    dataManager.AddItemToInventoryData(obj.prefabItemID, obj.type, obj.speType, obj.durability, int.Parse(DisplayInventory.Instance.SearchFirstEmptyUiSlot().name), obj.money);
                                    DisplayInventory.Instance.UpdateUI();
                                }

                                plant.SwitchState(PlantState.CropState.Seed);
                                plant.numberTimesHarvest++;
                                //DataManager.CropData cropdata = dataManager.SearchCropDataByPrefabId(InteractableTer.name);       // chua display UI

                            }
                        }
                        else if (plant.numberTimesHarvest == 10)
                        {
                            bool isHavervestable = false;
                            if (DisplayInventory.Instance.SearchNumberEmptyUiSlot(plant.numberOfHarvest) == false)
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

                                animator.SetTrigger("PickUp");
                                string name = hit.transform.name;
                                DataManager.CropData cropdata = dataManager.SearchCropDataByPrefabId(InteractableTer.name);
                                DataManager.TerrainData thisTer = dataManager.SearchTerrainInDataByObjectId(cropdata.terObjectID);

                                thisTer.type = "farmTer";
                                string harvest_name = name.Replace("Seed", "Harvest");
                                string desiredName = harvest_name.Substring(0, harvest_name.IndexOf('_'));
                                QuestManager.Instance.CheckQuestRequirementHarvest(desiredName);
                                QuestManager.Instance.CheckSideQuestRequirementHarvest(desiredName);
                                Debug.Log(desiredName);
                                InventoryItem.ItemData obj =
                                InventoryItem.Instance.SearchItemByID(harvest_name.Substring(0, harvest_name.IndexOf("_")));
                                for (int i = 0; i < plant.numberOfHarvest; i++)
                                {
                                   
                                    dataManager.AddItemToInventoryData(obj.prefabItemID, obj.type, obj.speType, obj.durability, int.Parse(DisplayInventory.Instance.SearchFirstEmptyUiSlot().name), obj.money);
                                }
                                DisplayInventory.Instance.UpdateUI();
                                ListObjectManager.Instance.plantObject.Remove(hit.collider.gameObject); // neu ma thu hoach roi thi xoa trong list di
                                dataManager.RemoveCropDataByPrefabID(name);       // chua display UI
                                Destroy(hit.transform.gameObject);
                            }
                        }
                    }
                    
                   
                }
            }
            //Harvest
        }
       
    }



    public void PlantTree(RaycastHit hit)
    {
        animator.SetTrigger("Plant");
        
        DataManager.TerrainData thisTer = dataManager.SearchTerrainInDataByObjectId(hit.collider.gameObject.name);
        Debug.Log(hit.collider.gameObject.name + "aa");
       // thisTer.itemID = "FarmingTer";
        thisTer.objectID = thisTer.itemID + hit.collider.gameObject.name.Substring(hit.collider.gameObject.name.IndexOf("_"));
        thisTer.type = "farmingTer";
       
        hit.collider.gameObject.name = thisTer.objectID;

        Debug.Log(thisTer.objectID + "BB");
        
    }
    public void Dig(RaycastHit hit)
    {
        animator.SetTrigger("Dig");
        StartCoroutine(playSFX(digSound, 1f));

        DataManager.TerrainData thisTer = dataManager.SearchTerrainInDataByObjectId(hit.collider.gameObject.name);

        thisTer.itemID = "DirtTer";
        thisTer.objectID = thisTer.itemID + hit.collider.gameObject.name.Substring(hit.collider.gameObject.name.IndexOf("_"));
        thisTer.type = "farmTer";

        hit.collider.gameObject.SetActive(false);
        GameObject newObj = Instantiate(Dirt, hit.collider.transform.position, Quaternion.identity);
        newObj.transform.rotation = hit.collider.transform.rotation;
        newObj.name = thisTer.objectID;
        newObj.transform.SetParent(hit.collider.transform.parent);
        Destroy(hit.collider.gameObject);
        //Debug.Log("is Dig");

        if (!ListObjectManager.Instance.CheckForDuplicateTerObject(newObj.gameObject.name.Split('_')[1]))
        {
            //Debug.Log("is Dig here");
            //Debug.Log(newObj.gameObject.name.Split('_')[1]);
            ListObjectManager.Instance.TerObject.Add(newObj.gameObject);
        }

        if (dataManager.SearchItemInDataByObjectId(handButtonManager.curToolObjId) != null)
        {
            dataManager.SearchItemInDataByObjectId(handButtonManager.curToolObjId).durability--;

            if (
            dataManager.SearchItemInDataByObjectId(handButtonManager.curToolObjId).durability <= 0)
            {
                dataManager.RemoveItemInDataByObjectId(handButtonManager.curToolObjId);

                handButtonManager.curObjTool.SetActive(false);
                playerControl.animator.SetTrigger("Idle");
                //StartCoroutine(deleteTool());
            }
        }     
    }
    IEnumerator playSFX(AudioSource sfx, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        sfx.Play();
    }
    public void Watering(RaycastHit hit)
    {
      
        animator.SetTrigger("Water");
        wateringSound.Play();

        /*
        DataManager.TerrainData thisTer = dataManager.SearchTerrainInDataByObjectId(hit.collider.gameObject.name);

        thisTer.itemID = "WateredDirtTer";
      
        thisTer.objectID = thisTer.itemID + hit.collider.gameObject.name.Substring(hit.collider.gameObject.name.IndexOf("_"));

        thisTer.type = "farmingTer";

        hit.collider.gameObject.name = thisTer.objectID;
        */

        /*
        hit.collider.gameObject.SetActive(false);
        GameObject newObj = Instantiate(WaterredDirt, hit.collider.transform.position, Quaternion.identity);
        newObj.transform.rotation = hit.collider.transform.rotation;
        newObj.name = thisTer.objectID;
        newObj.transform.SetParent(hit.collider.transform.parent);

        Destroy(hit.collider.gameObject);
        */

    }    
  
    
    public void Interactable(GameObject hit)
    {
        if((hit.transform.tag == "Plane") && (dataManager.SearchTerrainInDataByItemId(hit.transform.name.Substring(0, hit.transform.name.IndexOf("_"))) != null))
        {
            string Type = dataManager.SearchTerrainInDataByItemId(hit.transform.name.Substring(0, hit.transform.name.IndexOf("_"))).type;
            //check to disable prev UI
            if (curUI != null)
            {
                curUI.SetActive(false);
            }

            //display new
            InteractableTer = hit;
            if (Type == "fixTer")
            {
                InteractCube.transform.position = hit.transform.position + new Vector3(0,1.02f,0);
                InteractCube.transform.LookAt(hit.transform.position);

               fixTerUI.SetActive(true);
                fixTerUI.GetComponentInParent<UiFollowObject>().ObjFollow = InteractableTer.transform;
                curUI = fixTerUI;
            }
            else if (Type == "farmTer")
            {
                InteractCube.transform.position = hit.transform.position + new Vector3(0, 1.02f, 0);
                InteractCube.transform.LookAt(hit.transform.position);

                farmTerUI.SetActive(true);
                farmTerUI.GetComponentInParent<UiFollowObject>().ObjFollow = InteractableTer.transform;
                curUI = farmTerUI;
            }
            else if (Type == "farmingTer")
            {
                InteractCube.transform.position = hit.transform.position + new Vector3(0, 1.02f, 0);
                InteractCube.transform.LookAt(hit.transform.position);
            }
        }
        if(hit.transform.tag == "PickUp")
        {
            if (curUI != null)
            {
                curUI.SetActive(false);
            }
            InteractableTer = hit;
            InteractCube.transform.position = hit.transform.position + new Vector3(0, 0.05f, 0);
            InteractCube.transform.LookAt(hit.transform.position);

            pickUpUI.SetActive(true);
            pickUpUI.GetComponentInParent<UiFollowObject>().ObjFollow = InteractableTer.transform;
            curUI = pickUpUI;
        }
        if (hit.transform.tag == "Harvest")
        {
            if (curUI != null)
            {
                curUI.SetActive(false);
            }
            InteractableTer = hit;
            InteractCube.transform.position = hit.transform.position ;
            InteractCube.transform.LookAt(hit.transform.position);

            pickUpUI.SetActive(true);
            pickUpUI.GetComponentInParent<UiFollowObject>().ObjFollow = InteractableTer.transform;
            curUI = pickUpUI;
        }
    }

    public PlantState handReturnState()
    {
        if (handButtonManager.curTool == "carrotSeed")
        {
            return takePlantState = plantState[0];
        }
        else if (handButtonManager.curTool == "cornSeed")
        {
            return takePlantState = plantState[1];

        }
        else if (handButtonManager.curTool == "pumkinSeed")
        {
            return takePlantState = plantState[2];

        }
        else if (handButtonManager.curTool == "tomatoSeed")
        {
            return takePlantState = plantState[3];
        }
        else if (handButtonManager.curTool == "turnipSeed")
        {
            return takePlantState = plantState[4];
        }
        else if (handButtonManager.curTool == "watermelonSeed")
        {
            return takePlantState = plantState[5];
        }
        else if (handButtonManager.curTool == "wheatSeed")
        {
            return takePlantState = plantState[6];
        }
        else if (handButtonManager.curTool == "appleSeed")
        {
            return takePlantState = plantState[7];
        }
        else if (handButtonManager.curTool == "orangeSeed")
        {
            return takePlantState = plantState[8];
        }
        else if (handButtonManager.curTool == "pearSeed")
        {
            return takePlantState = plantState[9];
        }
        else if (handButtonManager.curTool == "plumSeed")
        {
            return takePlantState = plantState[10];
        }
        else if (handButtonManager.curTool == "coconutSeed")
        {
            return takePlantState = plantState[11];
        }
        return null; 
    }
    public void RemoveInventoryData() // Dung roi thi xoa  di --- trong hoi ngu sua sau
    {

        InventoryData item = dataManager.SearchItemInDataByButton(handButtonManager.currentHands);

        dataManager.inventoryData.Remove(item);

        DestroyHandUi.Instance.DestroyItem(handButtonManager.currentHands-1);

        handButtonManager.curTool = "none";
        // updaate lai
    }

 
}
