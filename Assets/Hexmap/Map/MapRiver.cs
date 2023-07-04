using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRiver : MonoBehaviour
{
    	
    //Variables

    public RiverData riverData;

    //Methods

    public void Initiate(RiverData riverData, TileData innerTileData){
        this.riverData = riverData;
        DrawRiver(innerTileData);
    }

    void DrawRiver(TileData innerTileData){

        float rad1 = Mathf.PI / 180 * (60*riverData.innerTileSide+30);
        float posX1 = Mathf.Cos(rad1) + innerTileData.mapPosition.x;
        float posY1 = Mathf.Sin(rad1) + innerTileData.mapPosition.y;
        Vector2 posCorner1 = new Vector2(posX1, posY1);

        float rad2 = Mathf.PI / 180 * (60*(riverData.innerTileSide+1)+30);
        float posX2 = Mathf.Cos(rad2) + innerTileData.mapPosition.x;
        float posY2 = Mathf.Sin(rad2) + innerTileData.mapPosition.y;
        Vector2 posCorner2 = new Vector2(posX2, posY2);

        Vector2 directVector2 = posCorner2-posCorner1;
        Vector3 directVector = new Vector3(directVector2.x, 0 , directVector2.y);

        this.transform.rotation = Quaternion.LookRotation(directVector);
        this.transform.position = new Vector3(posCorner1.x, -1000 , posCorner1.y) + 0.5f* directVector;

    }

    //System



}
