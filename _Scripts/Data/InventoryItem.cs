using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    public static InventoryItem Instance { get; set; }
    public List<ItemData> items;

    //public DataManager dataManager;

    private void Start()
    {
        DontDestroyOnLoad(this);

        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public ItemData SearchItemByID(string id)
    {
        return items.Where(p => p.prefabItemID == id).FirstOrDefault();
    }

    public ItemData SearchItemByObjectUIName(string name)
    {
        return items.Where(p => p.objUI.name == name).FirstOrDefault();
    }


    [System.Serializable]
    public class ItemData
    {
        public string prefabItemID;
        
        //public GameObject realObj;
        public GameObject objUI;

        public string type;
        public string speType;

        public float durability;
        public int money;
    }
}
