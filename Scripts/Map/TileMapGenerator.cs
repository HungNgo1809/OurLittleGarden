using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using static Lands;
using System.Runtime.CompilerServices;
using UnityEngine.Playables;

public class TileMapGenerator : MonoBehaviour
{
    public objectToSpawn[] ObjectType;
    public TerrainToDraw[] TerrianType;

    public Tilemap tilemap;
    public Tilemap[] ground;
    //public int numberObject;
    public int maxMapX;
    public int maxMapZ;

    public DataManager dataManager;

    public float[] cumulativeFrequencies;
    float totalF;

    public bool isDrawMap;
    public bool isSpawnObject;
    public bool isReCreate;
    public bool isSaveTer;

    //public GameObject Player;
    void Start()
    {
        StartCoroutine(WaitDataForStart());
    }
    IEnumerator WaitDataForStart()
    {
        yield return new WaitUntil(() => dataManager.isLoadedData);

        if (dataManager.isOldbie == 1)
        {
            //Debug.Log("create");
            StartCoroutine(ReCreateMap());
            Debug.Log("recreate");
        }
        else
        {
            //Nếu tài khoản mới đăng ký
            dataManager.SaveDataPlayfab("friendRequest", "[]");
            dataManager.SaveDataPlayfab("friendRequestSend", "[]");

            cumulativeFrequencies = new float[TerrianType.Length];
            Debug.Log("newmap");
            StartCoroutine(StartCallSpawn());
            if (ground.Length > 0)
            {
                Debug.Log("draw");
                StartCoroutine(StartCallMapDraw());
            }
        }
    }
    IEnumerator StartCallSpawn()
    {
        yield return new WaitForSeconds(0.001f);
        CallSpawn();
    }
    IEnumerator StartCallMapDraw()
    {
        yield return new WaitForSeconds(0.001f);
        CallMapDraw();
    }
    private void Update()
    {
        if(isSaveTer)
        {
            return;
        }    

        if(isDrawMap && isSpawnObject)
        {
            //Player.SetActive(true);
            dataManager.SaveFarmTerData();
            isSaveTer = true;
        }    
    }
    public void CallMapDraw()
    {
        Vector3Int position;
        Vector3 spawnPosition;

        for (int x = 0; x < maxMapX; x++)
        {
            for (int z = 0; z < maxMapZ; z++)
            {
                //Lay vi tri cell
                position = new Vector3Int(x, z, 0);
                spawnPosition = ground[0].GetCellCenterWorld(position);
                
                //Check
                for(int i = 0; i < TerrianType.Length; i++)
                {
                    TerrianType[i].canPlace = CheckTerrian(x, z, i);
                }
                //Ve
                ChooseTerrain(x, z, spawnPosition);
            }
        }
        //yield return null;
    }

    public bool CheckTerrian(int x, int z, int index)
    {
        if (TerrianType[index].UpperLimitX < x ||
            TerrianType[index].LowerLimitX > x ||
            TerrianType[index].UpperlimitZ < z ||
            TerrianType[index].LowerLimitZ > z)
        {
            return false;
        }

        return true;
    }

    public void CalculateToTalFrequence()
    {
        totalF = 0;

        int i = 0;

        foreach (TerrainToDraw ter in TerrianType)
        {
            if(ter.canPlace == true)
            {
                totalF = totalF + ter.frequency;
                cumulativeFrequencies[i] = totalF;
            }
            else
            {
                cumulativeFrequencies[i] = -1;
            }
            i++;
        }
    }
    public void ChooseTerrain(int x, int z, Vector3 pos)
    {
        CalculateToTalFrequence();

        int ran = Random.Range(0, (int)totalF);

        for (int i = 0; i < TerrianType.Length; i++)
        {
            if ((ran < cumulativeFrequencies[i]) && (cumulativeFrequencies[i]>-1))
            {
                DrawMap(TerrianType[i].ter, pos, TerrianType[i].type, TerrianType[i].itemID);
                break;
            }
        }

        isDrawMap = true;
    }
    public void DrawMap(GameObject obj, Vector3 position, string type, string itemId)
    {
        // Get the bounds of the tilemap
        //BoundsInt bounds = ground.cellBounds;

        GameObject newObj = Instantiate(obj, position, Quaternion.identity);
        newObj.transform.SetParent(ground[0].transform);

        int ran = Random.Range(0, 100);
        if (ran <= 50 && ran > 25)
        {
            newObj.transform.rotation = Quaternion.Euler(newObj.transform.rotation.x, 90, newObj.transform.rotation.z);
        }
        else if (ran <= 75 && ran > 50)
        {
            newObj.transform.rotation = Quaternion.Euler(newObj.transform.rotation.x, 180, newObj.transform.rotation.z);
        }
        else if (ran > 75)
        {
            newObj.transform.rotation = Quaternion.Euler(newObj.transform.rotation.x, 270, newObj.transform.rotation.z);
        }

        //Add to object list
        ListObjectManager.Instance.terObject.Add(newObj);
        //Add to dataManager
        if(itemId != null)
        {
            dataManager.AddTerrainToData(newObj, type, itemId, "terrian");
        }    
    }
    
