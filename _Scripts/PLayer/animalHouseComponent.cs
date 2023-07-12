using Photon.Pun;
using Photon.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static DataManager;

public class animalHouseComponent : MonoBehaviour
{
    public DataManager dataManager;
    public string itemToAddInventory;
    //public GameObject UiResponse;
    bool isTrigger;
    bool isUsing;

    Collider player;
    //bool isEdit;
    public ParticleSystem destroyVfx;
    public GameObject mesh;
    public GameObject canavas;
    public int maxAnimalNumber = 4;
    public int curNumberAnimal = 0;
    public int coefficient = 1;
    public int animalPrice;
    public float limitFromCenterPoint = 2.2f;
    public GameObject usePanel;
    public GameObject WordK;
    public GameObject WordP;
    public GameObject Max;

    public AudioSource interactSound;
    public AudioSource destroySound;
    public AudioSource harvestSound;
    public enum type
    {
        chickenHouse,
        pigHouse,
        cowHouse,
        duckHouse
    }

    public GameObject[] animalPrefab;
    public Transform spawnPosition;

    public type animalType;

    public List<AnimalState> listAnimalState = new List<AnimalState>();
    public int totalNumberTimeHarvest;
    public int totalGrowthMax;
    public Transform centerPoint;

    private Canvas can;
    private void Start()
    {
        StartCoroutine(AddNewAnimalHouseIfNew());
        can = usePanel.GetComponent<Canvas>();
        can.worldCamera = Camera.main;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && PhotonNetwork.CurrentRoom.Name == dataManager.userId)
        {
            player = other;

            isTrigger = true;
            //Debug.Log("storage");

            if (other.gameObject.GetComponentInChildren<playerFloatingUi>() != null)
            {
                other.gameObject.GetComponentInChildren<playerFloatingUi>().transform.GetChild(0).gameObject.SetActive(true);
                PlayerControl control = other.gameObject.GetComponent<PlayerControl>();
                control.bubbleAnimation.Play("startBubble");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && PhotonNetwork.CurrentRoom.Name == dataManager.userId)
        {
            isTrigger = false;
            if (other.gameObject.GetComponentInChildren<playerFloatingUi>() != null)
            {
                other.gameObject.GetComponentInChildren<playerFloatingUi>().transform.GetChild(0).gameObject.SetActive(false);
            }

            //CloseStorage();
            Close();
        }
    }

    IEnumerator AddNewAnimalHouseIfNew()
    {
        yield return new WaitForSeconds(0.5f);

        if (dataManager.SearchAnimalHouseById(transform.name) == null)
        {
            dataManager.AddAnimalHouse(transform.name, animalType.ToString(),totalNumberTimeHarvest);
        }
        else
        {
           StartCoroutine(ReCreateAnimal(dataManager.SearchAnimalHouseById(transform.name)));
        }
    }

    private void Update()
    {
        if (isTrigger)
        {
            RotateTowardsTarget();
            if (Input.GetButtonDown("Use") && !isUsing)
            {
                interactSound.Play();
                CheckCondition();
                //UseStorage();
                Use();
            }
            else if (Input.GetKeyDown(KeyCode.L) && !isUsing)
            {
                //destroy
                UiManager.InstanceDestroyPanel.gameObject.SetActive(true);
                UiManager.InstanceDestroyPanel.buildingToDestroy = this.gameObject;
            }
            else if (Input.GetButtonDown("EditPosition") && !Builder.Instance.isEdit && !Builder.Instance.isBuliding && !isUsing)
            {
                if (player != null)
                {
                    if (player.gameObject.GetComponentInChildren<playerFloatingUi>() != null)
                    {
                        player.gameObject.GetComponentInChildren<playerFloatingUi>().transform.GetChild(0).gameObject.SetActive(false);
                    }
                }
                EditPosition();
            }
            else if(Input.GetKeyDown(KeyCode.K) && isUsing)
            {
                //Mua thêm thú
                interactSound.Play();
                BuyAnimal();
                CheckCondition(); 
            }
            else if(Input.GetButtonDown("Pick") && isUsing)
            {
                //Thu hoạch
                interactSound.Play();
                Harvest();
                CheckCondition();
            }    
            else if (Input.GetButtonDown("Use") && isUsing)
            {
                //CloseStorage();
                Close();
            }
        }

        /*
        if (isEdit)
        {
            EditPosition();
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                isEdit = false;
            }
        }*/
    }

