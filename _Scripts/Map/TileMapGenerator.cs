using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

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
    
    void Start()
    {
        //Nếu đã đăng ký thì load dữ liệu từ data
            //do something

        //Nếu tài khoản mới đăng ký
        cumulativeFrequencies = new float[TerrianType.Length];
        CallSpawn();
        if (ground.Length > 0)
        {
            CallMapDraw();
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

        //Add to dataManager
        if(itemId != null)
        {
            dataManager.AddTerrainToData(newObj, type, itemId);
        }    
    }
    
    public void CallSpawn()
    {
        for(int i =0; i < ObjectType.Length; i++)
        {
            SpawnObject(ObjectType[i].obj, ObjectType[i].numberObjectWantSpawn, ObjectType[i].UpperLimitX, ObjectType[i].LowerLimitX, ObjectType[i].UpperlimitZ, ObjectType[i].LowerLimitZ, ObjectType[i].type, ObjectType[i].itemID);
            //Debug.Log(ObjectType[i].type);
        }
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

            //Add to data manager
            if((itemId != null) && (type != "LargePlane"))
            {
                dataManager.AddTerrainToData(newObj, type, itemId);
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

