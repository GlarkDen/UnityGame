using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CreateBlockMenu : MonoBehaviour
{
    public int mapSize;

    float mapScale;
    public float border;

    public Image tile;

    public Image mapBackground;

    public static Image[,] mapTiles;
    public static int[,] mapTilesValue;

    public float showObjectSize;

    private void Start()
    {
        mapScale = mapBackground.rectTransform.rect.width;

        mapTiles = new Image[mapSize + 1, mapSize + 1];
        mapTilesValue = new int[mapSize + 1, mapSize + 1];

        float tileSize = (mapScale - border * 2) / mapSize;
        float startPosX = border + tileSize / 2 - mapScale / 2;
        float startPosY = -startPosX + mapBackground.rectTransform.anchoredPosition.y;

        startPosX += mapBackground.rectTransform.anchoredPosition.x;

        Vector2 tileSizeVector = new Vector2(tileSize, tileSize);

        // Генерируем карту
        for (int x = 1; x <= mapSize; x++)
        {
            for (int y = 1; y <= mapSize; y++)
            {
                mapTiles[x, y] = Instantiate(tile, new Vector2(startPosX + (x - 1) * tileSize, startPosY + -(y - 1) * tileSize),
                    Quaternion.identity, mapBackground.transform);

                mapTiles[x, y].rectTransform.sizeDelta = tileSizeVector;
                mapTiles[x, y].GetComponent<CreateTileManager>().X = x;
                mapTiles[x, y].GetComponent<CreateTileManager>().Y = y;
                mapTiles[x, y].GetComponent<CreateTileManager>().Value = 0;

                mapTilesValue[x, y] = 0;
            }
        }

        tile.gameObject.SetActive(false);

        CreateBlocksVariables.ShowCurrentObject.rectTransform.sizeDelta = new Vector2(tileSize * showObjectSize, tileSize * showObjectSize);
    }

    public static void SetTile(int x, int y, int value)
    {
        mapTiles[x, y].GetComponent<CreateTileManager>().Value = value;
        mapTiles[x, y].GetComponent<Image>().sprite = CreateBlocksVariables.Sprites[value];
    }
}
