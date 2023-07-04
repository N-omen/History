using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomGenerator
{
    public static float LandmassNoise(Vector2 position, int mapHeight, int mapWidth, int seed){
        
        float sampleX1 = ((position.x + seed)) / (0.18f * mapWidth);
        float sampleY1 = ((position.y + seed)) / (0.32f * mapHeight);
        float randomeValue = Mathf.PerlinNoise(sampleX1, sampleY1);

        float sampleX2 = ((position.x + Random.Range(-99999,99999))) / (0.09f * mapWidth);
        float sampleY2 = ((position.y + Random.Range(-99999,99999))) / (0.16f * mapHeight);
        randomeValue += Mathf.PerlinNoise(sampleX2, sampleY2) /4;

        randomeValue = Mathf.InverseLerp(0,1f+1/4f,randomeValue);

        return randomeValue;
    }

    public static float TerrainNoise(Vector2 position, int seed){
        float sampleX1 = ((position.x + seed)) / 250;
        float sampleZ1 = ((position.y + seed)) / 250;
        float randomeValue = Mathf.PerlinNoise(sampleX1, sampleZ1);

        float sampleX2 = ((position.x + seed) / 50);
        float sampleZ2 = ((position.y + seed) / 50);
        randomeValue += Mathf.PerlinNoise(sampleX2, sampleZ2) /6;

        randomeValue = Mathf.InverseLerp(0,1f+1/6,randomeValue);

        return randomeValue;
    }

    public static float RiverNoise(Vector2 position, int seed){
        float sampleX1 = ((position.x + seed)) / 8;
        float sampleZ1 = ((position.y + seed)) / 8;
        float randomeValue = Mathf.PerlinNoise(sampleX1, sampleZ1);

        float sampleX2 = ((position.x + seed) / 30);
        float sampleZ2 = ((position.y + seed) / 30);
        randomeValue += Mathf.PerlinNoise(sampleX2, sampleZ2) /2;

        randomeValue = Mathf.InverseLerp(0,1f+1/2,randomeValue);

        return randomeValue;
    }

    public static float VegetationValueNoise(Vector2 position, int seed){
        float sampleX1 = ((position.x + seed)) / 5;
        float sampleZ1 = ((position.y + seed)) / 5;
        float randomeValue = Mathf.PerlinNoise(sampleX1, sampleZ1);

        float sampleX2 = ((position.x + seed) / 2);
        float sampleZ2 = ((position.y + seed) / 2);
        randomeValue += Mathf.PerlinNoise(sampleX2, sampleZ2) /6;

        randomeValue = Mathf.InverseLerp(0,1f+1/6,randomeValue);

        return randomeValue;
    }
}
