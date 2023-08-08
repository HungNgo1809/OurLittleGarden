using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using Newtonsoft.Json;
using System.Linq;
using System.Xml.Serialization;
using System.IO;
using System;

[CreateAssetMenu(fileName = "NewData", menuName = "LargeMapData")]
public class LargeMapData : ScriptableObject
{
    public DataManager dataManager;
    public List<TerrainData> terData;
    public List<TerrainData> objData;

    //public List<Grid> grid;

    public DateTime createMapDay;
    public DateTime createObjDay;

    public int saveCount;
    public bool loadSuccess;

    public Vector3 spawnPosition;

    public bool isCreateNewMap;

    public int resetMapDay;
    public int resetObjDay;
    public void RemoveObjFromData(string objId)
    {
        TerrainData ter = SearchObjInDataByObjectId(objId);
        objData.Remove(ter);
    }
    public TerrainData SearchObjInDataByObjectId(string id)
    {
        return objData.Where(p => p.objectID == id).FirstOrDefault();
    }

    public void AddTerrainToData(int cellX, int cellY, int cellState, Vector3 pos, float rotation, string itemID, string objType)
    {
        TerrainData ter = new TerrainData();

        //ter.block = obj.gameObject;

        ter.PosX = pos.x;
        ter.PosY = pos.y;
        ter.PosZ = pos.z;

        ter.Rotation = rotation;

        ter.itemID = itemID;
        ter.objectID = itemID + "_" + terData.Count;

        ter.objType = objType;

        ter.cellData = new Grid();
        ter.cellData.x = cellX;
        ter.cellData.y = cellY;
        ter.cellData.state = cellState;

        terData.Add(ter);
    }


