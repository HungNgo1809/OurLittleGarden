using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using static DataManager;

public class Builder : MonoBehaviour
{
    public static Builder Instance { private set; get; }
    public animationPlay ShopCover;
    public DataManager dataManager;
  
    Ray PointerRay;
    RaycastHit PointerHit;

    public bool isBuliding;

    public bool isEdit;
    public GameObject editingBuild;
    public GameObject editingBuild_Mesh;
    
    public PrefabBuild.PrefabBuildItem item;

    public GameObject shopPanel;

    int countRotation = 0;
    //public Tilemap tilemap;

    Vector3 pos = new Vector3(0,0,0);
    Vector3 offset = new Vector3(0, 0, 0);

    int coinsNeed = 0;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        StartCoroutine(ReLoadBuild());
    }
    private void Update()
    {
        if(isBuliding && item!=null)
        {
            Build();
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && isBuliding)
        {
            item.Mesh_GameObject.SetActive(false);
            isBuliding = false;

            UiManager.Instance.ActiveInventoryBtn(true);
            UiManager.Instance.ActiveShopBtn(true);
        }
        if(isEdit && editingBuild != null && editingBuild_Mesh != null)
        {
            ChangeBuildingPosition();
        }
    }
    public void StartBuild(string prefabId, Vector3 offset, int coins)
    {
        if(shopPanel.activeSelf)
        {
            ShopCover.OpenStore();

            //shopPanel.SetActive(false);
        }
        this.offset = offset;

        item = PrefabBuild.Instance.ReQuestItem(prefabId);
        isBuliding = true;

        coinsNeed = coins;

        UiManager.Instance.ActiveInventoryBtn(false);
        UiManager.Instance.ActiveShopBtn(false);
        UiManager.Instance.ActiveIdeaPanel(false);
        UiManager.Instance.ActiveAnimalHousePanel(false);
        UiManager.Instance.ActiveInteriorPanel(false);
    }

    public void StartEditPos(GameObject building, GameObject buildingMesh, Vector3 changeOffSet)
    {
        editingBuild = building;
        editingBuild_Mesh = buildingMesh;
        this.offset = changeOffSet;

        isEdit = true;

        UiManager.Instance.ActiveInventoryBtn(false);
        UiManager.Instance.ActiveShopBtn(false);
    }
    public void Build()
    {
        if (isRayHiting() && !EventSystem.current.IsPointerOverGameObject())
        {
            GetCellPos();
            item.Mesh_GameObject.SetActive(true);
            item.Mesh_GameObject.transform.position = pos + offset;
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                //Debug.Log("r");
                if(countRotation == 0)
                {
                    item.Mesh_GameObject.transform.rotation = Quaternion.Euler(item.Mesh_GameObject.transform.rotation.x, 90, item.Mesh_GameObject.transform.rotation.z);
                    countRotation++;
                }
                else if(countRotation == 1)
                {
                    item.Mesh_GameObject.transform.rotation = Quaternion.Euler(item.Mesh_GameObject.transform.rotation.x, 180, item.Mesh_GameObject.transform.rotation.z);
                    countRotation++;
                }
                else if (countRotation == 2)
                {
                    item.Mesh_GameObject.transform.rotation = Quaternion.Euler(item.Mesh_GameObject.transform.rotation.x, 270, item.Mesh_GameObject.transform.rotation.z);
                    countRotation++;
                }
                else if (countRotation == 3)
                {
                    item.Mesh_GameObject.transform.rotation = Quaternion.Euler(item.Mesh_GameObject.transform.rotation.x, 0, item.Mesh_GameObject.transform.rotation.z);
                    countRotation = 0;
                }
            }

            if(Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject() && item.Mesh_GameObject.GetComponentInChildren<BuildMesh>().canBuild)
            {
                GameObject obj = Instantiate(item.Prefab_GameObject, item.Mesh_GameObject.transform.position, Quaternion.identity);
                obj.transform.rotation = item.Mesh_GameObject.transform.rotation;

                //add to database
                obj.transform.name = item.Prefab_ID + "_" + dataManager.buildData.Count();

                dataManager.AddBuild(item.Prefab_ID, obj.transform.position, obj.transform.eulerAngles.y, item.Type);


              

                QuestManager.Instance.CheckQuestRequirementBuild(item.Prefab_ID);// check Quest
                QuestManager.Instance.CheckSideQuestRequirementBuild(item.Prefab_ID);
                item.Mesh_GameObject.SetActive(false);
                isBuliding = false;

                dataManager.coins = dataManager.coins - coinsNeed;
                ListObjectManager.Instance.buildObject.Add(obj);
                UiManager.Instance.ActiveInventoryBtn(true);
                UiManager.Instance.ActiveShopBtn(true);
               
            }    
        }
    } 

    public void ChangeBuildingPosition()
    {
        editingBuild.SetActive(false);
        if (isRayHiting() && !EventSystem.current.IsPointerOverGameObject())
        {
            GetCellPos();

            editingBuild_Mesh.SetActive(true);
            editingBuild_Mesh.transform.position = pos + offset;

            if (Input.GetKeyDown(KeyCode.R))
            {
                //Debug.Log("r");
                if (countRotation == 0)
                {
                   editingBuild_Mesh.transform.rotation = Quaternion.Euler(editingBuild_Mesh.transform.rotation.x, 90,editingBuild_Mesh.transform.rotation.z);
                    countRotation++;
                }
                else if (countRotation == 1)
                {
                   editingBuild_Mesh.transform.rotation = Quaternion.Euler(editingBuild_Mesh.transform.rotation.x, 180,editingBuild_Mesh.transform.rotation.z);
                    countRotation++;
                }
                else if (countRotation == 2)
                {
                   editingBuild_Mesh.transform.rotation = Quaternion.Euler(editingBuild_Mesh.transform.rotation.x, 270,editingBuild_Mesh.transform.rotation.z);
                    countRotation++;
                }
                else if (countRotation == 3)
                {
                   editingBuild_Mesh.transform.rotation = Quaternion.Euler(editingBuild_Mesh.transform.rotation.x, 0,editingBuild_Mesh.transform.rotation.z);
                    countRotation = 0;
                }
            }

            if (Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject() && editingBuild_Mesh.GetComponentInChildren<BuildMesh>().canBuild)
            {
                editingBuild.transform.position = editingBuild_Mesh.transform.position;
                editingBuild.transform.rotation = editingBuild_Mesh.transform.rotation;

                editingBuild_Mesh.SetActive(false);
                editingBuild.SetActive(true);

                //update data
                dataManager.UpdateBuildingTransformData(editingBuild.name, editingBuild.transform.position, editingBuild.transform.eulerAngles.y);
                isEdit = false;

                UiManager.Instance.ActiveInventoryBtn(true);
                UiManager.Instance.ActiveShopBtn(true);
            }
            else if(Input.GetKeyDown(KeyCode.Mouse1))
            {
                editingBuild_Mesh.SetActive(false);
                editingBuild.SetActive(true);

                isEdit = false;

                UiManager.Instance.ActiveInventoryBtn(true);
                UiManager.Instance.ActiveShopBtn(true);
            }
        }
    }    
    bool isRayHiting()
    {
        PointerRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(PointerRay, out PointerHit, Mathf.Infinity, 1 << 6);
    }

    public void GetCellPos()
    {
        if(PointerHit.collider.tag == "Plane")
        {
           pos = PointerHit.transform.position;
        }
    }
    public void StartReLoadBuild()
    {
        StartCoroutine(ReLoadBuild());
    }
    IEnumerator ReLoadBuild()
    {
        yield return new WaitForSeconds(0.01f);
        yield return new WaitUntil(()=> dataManager.isLoadedData);

        foreach(DataManager.Building build in dataManager.buildData)
        {
            Vector3 tmp = new Vector3();
            tmp.x = build.PosX;
            tmp.y = build.PosY;
            tmp.z = build.PosZ;

            GameObject obj = Instantiate(PrefabBuild.Instance.ReQuestItem(build.prefabID).Prefab_GameObject, tmp , Quaternion.identity);
            ListObjectManager.Instance.buildObject.Add(obj);//
            obj.transform.rotation = Quaternion.Euler(obj.transform.rotation.x , build.Rotation, obj.transform.rotation.z);

            obj.transform.name = build.objectID;
            BuildTime buildTime = obj.GetComponent<BuildTime>();
            buildTime.growth = build.timeBuild;
            buildTime.isDone = build.isDoneBuild;
        }    
    }


    
}
