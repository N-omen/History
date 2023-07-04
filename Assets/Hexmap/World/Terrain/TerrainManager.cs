using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    
    //Variables

    public Color[] terrainColors = new Color[] {
        Color.white, 
        new Color(48/255f, 117/255f, 33/255f), 
        new Color(235/255f, 196/255f, 91/255f), 
        new Color(99/255f, 76/255f, 10/255f)
    };

    public Vector2[] terrainheightBorders = new Vector2[] {
        new Vector2(-150,-75), 
        new Vector2(-20,-10), 
        new Vector2(0,30), 
        new Vector2(20,150), 
        new Vector2(50,400)
    };
    
    public bool showAll = false;
    public bool showCoord = false;
    public Vector2 showCoordFrom = new Vector2(0,0);
    public Vector2 showCoordTo = new Vector2(0,0);

    public GameObject terrainTilePrefab;

    [HideInInspector]
    public TileData[,] allTileDatas;
    [HideInInspector]
    public GameObject[,] allTerrainTiles;

    private VegetationGenerator vegetationGenerator;    

    //Methods

    public TileData GetNeighborTileData(int x, int y, int neighborIndex){
        int mapWidth = allTileDatas.GetLength(0);
        int mapHeight = allTileDatas.GetLength(1);
        switch(neighborIndex){
            case 0:{
                if(y+1 >= mapHeight || (x+(y%2)) >= mapWidth) return allTileDatas[x,y];
                return allTileDatas[x+(y%2),y+1];
            }
            case 1:{
                if(y+1 >= mapHeight || (x+(y%2)-1) < 0) return allTileDatas[x,y];
                return allTileDatas[x+(y%2)-1,y+1];
            }
            case 2:{
                if(x-1 < 0) return allTileDatas[x,y];
                return allTileDatas[x-1,y];
            }
            case 3:{
                if(y-1 < 0 || (x+(y%2)-1) < 0) return allTileDatas[x,y];
                return allTileDatas[x+(y%2)-1,y-1];
            }
            case 4:{
                if(y-1 < 0 || (x+(y%2)) >= mapWidth ) return allTileDatas[x,y];
                return allTileDatas[x+(y%2),y-1];
            }
            case 5:{
                if(x+1 >= mapWidth) return allTileDatas[x,y];
                return allTileDatas[x+1,y];
            }
        }
        return allTileDatas[x,y];
    }

    void ManageTerrainTiles(){
        if(showAll){
            for(int y = 0; y < allTileDatas.GetLength(1); y++){
                for(int x = 0; x < allTileDatas.GetLength(0); x++){
                    if(allTerrainTiles[x, y] == null){
                        allTerrainTiles[x, y] = Instantiate(terrainTilePrefab);
                        allTerrainTiles[x, y].transform.parent = this.transform;
                        allTerrainTiles[x, y].AddComponent<TerrainTile>().Initiate(allTileDatas[x,y]);
                        vegetationGenerator.GenerateVegetation(allTerrainTiles[x, y], allTileDatas[x, y]);
                    }
                    else if(allTerrainTiles[x,y] != null)allTerrainTiles[x, y].SetActive(true);
                }
            }
        }
        else if(showCoord){
            for(int y = 0; y < allTileDatas.GetLength(1); y++){
                for(int x = 0; x < allTileDatas.GetLength(0); x++){
                    if(x < showCoordFrom.x || x > showCoordTo.x || y < showCoordFrom.y || y > showCoordTo.y){
                        if(allTerrainTiles[x,y] != null) allTerrainTiles[x,y].SetActive(false);
                        continue;
                    }
                    if(allTerrainTiles[x, y] == null){
                        allTerrainTiles[x, y] = Instantiate(terrainTilePrefab);
                        allTerrainTiles[x, y].transform.parent = this.transform;
                        allTerrainTiles[x, y].AddComponent<TerrainTile>().Initiate(allTileDatas[x,y]);
                        vegetationGenerator.GenerateVegetation(allTerrainTiles[x, y], allTileDatas[x, y]);
                    }
                    else if(allTerrainTiles[x,y] != null)allTerrainTiles[x, y].SetActive(true);
                }
            }
        }
        else{
            for(int y = 0; y < allTileDatas.GetLength(1); y++){
                for(int x = 0; x < allTileDatas.GetLength(0); x++){
                    if(allTerrainTiles[x,y] != null)allTerrainTiles[x, y].SetActive(false);
                }
            }
        }
    }

    //System

    void Start(){
        allTerrainTiles = new GameObject[allTileDatas.GetLength(0), allTileDatas.GetLength(1)];
        vegetationGenerator = GetComponent<VegetationGenerator>();
    }

    void Update(){
        ManageTerrainTiles();
    }

}
