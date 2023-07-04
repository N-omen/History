using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    
    //Variables
    
    public bool showAll = false;

    public GameObject mapTilePrefab;
    public GameObject mapRiverPrefab;

    [HideInInspector]
    public TileData[,] allTileDatas;
    GameObject[,] allMapTiles = new GameObject[1,1];

    [HideInInspector]
    public RiverData[] allRiverDatas;
    GameObject[] allRiver;

    //Methods

    void ManageMapTiles(){
        if(showAll){
            for(int y = 0; y < allTileDatas.GetLength(1); y++){
                for(int x = 0; x < allTileDatas.GetLength(0); x++){
                    if(allMapTiles[x, y] == null){
                        allMapTiles[x, y] = Instantiate(mapTilePrefab);
                        allMapTiles[x, y].transform.parent = this.transform;
                        allMapTiles[x, y].AddComponent<MapTile>().Initiate(allTileDatas[x,y]);
                        allMapTiles[x,y].transform.position = new Vector3(allTileDatas[x,y].mapPosition.x, -1000, allTileDatas[x,y].mapPosition.y);
                    }
                    else allMapTiles[x, y].SetActive(true);
                }
            }
            for(int i = 0; i < allRiverDatas.Length; i++){
                if(allRiver[i] == null){
                    allRiver[i] = Instantiate(mapRiverPrefab);
                    allRiver[i].transform.parent = this.transform;
                    allRiver[i].AddComponent<MapRiver>().Initiate(allRiverDatas[i], allTileDatas[(int)allRiverDatas[i].innerTileCoords.x, (int)allRiverDatas[i].innerTileCoords.y]);
                }
                else allRiver[i].SetActive(true);
            }
        }
        else{
            for(int y = 0; y < allTileDatas.GetLength(1); y++){
                for(int x = 0; x < allTileDatas.GetLength(0); x++){
                    if(allMapTiles[x,y] != null)allMapTiles[x, y].SetActive(false);
                }
            }
            for(int i = 0; i < allRiver.Length; i++){
                allRiver[i].SetActive(false);
            }
        }
    }

    //System

    void Start(){
        allMapTiles = new GameObject[allTileDatas.GetLength(0), allTileDatas.GetLength(1)];
        allRiver = new GameObject[allRiverDatas.Length];
    }

    void Update(){
        ManageMapTiles();
    }

}
