using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RiverData
{
     
    //Variables

    public Vector2 innerTileCoords;
    public Vector2 outerTileCoords;

    public int innerTileSide;
    public int outerTileSide;

    //Methods

    //System

    public RiverData(Vector2 innerTileCoords, Vector2 outerTileCoords, int innerTileSide, int outerTileSide){
        this.innerTileCoords = innerTileCoords;
        this.outerTileCoords = outerTileCoords;
        this.innerTileSide = innerTileSide;
        this.outerTileSide = outerTileSide;
    }

}
