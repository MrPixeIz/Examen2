using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MovementGenerator : MonoBehaviour
{

    public int mapWidth = 256;
    public int mapHeigth = 256;
    public float noiseScale;
    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;
    public bool autoUpdate;
    public int seed;
    public Vector2 offset;

    public int maxNumberOfFishes = 100;
    private float timeLastUpdate;

    private float[,] noiseMapX;
    private float[,] noiseMapY;
    private void Start()
    {

        timeLastUpdate = Time.time;
        noiseMapX = Noise.GenerateNoiseMap(mapWidth, mapHeigth, UnityEngine.Random.Range(1, 25), noiseScale, octaves, persistance, lacunarity, offset);
        noiseMapY = Noise.GenerateNoiseMap(mapWidth, mapHeigth, UnityEngine.Random.Range(1, 25), noiseScale, octaves, persistance, lacunarity, offset);
    }

    private void Update()
    {

        foreach (Fish fish in FindObjectsOfType<Fish>())
        {
            //Code pour le mouvement des poissons

            Rigidbody2D fishToMove = fish.GetComponent<Rigidbody2D>();
            int x = (int)fish.transform.position.x;
            int y = (int)fish.transform.position.y;
            x = Math.Abs(x % mapWidth);
            y = Math.Abs(y % mapWidth);
            fishToMove.AddForce(new Vector2(noiseMapX[x, y] - 0.5f, noiseMapY[x, y] - 0.5f));

        }
        if (Time.time - timeLastUpdate > 5)
        {
            //Code pour nouveau mouvement

            noiseMapX = Noise.GenerateNoiseMap(mapWidth, mapHeigth, UnityEngine.Random.Range(1, 25), noiseScale, octaves, persistance, lacunarity, offset);
            noiseMapY = Noise.GenerateNoiseMap(mapWidth, mapHeigth, UnityEngine.Random.Range(1, 25), noiseScale, octaves, persistance, lacunarity, offset);
            timeLastUpdate = Time.time;
        }

    }



    private void OnValidate()
    {
        if (mapWidth < 1)
        {
            mapWidth = 1;
        }
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
        if (octaves < 0)
        {
            octaves = 0;
        }

    }


}

