using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StartTaskGame : MonoBehaviour
{
    float solutionEfficiency;

    string taskText;

    int mapSize = 10;

    float mapScale;
    float border = 10;

    public float showObjectSize;

    public Image tile;

    public Image mapBackground;

    public Text timer;

    public static Image[,] mapTiles;
    public static int[,] mapTilesValue;

    private void Start()
    {
        mapScale = mapBackground.rectTransform.rect.width;

        mapTiles = new Image[mapSize + 1, mapSize + 1];
        mapTilesValue = new int[mapSize + 1, mapSize + 1];

        float tileSize = (mapScale - border * 2) / mapSize;
        float startPosX = border + tileSize / 2 - mapScale / 2;
        float startPosY = - startPosX + mapBackground.rectTransform.anchoredPosition.y;

        // Генерируем карту
        for (int x = 1; x <= mapSize; x++)
        {
            for (int y = 1; y <= mapSize; y++)
            {
                mapTiles[x, y] = Instantiate(tile, new Vector2(startPosX + (x - 1) * tileSize, startPosY + - (y - 1) * tileSize),
                    Quaternion.identity, mapBackground.transform);

                mapTiles[x, y].rectTransform.sizeDelta = new Vector2(tileSize, tileSize);
                mapTiles[x, y].GetComponent<TileManagerTaskGame>().X = x;
                mapTiles[x, y].GetComponent<TileManagerTaskGame>().Y = y;
                mapTiles[x, y].GetComponent<TileManagerTaskGame>().Value = 0;

                mapTilesValue[x, y] = 0;
            }
        }

        tile.gameObject.SetActive(false);

        VariablesTaskGame.ShowCurrentObject.rectTransform.sizeDelta = new Vector2(tileSize * showObjectSize, tileSize * showObjectSize);

        Timer.timer = timer;
        Timer.stopTimer = StopTimer;
        Timer.Start(new Clock(10));
    }

    public static void SetTile(int x, int y, int value)
    {
        mapTiles[x, y].GetComponent<TileManagerTaskGame>().Value = value;
        mapTiles[x, y].GetComponent<Image>().sprite = VariablesTaskGame.Sprites[value];
    }

    public void StopTimer()
    {

    }
}