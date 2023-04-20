using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseGenerator
{
    public static float[,] GenerateChunk2dNoise(int mapWidth, int mapLenght, int xOffset, int yOffset, float scale, int octaves, float persistance, float lacunarity)
    {
        float[,] noiseMap = new float[mapWidth, mapLenght];

        if (scale <= 0) {
            scale = 0.00001f;
        }

        for (int x = 0; x < mapWidth; x++) {
            for (int y = 0; y < mapLenght; y++) {

                float amplitude = 1f;
                float frequency = 1f;
                float perlinValue = 0f;

                for (int i = 0; i < octaves; i++) {
                    float sampleX = (xOffset + x) / scale * frequency;
                    float sampleY = (yOffset + y) / scale * frequency;

                    perlinValue += Mathf.PerlinNoise(sampleX + 200000f, sampleY + 200000f) * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                noiseMap[x, y] = perlinValue;
            }
        }

        return noiseMap;
    }

    public static float[,,] GenerateChunk3dNoise(int mapWidth, int mapHeight, int mapLenght, int xOffset, int yOffset, int zOffset, 
                                                float scale, int octaves, float persistance, float lacunarity, float flatness, int normalHeight)
    {
        flatness *= 25;
        float[,,] noiseMap = new float[mapWidth, mapHeight, mapLenght];

        if (scale <= 0) {
            scale = 0.00001f;
        }

        for (int x = 0; x < mapWidth; x++) {
            for (int z = 0; z < mapLenght; z++) {
                for (int y = 0; y < mapHeight; y++) {

                    float amplitude = 1f;
                    float frequency = 1f;
                    float perlinValue = 0f;

                    for (int i = 0; i < octaves; i++) {
                        float sampleX = (xOffset + x) / scale * frequency;
                        float sampleY = (yOffset + y) / scale * frequency;
                        float sampleZ = (zOffset + z) / scale * frequency;

                        // 200000f added to not see mirroring
                        perlinValue += PerlinNoise3D(sampleX + 100000f, sampleY + 200000f, sampleZ + 300000f) * amplitude;

                        amplitude *= persistance;
                        frequency *= lacunarity;
                    }

                    // below is positive
                    int distanceFromGroundLevel = y - normalHeight;

                    perlinValue = Mathf.Clamp01(perlinValue - distanceFromGroundLevel / flatness);

                    noiseMap[x, y, z] = perlinValue;
                }
            }
        }

        return noiseMap;
    }

    // Todo: optimise
    public static float PerlinNoise3D(float x, float y, float z)
    {
        float xy = Mathf.PerlinNoise(x, y);
        float xz = Mathf.PerlinNoise(x, z);
        float yz = Mathf.PerlinNoise(y, z);
        float yx = Mathf.PerlinNoise(y, x);
        float zx = Mathf.PerlinNoise(z, x);
        float zy = Mathf.PerlinNoise(z, y);
    
        return (xy + xz + yz + yx + zx + zy) / 6;
    }
}
