using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTile : MonoBehaviour
{
    
    //Variables

    public TileData tileData;
    public GameObject vegetation;

    Vector3[] corners = new Vector3[6];

    //Methods

    public void Initiate(TileData tileData){
        this.tileData = tileData;
        vegetation = new GameObject("Vegetation");
        vegetation.transform.parent = this.transform;
        SetPosition();
        SetCorners();
        DrawTile();
        SetTexture();
    }

    void SetTexture(){
        Texture2D texture = new Texture2D(174,200);
        Color[] colorMap = new Color[174*200];

        for(int i = 0; i < colorMap.Length; i++){
            int pPosX = (i%174 - 87)*10;
            int pPosY = ((i - i%174) / 174 - 100)*10;
            Vector2 pos = new Vector2(pPosX, pPosY);
            Vector3 pos3 = new Vector3(pPosX, 0, pPosY);
            float angle = Vector2.SignedAngle(pos, Vector2.right);
            angle = Mathf.Abs(angle-330) % 360;

            int rectId = -1;
            float alpha = 1;

            for(int r = 0; r < 6; r++){
                int r2 = r == 5 ? 0 : r+1;
                int r3 = r2 == 5 ? 0 : r2+1;
                int r4 = r == 0 ? 5 : r-1;

                Vector3 c1 = corners[r];
                Vector3 c2 = corners[r2];
                Vector3 c3 = corners[r3];
                Vector3 c4 = corners[r4];

                float dist1 = ((Vector3.Cross((pos3 - c1) , (c1-c2))).magnitude / (c1-c2).magnitude);
                float dist2 = ((Vector3.Cross((pos3 - c2) , (c2-c3))).magnitude / (c2-c3).magnitude);
                float dist3 = ((Vector3.Cross((pos3 - c4) , (c4-c1))).magnitude / (c4-c1).magnitude);

                if(dist1 < 125){
                    if(dist1 > dist2){
                        alpha = dist2/125;
                        rectId = r == 5 ? 0 : r+1;
                    }
                    else if(dist1 > dist3){
                        alpha = dist3/125;
                        rectId = r == 0 ? 5 : r-1;
                    }
                    else {
                        alpha = dist1/125;
                        rectId = r;
                    }
                }

            }

            if(rectId != -1){
                TileData.ClimateType neighboreClimate = transform.parent.GetComponent<TerrainManager>().GetNeighborTileData((int)tileData.coordinates.x, (int)tileData.coordinates.y, rectId).climateType;
                TileData.ClimateType selfClimate = tileData.climateType;
                if(neighboreClimate == selfClimate) colorMap[i] = transform.parent.GetComponent<TerrainManager>().terrainColors[(int)selfClimate];
                else colorMap[i] = Color.Lerp(transform.parent.GetComponent<TerrainManager>().terrainColors[1], transform.parent.GetComponent<TerrainManager>().terrainColors[(int)selfClimate], alpha);
            }

            if(rectId == -1){
                colorMap[i] = transform.parent.GetComponent<TerrainManager>().terrainColors[(int)tileData.climateType];
            }

        }

        texture.SetPixels(colorMap);
        texture.Apply();
        GetComponent<MeshRenderer>().material.mainTexture = texture;
    }

    void DrawTile(){
        Vector3[] verticies = new Vector3[26259];
        int[] triangles = new int[156348];
        Vector2[] uvs = new Vector2[26259];
        
        int vIndex = 0;
        int currentFirstV = 0;
        int currentV = 1;
        int nextV = 1;
        int tIndex = 0;
        for(int z = -1000; z <= 1000; z += 10){
            int anzahlV = 0;
            currentV = nextV;
            const int alpha = 30;
            const float alphaRad = alpha/180f * (float)Mathf.PI;
            const int beta = 90;
            const float betaRad = beta/180f * (float)Mathf.PI;
            const int gamma = 60;
            const float gammaRad = gamma/180f * (float)Mathf.PI;
            int a = z < -500? z + 1000: z > 500 ? z - 1000: 500;
            float b = (a * (float)Mathf.Sin(betaRad)) / (float) Mathf.Sin(alphaRad);
            float c = (float)Mathf.Sqrt((a*a)-(2*a*b*Mathf.Cos(gammaRad))+(b*b));
            float mod = c%10;
            float x = c * -1;

            //creating Vertecies
            if(c == 0) {
                verticies[vIndex] = CreateVertex(0,z);
                uvs[vIndex] = new Vector2((verticies[vIndex].x + 870)/1740, (verticies[vIndex].z + 1000)/2000);
                vIndex++;
            }
            else if (mod < 5){
                verticies[vIndex] = CreateVertex(x,z);
                uvs[vIndex] = new Vector2((verticies[vIndex].x + 870)/1740, (verticies[vIndex].z + 1000)/2000);
                x = (c-mod-10) * -1;
                vIndex++;
            }
            else if (mod >= 5){
                verticies[vIndex] = CreateVertex(x,z);
                uvs[vIndex] = new Vector2((verticies[vIndex].x + 870)/1740, (verticies[vIndex].z + 1000)/2000);
                x = (c-mod) * -1;
                vIndex++;
            }

            while(x <= c-mod-10){
                verticies[vIndex] = CreateVertex(x,z);
                uvs[vIndex] = new Vector2((verticies[vIndex].x + 870)/1740, (verticies[vIndex].z + 1000)/2000);
                x += 10;
                vIndex++;
            }

            if (mod < 5 && c != 0){
                x = c;
                verticies[vIndex] = CreateVertex(x,z);
                uvs[vIndex] = new Vector2((verticies[vIndex].x + 870)/1740, (verticies[vIndex].z + 1000)/2000);
                vIndex++;
            }
            else if (mod >= 5 && c != 0){
                verticies[vIndex] = CreateVertex(x,z);
                uvs[vIndex] = new Vector2((verticies[vIndex].x + 870)/1740, (verticies[vIndex].z + 1000)/2000);
                x = c;
                vIndex++;
                verticies[vIndex] = CreateVertex(x,z);
                uvs[vIndex] = new Vector2((verticies[vIndex].x + 870)/1740, (verticies[vIndex].z + 1000)/2000);
                vIndex++;
            }

            //Creating Triangles
            anzahlV += (int)(c-mod)/10*2 +1;
            if(mod >= 5) anzahlV +=2;

            nextV = anzahlV;

            if(nextV == 1 && currentV == 1) continue;

            int anzahlTriangles = 0;
            if(currentV < nextV){
                anzahlTriangles += (nextV - currentV) + (currentV -1)*2;
            }
            else if (currentV == nextV){
                anzahlTriangles += (currentV-1)*2;
            }
            else if (currentV > nextV){
                anzahlTriangles += (currentV - nextV) + (nextV -1)*2;
            }

            int abR = 0;
            for(int t = 0; t < currentV; t++){

                if(currentV < nextV && t == 0){
                    while(abR < (nextV-currentV)/2){
                        triangles[tIndex] = currentFirstV;
                        triangles[tIndex+1] = currentFirstV+currentV+abR;
                        triangles[tIndex+2] = currentFirstV+currentV+abR+1;
                        tIndex += 3;
                        abR++;
                    }
                }
                else if(currentV > nextV && t < (currentV-nextV)/2){
                    while(t < (currentV-nextV)/2){
                        triangles[tIndex] = currentFirstV+t;
                        triangles[tIndex+1] = currentFirstV+currentV+abR;
                        triangles[tIndex+2] = currentFirstV+t+1;
                        tIndex += 3;
                        t++;
                    }
                }

                if ((t < currentV-1 && currentV <= nextV) || (nextV < currentV && t < currentV-((currentV-nextV)/2)-1)){
                    triangles[tIndex] = currentFirstV+t;
                    triangles[tIndex+1] = currentFirstV+currentV+abR;
                    triangles[tIndex+2] = currentFirstV+currentV+abR+1;
                    tIndex += 3;
                    triangles[tIndex] = currentFirstV+t;
                    triangles[tIndex+1] = currentFirstV+currentV+abR+1;
                    triangles[tIndex+2] = currentFirstV+t+1;
                    tIndex += 3;
                    abR++;
                }
                else if (currentV < nextV){
                    while(abR < nextV-1){
                        triangles[tIndex] = currentFirstV+t;
                        triangles[tIndex+1] = currentFirstV+currentV+abR;
                        triangles[tIndex+2] = currentFirstV+currentV+abR+1;
                        tIndex += 3;
                        abR++;
                    }
                }
                else if(currentV > nextV){
                    while(t < currentV-1){
                        triangles[tIndex] = currentFirstV+t;
                        triangles[tIndex+1] = currentFirstV+currentV+abR;
                        triangles[tIndex+2] = currentFirstV+t+1;
                        tIndex += 3;
                        t++;
                    }
                }
            }
            currentFirstV += currentV;
        }

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        mesh.Clear();
        mesh.vertices = verticies;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        meshCollider.sharedMesh = mesh;
    }

    public Vector3 CreateVertex(float x, float z){
        float y = RandomGenerator.TerrainNoise(new Vector2(x + tileData.terrainPosition.x, z + tileData.terrainPosition.y), GameObject.FindGameObjectWithTag("MapMaster").GetComponent<MapMaster>().seed);
        
        y = MapGenerator.CalcVertexHeight(x, y, z, transform.parent, tileData);

        return new Vector3(x, y, z);
    }

    void SetCorners(){
        for(int c = 0; c < 6; c++){
            float rad = Mathf.PI / 180 * (60*(c+1)-30);
            float posX = 1000 * Mathf.Cos(rad);
            float posZ = 1000 * Mathf.Sin(rad);
            corners[c] = new Vector3(posX, 0, posZ);
        }
    }

    void SetPosition(){
        float posX = Mathf.Sqrt(3) * 1000 * (tileData.coordinates.x + (tileData.coordinates.y%2 == 1 && tileData.coordinates.y != 0 ? + 0.5f : 0));
        float posY = 3/2f * 1000 * tileData.coordinates.y;

        tileData.terrainPosition = new Vector2(posX, posY);
        transform.localPosition = new Vector3(posX, 0, posY);
    }

    //System

}