    public void CallSpawn()
    {
        for(int i =0; i < ObjectType.Length; i++)
        {
            SpawnObject(ObjectType[i].obj, ObjectType[i].numberObjectWantSpawn, ObjectType[i].UpperLimitX, ObjectType[i].LowerLimitX, ObjectType[i].UpperlimitZ, ObjectType[i].LowerLimitZ, ObjectType[i].type, ObjectType[i].itemID);
            //Debug.Log(ObjectType[i].type);
        }

        isSpawnObject = true;
        //yield return null;
    }
    public void SpawnObject(GameObject obj, int number, int UpperX, int LowerX, int UpperZ, int LowerZ, string type, string itemId)
    {
        // Get the bounds of the tilemap
        BoundsInt bounds = tilemap.cellBounds;

        for(int i = 0; i < number; i++)
        {
            Vector3Int position = new Vector3Int(Random.Range(LowerX, UpperX), Random.Range(LowerZ, UpperZ), 0);
            Vector3 spawnPosition = tilemap.GetCellCenterWorld(position);

            //spawnPosition = spawnPosition + SetPositonYInPlane(position) - tilemap.transform.position;

            int j = 0;
            while (TileHasObject(position) && j<100)
            {
                //Debug.Log("want:" + obj);
                position = new Vector3Int(Random.Range(0, maxMapX), Random.Range(0, maxMapZ), 0);
                spawnPosition = tilemap.GetCellCenterWorld(position);
                j++;
            }

            GameObject newObj = Instantiate(obj, spawnPosition, Quaternion.identity);
            newObj.transform.SetParent(tilemap.transform);

            int ran = Random.Range(0, 100);
            if (ran <= 50 && ran > 25)
            {
                newObj.transform.rotation = Quaternion.Euler(newObj.transform.rotation.x, 90, newObj.transform.rotation.z);
            }else if (ran <= 75 && ran > 50)
            {
                newObj.transform.rotation = Quaternion.Euler(newObj.transform.rotation.x, 180, newObj.transform.rotation.z);
            }else if(ran > 75)
            {
                newObj.transform.rotation = Quaternion.Euler(newObj.transform.rotation.x, 270, newObj.transform.rotation.z);
            }


            //Add to object list
            ListObjectManager.Instance.terObject.Add(newObj);
            //Add to data manager
            if ((itemId != null) && (type != "LargePlane"))
            {
                dataManager.AddTerrainToData(newObj, type, itemId, "object");
            }    
            //Debug.Log(itemId);
        }
    }

    #region hanhdleYPos
    /*
    public Vector3 SetPositonYInPlane(Vector3Int tilePos)
    {
        Vector3 worldPos = tilemap.GetCellCenterWorld(tilePos);

        Vector3 rayStart = worldPos + (Vector3.up * 0.1f);

        Vector3 output;

        // Cast a ray from the center of the cell and see if it hits any colliders
        RaycastHit hit;
        if (Physics.Raycast(rayStart, Vector3.down, out hit) && hit.collider.tag == "Plane")
        {
            return output = new Vector3(0, hit.point.y, 0);
        }

        return output = new Vector3(0, 0, 0);
    }*/
    #endregion
    public bool TileHasObject(Vector3Int tilePos)
    {
        Vector3 worldPos = tilemap.GetCellCenterWorld(tilePos);

        Vector3 rayStart = worldPos - (Vector3.up * 0.1f);

        // Cast a ray from the center of the cell and see if it hits any colliders
        RaycastHit hit;
        if (Physics.Raycast(rayStart, Vector3.up, out hit))
        {
            return true;
            //Debug.Log(hit.collider.gameObject);
        }

        return false;
    }
    