    public void Use()
    {
        usePanel.SetActive(true);
        // hiển thị icon gợi ý 
        CheckCondition();
        //set isUsing
        isUsing = true;
    }

    public void CheckCondition()
    {
        WordK.SetActive(false
            );
        WordP.SetActive(false);
        Max.SetActive(false);
        if (totalNumberTimeHarvest >= totalGrowthMax && listAnimalState.Count > 0)
        {
            WordP.SetActive(true);
           
        }
        else
        {
            WordP.SetActive(false);

        }
        if (listAnimalState.Count >= maxAnimalNumber)
        {
            WordK.SetActive(false);
            Max.SetActive(true);
        }
        if(listAnimalState.Count >= 0 && listAnimalState.Count < maxAnimalNumber)
        {
            
            WordK.SetActive(true);
          
        }
      

    

    }


    public void Close()
    {
        // tắt icon gợi ý 
        usePanel.SetActive(false);
        //set isUsing
        isUsing = false;
    }
    public void BuyAnimal()
    {
        if (curNumberAnimal < maxAnimalNumber)
        {
            //public int curPrefab = curNumberAnimal % animalPrefab.Length;
            GameObject newAnimal = Instantiate(animalPrefab[curNumberAnimal % animalPrefab.Length], spawnPosition.position, Quaternion.identity);

            DataManager.Animal animal = new DataManager.Animal();

            newAnimal.name = animal.animalId = animalPrefab[curNumberAnimal % animalPrefab.Length].name + "_" + curNumberAnimal;
            newAnimal.transform.SetParent(this.transform);
           
         

            dataManager.SearchAnimalHouseById(transform.name).animals.Add(animal);

            dataManager.coins = dataManager.coins - animalPrice;
            curNumberAnimal++;
            AnimalState state = newAnimal.GetComponent<AnimalState>();
            AnimalBehavior behavior = newAnimal.GetComponent<AnimalBehavior>();
            behavior.animalHouseComponent = this;
            behavior.SwitchStateAction(AnimalBehavior.AnimalAction.GoingSomeWhere);
            state.animalHouse = this;
            animal.curLife = state.animalLife;
            animal.currentTime = state.growth;
            listAnimalState.Add(state);
            totalGrowthMax += state.growthMax;

        }
        else
        {
            //thông báo chuồng đang full
        }
    } 
    public void Harvest()
    {
        // thu hoạch khi đến thời gian thu hoạch, số lượng vật phẩm thu được = số thú hiện tại X hệ số(coefficient), tất cả thú trong nhà bị trừ -life, khi life về 0 thì xóa thú khỏi data và object đồng thời giảm số thú hiện tại
        // check inventory truoc xong moi toi curNumberAnimal

        if (DisplayInventory.Instance.SearchNumberEmptyUiSlot(curNumberAnimal * coefficient) == false)
        {
            // Khoohg cho nhặt vì túi đầy, thông báo
            UiManager.Instance.StartCoroutine(UiManager.Instance.SlowOffGameObject(UiManager.Instance.fullInvenPanel));
        }
        else
        {
            if (curNumberAnimal > 0)
            {
                if (totalNumberTimeHarvest >= totalGrowthMax)
                {
                    // thu hoach duoc 
                    if (dataManager.animalHouseData.FirstOrDefault(q => q.objectId == gameObject.name) != null)
                    {
                        harvestSound.Play();
                        DataManager.AnimalHouse animalHouse = dataManager.animalHouseData.FirstOrDefault(q => q.objectId == gameObject.name);
                        List<AnimalState> animalStates = new List<AnimalState>();

                        // thu hoach vao inventory o day
                        InventoryItem.ItemData obj =
                                InventoryItem.Instance.SearchItemByID(itemToAddInventory);

                        for (int i = 0; i < curNumberAnimal * coefficient; i++)
                        {
                            dataManager.AddItemToInventoryData(obj.prefabItemID, obj.type, obj.speType, obj.durability, int.Parse(DisplayInventory.Instance.SearchFirstEmptyUiSlot().name), obj.money);
                            DisplayInventory.Instance.UpdateUI();
                        }

                        // xu li animal sau khi thu hoach
                        foreach (var child in listAnimalState)
                        {

                            if (child.animalLife == 1 && animalHouse.animals.Count > 0)
                            {
                                var animalsToRemove = new List<Animal>();
                                foreach (var animal_ in animalHouse.animals)
                                {
                                    if (animal_.animalId == child.gameObject.name)
                                    {
                                        // xoa data
                                        animalsToRemove.Add(animal_);
                                        animalStates.Add(child);
                                        totalGrowthMax -= child.growthMax;
                                        curNumberAnimal--;

                                        //destroy - play anmation chet duoi nay 
                                        AnimalBehavior behavior = child.GetComponent<AnimalBehavior>();
                                        StartCoroutine(behavior.PlayDeath());

                                        // listAnimalState.Remove(child);
                                    }
                                }
                                foreach (var animalToRemove in animalsToRemove)
                                {
                                    animalHouse.animals.Remove(animalToRemove);
                                }
                            }
                            else
                            {

                                child.SwitchState(AnimalState.AnimalSate.Growth); // chuyen ve growth - giam animalLife trong switch roi
                                foreach (var animal_ in animalHouse.animals)
                                {
                                    if (animal_.animalId == child.gameObject.name)
                                    {
                                        // update data
                                        animal_.curLife--;
                                    }
                                }

                            }
                        }
                        foreach (var animalToRemove in animalStates)
                        {
                            listAnimalState.Remove(animalToRemove);
                        }
                        animalHouse.currentTime = 0;
                        totalNumberTimeHarvest = 0;
                    }
                }
                else
                {
                    //chua du tgian
                }
            }
            else
            {
                // khong co thu'
            }
        }
       
    }

