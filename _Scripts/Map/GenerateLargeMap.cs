//using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public class GenerateLargeMap : MonoBehaviour
{
    public float waterLevel = .4f;
    public float sandLevel = 0.45f;

    public float savalBot = 0.9f;
    public float savalHead = 1f;

    public float scale = .1f;
    public int size = 100;

    [SerializeField]
    public Cell[,] grid;

    public Tilemap ter;

    public GameObject Grass;
    public GameObject Sand;
    public GameObject Dirt;

    public GlobalInteract boxOfApple;

    bool isSetSpawnPosition;
    [SerializeField]
    public List<ObjectToDraw> MultiOjects;

    [SerializeField]
    public List<ObjectToDraw> singleObjects;

    public bool isDrawed;

    public LargeMapData largeMapData;

    private void Start()
    {
        StartCoroutine(DelayStart());
        //CreateNewMap();
    }   
        
    IEnumerator DelayStart()
    {
        yield return new WaitUntil(() => largeMapData.loadSuccess);
        //yield return null;
        if (/*largeMapData.terData.Count > 0 ||*/ largeMapData.CalculateDay(largeMapData.createMapDay) < largeMapData.resetMapDay)
        {
            grid = new Cell[size, size];
            ReCreateMap();

            boxOfApple.SetPos();
            largeMapData.loadSuccess = false;
            if (largeMapData.objData.Count <= 0 || largeMapData.CalculateDay(largeMapData.createObjDay) >= largeMapData.resetObjDay)
            {
                //Debug.Log("call");
                largeMapData.objData.Clear();
                StartCoroutine(StartDrawObjectAfterReCreateMap());
            }
        }
        else
        {
            largeMapData.ClearLargeMapData();
            CreateNewMap();
        }
    }
    public void CreateNewMap()
    {
        DrawMap();

        StartCoroutine(StartDrawObject(grid));

        largeMapData.isCreateNewMap = true;
    }    

    //Vẽ map
    public void DrawMap()
    {
        float[,] noiseMap = new float[size, size];
        (float xOffset, float yOffset) = (Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = Mathf.PerlinNoise(x * scale + xOffset, y * scale + yOffset);
                noiseMap[x, y] = noiseValue;
            }
        }

        float[,] falloffMap = new float[size, size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float xv = x / (float)size * 2 - 1;
                float yv = y / (float)size * 2 - 1;
                float v = Mathf.Max(Mathf.Abs(xv), Mathf.Abs(yv));
                falloffMap[x, y] = Mathf.Pow(v, 3f) / (Mathf.Pow(v, 3f) + Mathf.Pow(2.2f - 2.2f * v, 3f));
            }
        }

        grid = new Cell[size, size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = noiseMap[x, y];
                noiseValue -= falloffMap[x, y];

                Vector3Int position = new Vector3Int(x, y, 0);
                Vector3 spawnPosition = ter.GetCellCenterWorld(position);

                Cell cell = new Cell(0);
                if (noiseValue < waterLevel)
                {
                    cell = new Cell(0);
                }
                if (noiseValue >= waterLevel && noiseValue < sandLevel)
                {
                    cell = new Cell(1);
                    Create3D(x, y, 1, Sand, new Vector3(spawnPosition.x, 0, spawnPosition.z), ter, "sand", "ter");
                }
                if (noiseValue >= sandLevel && noiseValue < savalBot)
                {
                    cell = new Cell(2);
                    if (noiseValue <= (sandLevel + savalBot) / 2)
                    {
                        Create3D(x, y, 2, Grass, new Vector3(spawnPosition.x, 0, spawnPosition.z), ter, "grass", "ter");
                        //Set spawnPosition;
                        if(!isSetSpawnPosition)
                        {
                            largeMapData.spawnPosition = new Vector3(spawnPosition.x, 2, spawnPosition.z);
                            isSetSpawnPosition = true;
                        }    
                    }
                    else if (noiseValue > (sandLevel + savalBot) / 2 && noiseValue <= (sandLevel + savalBot) / 1.5)
                    {
                        Create3D(x, y, 2, Grass, new Vector3(spawnPosition.x, 1, spawnPosition.z), ter, "grass", "ter");
                    }
                    else
                    {
                        Create3D(x, y, 2, Grass, new Vector3(spawnPosition.x, 2, spawnPosition.z), ter, "grass", "ter");
                    }

                }
                if (noiseValue >= savalBot && noiseValue < savalHead)
                {
                    cell = new Cell(3);
                    if (noiseValue <= (savalBot + savalHead) / 1.5)
                    {
                        Create3D(x, y, 3, Dirt, new Vector3(spawnPosition.x, 1, spawnPosition.z), ter, "dirt", "ter");
                    }
                    else
                    {
                        Create3D(x, y, 3, Dirt, new Vector3(spawnPosition.x, 2, spawnPosition.z), ter, "dirt", "ter");
                    }
                }
                if (noiseValue >= savalHead)
                {
                    cell = new Cell(0);
                }

                grid[x, y] = cell;

                //Debug.Log(x + " " + y + " " + " " + grid[x, y].state);
            }
        }

        if (grid != null)
        {
            isDrawed = true;
            largeMapData.createMapDay = System.DateTime.Now;
        }
    }

    //Lưu vào scripTableObject
    public void SaveTerData(int cellX, int cellY, int cellState, Vector3 pos, float rotation, string itemID, string objType, GameObject obj)
    {
        if(objType == "ter")
        {
            largeMapData.AddTerrainToData(cellX, cellY, cellState, pos, rotation, itemID, objType);
        }else if(objType == "obj")
        {
            largeMapData.AddObjToData(pos, rotation, itemID, objType, obj);
        }
        else if (objType == "objSingle")
        {
            largeMapData.AddObjToData(pos, rotation, itemID, objType, obj);
        }

    }
    
    //Khởi tạo đối tượng 3D
    public void Create3D(int cellX, int cellY, int cellState, GameObject obj, Vector3 position, Tilemap ground, string itemID, string objType)
    {
        GameObject newObj = Instantiate(obj, position, Quaternion.identity);
        newObj.transform.SetParent(ground.transform);

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

        SaveTerData(cellX, cellY, cellState, newObj.transform.position, newObj.transform.rotation.y, itemID, objType, newObj);   
    }

    //Bắt đầu khởi tạo object đàn
    IEnumerator StartDrawObjectAfterReCreateMap()
    {
        yield return new WaitUntil(() => isDrawed);

        StartCoroutine(StartDrawObject(grid));
    }
    IEnumerator StartDrawObject(Cell[,] grid)
    {
        yield return new WaitUntil(() => isDrawed);

        //Debug.Log("run");
        if (MultiOjects.Count > 0);
        {
            foreach (ObjectToDraw objectToDraw in MultiOjects)
            {
                GenerateObject(grid, objectToDraw, objectToDraw.state, objectToDraw.density);
            }
        }
        if(singleObjects.Count > 0)
        {
            foreach (ObjectToDraw objectToDraw in singleObjects)
            {
                CreateSingleObj(grid, objectToDraw, objectToDraw.state, objectToDraw.density);
            }
        }
        largeMapData.createObjDay = System.DateTime.Now;
    }

    //Khởi tạo object đàn
    public void GenerateObject(Cell[,] grid, ObjectToDraw obj, int state, float density)
    {
        float[,] noiseMap = new float[size, size];
        (float xOffset, float yOffset) = (Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = Mathf.PerlinNoise(x * scale + xOffset, y * scale + yOffset);
                noiseMap[x, y] = noiseValue;
            }
        }

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                Vector3 spawnPosition = ter.GetCellCenterWorld(position);

                //Debug.Log(grid[x, y]);
                if(grid[x,y] != null)
                {
                    Cell cell = grid[x, y];
                    if (cell.state == state)
                    {
                        float v = Random.Range(0f, density);
                        if (noiseMap[x, y] < v)
                        {
                            Create3D(x, y, state, obj.prefab, spawnPosition + new Vector3(0, obj.spawnY, 0), ter, obj.itemID, "obj");

                            cell.state = 5;
                        }
                    }
                }
            }
        }
    }

    //Khởi tạo object đơn
    public void CreateSingleObj(Cell[,] grid, ObjectToDraw obj, int state, float density)
    {
        float[,] noiseMap = new float[size, size];

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = Random.Range(0f, 1f);
                noiseMap[x, y] = noiseValue;
            }
        }

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                Vector3 spawnPosition = ter.GetCellCenterWorld(position);

                //Debug.Log(grid[x, y]);
                if(grid[x,y] != null)
                {
                    Cell cell = grid[x, y];
                    if (cell.state == state)
                    {
                        float v = Random.Range(0f, density);

                        //Debug.Log(noiseMap[x, y] + "/" + v);
                        if (noiseMap[x, y] < v)
                        {
                            Create3D(x, y, state, obj.prefab, spawnPosition + new Vector3(0, obj.spawnY, 0), ter, obj.itemID, "objSingle");

                            cell.state = 5;
                        }
                    }
                }
            }
        }
    }    
    //Tạo lại map ở client
    public void ReCreateMap()
    {
        foreach (LargeMapData.TerrainData ter in largeMapData.terData)
        {
            if (ter.objType == "ter")
            {
                GameObject objSpawn = Grass;
                grid[ter.cellData.x, ter.cellData.y] = new Cell(ter.cellData.state);

                if (ter.itemID == "sand")
                {
                    objSpawn = Sand;
                }else if(ter.itemID == "dirt")
                {
                    objSpawn = Dirt;
                }
                else if (ter.itemID == "grass")
                {
                    objSpawn = Grass;
                }

                Vector3 tmp = new Vector3();
                tmp.x = ter.PosX;
                tmp.y = ter.PosY;
                tmp.z = ter.PosZ;

                //Vector3 spawnPosition = ground[0].GetCellCenterWorld(tmp);

                GameObject obj = Instantiate(objSpawn, tmp, Quaternion.identity);

                obj.transform.rotation = Quaternion.Euler(obj.transform.rotation.x, ter.Rotation, obj.transform.rotation.z);

                obj.transform.name = ter.objectID;

                obj.transform.SetParent(this.ter.transform);

               
              
            }
        }
        isDrawed = true;
        foreach (LargeMapData.TerrainData ter in largeMapData.objData)
        {
            if (ter.objType == "obj")
            {
                if (MultiOjects.Where(p => p.itemID == ter.itemID).FirstOrDefault() != null)
                {
                    GameObject objSpawn = MultiOjects.Where(p => p.itemID == ter.itemID).FirstOrDefault().prefab;

                    Vector3 tmp = new Vector3();
                    tmp.x = ter.PosX;
                    tmp.y = ter.PosY;
                    tmp.z = ter.PosZ;

                    GameObject obj = Instantiate(objSpawn, tmp, Quaternion.identity);
                    obj.transform.rotation = Quaternion.Euler(obj.transform.rotation.x, ter.Rotation, obj.transform.rotation.z);

                    obj.transform.name = ter.objectID;

                    obj.transform.SetParent(this.ter.transform);
                }
            }
            else if (ter.objType == "objSingle")
            {
                // khởi tạo lại trong list single
                if (singleObjects.Where(p => p.itemID == ter.itemID).FirstOrDefault() != null)
                {
                    GameObject objSpawn = singleObjects.Where(p => p.itemID == ter.itemID).FirstOrDefault().prefab;

                    Vector3 tmp = new Vector3();
                    tmp.x = ter.PosX;
                    tmp.y = ter.PosY;
                    tmp.z = ter.PosZ;

                    GameObject obj = Instantiate(objSpawn, tmp, Quaternion.identity);
                    obj.transform.rotation = Quaternion.Euler(obj.transform.rotation.x, ter.Rotation, obj.transform.rotation.z);

                    obj.transform.name = ter.objectID;

                    obj.transform.SetParent(this.ter.transform);
                }
            }
        }    
    }
    [System.Serializable]
    public class ObjectToDraw
    {
        public string itemID;

        public GameObject prefab;
        public int state;
        public float density;

        public float spawnY;
    }
}