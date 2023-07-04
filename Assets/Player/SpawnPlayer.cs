using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    public MapMaster mapMaster;
    public WorldPositionManager worldPositionManager;
    public TerrainManager terrainManager;
    public GameObject mapManagerObj;
    public GameObject playerObj;

    private bool isSpawned = false;
    private TileData tileData;

    private void Start(){
        bool check = false;
        while(!check){
            tileData = mapMaster.allTileDatas[Random.Range(0, mapMaster.mapWidth), Random.Range(0, mapMaster.mapHeight)];
            if(tileData.reliefType != TileData.ReliefType.DeepWater
            && tileData.reliefType != TileData.ReliefType.Water
            && tileData.reliefType != TileData.ReliefType.Mountain) check = true;
        }
        worldPositionManager.playerCoords = tileData.coordinates;
        terrainManager.showCoord = true;
        mapManagerObj.SetActive(false);
    }

    private void Update(){
        if(!isSpawned && terrainManager.allTerrainTiles[(int)tileData.coordinates.x, (int)tileData.coordinates.y] != null){
            Transform player = GameObject.Instantiate(playerObj).transform;
            player.parent = terrainManager.allTerrainTiles[(int)tileData.coordinates.x, (int)tileData.coordinates.y].transform;
            isSpawned = true;
            SetPosition(player);
            Destroy(this);
        }
    }

    private void SetPosition(Transform player){
        player.position = Vector3.zero + Vector3.up * terrainManager.terrainheightBorders[(int)tileData.reliefType].y;
    }
}
