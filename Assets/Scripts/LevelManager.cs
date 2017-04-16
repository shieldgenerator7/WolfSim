using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public GameObject grassPrefab;
    public GameObject waterPrefab;
    public GameObject treePrefab;
    public int tileHeight = 100;//how many tiles across
    public int tileWidth = 100;//how many tiles from top to bottom
    public GameObject[,] tileMap;//the map of tiles

    private static LevelManager instance;

	// Use this for initialization
	void Start () {
        //Singleton sorting out
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
        //Initialization stuff
        generateLevel(tileWidth, tileHeight);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public static LevelTile getTile(Vector2 pos)
    {
        return instance.tileMap[(int)Mathf.Ceil(pos.x)+instance.tileWidth/2, (int)Mathf.Ceil(pos.y)+instance.tileHeight/2].GetComponent<LevelTile>();
    }
    public static int getDisplaySortingOrder(Vector2 pos)
    {
        return (int)((instance.tileHeight/2 - pos.y) * 100);
    }
    public static Vector2 randomPosition()
    {
        return new Vector2(
            Random.Range(-instance.tileWidth / 2, instance.tileWidth / 2),
            Random.Range(-instance.tileHeight / 2, instance.tileHeight / 2)
            );
    }
    public static bool inBounds(Vector2 pos)
    {
        return pos.x > -instance.tileWidth / 2
            && pos.x < instance.tileWidth / 2
            && pos.y > -instance.tileHeight / 2
            && pos.y < instance.tileHeight / 2;
    }

    private void generateLevel(int width, int height)
    {
        GameObject[,] tiles = new GameObject[width, height];
        generateFill(grassPrefab, tiles, width, height);
        generateRiver(waterPrefab, tiles, width, height, height/4);
        generateRiver(waterPrefab, tiles, width, height, height/2);
        generateRiver(waterPrefab, tiles, width, height, height / 2);
        generateRiver(waterPrefab, tiles, width, height, 3*height / 4);
        tileMap = new GameObject[width, height];
        for (int xi = 0; xi < width; xi++)
        {
            for (int yi = 0; yi < height; yi++)
            {
                GameObject go = GameObject.Instantiate(tiles[xi, yi]);
                go.transform.position = new Vector2(xi - width/2, yi - height/2);
                tileMap[xi, yi] = go;
            }
        }
        generateForest(treePrefab, new Vector2(90, 90), new Vector2(0, 0));
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

    private void generateForest(GameObject prefab, Vector2 size, Vector2 pos)
    {
        for (int count = 0; count < size.x * size.y / 3; count++)
        {
            float randomX = Random.Range(-size.x / 2, size.x / 2) + pos.x;
            float randomY = Random.Range(-size.y / 2, size.y / 2) + pos.y;
            Vector2 randomPos = new Vector2(randomX, randomY);
            if (!getTile(randomPos).gameObject.name.Contains("water"))
            {
                GameObject go = GameObject.Instantiate(prefab);
                go.transform.position = randomPos;
                go.GetComponent<SpriteRenderer>().sortingOrder = getDisplaySortingOrder(randomPos);
            }
        }
    }
}
