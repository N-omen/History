using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldPositionManager : MonoBehaviour
{
    
    //Variables

    public TerrainManager terrainManager;
    public Vector2 playerCoords;

    Vector2 playerCoordsOld;

    //Methods

    void AdjustWorldPosition(){
        Vector2 pos = terrainManager.allTileDatas[(int)playerCoords.x, (int)playerCoords.y].terrainPosition;
        this.transform.position = new Vector3(-pos.x, 0, -pos.y);
    }

    //System

    void Update(){
        if(playerCoords != playerCoordsOld){
            AdjustWorldPosition();
            terrainManager.showCoordFrom = new Vector2(playerCoords.x-1, playerCoords.y-1);
            terrainManager.showCoordTo = new Vector2(playerCoords.x+1, playerCoords.y+1);
        }
    }

}
