using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGameMechanic : MonoBehaviour
{
    public static string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

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

    public static List<Block> ActivateSensorBlocks = new List<Block>();
    private List<int> activateSensorBlocksCount = new List<int>();
    public static List<Transform> setSensorBlocks = new List<Transform>();

    private Block mehanicBlock;
    public static Coordinate mehanicBlockCoodinate;
    public Image ShowCurrentObject;

    private void Start()
    {

    }

    public void SetTaskText(string text)
    {
        taskText.text = text;
    }

    public void SetBlockCount(List<Block> sensorBlocks, Dictionary<int, Transform> setBlocksCount)
    {
        foreach (int key in setBlocksCount.Keys)
            ActivateSensorBlocks.Add(sensorBlocks[key]);

        foreach (Transform value in setBlocksCount.Values)
            activateSensorBlocksCount.Add(int.Parse(value.GetChild(0).GetComponent<InputField>().text));

        VariablesMechanic.CountSensorBlocks = activateSensorBlocksCount.ToArray();
        VariablesMechanic.CountSensors = ActivateSensorBlocks.Count;

        BlockNumberPanel = BlockNumber.parent;

        BlockNumber.GetChild(0).GetComponent<Image>().sprite = ActivateSensorBlocks[0].sprite;
        BlockNumber.GetChild(1).GetComponent<Text>().text = activateSensorBlocksCount[0].ToString();
        BlockNumber.GetChild(2).GetComponent<Text>().text = Alphabet[0].ToString();
        BlockNumber.GetComponent<SensorBlockManagerMechanic>().Number = 0;

        setSensorBlocks.Add(BlockNumber);

        for (int i = 1; i < ActivateSensorBlocks.Count; i++)
        {
            setSensorBlocks.Add(Instantiate(BlockNumber, new Vector2(0, 0),
                    Quaternion.identity, BlockNumberPanel));

            setSensorBlocks[i].GetChild(0).GetComponent<Image>().sprite = ActivateSensorBlocks[i].sprite;
            setSensorBlocks[i].GetChild(1).GetComponent<Text>().text = activateSensorBlocksCount[i].ToString();
            setSensorBlocks[i].GetChild(2).GetComponent<Text>().text = Alphabet[i].ToString();
            setSensorBlocks[i].GetComponent<SensorBlockManagerMechanic>().Number = i;
        }

        BlockDescription.SetActive(false);
    }

    public void GenerateMap(int mapSize, Block mehanicBlock, Image[,] tileMap = null, int[,] tileMapValue = null)
    {
        if (!tile.gameObject.activeSelf)
            tile.gameObject.SetActive(true);

        this.mehanicBlock = mehanicBlock;
        this.mapSize = mapSize;

        if ((tileMap == null) || (tileMapValue == null))
        {
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
        else
        {
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

    public void RemoveBlockCount()
    {
        for (int i = 1; i < BlockNumberPanel.childCount; i++)
            Destroy(BlockNumberPanel.GetChild(i).gameObject);

        setSensorBlocks.Clear();
        activateSensorBlocksCount.Clear();
        ActivateSensorBlocks.Clear();
    }
}
