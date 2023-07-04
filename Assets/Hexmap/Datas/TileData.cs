using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileData
{
    
    //Variables

    public Vector2 mapPosition;
    public Vector2 terrainPosition;
    public Vector2 coordinates;

    public enum ReliefType{
        DeepWater = 0,
        Water = 1,
        Even = 2, 
        Hill = 3,
        Mountain = 4,
    }
    public enum ClimateType{
        Polar = 0,
        Temperate = 1,
        SubTropical = 2,
        Tropical = 3,
    }
    public enum VegetationType{
        Barren = 0,
        Shrubs = 1,
        Forrest = 2,
    }

    public ReliefType reliefType;
    public ClimateType climateType;
    public VegetationType vegetationType;

    public RiverData[] rivers = new RiverData[6];

    //Methods

    void SetReliefType(float reliefValue, float[] reliefTypeBorders){
        if(reliefValue < reliefTypeBorders[0]) reliefType = ReliefType.Water;
        else if(reliefValue < reliefTypeBorders[1]) reliefType = ReliefType.Even;
        else if(reliefValue < reliefTypeBorders[2]) reliefType = ReliefType.Hill;
        else if(reliefValue < reliefTypeBorders[3]) reliefType = ReliefType.Mountain;
    }

    public void SetClimateType(int climateRank, int possibilityTOChange){
        if(climateRank != 1 && possibilityTOChange > 0 ) climateRank -= Random.value < possibilityTOChange/100f ? 1 : 0;
        else if(climateRank != 9 && possibilityTOChange < 0 ) climateRank += Random.value < possibilityTOChange/-100f ? 1 : 0;

        if(climateRank == 1 || climateRank == 9) climateType = ClimateType.Polar;
        else if(climateRank == 2 || climateRank == 3 || climateRank == 7 ||  climateRank == 8) climateType = ClimateType.Temperate;
        else if(climateRank == 4 || climateRank == 6) climateType = ClimateType.SubTropical;
        else if(climateRank == 5) climateType = ClimateType.Tropical;

        if(reliefType == ReliefType.Water) climateType = ClimateType.Temperate;
    }

    public void SetVegetationType(float vegetationValue){
        if(climateType == ClimateType.Polar) vegetationType = VegetationType.Barren;
        else if(climateType == ClimateType.Temperate) vegetationType = (VegetationType)(int)(vegetationValue*3-0.01f);
        else if(climateType == ClimateType.SubTropical) vegetationType = (VegetationType)(int)(vegetationValue*2-0.01f);
        else if(climateType == ClimateType.Tropical) vegetationType = (VegetationType)(int)(vegetationValue*2-0.01f)+1;
    }

    //System

    public TileData(Vector2 mapPosition, Vector2 coordinates, float reliefValue, float[] reliefTypeBorders){
        this.mapPosition = mapPosition;
        this.coordinates = coordinates;
        SetReliefType(reliefValue, reliefTypeBorders);
    }

}
