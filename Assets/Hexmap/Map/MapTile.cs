using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile : MonoBehaviour
{
    
    //Variables

    public TileData tileData;

    //Methods

    public void Initiate(TileData tileData){
        this.tileData = tileData;
        DrawTile();
        SetTexture();
    }

    void DrawTile(){
        Vector3[] verticies = new Vector3[7];
        verticies[0] = Vector3.zero;
        for(int i = 1; i < 7; i++){
            float rad = Mathf.PI / 180 * (60*i-30);
            float posX = Mathf.Cos(rad);
            float posZ = Mathf.Sin(rad);
            verticies[i] = new Vector3(posX, 0, posZ);
        }

        int[] triangles = new int[3];
        triangles = new int[]{
            0,1,6,
            0,2,1,
            0,3,2,
            0,4,3,
            0,5,4,
            0,6,5,
        };

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.vertices = verticies;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    void SetTexture(){
        Material material = GetComponent<MeshRenderer>().material;
        if(tileData.reliefType == TileData.ReliefType.DeepWater && tileData.climateType == TileData.ClimateType.Polar) material.color = new Color(102/255f,178/255f,255/255f);
        else if(tileData.reliefType == TileData.ReliefType.DeepWater) material.color = new Color(0,0,204/255f);

        else if(tileData.reliefType == TileData.ReliefType.Water && tileData.climateType == TileData.ClimateType.Polar) material.color = new Color(153/255f,255/255f,255/255f);
        else if(tileData.reliefType == TileData.ReliefType.Water) material.color = new Color(0,128/255f,255/255f);

        else if(tileData.reliefType == TileData.ReliefType.Even && tileData.climateType == TileData.ClimateType.Polar) material.color = new Color(153/255f,255/255f,204/255f);
        else if(tileData.reliefType == TileData.ReliefType.Even && tileData.climateType == TileData.ClimateType.Temperate) material.color = new Color(102/255f,204/255f,0);
        else if(tileData.reliefType == TileData.ReliefType.Even && tileData.climateType == TileData.ClimateType.SubTropical) material.color = new Color(204/255f,204/255f,0);
        else if(tileData.reliefType == TileData.ReliefType.Even && tileData.climateType == TileData.ClimateType.Tropical) material.color = new Color(153/255f,76/255f,0);

        else if(tileData.reliefType == TileData.ReliefType.Hill && tileData.climateType == TileData.ClimateType.Polar) material.color = new Color(0,153/255f,153/255f);
        else if(tileData.reliefType == TileData.ReliefType.Hill && tileData.climateType == TileData.ClimateType.Temperate) material.color = new Color(0,102/255f,0);
        else if(tileData.reliefType == TileData.ReliefType.Hill && tileData.climateType == TileData.ClimateType.SubTropical) material.color = new Color(102/255f,102/255f,0);
        else if(tileData.reliefType == TileData.ReliefType.Hill && tileData.climateType == TileData.ClimateType.Tropical) material.color = new Color(102/255f,51/255f,0);

        else if(tileData.reliefType == TileData.ReliefType.Mountain) material.color = new Color(192/255f,192/255f,192/255f);
    }

    //System



}
