using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapGenerator
{
    public static TileData[,] GenerateMap(int mapHeight, int mapWidth, int seed, float[] reliefTypeBorders){

        TileData[,] tileDatas = new TileData[mapWidth, mapHeight];

        System.Random randomGenerator = new System.Random(seed);
        int random = randomGenerator.Next(-99999,99999);
        
        for(int y = 0; y < mapHeight; y++){
            for(int x = 0; x < mapWidth; x++){
                //Setting Position
                float posX = Mathf.Sqrt(3) * (x + (y%2 == 1 && y != 0 ? + 0.5f : 0));
                float posY = 3/2f * y;
                Vector2 pos2 = new Vector2(posX, posY);

                //Setting landMassValue
                float landMassValue = RandomGenerator.LandmassNoise(pos2, mapHeight, mapWidth, random);

                //Setting Tile
                tileDatas[x, y] = new TileData(pos2,new Vector2(x,y), landMassValue, reliefTypeBorders);
            }
        }

        //Edditing Shore Water Tiles
        for(int y = 0; y < mapHeight; y++){
            for(int x = 0; x < mapWidth; x++){      

                if(
                    tileDatas[x,y].reliefType == TileData.ReliefType.Water
                    && ((y+1 < mapHeight && (x+(y%2)-1) >= 0 && (tileDatas[(x+(y%2)-1),y+1].reliefType == TileData.ReliefType.Even || tileDatas[(x+(y%2)-1),y+1].reliefType == TileData.ReliefType.Hill || tileDatas[(x+(y%2)-1),y+1].reliefType == TileData.ReliefType.Mountain))
                    || (y+1 < mapHeight && (x+(y%2)) < mapWidth && (tileDatas[(x+(y%2)),y+1].reliefType == TileData.ReliefType.Even ||tileDatas[(x+(y%2)),y+1].reliefType == TileData.ReliefType.Hill || tileDatas[(x+(y%2)),y+1].reliefType == TileData.ReliefType.Mountain))
                    || (x-1 >= 0 && (tileDatas[x-1,y].reliefType == TileData.ReliefType.Even || tileDatas[x-1,y].reliefType == TileData.ReliefType.Hill || tileDatas[x-1,y].reliefType == TileData.ReliefType.Mountain))
                    || (x+1 < mapWidth && (tileDatas[x+1,y].reliefType == TileData.ReliefType.Even || tileDatas[x+1,y].reliefType == TileData.ReliefType.Hill || tileDatas[x+1,y].reliefType == TileData.ReliefType.Mountain))
                    || (y-1 >= 0 && (x+(y%2)-1) >= 0 && (tileDatas[(x+(y%2)-1),y-1].reliefType == TileData.ReliefType.Even || tileDatas[(x+(y%2)-1),y-1].reliefType == TileData.ReliefType.Hill || tileDatas[(x+(y%2)-1),y-1].reliefType == TileData.ReliefType.Mountain))
                    || (y-1 >= 0 && (x+(y%2)) < mapWidth && (tileDatas[(x+(y%2)),y-1].reliefType == TileData.ReliefType.Even || tileDatas[(x+(y%2)),y-1].reliefType == TileData.ReliefType.Hill || tileDatas[(x+(y%2)),y-1].reliefType == TileData.ReliefType.Mountain)))
                ){
                    tileDatas[x,y].reliefType = TileData.ReliefType.Water;
                }
                else if(
                    tileDatas[x,y].reliefType == TileData.ReliefType.Water
                    && ((y+2 < mapHeight && x-1 >= 0 && tileDatas[x-1,y+2].reliefType == TileData.ReliefType.Even)
                    || (y+2 < mapHeight && tileDatas[x,y+2].reliefType == TileData.ReliefType.Even)
                    || (y+2 < mapHeight && x+1 < mapWidth && tileDatas[x+1,y+2].reliefType == TileData.ReliefType.Even)
                    || (y+1 < mapHeight && (x+(y%2)-2) >= 0 && tileDatas[(x+(y%2)-2),y+1].reliefType == TileData.ReliefType.Even)
                    || (y+1 < mapHeight && (x+(y%2)+1) < mapWidth && tileDatas[(x+(y%2)+1),y+1].reliefType == TileData.ReliefType.Even)
                    || (x-2 >= 0 && tileDatas[x-2,y].reliefType == TileData.ReliefType.Even)
                    || (x+2 < mapWidth && tileDatas[x+2,y].reliefType == TileData.ReliefType.Even)
                    || (y-1 >= 0 && (x+(y%2)-2) >= 0 && tileDatas[(x+(y%2)-2),y-1].reliefType == TileData.ReliefType.Even)
                    || (y-1 >= 0 && (x+(y%2)+1) < mapWidth && tileDatas[(x+(y%2)+1),y-1].reliefType == TileData.ReliefType.Even)
                    || (y-2 >= 0 && x-1 >= 0 && tileDatas[x-1,y-2].reliefType == TileData.ReliefType.Even)
                    || (y-2 >= 0 && tileDatas[x,y-2].reliefType == TileData.ReliefType.Even)
                    || (y-2 >= 0 && x+1 < mapWidth && tileDatas[x+1,y-2].reliefType == TileData.ReliefType.Even))
                ){
                    tileDatas[x,y].reliefType = Random.value < 0.5f ? TileData.ReliefType.DeepWater : TileData.ReliefType.Water;
                }
                else if(tileDatas[x,y].reliefType == TileData.ReliefType.Water){
                    tileDatas[x, y].reliefType = TileData.ReliefType.DeepWater;
                }

            }
        }

        //Edditing Land Tiles
        for(int y = 0; y < mapHeight; y++){
            for(int x = 0; x < mapWidth; x++){
                if(tileDatas[x,y].reliefType == TileData.ReliefType.Even){
                    if(Random.value > 0.75){
                        tileDatas[x,y].reliefType = Random.value < 0.75 ? TileData.ReliefType.Hill : TileData.ReliefType.Mountain;
                    }
                }
            }
        }

        //Edditing Tiles TarrainType
        int climateBorders = mapHeight/9;
        int climateBordersHalf = climateBorders/2;
        for(int y = 0; y < mapHeight; y++){
            for(int x = 0; x < mapWidth; x++){
                for(int c = 1; c <= 9; c++){
                    if(y < c*climateBorders){
                        int j = y - (c-1)*climateBorders;
                        int possibilityTOChange;
                        if(j == 0) possibilityTOChange = 50;
                        else if( j == climateBorders-1) possibilityTOChange = -50;
                        else possibilityTOChange = 0;
                        tileDatas[x, y].SetClimateType(c, possibilityTOChange);
                        break;
                    }
                }
            }
        }

        //Setting Tile VegetationType
        for(int y = 0; y < mapHeight; y++){
            for(int x = 0; x < mapWidth; x++){
                tileDatas[x, y].SetVegetationType(RandomGenerator.VegetationValueNoise(tileDatas[x,y].coordinates, seed));
            }
        }

        //Edditing BorderTiles to Mountains
        for(int y = 0; y < mapHeight; y++){
            for(int x = 0; x < mapWidth; x++){      

                if(x == 0 || x == mapWidth-1 || y == 0 || y == mapHeight-1){
                    tileDatas[x,y].reliefType = TileData.ReliefType.Mountain;
                }

            }
        }

        return tileDatas;

    }

    public static float CalcVertexHeight(float x, float y , float z, Transform parent, TileData tileData){

        Vector2[] terrainHeightBorders = parent.GetComponent<TerrainManager>().terrainheightBorders;

        Vector3 pos = new Vector3(x, 0, z);
        int rectId = -1;
        int triId = -1;
        float rectAlpha = 0;
        float min = 0;
        float max = 0;
        
        Vector3[] corners = new Vector3[6];
        for(int i = 0; i < 6; i++){
            float rad = Mathf.PI / 180 * (60*(i+1)-30);
            float posX = 1000 * Mathf.Cos(rad);
            float posZ = 1000 * Mathf.Sin(rad);
            corners[i] = new Vector3(posX, 0, posZ);
        }

        //loop rect
        for(int i = 0; i < 6; i++){
            Vector3 c1 = 0.8f * corners[i];
            Vector3 c2 = 0.8f * corners[i == 5 ? 0 : i+1];
            Vector3 c3 = (corners[i == 5 ? 0 : i+1] - corners[i == 4 ? 0 : i == 5 ? 1 : i+2]) * 1.2f +corners[i == 4 ? 0 : i == 5 ? 1 : i+2];
            Vector3 c4 = (corners[i] - corners[i == 0 ? 5 : i-1]) * 1.2f + corners[i == 0 ? 5 : i-1];

            float w1 = (c1.x*(c3.z-c1.z)+(z-c1.z)*(c3.x-c1.x)-x*(c3.z-c1.z))/((c2.z-c1.z)*(c3.x-c1.x)-(c2.x-c1.x)*(c3.z-c1.z));
            float w2 = (z-c1.z-w1*(c2.z-c1.z))/(c3.z-c1.z);
            float w3 = (c3.x*(c1.z-c3.z)+(z-c3.z)*(c1.x-c3.x)-x*(c1.z-c3.z))/((c4.z-c3.z)*(c1.x-c3.x)-(c4.x-c3.x)*(c1.z-c3.z));
            float w4 = (z-c3.z-w3*(c4.z-c3.z))/(c1.z-c3.z);

            if((w1 >= 0 && w2 >= 0 && w1+w2 <= 1) || (w3 >= 0 && w4 >= 0 && w3+w4 <= 1)){
                rectId = i;
                rectAlpha = ((Vector3.Cross((pos - c1) , (c1-c2))).magnitude / (c1-c2).magnitude)/(400 * Mathf.Sin(Mathf.PI / 180 * 60));

                if(tileData.rivers[i] != null){
                    if(tileData.rivers[i].innerTileCoords == tileData.coordinates || tileData.rivers[i].outerTileCoords == tileData.coordinates){
                        float dist = ((Vector3.Cross((pos - corners[i]) , (corners[i]-corners[i == 5 ? 0 : i+1]))).magnitude / (corners[i]-corners[i == 5 ? 0 : i+1]).magnitude) / 100;
                        min = Mathf.Lerp(terrainHeightBorders[1].x, terrainHeightBorders[(int)tileData.reliefType].x, dist);
                        max = Mathf.Lerp(terrainHeightBorders[1].y, terrainHeightBorders[(int)tileData.reliefType].y, dist);
                        return Mathf.Lerp(min, max, y);
                    }
                }
                
                min = Mathf.Lerp(terrainHeightBorders[(int)tileData.reliefType].x, terrainHeightBorders[(int)parent.GetComponent<TerrainManager>().GetNeighborTileData((int)tileData.coordinates.x, (int)tileData.coordinates.y, rectId).reliefType].x, rectAlpha);
                max = Mathf.Lerp(terrainHeightBorders[(int)tileData.reliefType].y, terrainHeightBorders[(int)parent.GetComponent<TerrainManager>().GetNeighborTileData((int)tileData.coordinates.x, (int)tileData.coordinates.y, rectId).reliefType].y, rectAlpha);
                return Mathf.Lerp(min, max, y);
            }

        }

        //loop triangles
        for(int i = 0; i < 6; i++){
            if(rectId != -1) break;
            Vector3 c1 = 0.8f * corners[i];
            Vector3 c2 = (corners[i] - corners[(i == 0 ? 5 : i-1)]) * 1.2f +corners[(i == 0 ? 5 : i-1)];
            Vector3 c3 = (corners[i] - corners[(i == 5 ? 0 : i+1)]) * 1.2f +corners[(i == 5 ? 0 : i+1)];

            Vector3 _c2 = c2;
            Vector3 _c3 = c3;

            if(c1.z+1 > _c3.z && c1.z-1 < _c3.z){
                c2 = _c3;
                c3 = _c2;
            }
            
            float w1 = (c1.x*(c3.z-c1.z)+(z-c1.z)*(c3.x-c1.x)-x*(c3.z-c1.z))/((c2.z-c1.z)*(c3.x-c1.x)-(c2.x-c1.x)*(c3.z-c1.z));
            float w2 = (z-c1.z-w1*(c2.z-c1.z))/(c3.z-c1.z);
            
            if(c1.z+1 > _c3.z && c1.z-1 < _c3.z){
                c2 = _c2;
                c3 = _c3;
            }

            if(w1 >= 0 && w2 >= 0 && w1+w2 <= 1){
                triId = i;
                TileData.ReliefType c2Rel = parent.GetComponent<TerrainManager>().GetNeighborTileData((int)tileData.coordinates.x, (int)tileData.coordinates.y, i).reliefType;
                TileData.ReliefType c3Rel = parent.GetComponent<TerrainManager>().GetNeighborTileData((int)tileData.coordinates.x, (int)tileData.coordinates.y, i == 0 ? 5 : i-1).reliefType;

                TileData neightbor  = GetNeighbor(i, parent.GetComponent<TerrainManager>().allTileDatas, tileData.coordinates);
                
                if((tileData.rivers[i] != null ? tileData.rivers[i].innerTileCoords == tileData.coordinates || tileData.rivers[i].outerTileCoords == tileData.coordinates : false)
                && tileData.rivers[i == 0?5:i-1] != null ? tileData.rivers[i == 0?5:i-1].innerTileCoords == tileData.coordinates || tileData.rivers[i == 0?5:i-1].outerTileCoords == tileData.coordinates : false){
                    float dist = ((Vector3.Cross((pos - corners[i]) , (corners[i]-corners[i == 5 ? 0 : i+1]))).magnitude / (corners[i]-corners[i == 5 ? 0 : i+1]).magnitude) / 100;
                    float dist1 = ((Vector3.Cross((pos - corners[i == 0 ? 5 : i-1]) , (corners[i == 0 ? 5 : i-1]-corners[i]))).magnitude / (corners[i == 0 ? 5 : i-1]-corners[i]).magnitude) / 100;
                    if(dist1 < dist) dist = dist1;
                    
                    min = Mathf.Lerp(terrainHeightBorders[1].x, terrainHeightBorders[(int)tileData.reliefType].x, dist);
                    max = Mathf.Lerp(terrainHeightBorders[1].y, terrainHeightBorders[(int)tileData.reliefType].y, dist);
                    return Mathf.Lerp(min, max, y);
                }
                else if((tileData.rivers[i] != null ? tileData.rivers[i].innerTileCoords == tileData.coordinates || tileData.rivers[i].outerTileCoords == tileData.coordinates : false)
                && (neightbor.rivers[i==0?4:i==1?5:i-2] != null ? neightbor.rivers[i==0?4:i==1?5:i-2].innerTileCoords == neightbor.coordinates || neightbor.rivers[i==0?4:i==1?5:i-2].outerTileCoords == neightbor.coordinates : false)){
                    float dist = ((Vector3.Cross((pos - corners[i]) , (corners[i]-corners[i == 5 ? 0 : i+1]))).magnitude / (corners[i]-corners[i == 5 ? 0 : i+1]).magnitude) / 100;

                    Vector3 mirrorVector = corners[i] - c1;
                    Vector3 normalVector = new Vector3(-mirrorVector.y, 0 , mirrorVector.x);
                    Vector3 dotVector = pos - c1;
                    
                    min = Mathf.Lerp(terrainHeightBorders[1].x, terrainHeightBorders[(int)tileData.reliefType].x, dist);
                    max = Mathf.Lerp(terrainHeightBorders[1].y, terrainHeightBorders[(int)tileData.reliefType].y, dist);
                    return Mathf.Lerp(min, max, y);
                }
                else if((tileData.rivers[i==0?5:i-1] != null ? tileData.rivers[i==0?5:i-1].innerTileCoords == tileData.coordinates || tileData.rivers[i==0?5:i-1].outerTileCoords == tileData.coordinates : false)
                && (neightbor.rivers[i==0?4:i==1?5:i-2] != null ? neightbor.rivers[i==0?4:i==1?5:i-2].innerTileCoords == neightbor.coordinates || neightbor.rivers[i==0?4:i==1?5:i-2].outerTileCoords == neightbor.coordinates : false)){
                    float dist = ((Vector3.Cross((pos - corners[i == 0 ? 5 : i-1]) , (corners[i == 0 ? 5 : i-1]-corners[i]))).magnitude / (corners[i == 0 ? 5 : i-1]-corners[i]).magnitude) / 100;

                    Vector3 mirrorVector = corners[i] - c1;
                    Vector3 normalVector = new Vector3(-mirrorVector.y, 0 , mirrorVector.x);
                    Vector3 dotVector = pos - c1;
                    
                    min = Mathf.Lerp(terrainHeightBorders[1].x, terrainHeightBorders[(int)tileData.reliefType].x, dist);
                    max = Mathf.Lerp(terrainHeightBorders[1].y, terrainHeightBorders[(int)tileData.reliefType].y, dist);
                    return Mathf.Lerp(min, max, y);
                }
                else if((tileData.rivers[i] != null ? tileData.rivers[i].innerTileCoords == tileData.coordinates || tileData.rivers[i].outerTileCoords == tileData.coordinates : false)){
                    float dist = ((Vector3.Cross((pos - corners[i]) , (corners[i]-corners[i == 5 ? 0 : i+1]))).magnitude / (corners[i]-corners[i == 5 ? 0 : i+1]).magnitude) / 100;
                    if(c3Rel == TileData.ReliefType.Water){
                        if(dist < 0.8f){
                            return Mathf.Lerp(terrainHeightBorders[1].x, terrainHeightBorders[1].y, y);
                        }
                        else {
                            min = CalcVertexHeightOnTriangle(c1+Vector3.up*terrainHeightBorders[(int)tileData.reliefType].x, c2+Vector3.up*terrainHeightBorders[(int)c2Rel].x, c3+Vector3.up*terrainHeightBorders[(int)c3Rel].x, pos);
                            max =  CalcVertexHeightOnTriangle(c1+Vector3.up*terrainHeightBorders[(int)tileData.reliefType].y, c2+Vector3.up*terrainHeightBorders[(int)c2Rel].y, c3+Vector3.up*terrainHeightBorders[(int)c3Rel].y, pos);
                            return Mathf.Lerp(min, max, y);
                        }
                    }
                    float distToLine = ((Vector3.Cross((pos - c1) , (c1-c2))).magnitude / (c1-c2).magnitude);
                    if(distToLine < 25){
                        min = Mathf.Lerp(terrainHeightBorders[1].x, terrainHeightBorders[(int)tileData.reliefType].x, dist);
                        max = Mathf.Lerp(terrainHeightBorders[1].y, terrainHeightBorders[(int)tileData.reliefType].y, dist);
                        return Mathf.Lerp(min, max, y);
                    }
                }
                else if((tileData.rivers[i==0?5:i-1] != null ? tileData.rivers[i==0?5:i-1].innerTileCoords == tileData.coordinates || tileData.rivers[i==0?5:i-1].outerTileCoords == tileData.coordinates : false)){
                    float dist = ((Vector3.Cross((pos - corners[i == 0 ? 5 : i-1]) , (corners[i == 0 ? 5 : i-1]-corners[i]))).magnitude / (corners[i == 0 ? 5 : i-1]-corners[i]).magnitude) / 100;
                    if(c2Rel == TileData.ReliefType.Water){
                        if(dist < 0.8f){
                            return Mathf.Lerp(terrainHeightBorders[1].x, terrainHeightBorders[1].y, y);
                        }
                        else{
                            min = CalcVertexHeightOnTriangle(c1+Vector3.up*terrainHeightBorders[(int)tileData.reliefType].x, c2+Vector3.up*terrainHeightBorders[(int)c2Rel].x, c3+Vector3.up*terrainHeightBorders[(int)c3Rel].x, pos);
                            max =  CalcVertexHeightOnTriangle(c1+Vector3.up*terrainHeightBorders[(int)tileData.reliefType].y, c2+Vector3.up*terrainHeightBorders[(int)c2Rel].y, c3+Vector3.up*terrainHeightBorders[(int)c3Rel].y, pos);
                            return Mathf.Lerp(min, max, y);
                        }
                    }
                    float distToLine = ((Vector3.Cross((pos - c1) , (c1-c3))).magnitude / (c1-c3).magnitude);
                    if(distToLine < 25){
                        min = Mathf.Lerp(terrainHeightBorders[1].x, terrainHeightBorders[(int)tileData.reliefType].x, dist);
                        max = Mathf.Lerp(terrainHeightBorders[1].y, terrainHeightBorders[(int)tileData.reliefType].y, dist);
                        return Mathf.Lerp(min, max, y);
                    }
                }
                else if((neightbor.rivers[i==0?4:i==1?5:i-2] != null ? neightbor.rivers[i==0?4:i==1?5:i-2].innerTileCoords == neightbor.coordinates || neightbor.rivers[i==0?4:i==1?5:i-2].outerTileCoords == neightbor.coordinates : false)
                && tileData.reliefType == TileData.ReliefType.Water){
                    float dist = ((Vector3.Cross((pos - corners[i]) , (corners[i]-c1))).magnitude / (corners[i]-c1).magnitude) / 100;
                    if(dist < 0.8f){
                        break;
                    }
                    else{
                        min = CalcVertexHeightOnTriangle(c1+Vector3.up*terrainHeightBorders[(int)tileData.reliefType].x, c2+Vector3.up*terrainHeightBorders[(int)c2Rel].x, c3+Vector3.up*terrainHeightBorders[(int)c3Rel].x, pos);
                            max =  CalcVertexHeightOnTriangle(c1+Vector3.up*terrainHeightBorders[(int)tileData.reliefType].y, c2+Vector3.up*terrainHeightBorders[(int)c2Rel].y, c3+Vector3.up*terrainHeightBorders[(int)c3Rel].y, pos);
                            return Mathf.Lerp(min, max, y);
                    }
                }
                
                
                min = CalcVertexHeightOnTriangle(c1+Vector3.up*terrainHeightBorders[(int)tileData.reliefType].x, c2+Vector3.up*terrainHeightBorders[(int)c2Rel].x, c3+Vector3.up*terrainHeightBorders[(int)c3Rel].x, pos);
                max =  CalcVertexHeightOnTriangle(c1+Vector3.up*terrainHeightBorders[(int)tileData.reliefType].y, c2+Vector3.up*terrainHeightBorders[(int)c2Rel].y, c3+Vector3.up*terrainHeightBorders[(int)c3Rel].y, pos);
                return Mathf.Lerp(min, max, y);

            }

        }
        
        return Mathf.Lerp(terrainHeightBorders[(int)tileData.reliefType].x, terrainHeightBorders[(int)tileData.reliefType].y, y);
        
    }

    public static float CalcVertexHeightOnTriangle(Vector3 c1, Vector3 c2, Vector3 c3, Vector3 pos){

        Vector3 normal = Vector3.Cross(c2-c1, c3-c1).normalized;

        float height = (-(normal.x*(pos.x-c1.x))-(normal.z*(pos.z-c1.z)))/(normal.y)+c1.y;

        return height;
    }

    public static RiverData[] GenerateRiver(TileData[,] tileDatas, float riverBorder, int seed){

        RiverData[] allRiverDatas = new RiverData[0];
        
        foreach(TileData tileData in tileDatas){
            for(int i = 0; i < 6; i++){

                TileData neightbor = GetNeighbor(i, tileDatas, tileData.coordinates);
                int neightborRiverIndex = i == 3? 0: i == 4? 1: i == 5? 2 : i+3;

                if(tileData.rivers[i] != null || neightbor.rivers[neightborRiverIndex] != null) continue;
                if(tileData.reliefType != TileData.ReliefType.Even || neightbor.reliefType != TileData.ReliefType.Even) continue;

                float rad = Mathf.PI / 180 * (60*i-30);
                float posX = Mathf.Cos(rad) + tileData.mapPosition.x;
                float posY = Mathf.Sin(rad) + tileData.mapPosition.y;
                Vector2 pos = new Vector2(posX, posY);

                float riverValue = RandomGenerator.RiverNoise(pos, seed);

                if(riverValue > riverBorder) continue;

                RiverData newRiver = new RiverData(tileData.coordinates, neightbor.coordinates , i, neightborRiverIndex);

                tileData.rivers[i] = newRiver;

                neightbor.rivers[neightborRiverIndex] = newRiver;

                allRiverDatas = AddRiverToArray(allRiverDatas, newRiver);

            }
        }

        return allRiverDatas;

    }

    static RiverData[] AddRiverToArray(RiverData[] oldArray, RiverData newRiver){

        RiverData[] newArray = new RiverData[oldArray.Length+1];

        oldArray.CopyTo(newArray, 0);
        newArray[oldArray.Length] = newRiver;

        return newArray;

    }

    static TileData GetNeighbor(int neighborIndex, TileData[,] allTileDatas, Vector2 coordinates){

        int mapHeight = allTileDatas.GetLength(1);
        int mapWidth = allTileDatas.GetLength(0);

        int x = (int)coordinates.x;
        int y = (int)coordinates.y;

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
        return null;

    }
}