    public void EditPosition()
    {
        Builder.Instance.StartEditPos(transform.gameObject, PrefabBuild.Instance.ReQuestItem(animalType.ToString()).Mesh_GameObject, new Vector3(0, 1, 0));
    }
    public void playDestroyVfx()
    {
        destroySound.Play();
        destroyVfx.Play();
    }
    public void StartDestroy()
    {
        StartCoroutine(DestroyBuilding());
    }
    IEnumerator DestroyBuilding()
    {
        yield return new WaitForSeconds(50.0f * Time.deltaTime);
        Destroy(this.gameObject);
    }
    IEnumerator ReCreateAnimal(DataManager.AnimalHouse animalHouse)
    {
        yield return new WaitUntil(() => dataManager.isLoadedData);

        foreach(DataManager.Animal animal in animalHouse.animals)
        {

            GameObject newAnimal = Instantiate(animalPrefab[curNumberAnimal % animalPrefab.Length], spawnPosition.position, Quaternion.identity);
            newAnimal.name = animal.animalId;
            newAnimal.transform.SetParent(this.transform);
            AnimalState state = newAnimal.GetComponent<AnimalState>();
            state.growth = animal.currentTime;
            state.animalHouse = this;
            AnimalBehavior behavior = newAnimal.GetComponent<AnimalBehavior>();
            behavior.animalHouseComponent = this;
            behavior.SwitchStateAction(AnimalBehavior.AnimalAction.GoingSomeWhere);
            listAnimalState.Add(state);
            curNumberAnimal++;
            totalNumberTimeHarvest += state.growth;
            totalGrowthMax += state.growthMax;
        }    
    }

    public void RotateTowardsTarget()
    {

        // Get the direction to the main camera
        //can.transform.LookAt(can.transform.position + Camera.main.transform.rotation * -Vector3.back, Camera.main.transform.rotation * Vector3.up);
        can.transform.rotation = Quaternion.Euler(Quaternion.LookRotation(can.transform.position - Camera.main.transform.position).eulerAngles.x, Quaternion.LookRotation(can.transform.position - Camera.main.transform.position).eulerAngles.y - 13.0f, 0);

    }
}
