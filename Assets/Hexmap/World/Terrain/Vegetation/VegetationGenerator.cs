using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegetationGenerator : MonoBehaviour
{
    
    //Variables

    public ObjectData[] objectDatas;

    //Methods

    public void GenerateVegetation(GameObject parent, TileData tileData){
        int fromX = -1000;
        int toX = 1000;
        int fromY = -1000;
        int toY = 1000;

        for(int y = fromY; y < toY; y+=10){
            for(int x = fromX; x < toX; x+=10){
                
                float random = Random.Range(0f,1f);
                if(random > 0.2f) continue;

                GameObject _gameObject = GetObject(tileData);
                if(_gameObject == null) continue;

                GameObject gameObject = Instantiate(_gameObject);
                gameObject.transform.parent = parent.GetComponent<TerrainTile>().vegetation.transform;
                gameObject.transform.position = new Vector3(x+tileData.terrainPosition.x, 500, y+tileData.terrainPosition.y);
                gameObject.GetComponent<VegetationPlacement>().PlaceObject(parent);
                //Initiate aufrufen

            }
        }
    }

    public GameObject GetObject(TileData tileData){

        foreach(ObjectData objectData in objectDatas){
            
            if((tileData.climateType == TileData.ClimateType.Polar && objectData.polar)
            || (tileData.climateType == TileData.ClimateType.Temperate && objectData.temperate)
            || (tileData.climateType == TileData.ClimateType.SubTropical && objectData.subTropical)
            || (tileData.climateType == TileData.ClimateType.Tropical && objectData.tropical)){

                if((tileData.vegetationType == TileData.VegetationType.Barren && objectData.barren)
                || (tileData.vegetationType == TileData.VegetationType.Shrubs && objectData.shrubs)
                || (tileData.vegetationType == TileData.VegetationType.Forrest && objectData.forrest)){

                    if(((tileData.reliefType == TileData.ReliefType.DeepWater || tileData.reliefType == TileData.ReliefType.Water) && objectData.water)
                    || ((tileData.reliefType == TileData.ReliefType.Even || tileData.reliefType == TileData.ReliefType.Hill) && objectData.ground)
                    || (tileData.reliefType == TileData.ReliefType.Mountain && objectData.mountain)){

                        int random = Random.Range(0,100);

                        if(random.CompareTo(objectData.possibility) < 1) return objectData.gameObject;

                    }

                }

            }

        }
        return null;

    }

    //System



}

[System.Serializable]
public class ObjectData{
    public GameObject gameObject;

    public bool polar;
    public bool temperate;
    public bool subTropical;
    public bool tropical;

    public bool barren;
    public bool shrubs;
    public bool forrest;

    public bool water;
    public bool ground;
    public bool mountain;

    public int possibility;
}