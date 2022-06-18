using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGameMechanic : MonoBehaviour
{
    public static string Alphabet = "ABCDEGHIJKLMNOPQRSTUVWXYZ";

    int mapSize = 11;

    float border = 10;

    public float showObjectSize;

    public Image tile;

    public Image mapBackground;

    public static Image[,] mapTiles;
    public static int[,] mapTilesValue;

    public Text taskText;

    public Transform BlockNumber;
    private Transform BlockNumberPanel;

    public GameObject BlockDescription;

    public static List<Block> sensorBlockList = new List<Block>();
    private List<int> sensorBlocksCount = new List<int>();
    public static List<Transform> setSensorBlocks = new List<Transform>();

    public static Block MehanicBlock;
    public static Coordinate mehanicBlockCoodinate;
    public Image ShowCurrentObject;

    private void Start()
    {

    }

    public void SetTaskText(string text)
    {
        taskText.text = text;
    }

    public void SetBlockCount(List<Block> sensorBlockList, List<int> sensorBlocksCount)
    {
        StartGameMechanic.sensorBlockList = sensorBlockList;
        this.sensorBlocksCount = sensorBlocksCount;

        VariablesMechanic.CountSensorBlocks = this.sensorBlocksCount.ToArray();
        VariablesMechanic.CountSensors = StartGameMechanic.sensorBlockList.Count;

        BlockNumberPanel = BlockNumber.parent;
        BlockNumber.GetChild(0).GetComponent<Image>().sprite = StartGameMechanic.sensorBlockList[0].sprite;
        BlockNumber.GetChild(1).GetComponent<Text>().text = this.sensorBlocksCount[0].ToString();
        BlockNumber.GetChild(2).GetComponent<Text>().text = Alphabet[0].ToString();
        BlockNumber.GetComponent<SensorBlockManagerMechanic>().Number = 0;

        setSensorBlocks.Add(BlockNumber);

        for (int i = 1; i < StartGameMechanic.sensorBlockList.Count; i++)
        {
            setSensorBlocks.Add(Instantiate(BlockNumber, new Vector2(0, 0),
                    Quaternion.identity, BlockNumberPanel));

            setSensorBlocks[i].GetChild(0).GetComponent<Image>().sprite = StartGameMechanic.sensorBlockList[i].sprite;
            setSensorBlocks[i].GetChild(1).GetComponent<Text>().text = this.sensorBlocksCount[i].ToString();
            setSensorBlocks[i].GetChild(2).GetComponent<Text>().text = Alphabet[i].ToString();
            setSensorBlocks[i].GetComponent<SensorBlockManagerMechanic>().Number = i;
        }

        BlockDescription.SetActive(false);

        VariablesMechanic.SetBlockSprites();
    }

    public void ResetBlockCount()
    {
        for (int i = 0; i < sensorBlockList.Count; i++)
        {
            setSensorBlocks[i].GetChild(1).GetComponent<Text>().text = this.sensorBlocksCount[i].ToString();
            VariablesMechanic.CountSensorBlocks[i] = this.sensorBlocksCount[i];
        }
    }

    public void ClearBlockCount()
    {
        for (int i = 0; i < sensorBlockList.Count; i++)
        {
            setSensorBlocks[i].GetChild(1).GetComponent<Text>().text = "0";
            VariablesMechanic.CountSensorBlocks[i] = 0;
        }
    }

    public void SetBlocksCount(int[] blocksCount)
    {
        for (int i = 0; i < blocksCount.Length; i++)
        {
            setSensorBlocks[i].GetChild(1).GetComponent<Text>().text = blocksCount[i].ToString();
            VariablesMechanic.CountSensorBlocks[i] = blocksCount[i];
        }
    }

    public void GenerateMap(int mapSize, Block mehanicBlock)
    {
        if (!tile.gameObject.activeSelf)
            tile.gameObject.SetActive(true);

        MehanicBlock = mehanicBlock;
        this.mapSize = mapSize;

        mapTiles = new Image[mapSize + 1, mapSize + 1];
        mapTilesValue = new int[mapSize + 1, mapSize + 1];

        float mapScale = mapBackground.rectTransform.rect.width;

        float tileSize = (mapScale - border * 2) / mapSize;
        float startPosX = border + tileSize / 2 - mapScale / 2;
        float startPosY = -startPosX + mapBackground.rectTransform.anchoredPosition.y;

        mehanicBlockCoodinate.X = mapSize / 2 + 1;
        mehanicBlockCoodinate.Y = 2;

        // Генерируем карту
        for (int x = 1; x <= mapSize; x++)
        {
            for (int y = 1; y <= mapSize; y++)
            {
                mapTiles[x, y] = Instantiate(tile, new Vector2(startPosX + (x - 1) * tileSize, startPosY + -(y - 1) * tileSize),
                    Quaternion.identity, mapBackground.transform);

                mapTiles[x, y].rectTransform.sizeDelta = new Vector2(tileSize, tileSize);
                mapTiles[x, y].GetComponent<TileManagerMechanic>().X = x;
                mapTiles[x, y].GetComponent<TileManagerMechanic>().Y = y;
                mapTiles[x, y].GetComponent<TileManagerMechanic>().Value = 0;

                mapTilesValue[x, y] = 0;
            }
        }

        tile.gameObject.SetActive(false);

        mapTiles[mehanicBlockCoodinate.X, mehanicBlockCoodinate.Y].rectTransform.sizeDelta = new Vector2(tileSize, tileSize);
        mapTiles[mehanicBlockCoodinate.X, mehanicBlockCoodinate.Y].GetComponent<TileManagerMechanic>().X = mehanicBlockCoodinate.X;
        mapTiles[mehanicBlockCoodinate.X, mehanicBlockCoodinate.Y].GetComponent<TileManagerMechanic>().Y = mehanicBlockCoodinate.Y;
        mapTiles[mehanicBlockCoodinate.X, mehanicBlockCoodinate.Y].GetComponent<TileManagerMechanic>().Value = -1;
        mapTiles[mehanicBlockCoodinate.X, mehanicBlockCoodinate.Y].GetComponent<TileManagerMechanic>().GetComponent<Image>().sprite = mehanicBlock.sprite;

        ShowCurrentObject.rectTransform.sizeDelta = new Vector2(tileSize * showObjectSize, tileSize * showObjectSize);
    }

    public void GenerateMap(Block mehanicBlock, Image[,] tileMap, int[,] tileMapValue)
    {
        if (!tile.gameObject.activeSelf)
            tile.gameObject.SetActive(true);

        MehanicBlock = mehanicBlock;
        mapSize = tileMap.Length + 1;

        mapTiles = new Image[mapSize + 1, mapSize + 1];
        mapTilesValue = new int[mapSize + 1, mapSize + 1];

        float mapScale = mapBackground.rectTransform.rect.width;

        float tileSize = (mapScale - border * 2) / mapSize;
        float startPosX = border + tileSize / 2 - mapScale / 2;
        float startPosY = -startPosX + mapBackground.rectTransform.anchoredPosition.y;

        mehanicBlockCoodinate.X = mapSize / 2 + 1;
        mehanicBlockCoodinate.Y = 2;

        // Генерируем карту
        for (int x = 1; x <= mapSize; x++)
        {
            for (int y = 1; y <= mapSize; y++)
            {
                mapTiles[x, y] = Instantiate(tile, new Vector2(startPosX + (x - 1) * tileSize, startPosY + -(y - 1) * tileSize),
                    Quaternion.identity, mapBackground.transform);

                mapTiles[x, y].rectTransform.sizeDelta = new Vector2(tileSize, tileSize);
                mapTiles[x, y].GetComponent<TileManagerMechanic>().X = x;
                mapTiles[x, y].GetComponent<TileManagerMechanic>().Y = y;
                mapTiles[x, y].GetComponent<TileManagerMechanic>().Value = tileMapValue[x, y];
                mapTiles[x, y].GetComponent<TileManagerMechanic>().GetComponent<Image>().sprite = tileMap[x, y].sprite;

                mapTilesValue[x, y] = tileMapValue[x, y];
            }
        }

        tile.gameObject.SetActive(false);

        VariablesMechanic.ShowCurrentObject.rectTransform.sizeDelta = new Vector2(tileSize * showObjectSize, tileSize * showObjectSize);
        
    }

    public void GenerateMap(Block mehanicBlock, int[,] tileMapValue)
    {
        if (!tile.gameObject.activeSelf)
            tile.gameObject.SetActive(true);

        MehanicBlock = mehanicBlock;
        mapSize = tileMapValue.Length + 1;

        mapTiles = new Image[mapSize + 1, mapSize + 1];
        mapTilesValue = new int[mapSize + 1, mapSize + 1];

        float mapScale = mapBackground.rectTransform.rect.width;

        float tileSize = (mapScale - border * 2) / mapSize;
        float startPosX = border + tileSize / 2 - mapScale / 2;
        float startPosY = -startPosX + mapBackground.rectTransform.anchoredPosition.y;

        mehanicBlockCoodinate.X = mapSize / 2 + 1;
        mehanicBlockCoodinate.Y = 2;

        // Генерируем карту
        for (int x = 1; x <= mapSize; x++)
        {
            for (int y = 1; y <= mapSize; y++)
            {
                mapTiles[x, y] = Instantiate(tile, new Vector2(startPosX + (x - 1) * tileSize, startPosY + -(y - 1) * tileSize),
                    Quaternion.identity, mapBackground.transform);

                mapTiles[x, y].rectTransform.sizeDelta = new Vector2(tileSize, tileSize);
                mapTiles[x, y].GetComponent<TileManagerMechanic>().X = x;
                mapTiles[x, y].GetComponent<TileManagerMechanic>().Y = y;
                mapTiles[x, y].GetComponent<TileManagerMechanic>().Value = tileMapValue[x, y];
                mapTiles[x, y].GetComponent<TileManagerMechanic>().GetComponent<Image>().sprite = VariablesMechanic.Sprites[tileMapValue[x, y]];

                mapTilesValue[x, y] = tileMapValue[x, y];
            }
        }

        tile.gameObject.SetActive(false);

        VariablesMechanic.ShowCurrentObject.rectTransform.sizeDelta = new Vector2(tileSize * showObjectSize, tileSize * showObjectSize);

    }

    public void ChangeMap(int[,] tileMapValue)
    {
        for (int x = 1; x <= mapSize; x++)
        {
            for (int y = 1; y <= mapSize; y++)
            {
                if (x == mehanicBlockCoodinate.X && y == mehanicBlockCoodinate.Y)
                    continue;

                mapTiles[x, y].GetComponent<TileManagerMechanic>().Value = tileMapValue[x, y];
                mapTiles[x, y].GetComponent<TileManagerMechanic>().GetComponent<Image>().sprite = VariablesMechanic.Sprites[tileMapValue[x, y]];

                mapTilesValue[x, y] = tileMapValue[x, y];
            }
        }
    }

    public void SetTile(int x, int y, int value)
    {
        mapTiles[x, y].GetComponent<TileManagerMechanic>().Value = value;
        mapTiles[x, y].GetComponent<Image>().sprite = VariablesMechanic.Sprites[value];
    }

    public void RemoveMap()
    {
        for (int x = 1; x <= mapSize; x++)
            for (int y = 1; y <= mapSize; y++)
                Destroy(mapTiles[x, y].gameObject);

        mapTiles = null;
        mapTilesValue = null;
    }

    public void ClearMap()
    {
        for (int x = 1; x <= mapSize; x++)
        {
            for (int y = 1; y <= mapSize; y++)
            {
                if (mapTiles[x, y].GetComponent<TileManagerMechanic>().Value <= 0)
                    continue;

                mapTiles[x, y].GetComponent<TileManagerMechanic>().X = x;
                mapTiles[x, y].GetComponent<TileManagerMechanic>().Y = y;
                mapTiles[x, y].GetComponent<TileManagerMechanic>().Value = 0;
                mapTiles[x, y].GetComponent<TileManagerMechanic>().GetComponent<Image>().sprite = tile.sprite;

                mapTilesValue[x, y] = 0;

            }
        }

        for (int i = 0; i < sensorBlockList.Count; i++)
        {
            setSensorBlocks[i].GetChild(1).GetComponent<Text>().text = sensorBlocksCount[i].ToString();
            VariablesMechanic.CountSensorBlocks[i] = sensorBlocksCount[i];
        }
    }

    public void RemoveBlockCount()
    {
        for (int i = 1; i < BlockNumberPanel.childCount; i++)
            Destroy(BlockNumberPanel.GetChild(i).gameObject);

        setSensorBlocks.Clear();
        sensorBlocksCount.Clear();
        sensorBlockList.Clear();
    }

    public Map SaveMap()
    {
        Map map = new Map();
        map.mapSize = mapSize;
        map.mapTilesValue = mapTilesValue;

        return map;
    }
}