    public void StartReCreateMap()
    {
        StartCoroutine(ReCreateMap()); ;
    }
    IEnumerator ReCreateMap()
    {
        yield return new WaitForSeconds(0.01f);

        foreach (DataManager.TerrainData ter in dataManager.terData)
        {
            if (ter.objType == "terrian")
            {
                if (TerrianType.Where(p => p.itemID == ter.itemID).FirstOrDefault() != null)
                {
                    GameObject objSpawn = TerrianType.Where(p => p.itemID == ter.itemID).FirstOrDefault().ter;
                   
                  
                    Vector3 tmp = new Vector3();
                    tmp.x = ter.PosX;
                    tmp.y = ter.PosY;
                    tmp.z = ter.PosZ;

                    //Vector3 spawnPosition = ground[0].GetCellCenterWorld(tmp);

                    GameObject obj = Instantiate(objSpawn, tmp, Quaternion.identity);

                    //Add to object list
                    ListObjectManager.Instance.terObject.Add(obj);

                    obj.transform.rotation = Quaternion.Euler(obj.transform.rotation.x, ter.Rotation, obj.transform.rotation.z);

                    obj.transform.name = ter.objectID;

                    obj.transform.SetParent(ground[0].transform);


                    //Debug.Log("1");
                    Lands lands = obj.GetComponent<Lands>();
                    if (lands != null)
                    {
                        ListObjectManager.Instance.TerObject.Add(obj);

                        //Debug.Log("2");

                        if (ter.LandsMode == 0)
                        {

                            //Debug.Log("3");
                            lands.landStatus = LandStatus.FarmLand;
                    
                        }
                        if (ter.LandsMode == 1)
                        {

                            //Debug.Log("4");
                            lands.isWater = true;
                            lands.isLoad = true;
                            lands.landStatus = LandStatus.Watered;
                            lands.timeWatered = GameTimestamp.CalculateApproximateTimestamp(TimeManager.Instance.timestamp ,ter.wateredTime);
                            lands.ReloadWateredLand();
                        }
           
                    }
                 
                }
            }
            if (ter.objType == "object")
            {
                if(ObjectType.Where(p => p.itemID == ter.itemID).FirstOrDefault() != null)
                {
                    GameObject objSpawn = ObjectType.Where(p => p.itemID == ter.itemID).FirstOrDefault().obj;

                    Vector3 tmp = new Vector3();
                    tmp.x = ter.PosX;
                    tmp.y = ter.PosY;
                    tmp.z = ter.PosZ;

                    GameObject obj = Instantiate(objSpawn, tmp, Quaternion.identity);

                    //Add to object list
                    ListObjectManager.Instance.terObject.Add(obj);

                    obj.transform.rotation = Quaternion.Euler(obj.transform.rotation.x, ter.Rotation, obj.transform.rotation.z);

                    obj.transform.name = ter.objectID;

                    obj.transform.SetParent(tilemap.transform);
                }
            }
        }
        isReCreate = true;
                    //Player.SetActive(true);
    }

    public void ClockUpdate(GameTimestamp timestamp)
    {
        throw new System.NotImplementedException();
    }  
    [System.Serializable]
    public class objectToSpawn
    {
        public string itemID;

        public GameObject obj;
        public int numberObjectWantSpawn;

        public int UpperLimitX;
        public int LowerLimitX;

        public int UpperlimitZ;
        public int LowerLimitZ;

        public string type;
    }

    [System.Serializable]
    public class TerrainToDraw
    {
        public string itemID;

        public GameObject ter;

        public int UpperLimitX;
        public int LowerLimitX;

        public int UpperlimitZ;
        public int LowerLimitZ;

        public float frequency;

        public bool canPlace;

        public string type;
    }
}