    public void AddObjToData(Vector3 pos, float rotation, string itemID, string objType, GameObject obj)
    {
        TerrainData ter = new TerrainData();

        //ter.block = obj.gameObject;

        ter.PosX = pos.x;
        ter.PosY = pos.y;
        ter.PosZ = pos.z;

        ter.Rotation = rotation;

        ter.itemID = itemID;
        ter.objectID = itemID + "_" + objData.Count;

        obj.name = ter.objectID;
        ter.objType = objType;
        ter.cellData = new Grid();

        objData.Add(ter);
    }
    public void SaveCreateDayData()
    {
        SaveDataToAdmin("createMapDay", createMapDay.ToString());
        SaveDataToAdmin("createObjDay", createObjDay.ToString());
    }    
    public void SaveSpawnPosition()
    {
        Vector3ToSaveLoad position = new Vector3ToSaveLoad();

        position.x = spawnPosition.x;
        position.y = spawnPosition.y;
        position.z = spawnPosition.z;

        SaveDataToAdmin("spawnPosition", JsonConvert.SerializeObject(position)); ;
    }    
    public void SaveLarObjData()
    {
        int totalCount = objData.Count;
        int Count = totalCount / 20;

        List<TerrainData> selectedData1 = objData.Take(Count).ToList();
        List<TerrainData> selectedData2 = objData.Skip(Count).Take(Count).ToList();
        List<TerrainData> selectedData3 = objData.Skip(Count * 2).Take(Count).ToList();
        List<TerrainData> selectedData4 = objData.Skip(Count * 3).Take(Count).ToList();
        List<TerrainData> selectedData5 = objData.Skip(Count * 4).Take(Count).ToList();
        List<TerrainData> selectedData6 = objData.Skip(Count * 5).Take(Count).ToList();
        List<TerrainData> selectedData7 = objData.Skip(Count * 6).Take(Count).ToList();
        List<TerrainData> selectedData8 = objData.Skip(Count * 7).Take(Count).ToList();
        List<TerrainData> selectedData9 = objData.Skip(Count * 8).Take(Count).ToList();
        List<TerrainData> selectedData10 = objData.Skip(Count * 9).Take(Count).ToList();
        List<TerrainData> selectedData11 = objData.Skip(Count * 10).Take(Count).ToList();
        List<TerrainData> selectedData12 = objData.Skip(Count * 11).Take(Count).ToList();
        List<TerrainData> selectedData13 = objData.Skip(Count * 12).Take(Count).ToList();
        List<TerrainData> selectedData14 = objData.Skip(Count * 13).Take(Count).ToList();
        List<TerrainData> selectedData15 = objData.Skip(Count * 14).Take(Count).ToList();
        List<TerrainData> selectedData16 = objData.Skip(Count * 15).Take(Count).ToList();
        List<TerrainData> selectedData17 = objData.Skip(Count * 16).Take(Count).ToList();
        List<TerrainData> selectedData18 = objData.Skip(Count * 17).Take(Count).ToList();
        List<TerrainData> selectedData19 = objData.Skip(Count * 18).Take(Count).ToList();
        List<TerrainData> selectedData20 = objData.Skip(Count * 19).ToList();

        string json1 = JsonConvert.SerializeObject(selectedData1);
        string json2 = JsonConvert.SerializeObject(selectedData2);
        string json3 = JsonConvert.SerializeObject(selectedData3);
        string json4 = JsonConvert.SerializeObject(selectedData4);
        string json5 = JsonConvert.SerializeObject(selectedData5);
        string json6 = JsonConvert.SerializeObject(selectedData6);
        string json7 = JsonConvert.SerializeObject(selectedData7);
        string json8 = JsonConvert.SerializeObject(selectedData8);
        string json9 = JsonConvert.SerializeObject(selectedData9);
        string json10 = JsonConvert.SerializeObject(selectedData10);
        string json11 = JsonConvert.SerializeObject(selectedData11);
        string json12 = JsonConvert.SerializeObject(selectedData12);
        string json13 = JsonConvert.SerializeObject(selectedData13);
        string json14 = JsonConvert.SerializeObject(selectedData14);
        string json15 = JsonConvert.SerializeObject(selectedData15);
        string json16 = JsonConvert.SerializeObject(selectedData16);
        string json17 = JsonConvert.SerializeObject(selectedData17);
        string json18 = JsonConvert.SerializeObject(selectedData18);
        string json19 = JsonConvert.SerializeObject(selectedData19);
        string json20 = JsonConvert.SerializeObject(selectedData20);

        SaveDataToAdmin("objData1", json1);
        SaveDataToAdmin("objData2", json2);
        SaveDataToAdmin("objData3", json3);
        SaveDataToAdmin("objData4", json4);
        SaveDataToAdmin("objData5", json5);
        SaveDataToAdmin("objData6", json6);
        SaveDataToAdmin("objData7", json7);
        SaveDataToAdmin("objData8", json8);
        SaveDataToAdmin("objData9", json9);
        SaveDataToAdmin("objData10", json10);
        SaveDataToAdmin("objData11", json11);
        SaveDataToAdmin("objData12", json12);
        SaveDataToAdmin("objData13", json13);
        SaveDataToAdmin("objData14", json14);
        SaveDataToAdmin("objData15", json15);
        SaveDataToAdmin("objData16", json16);
        SaveDataToAdmin("objData17", json17);
        SaveDataToAdmin("objData18", json18);
        SaveDataToAdmin("objData19", json19);
        SaveDataToAdmin("objData20", json20);
    }    
    public void SaveLarTerData()
    {
        int totalCount = terData.Count;
        int Count = totalCount / 20;

        List<TerrainData> selectedData1 = terData.Take(Count).ToList();
        List<TerrainData> selectedData2 = terData.Skip(Count).Take(Count).ToList();
        List<TerrainData> selectedData3 = terData.Skip(Count * 2).Take(Count).ToList();
        List<TerrainData> selectedData4 = terData.Skip(Count * 3).Take(Count).ToList();
        List<TerrainData> selectedData5 = terData.Skip(Count * 4).Take(Count).ToList();
        List<TerrainData> selectedData6 = terData.Skip(Count * 5).Take(Count).ToList();
        List<TerrainData> selectedData7 = terData.Skip(Count * 6).Take(Count).ToList();
        List<TerrainData> selectedData8 = terData.Skip(Count * 7).Take(Count).ToList();
        List<TerrainData> selectedData9 = terData.Skip(Count * 8).Take(Count).ToList();
        List<TerrainData> selectedData10 = terData.Skip(Count * 9).Take(Count).ToList();
        List<TerrainData> selectedData11 = terData.Skip(Count * 10).Take(Count).ToList();
        List<TerrainData> selectedData12 = terData.Skip(Count * 11).Take(Count).ToList();
        List<TerrainData> selectedData13 = terData.Skip(Count * 12).Take(Count).ToList();
        List<TerrainData> selectedData14 = terData.Skip(Count * 13).Take(Count).ToList();
        List<TerrainData> selectedData15 = terData.Skip(Count * 14).Take(Count).ToList();
        List<TerrainData> selectedData16 = terData.Skip(Count * 15).Take(Count).ToList();
        List<TerrainData> selectedData17 = terData.Skip(Count * 16).Take(Count).ToList();
        List<TerrainData> selectedData18 = terData.Skip(Count * 17).Take(Count).ToList();
        List<TerrainData> selectedData19 = terData.Skip(Count * 18).Take(Count).ToList();
        List<TerrainData> selectedData20 = terData.Skip(Count * 19).ToList();

        string json1 = JsonConvert.SerializeObject(selectedData1);
        string json2 = JsonConvert.SerializeObject(selectedData2);
        string json3 = JsonConvert.SerializeObject(selectedData3);
        string json4 = JsonConvert.SerializeObject(selectedData4);
        string json5 = JsonConvert.SerializeObject(selectedData5);
        string json6 = JsonConvert.SerializeObject(selectedData6);
        string json7 = JsonConvert.SerializeObject(selectedData7);
        string json8 = JsonConvert.SerializeObject(selectedData8);
        string json9 = JsonConvert.SerializeObject(selectedData9);
        string json10 = JsonConvert.SerializeObject(selectedData10);
        string json11 = JsonConvert.SerializeObject(selectedData11);
        string json12 = JsonConvert.SerializeObject(selectedData12);
        string json13 = JsonConvert.SerializeObject(selectedData13);
        string json14 = JsonConvert.SerializeObject(selectedData14);
        string json15 = JsonConvert.SerializeObject(selectedData15);
        string json16 = JsonConvert.SerializeObject(selectedData16);
        string json17 = JsonConvert.SerializeObject(selectedData17);
        string json18 = JsonConvert.SerializeObject(selectedData18);
        string json19 = JsonConvert.SerializeObject(selectedData19);
        string json20 = JsonConvert.SerializeObject(selectedData20);

        SaveDataToAdmin("terData1", json1);
        SaveDataToAdmin("terData2", json2);
        SaveDataToAdmin("terData3", json3);
        SaveDataToAdmin("terData4", json4);
        SaveDataToAdmin("terData5", json5);
        SaveDataToAdmin("terData6", json6);
        SaveDataToAdmin("terData7", json7);
        SaveDataToAdmin("terData8", json8);
        SaveDataToAdmin("terData9", json9);
        SaveDataToAdmin("terData10", json10);
        SaveDataToAdmin("terData11", json11);
        SaveDataToAdmin("terData12", json12);
        SaveDataToAdmin("terData13", json13);
        SaveDataToAdmin("terData14", json14);
        SaveDataToAdmin("terData15", json15);
        SaveDataToAdmin("terData16", json16);
        SaveDataToAdmin("terData17", json17);
        SaveDataToAdmin("terData18", json18);
        SaveDataToAdmin("terData19", json19);
        SaveDataToAdmin("terData20", json20);
    } 
    /*
    public void SaveGridTerData()
    {
        int totalCount = grid.Count;
        int Count = totalCount / 40;

        List<Grid> selectedData1 = grid.Take(Count).ToList();
        List<Grid> selectedData2 = grid.Skip(Count).Take(Count).ToList();
        List<Grid> selectedData3 = grid.Skip(Count * 2).Take(Count).ToList();
        List<Grid> selectedData4 = grid.Skip(Count * 3).Take(Count).ToList();
        List<Grid> selectedData5 = grid.Skip(Count * 4).Take(Count).ToList();
        List<Grid> selectedData6 = grid.Skip(Count * 5).Take(Count).ToList();
        List<Grid> selectedData7 = grid.Skip(Count * 6).Take(Count).ToList();
        List<Grid> selectedData8 = grid.Skip(Count * 7).Take(Count).ToList();
        List<Grid> selectedData9 = grid.Skip(Count * 8).Take(Count).ToList();
        List<Grid> selectedData10 = grid.Skip(Count * 9).Take(Count).ToList();
        List<Grid> selectedData11 = grid.Skip(Count * 10).Take(Count).ToList();
        List<Grid> selectedData12 = grid.Skip(Count * 11).Take(Count).ToList();
        List<Grid> selectedData13 = grid.Skip(Count * 12).Take(Count).ToList();
        List<Grid> selectedData14 = grid.Skip(Count * 13).Take(Count).ToList();
        List<Grid> selectedData15 = grid.Skip(Count * 14).Take(Count).ToList();
        List<Grid> selectedData16 = grid.Skip(Count * 15).Take(Count).ToList();
        List<Grid> selectedData17 = grid.Skip(Count * 16).Take(Count).ToList();
        List<Grid> selectedData18 = grid.Skip(Count * 17).Take(Count).ToList();
        List<Grid> selectedData19 = grid.Skip(Count * 18).Take(Count).ToList();
        List<Grid> selectedData20 = grid.Skip(Count * 19).Take(Count).ToList();
        List<Grid> selectedData21 = grid.Skip(Count * 20).Take(Count).ToList();
        List<Grid> selectedData22 = grid.Skip(Count * 21).Take(Count).ToList();
        List<Grid> selectedData23 = grid.Skip(Count * 22).Take(Count).ToList();
        List<Grid> selectedData24 = grid.Skip(Count * 23).Take(Count).ToList();
        List<Grid> selectedData25 = grid.Skip(Count * 24).Take(Count).ToList();
        List<Grid> selectedData26 = grid.Skip(Count * 25).Take(Count).ToList();
        List<Grid> selectedData27 = grid.Skip(Count * 26).Take(Count).ToList();
        List<Grid> selectedData28 = grid.Skip(Count * 27).Take(Count).ToList();
        List<Grid> selectedData29 = grid.Skip(Count * 28).Take(Count).ToList();
        List<Grid> selectedData30 = grid.Skip(Count * 29).Take(Count).ToList();
        List<Grid> selectedData31 = grid.Skip(Count * 30).Take(Count).ToList();
        List<Grid> selectedData32 = grid.Skip(Count * 31).Take(Count).ToList();
        List<Grid> selectedData33 = grid.Skip(Count * 32).Take(Count).ToList();
        List<Grid> selectedData34 = grid.Skip(Count * 33).Take(Count).ToList();
        List<Grid> selectedData35 = grid.Skip(Count * 34).Take(Count).ToList();
        List<Grid> selectedData36 = grid.Skip(Count * 35).Take(Count).ToList();
        List<Grid> selectedData37 = grid.Skip(Count * 36).Take(Count).ToList();
        List<Grid> selectedData38 = grid.Skip(Count * 37).Take(Count).ToList();
        List<Grid> selectedData39 = grid.Skip(Count * 38).Take(Count).ToList();
        List<Grid> selectedData40 = grid.Skip(Count * 39).ToList();


        string json1 = JsonConvert.SerializeObject(selectedData1);
        string json2 = JsonConvert.SerializeObject(selectedData2);
        string json3 = JsonConvert.SerializeObject(selectedData3);
        string json4 = JsonConvert.SerializeObject(selectedData4);
        string json5 = JsonConvert.SerializeObject(selectedData5);
        string json6 = JsonConvert.SerializeObject(selectedData6);
        string json7 = JsonConvert.SerializeObject(selectedData7);
        string json8 = JsonConvert.SerializeObject(selectedData8);
        string json9 = JsonConvert.SerializeObject(selectedData9);
        string json10 = JsonConvert.SerializeObject(selectedData10);
        string json11 = JsonConvert.SerializeObject(selectedData11);
        string json12 = JsonConvert.SerializeObject(selectedData12);
        string json13 = JsonConvert.SerializeObject(selectedData13);
        string json14 = JsonConvert.SerializeObject(selectedData14);
        string json15 = JsonConvert.SerializeObject(selectedData15);
        string json16 = JsonConvert.SerializeObject(selectedData16);
        string json17 = JsonConvert.SerializeObject(selectedData17);
        string json18 = JsonConvert.SerializeObject(selectedData18);
        string json19 = JsonConvert.SerializeObject(selectedData19);
        string json20 = JsonConvert.SerializeObject(selectedData20);
        string json21 = JsonConvert.SerializeObject(selectedData21);
        string json22 = JsonConvert.SerializeObject(selectedData22);
        string json23 = JsonConvert.SerializeObject(selectedData23);
        string json24 = JsonConvert.SerializeObject(selectedData24);
        string json25 = JsonConvert.SerializeObject(selectedData25);
        string json26 = JsonConvert.SerializeObject(selectedData26);
        string json27 = JsonConvert.SerializeObject(selectedData27);
        string json28 = JsonConvert.SerializeObject(selectedData28);
        string json29 = JsonConvert.SerializeObject(selectedData29);
        string json30 = JsonConvert.SerializeObject(selectedData30);
        string json31 = JsonConvert.SerializeObject(selectedData31);
        string json32 = JsonConvert.SerializeObject(selectedData32);
        string json33 = JsonConvert.SerializeObject(selectedData33);
        string json34 = JsonConvert.SerializeObject(selectedData34);
        string json35 = JsonConvert.SerializeObject(selectedData35);
        string json36 = JsonConvert.SerializeObject(selectedData36);
        string json37 = JsonConvert.SerializeObject(selectedData37);
        string json38 = JsonConvert.SerializeObject(selectedData38);
        string json39 = JsonConvert.SerializeObject(selectedData39);
        string json40 = JsonConvert.SerializeObject(selectedData40);

        SaveDataToAdmin("grid1", json1);
        SaveDataToAdmin("grid2", json2);
        SaveDataToAdmin("grid3", json3);
        SaveDataToAdmin("grid4", json4);
        SaveDataToAdmin("grid5", json5);
        SaveDataToAdmin("grid6", json6);
        SaveDataToAdmin("grid7", json7);
        SaveDataToAdmin("grid8", json8);
        SaveDataToAdmin("grid9", json9);
        SaveDataToAdmin("grid10", json10);
        SaveDataToAdmin("grid11", json11);
        SaveDataToAdmin("grid12", json12);
        SaveDataToAdmin("grid13", json13);
        SaveDataToAdmin("grid14", json14);
        SaveDataToAdmin("grid15", json15);
        SaveDataToAdmin("grid16", json16);
        SaveDataToAdmin("grid17", json17);
        SaveDataToAdmin("grid18", json18);
        SaveDataToAdmin("grid19", json19);
        SaveDataToAdmin("grid20", json20);
        SaveDataToAdmin("grid21", json21);
        SaveDataToAdmin("grid22", json22);
        SaveDataToAdmin("grid23", json23);
        SaveDataToAdmin("grid24", json24);
        SaveDataToAdmin("grid25", json25);
        SaveDataToAdmin("grid26", json26);
        SaveDataToAdmin("grid27", json27);
        SaveDataToAdmin("grid28", json28);
        SaveDataToAdmin("grid29", json29);
        SaveDataToAdmin("grid30", json30);
        SaveDataToAdmin("grid31", json31);
        SaveDataToAdmin("grid32", json32);
        SaveDataToAdmin("grid33", json33);
        SaveDataToAdmin("grid34", json34);
        SaveDataToAdmin("grid35", json35);
        SaveDataToAdmin("grid36", json36);
        SaveDataToAdmin("grid37", json37);
        SaveDataToAdmin("grid38", json38);
        SaveDataToAdmin("grid39", json39);
        SaveDataToAdmin("grid40", json40);
    }*/    
    public void SaveDataToAdmin(string key, string data)
    {
        var request = new PlayFab.AdminModels.UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { key, data}
            },
            PlayFabId = "E5637C85D555F071",
            Permission = PlayFab.AdminModels.UserDataPermission.Public
        };
        PlayFabAdminAPI.UpdateUserData(request, OnDataSend, OnDataSendErr);
    }
    public void LoadSpeKeyDataFromPlayfab(string key)
    {
        var request = new PlayFab.AdminModels.GetUserDataRequest
        {
            PlayFabId = "E5637C85D555F071",
            Keys = new List<string> { key }
        };
        PlayFabAdminAPI.GetUserData(request, LoadDataSuccess, Err);
    }    
    public void LoadDataFromPlayFab()
    {
        var request = new PlayFab.AdminModels.GetUserDataRequest
        {
            PlayFabId = "E5637C85D555F071"
            //PlayFabId = "ACCD13E12EAFB1E8"
        };
        PlayFabAdminAPI.GetUserData(request, LoadDataSuccess, Err);
    }

    void LoadDataSuccess(PlayFab.AdminModels.GetUserDataResult result)
    {
        /*
        if (result.Data == null || (result.Data["version"].Value == dataManager.version))
        {
            LoadDataFromCom("map", terData);
            LoadDataFromCom("obj", objData);
            return;
        }*/

        if (result.Data.ContainsKey("terData1"))
        {
            List<TerrainData> terData1 = JsonConvert.DeserializeObject<List<TerrainData>>(result.Data["terData1"].Value);
            terData.AddRange(terData1);
        }
        if (result.Data.ContainsKey("terData2"))
        {
            List<TerrainData> terData2 = JsonConvert.DeserializeObject<List<TerrainData>>(result.Data["terData2"].Value);
            terData.AddRange(terData2);
        }
        if (result.Data.ContainsKey("terData3"))
        {
            List<TerrainData> terData3 = JsonConvert.DeserializeObject<List<TerrainData>>(result.Data["terData3"].Value);
            terData.AddRange(terData3);
        }
        if (result.Data.ContainsKey("terData4"))
        {
            List<TerrainData> terData4 = JsonConvert.DeserializeObject<List<TerrainData>>(result.Data["terData4"].Value);
            terData.AddRange(terData4);
        }
        if (result.Data.ContainsKey("terData5"))
        {
            List<TerrainData> terData5 = JsonConvert.DeserializeObject<List<TerrainData>>(result.Data["terData5"].Value);
            terData.AddRange(terData5);
        }
        if (result.Data.ContainsKey("terData6"))
        {
            List<TerrainData> terData6 = JsonConvert.DeserializeObject<List<TerrainData>>(result.Data["terData6"].Value);
            terData.AddRange(terData6);
        }
        if (result.Data.ContainsKey("terData7"))
        {
            List<TerrainData> terData7 = JsonConvert.DeserializeObject<List<TerrainData>>(result.Data["terData7"].Value);
            terData.AddRange(terData7);
        }
        if (result.Data.ContainsKey("terData8"))
        {
            List<TerrainData> terData8 = JsonConvert.DeserializeObject<List<TerrainData>>(result.Data["terData8"].Value);
            terData.AddRange(terData8);
        }
        if (result.Data.ContainsKey("terData9"))
        {
            List<TerrainData> terData9 = JsonConvert.DeserializeObject<List<TerrainData>>(result.Data["terData9"].Value);
            terData.AddRange(terData9);
        }
        if (result.Data.ContainsKey("terData10"))
        {
            List<TerrainData> terData10 = JsonConvert.DeserializeObject<List<TerrainData>>(result.Data["terData10"].Value);
            terData.AddRange(terData10);
        }
        if (result.Data.ContainsKey("terData11"))
        {
            List<TerrainData> terData11 = JsonConvert.DeserializeObject<List<TerrainData>>(result.Data["terData11"].Value);
            terData.AddRange(terData11);
        }
        if (result.Data.ContainsKey("terData12"))
        {
            List<TerrainData> terData12 = JsonConvert.DeserializeObject<List<TerrainData>>(result.Data["terData12"].Value);
            terData.AddRange(terData12);
        }
        if (result.Data.ContainsKey("terData13"))
        {
            List<TerrainData> terData13 = JsonConvert.DeserializeObject<List<TerrainData>>(result.Data["terData13"].Value);
            terData.AddRange(terData13);
        }
        if (result.Data.ContainsKey("terData14"))
        {
            List<TerrainData> terData14 = JsonConvert.DeserializeObject<List<TerrainData>>(result.Data["terData14"].Value);
            terData.AddRange(terData14);
        }
        if (result.Data.ContainsKey("terData15"))
        {
            List<TerrainData> terData15 = JsonConvert.DeserializeObject<List<TerrainData>>(result.Data["terData15"].Value);
            terData.AddRange(terData15);
        }
        if (result.Data.ContainsKey("terData16"))
        {
            List<TerrainData> terData16 = JsonConvert.DeserializeObject<List<TerrainData>>(result.Data["terData16"].Value);
            terData.AddRange(terData16);
        }
        if (result.Data.ContainsKey("terData17"))
        {
            List<TerrainData> terData17 = JsonConvert.DeserializeObject<List<TerrainData>>(result.Data["terData17"].Value);
            terData.AddRange(terData17);
        }
        if (result.Data.ContainsKey("terData18"))
        {
            List<TerrainData> terData18 = JsonConvert.DeserializeObject<List<TerrainData>>(result.Data["terData18"].Value);
            terData.AddRange(terData18);
        }
        if (result.Data.ContainsKey("terData19"))
        {
            List<TerrainData> terData19 = JsonConvert.DeserializeObject<List<TerrainData>>(result.Data["terData19"].Value);
            terData.AddRange(terData19);
        }
        if (result.Data.ContainsKey("terData20"))
        {
            List<TerrainData> terData20 = JsonConvert.DeserializeObject<List<TerrainData>>(result.Data["terData20"].Value);
            terData.AddRange(terData20);
        }
        /*
        if (result.Data.ContainsKey("grid1"))
        {
            List<Grid> gridData = JsonConvert.DeserializeObject<List<Grid>>(result.Data["grid1"].Value);
            grid.AddRange(gridData);
        }
        if (result.Data.ContainsKey("grid2"))
        {
            List<Grid> gridData = JsonConvert.DeserializeObject<List<Grid>>(result.Data["grid2"].Value);
            grid.AddRange(gridData);
        }
        if (result.Data.ContainsKey("grid3"))
        {
            List<Grid> gridData = JsonConvert.DeserializeObject<List<Grid>>(result.Data["grid3"].Value);
            grid.AddRange(gridData);
        }
        if (result.Data.ContainsKey("grid4"))
        {
            List<Grid> gridData = JsonConvert.DeserializeObject<List<Grid>>(result.Data["grid4"].Value);
            grid.AddRange(gridData);
        }
        if (result.Data.ContainsKey("grid5"))
        {
            List<Grid> gridData = JsonConvert.DeserializeObject<List<Grid>>(result.Data["grid5"].Value);
            grid.AddRange(gridData);
        }
        if (result.Data.ContainsKey("grid6"))
        {
            List<Grid> gridData = JsonConvert.DeserializeObject<List<Grid>>(result.Data["grid5"].Value);
            grid.AddRange(gridData);
        }
        if (result.Data.ContainsKey("grid7"))
        {
            List<Grid> gridData = JsonConvert.DeserializeObject<List<Grid>>(result.Data["grid7"].Value);
            grid.AddRange(gridData);
        }
        if (result.Data.ContainsKey("grid8"))
        {
            List<Grid> gridData = JsonConvert.DeserializeObject<List<Grid>>(result.Data["grid8"].Value);
            grid.AddRange(gridData);
        }
        if (result.Data.ContainsKey("grid9"))
        {
            List<Grid> gridData = JsonConvert.DeserializeObject<List<Grid>>(result.Data["grid9"].Value);
            grid.AddRange(gridData);
        }
        if (result.Data.ContainsKey("grid10"))
        {
            List<Grid> gridData = JsonConvert.DeserializeObject<List<Grid>>(result.Data["grid10"].Value);
            grid.AddRange(gridData);
        }
        if (result.Data.ContainsKey("grid11"))
        {
            List<Grid> gridData = JsonConvert.DeserializeObject<List<Grid>>(result.Data["grid11"].Value);
            grid.AddRange(gridData);
        }
        if (result.Data.ContainsKey("grid12"))
        {
            List<Grid> gridData = JsonConvert.DeserializeObject<List<Grid>>(result.Data["grid12"].Value);
            grid.AddRange(gridData);
        }
        if (result.Data.ContainsKey("grid13"))
        {
            List<Grid> gridData = JsonConvert.DeserializeObject<List<Grid>>(result.Data["grid13"].Value);
            grid.AddRange(gridData);
        }
        if (result.Data.ContainsKey("grid14"))
        {
            List<Grid> gridData = JsonConvert.DeserializeObject<List<Grid>>(result.Data["grid14"].Value);
            grid.AddRange(gridData);
        }
        if (result.Data.ContainsKey("grid15"))
        {
            List<Grid> gridData = JsonConvert.DeserializeObject<List<Grid>>(result.Data["grid15"].Value);
            grid.AddRange(gridData);
        }
        if (result.Data.ContainsKey("grid16"))
        {
            List<Grid> gridData = JsonConvert.DeserializeObject<List<Grid>>(result.Data["grid16"].Value);
            grid.AddRange(gridData);
        }
        */
        if (result.Data.ContainsKey("objData1"))
        {
            List<TerrainData> objData1 = JsonConvert.DeserializeObject<List<TerrainData>>(result.Data["objData1"].Value);
            objData.AddRange(objData1);
        }
        if (result.Data.ContainsKey("objData2"))
        {
            List<TerrainData> objData2 = JsonConvert.DeserializeObject<List<TerrainData>>(result.Data["objData2"].Value);
            objData.AddRange(objData2);
        }
        if (result.Data.ContainsKey("objData3"))
        {
            List<TerrainData> objData3 = JsonConvert.DeserializeObject<List<TerrainData>>(result.Data["objData3"].Value);
            objData.AddRange(objData3);
        }
        if (result.Data.ContainsKey("objData4"))
        {
            List<TerrainData> objData4 = JsonConvert.DeserializeObject<List<TerrainData>>(result.Data["objData4"].Value);
            objData.AddRange(objData4);
        }
        if (result.Data.ContainsKey("objData5"))
        {
            List<TerrainData> objData5 = JsonConvert.DeserializeObject<List<TerrainData>>(result.Data["objData5"].Value);
            objData.AddRange(objData5);
        }
        if (result.Data.ContainsKey("objData6"))
        {
            List<TerrainData> objData6 = JsonConvert.DeserializeObject<List<TerrainData>>(result.Data["objData6"].Value);
            objData.AddRange(objData6);
        }
        if (result.Data.ContainsKey("objData7"))
        {
            List<TerrainData> objData7 = JsonConvert.DeserializeObject<List<TerrainData>>(result.Data["objData7"].Value);
            objData.AddRange(objData7);
        }
        if (result.Data.ContainsKey("objData8"))
        {
            List<TerrainData> objData8 = JsonConvert.DeserializeObject<List<TerrainData>>(result.Data["objData8"].Value);
            objData.AddRange(objData8);
        }
        if (result.Data.ContainsKey("objData9"))
        {
            List<TerrainData> objData9 = JsonConvert.DeserializeObject<List<TerrainData>>(result.Data["objData9"].Value);
            objData.AddRange(objData9);
        }
        if (result.Data.ContainsKey("objData10"))
        {
            List<TerrainData> objData10 = JsonConvert.DeserializeObject<List<TerrainData>>(result.Data["objData10"].Value);
            objData.AddRange(objData10);
        }
        if (result.Data.ContainsKey("objData11"))
        {
            List<TerrainData> objData11 = JsonConvert.DeserializeObject<List<TerrainData>>(result.Data["objData11"].Value);
            objData.AddRange(objData11);
        }
        if (result.Data.ContainsKey("objData12"))
        {
            List<TerrainData> objData12 = JsonConvert.DeserializeObject<List<TerrainData>>(result.Data["objData12"].Value);
            objData.AddRange(objData12);
        }
        if (result.Data.ContainsKey("objData13"))
        {
            List<TerrainData> objData13 = JsonConvert.DeserializeObject<List<TerrainData>>(result.Data["objData13"].Value);
            objData.AddRange(objData13);
        }
        if (result.Data.ContainsKey("objData14"))
        {
            List<TerrainData> objData14 = JsonConvert.DeserializeObject<List<TerrainData>>(result.Data["objData14"].Value);
            objData.AddRange(objData14);
        }
        if (result.Data.ContainsKey("objData15"))
        {
            List<TerrainData> objData15 = JsonConvert.DeserializeObject<List<TerrainData>>(result.Data["objData15"].Value);
            objData.AddRange(objData15);
        }
        if (result.Data.ContainsKey("objData16"))
        {
            List<TerrainData> objData16 = JsonConvert.DeserializeObject<List<TerrainData>>(result.Data["objData16"].Value);
            objData.AddRange(objData16);
        }
        if (result.Data.ContainsKey("objData17"))
        {
            List<TerrainData> objData17 = JsonConvert.DeserializeObject<List<TerrainData>>(result.Data["objData17"].Value);
            objData.AddRange(objData17);
        }
        if (result.Data.ContainsKey("objData18"))
        {
            List<TerrainData> objData18 = JsonConvert.DeserializeObject<List<TerrainData>>(result.Data["objData18"].Value);
            objData.AddRange(objData18);
        }
        if (result.Data.ContainsKey("objData19"))
        {
            List<TerrainData> objData19 = JsonConvert.DeserializeObject<List<TerrainData>>(result.Data["objData19"].Value);
            objData.AddRange(objData19);
        }
        if (result.Data.ContainsKey("objData20"))
        {
            List<TerrainData> objData20 = JsonConvert.DeserializeObject<List<TerrainData>>(result.Data["objData20"].Value);
            objData.AddRange(objData20);
        }
        if(result.Data.ContainsKey("spawnPosition"))
        {
            Vector3ToSaveLoad spawnPos = JsonConvert.DeserializeObject<Vector3ToSaveLoad>(result.Data["spawnPosition"].Value);

            spawnPosition = new Vector3(spawnPos.x, spawnPos.y, spawnPos.z);
        }
        if (result.Data.ContainsKey("createMapDay"))
        {
            createMapDay = DateTime.Parse(result.Data["createMapDay"].Value);
        }
        if (result.Data.ContainsKey("createObjDay"))
        {
            createObjDay = DateTime.Parse(result.Data["createObjDay"].Value);
        }
        //SaveDataToCom("map", terData);
        //SaveDataToCom("obj", objData);

        dataManager.version = result.Data["version"].Value;
        loadSuccess = true;
        //SaveVersionData("version", result.Data["version"].Value);
    }
    public void SaveDataToCom(string fileName, List<TerrainData> target)
    {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<TerrainData>));
        FileStream stream = new FileStream(Application.persistentDataPath + "/" + fileName, FileMode.Create);
        //Debug.Log(Application.persistentDataPath.ToString());
        xmlSerializer.Serialize(stream, target);
        stream.Close();
    }
    public void LoadDataFromCom(string fileName, List<TerrainData> target)
    {
        if (File.Exists(Application.persistentDataPath + "/" + fileName))
        {
            Debug.Log("Loading data from file...");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<TerrainData>));
            FileStream stream = new FileStream(Application.persistentDataPath + "/" + fileName, FileMode.Open);

            try
            {
                target = (List<TerrainData>)xmlSerializer.Deserialize(stream);
                Debug.Log(target);

                Debug.Log("Data loaded successfully.");
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to load data: " + e.Message);
            }

            stream.Close();
        }
        else
        {
            Debug.LogWarning("File not found: " + fileName);
        }
    }

    public void SaveVersionData(string fileName, string version)
    {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(string));
        FileStream stream = new FileStream(Application.persistentDataPath + "/" + fileName, FileMode.Create);
        //Debug.Log(Application.persistentDataPath.ToString());
        xmlSerializer.Serialize(stream, version);
        stream.Close();
    } 
    void Err(PlayFabError error)
    {
        Debug.Log(error);
    }
    public void OnDataSend(PlayFab.AdminModels.UpdateUserDataResult result)
    {
        saveCount++;
        //Debug.LogError(saveCount);
    }
    public int CalculateDay(System.DateTime dayBefore)
    {
        System.TimeSpan timeSinceCreation = System.DateTime.Now - dayBefore;
        return timeSinceCreation.Days;
    }
    public void SaveAction()
    {
        if(isCreateNewMap)
        {
            SaveLarTerData();
            SaveSpawnPosition();
        }
        else
        {
            saveCount = saveCount + 21;
        }    
        if(CalculateDay(createObjDay) < resetObjDay)
        {
            SaveLarObjData();
        }
        else
        {
            saveCount = saveCount + 20;
        }    
        SaveCreateDayData();

    }  
    public void ClearLargeMapData()
    {
        terData.Clear();
        objData.Clear();

        isCreateNewMap = false;
        loadSuccess = false;

        createMapDay = new System.DateTime();
        createObjDay = new System.DateTime();
        spawnPosition = new Vector3();
        saveCount = 0;
    }    
    public void OnDataSendErr(PlayFabError error)
    {
        Debug.Log(error);
    }
    [System.Serializable]
    public class TerrainData
    {
        public string objectID;

        public string itemID;

        public float PosX;
        public float PosY;
        public float PosZ;

        public float Rotation;

        public string objType;

        public Grid cellData;
    }
    public class Vector3ToSaveLoad
    {
        public float x;
        public float y;
        public float z;
    }

    [System.Serializable]
    public class Grid
    {
        public int x;
        public int y;

        public int state;
    }    
}
