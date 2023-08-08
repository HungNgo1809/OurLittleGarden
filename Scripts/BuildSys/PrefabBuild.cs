using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PrefabBuild : MonoBehaviour
{
    public List<PrefabBuildItem> prefabsItems = new List<PrefabBuildItem>();
    public static PrefabBuild Instance { private set; get; }
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        //Debug.Log(gameObject.name);
    }

    public PrefabBuildItem ReQuestItem(string Prefab_ID_)
    {
        PrefabBuildItem prefabItem = prefabsItems.Where(p => p.Prefab_ID == Prefab_ID_).First();
     
        return prefabItem;
    }

    [System.Serializable]
    public class PrefabBuildItem
    {
        public string Prefab_ID;

        public GameObject Prefab_GameObject;
        public string Type;

        public GameObject Mesh_GameObject;
    }
}
