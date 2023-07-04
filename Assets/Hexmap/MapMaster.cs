using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMaster : MonoBehaviour
{
    //Variables

    public int mapWidth = 1;
    public int mapHeight = 1;
    public int seed = 0;

    public MapManager mapManager;
    public TerrainManager terrainManager;

    public float[] reliefTypeBorders = {0.53f, 0.8f, 0.85f, 1};
    public float riverBorder = 0.5f;
    
    [HideInInspector]
    public TileData[,] allTileDatas;
    private RiverData[] allRiverDatas;

    //Methods

    void LoadTileData(){
        //Check if some data is saved
        allTileDatas = MapGenerator.GenerateMap(mapHeight, mapWidth, seed, reliefTypeBorders);
    }

    void LoadRiverData(){
        //Check if some data is saved
        allRiverDatas = MapGenerator.GenerateRiver(allTileDatas, riverBorder, seed);
    }

    //System

    void Awake(){
        seed = seed == 0 ? Random.Range(-99999,99999) : seed;
        LoadTileData();
        LoadRiverData();
        mapManager.allTileDatas = allTileDatas;
        terrainManager.allTileDatas = allTileDatas;
        mapManager.allRiverDatas = allRiverDatas;
    }

}
