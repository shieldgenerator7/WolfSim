using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public GameObject grassPrefab;
    public GameObject waterPrefab;
    public int tileHeight = 100;//how many tiles across
    public int tileWidth = 100;//how many tiles from top to bottom

	// Use this for initialization
	void Start () {
        generateLevel(tileWidth, tileHeight);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void generateLevel(int width, int height)
    {
        GameObject[,] tiles = new GameObject[width, height];
        generateFill(grassPrefab, tiles, width, height);
        generateRiver(waterPrefab, tiles, width, height, height/4);
        generateRiver(waterPrefab, tiles, width, height, height/2);
        generateRiver(waterPrefab, tiles, width, height, height / 2);
        generateRiver(waterPrefab, tiles, width, height, 3*height / 4);
        for (int xi = 0; xi < width; xi++)
        {
            for (int yi = 0; yi < height; yi++)
            {
                GameObject go = GameObject.Instantiate(tiles[xi, yi]);
                go.transform.position = new Vector2(xi - width/2, yi - height/2);
            }
        }
    }

    private void generateFill(GameObject prefab, GameObject[,] prefabMap, int width, int height)
    {
        for (int xi = 0; xi < width; xi++)
        {
            for (int yi = 0; yi < height; yi++)
            {
                prefabMap[xi, yi] = prefab;
            }
        }
    }

    private void generateRiver(GameObject prefab, GameObject[,] prefabMap, int width, int height, int startY)
    {
        int currentY = startY;
        int prevY = currentY;
        for (int xi = 0; xi < width; xi++)
        {
            if (Random.Range(0,2) > 0)
            {
                currentY += Random.Range(-2, 2);
            }
            for (int yi = prevY; yi != currentY; yi += (int)Mathf.Sign(currentY - prevY))
            {
                if (yi >= 0 && yi < height)
                {
                    prefabMap[xi, yi] = prefab;
                }
            }
            if (currentY >= 0 && currentY < height)
            {
                prefabMap[xi, currentY] = prefab;
            }
            prevY = currentY;
        }
    }
}
