using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Water;
using static Lands;

public class Interactor : MonoBehaviour
{
    public DataManager dataManager;
    public PlantState plantState;
    public PlayerControl playerControl;

    public Animator animator;

    public GameObject Dirt;
    public GameObject WaterredDirt;

 

    public GameObject InteractableTer;
    public GameObject testDigPS;

    public Material digDirt_material;
    public Material wateringDirt_material;
    public float time;
    //UI list
    public GameObject fixTerUI;
    public GameObject farmTerUI;
    public GameObject pickUpUI;

  
    //current UI
    GameObject curUI;
    Lands lands;
    public GameObject InteractCube;
    // Update is called once per frame
    private void Start()
    {
       
    }
    void Update()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position, Vector3.down, out hit) && (dataManager.terData.Count > 0) && (hit.collider.gameObject.name.Length > 0))
        {
            //string typeObj = dataManager.SearchTerrainInDataByItemId(hit.collider.gameObject.name.Substring(0, hit.collider.gameObject.name.IndexOf("_"))).type;
            //Debug.Log(hit.transform.name);
            Interactable(hit.collider.gameObject);
            if (Input.GetButtonDown("Interact"))
            //if(true)
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
            if (Input.GetKeyDown(KeyCode.K))
            //if(true)
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
                    if (typeObj == "farmTer")
                    {
                        //plant tree 
                        Debug.Log("Plant");
                        PlantTree(hit);
                        lands = hit.collider.gameObject.GetComponent<Lands>();
                        plantState.Plant(hit.transform);
                        lands.takePlantsInChild();
                    }
                    else { Debug.Log("not type farmTer"); }
                }               
            }
            if (Input.GetKeyDown(KeyCode.L))
            //if(true)
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
                    if (typeObj == "farmingTer" || typeObj == "farmTer")
                    {
                        Debug.Log("Watering");
                        
                        Watering(hit);
                        lands = hit.collider.gameObject.GetComponent<Lands>();
                        lands.SwitchState(LandStatus.Watered);
                        Debug.Log("fromFarmToWatered = true roi");





                        //Watering
                        //Havesting -> Tìm crof bằng terrain object Id và check curType xem thu hoạch được chưa
                    }
                    else { Debug.Log("not type farmTer"); }
                }                
            }
            if (Input.GetButtonDown("Pick"))
            {
                if (hit.transform.tag == "PickUp")
                {
                    animator.SetTrigger("PickUp");
                    //Them vao tui
                    string name = hit.transform.name;
                    InventoryItem.ItemData obj =
                    InventoryItem.Instance.SearchItemByID(name.Substring(0, name.IndexOf("_")));

                    dataManager.AddItemToInventoryData(obj.prefabItemID, obj.type, obj.speType);

                    //Xoa vat the da nhat
                    dataManager.RemoveTerrainFromData(name);

                    Destroy(hit.transform.gameObject);
                }
            }
        }
       
    }

    public void PlantTree(RaycastHit hit)
    {
        animator.SetTrigger("Plant");
        
        DataManager.TerrainData thisTer = dataManager.SearchTerrainInDataByObjectId(hit.collider.gameObject.name);
        
        thisTer.itemID = "FarmingTer";
        thisTer.objectID = thisTer.itemID + hit.collider.gameObject.name.Substring(hit.collider.gameObject.name.IndexOf("_"));
        thisTer.type = "farmingTer";
       
        hit.collider.gameObject.name = thisTer.objectID;
       
       
        
    }
    public void Dig(RaycastHit hit)
    {
        animator.SetTrigger("Dig");

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

        
       
    }
    public void Watering(RaycastHit hit)
    {
      
        animator.SetTrigger("Water");
        DataManager.TerrainData thisTer = dataManager.SearchTerrainInDataByObjectId(hit.collider.gameObject.name);

        thisTer.itemID = "WateredDirtTer";
      
        thisTer.objectID = thisTer.itemID + hit.collider.gameObject.name.Substring(hit.collider.gameObject.name.IndexOf("_"));
     
        hit.collider.gameObject.name = thisTer.objectID;
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
        if(hit.transform.tag == "Plane")
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
                InteractCube.transform.position = hit.transform.position + new Vector3(0,1,0);

                fixTerUI.SetActive(true);
                fixTerUI.GetComponentInParent<UiFollowObject>().ObjFollow = InteractableTer.transform;
                curUI = fixTerUI;
            }
            else if (Type == "farmTer")
            {
                InteractCube.transform.position = hit.transform.position + new Vector3(0, 1, 0);

                farmTerUI.SetActive(true);
                farmTerUI.GetComponentInParent<UiFollowObject>().ObjFollow = InteractableTer.transform;
                curUI = farmTerUI;
            }
            else if (Type == "farmingTer")
            {
                InteractCube.transform.position = hit.transform.position + new Vector3(0, 1, 0);
            }
        }
        if(hit.transform.tag == "PickUp")
        {
            if (curUI != null)
            {
                curUI.SetActive(false);
            }
            InteractableTer = hit;
            InteractCube.transform.position = hit.transform.position;

            pickUpUI.SetActive(true);
            pickUpUI.GetComponentInParent<UiFollowObject>().ObjFollow = InteractableTer.transform;
            curUI = pickUpUI;
        }
    }

   
}
