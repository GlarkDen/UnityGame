using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Threading;

public class VariablesMechanic : MonoBehaviour
{
    public Image and;
    public Image or;
    public Image not;
    public Image xor;
    public Image wires;

    public Sprite WireVertical;
    public Sprite WireHorizontal;
    public Sprite WireTopVertical;
    public Sprite WireTopHorizontal;
    public Sprite WireRigthTop;
    public Sprite WireRightDown;
    public Sprite WireLeftTop;
    public Sprite WireLeftDown;

    public Image tile;

    public Image CurrentObject;

    public Transform solutionPanel;

    public static Transform SolutionPanel;

    public static Image CurrentTile;

    public Button finishCreate;

    public GameObject loading;
    public static GameObject Loading;

    public GameObject startMechanic;
    public static GameObject StartMechanic;

    public static Image ShowCurrentObject;
    public static int CurrentBlock = 0;
    public static bool CurrentBlockActive = true;
    public static Sprite[] Sprites = new Sprite[30];
    public static byte[] Connections = new byte[4];
    public static byte Type;

    public static int CountSensors;
    public static int[] CountSensorBlocks;

    public static int SensorStartIndex = 20;

    public static GameObject thisGameObject;

    public static TileBinaryTree solutionTree = new TileBinaryTree(-1);

    public static Color ResultColor = new Color(1f, 0.5f, 0f);

    private void Start()
    {
        Loading = loading;
        Loading.SetActive(false);

        SolutionPanel = solutionPanel;

        thisGameObject = gameObject;

        ShowCurrentObject = CurrentObject;

        ShowCurrentObject.sprite = tile.sprite;

        Sprites[0] = tile.sprite;
        Sprites[1] = and.sprite;
        Sprites[2] = or.sprite;
        Sprites[3] = not.sprite;
        Sprites[4] = xor.sprite;
        Sprites[5] = WireVertical;

        Sprites[6] = WireLeftTop;
        Sprites[7] = WireRigthTop;
        Sprites[8] = WireRightDown;
        Sprites[9] = WireLeftDown;
        Sprites[10] = WireHorizontal;
        Sprites[11] = WireTopVertical;
        Sprites[12] = WireTopHorizontal;

        TileManagerMechanic.SetBlock += TileMapChanged;
        CurrentObjectMechanic.RemoveTile += TileMapChanged;

        SetBlockSprites();
    }

    public static void SetBlockSprites()
    {
        for (int i = 0; i < CountSensors; i++)
            Sprites[20 + i] = StartGameMechanic.sensorBlockList[i].sprite;
    }

    public void TileMapChanged(int x, int y, int value)
    {
        finishCreate.interactable = false;
        Loading.SetActive(true);
        CreateThuthTableWait();
    }

    public static void SetCurrentBlock(string name)
    {
        int id = 0;

        switch (name)
        {
            case "null":
                id = 0;
                ShowCurrentObject.gameObject.SetActive(false);
                CurrentBlock = id;
                ShowCurrentObject.sprite = Sprites[id];
                return;

            case "swip":
                if (CurrentBlockActive)
                    CurrentBlockActive = false;
                else
                    CurrentBlockActive = true;
                

                if (!CurrentBlockActive)
                {
                    ShowCurrentObject.gameObject.SetActive(false);
                }
                else
                {
                    if (IsNull())
                        return;

                    Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    ShowCurrentObject.transform.position = new Vector2(pos.x, pos.y);
                    ShowCurrentObject.gameObject.SetActive(true);
                }
                return;

            case "and":
                id = 1;
                break;

            case "or":
                id = 2;
                break;

            case "not":
                id = 3;
                break;

            case "xor":
                id = 4;
                break;

            case "wire":
                id = 5;
                break;

            case "sensor":
                id = 20;
                break;
        }

        CurrentBlock = id;
        ShowCurrentObject.sprite = Sprites[id];

        if (!CurrentBlockActive)
        {
            SetCurrentBlock("swip");
        }
        else if (!ShowCurrentObject.gameObject.activeSelf)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ShowCurrentObject.transform.position = new Vector2(pos.x, pos.y);
            ShowCurrentObject.gameObject.SetActive(true);
        }
    }  

    public static void SetCurrentBlockSensor(GameObject obj)
    {
        int id = obj.transform.parent.GetComponent<SensorBlockManagerMechanic>().Number;

        CurrentBlock = 20 + id;
        ShowCurrentObject.sprite = Sprites[20 + id];

        if (!CurrentBlockActive)
        {
            SetCurrentBlock("swip");
        }
        else if (!ShowCurrentObject.gameObject.activeSelf)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ShowCurrentObject.transform.position = new Vector2(pos.x, pos.y);
            ShowCurrentObject.gameObject.SetActive(true);
        }
    }

    public static void ChooseBlockButtons(string key)
    {
        switch (key)
        {
            case "C":
                SetCurrentBlock("and");
                break;

            case "R":
                SetCurrentBlock("wire");
                break;

            case "V":
                SetCurrentBlock("sensor");
                break;
        }
    }

    public static void SetCurrentWire(string operation)
    {
        switch (operation)
        {
            case "forward":
                if (CurrentBlock < 10)
                    CurrentBlock++;
                else
                    CurrentBlock = 5;
                break;

            case "back":
                if (CurrentBlock > 5)
                    CurrentBlock--;
                else
                    CurrentBlock = 10;
                break;

            case "W":
                if (CurrentBlock == (int)Wires.WireLeftDown)
                {
                    CurrentBlock = (int)Wires.WireLeftTop;
                } 
                else if (CurrentBlock == (int)Wires.WireRightDown)
                {
                    CurrentBlock = (int)Wires.WireRigthTop;
                }
                else if (CurrentBlock == (int)Wires.WireVertical)
                {
                    CurrentBlock = (int)Wires.WireRigthTop;
                }
                else if (CurrentBlock == (int)Wires.WireHorizontal)
                {
                    CurrentBlock = (int)Wires.WireRigthTop;
                }
                else
                {
                    CurrentBlock = (int)Wires.WireVertical;
                }
                break;

            case "A":
                if (CurrentBlock == (int)Wires.WireLeftDown)
                {
                    CurrentBlock = (int)Wires.WireRightDown;
                }
                else if (CurrentBlock == (int)Wires.WireLeftTop)
                {
                    CurrentBlock = (int)Wires.WireRigthTop;
                }
                else if (CurrentBlock == (int)Wires.WireHorizontal)
                {
                    CurrentBlock = (int)Wires.WireRigthTop;
                }
                else
                {
                    CurrentBlock = (int)Wires.WireHorizontal;
                }
                break;

            case "S":
                if (CurrentBlock == (int)Wires.WireLeftTop)
                {
                    CurrentBlock = (int)Wires.WireLeftDown;
                }
                else if (CurrentBlock == (int)Wires.WireRigthTop)
                {
                    CurrentBlock = (int)Wires.WireRightDown;
                }
                else if (CurrentBlock == (int)Wires.WireVertical)
                {
                    CurrentBlock = (int)Wires.WireLeftDown;
                }
                else if (CurrentBlock == (int)Wires.WireHorizontal)
                {
                    CurrentBlock = (int)Wires.WireLeftDown;
                }
                else
                {
                    CurrentBlock = (int)Wires.WireVertical;
                }
                break;

            case "D":
                if (CurrentBlock == (int)Wires.WireRightDown)
                {
                    CurrentBlock = (int)Wires.WireLeftDown;
                }
                else if (CurrentBlock == (int)Wires.WireRigthTop)
                {
                    CurrentBlock = (int)Wires.WireLeftTop;
                }
                else if (CurrentBlock == (int)Wires.WireHorizontal)
                {
                    CurrentBlock = (int)Wires.WireLeftTop;
                }
                else
                {
                    CurrentBlock = (int)Wires.WireHorizontal;
                }
                break;
        }

        ShowCurrentObject.sprite = Sprites[CurrentBlock];
    }

    public static void SetCurrentLogic(string operation)
    {
        switch (operation)
        {
            case "forward":
                if (CurrentBlock < 4)
                    CurrentBlock++;
                else
                    CurrentBlock = 1;
                break;

            case "back":
                if (CurrentBlock > 2)
                    CurrentBlock--;
                else
                    CurrentBlock = 4;
                break;
        }

        ShowCurrentObject.sprite = Sprites[CurrentBlock];
    }

    public static void SetCurrentSensor(string operation)
    {
        switch (operation)
        {
            case "forward":
                if (CurrentBlock < SensorStartIndex + CountSensors - 1)
                    CurrentBlock++;
                else
                    CurrentBlock = SensorStartIndex;
                break;

            case "back":
                if (CurrentBlock > SensorStartIndex)
                    CurrentBlock--;
                else
                    CurrentBlock = SensorStartIndex + CountSensors - 1;
                break;
        }

        ShowCurrentObject.sprite = Sprites[CurrentBlock];
    }

    public static bool UpdateCountSensors()
    {
        int id = CurrentBlock - SensorStartIndex;

        if (CountSensorBlocks[id] > 0)
        {
            CountSensorBlocks[id]--;
            StartGameMechanic.setSensorBlocks[id].GetChild(1).GetComponent<Text>().text = CountSensorBlocks[id].ToString();
            
            int count = 0;

            foreach (var number in CountSensorBlocks)
                count += number;

            OnUpdateSensorCount(count);
            return true;
        }
        else
        {
            StartGameMechanic.setSensorBlocks[id].GetChild(1).GetComponent<Text>().color = Color.red;
            StartGameMechanic.setSensorBlocks[id].GetChild(0).GetComponent<Button>().image.color = Color.red;
            StartGameMechanic.setSensorBlocks[id].GetChild(2).GetComponent<Text>().color = Color.red;
            thisGameObject.GetComponent<VariablesMechanic>().StartCoroutine(resetColor(id));
            return false;
        }
    }

    private static IEnumerator resetColor(int id)
    {
        yield return new WaitForSeconds(1);
        StartGameMechanic.setSensorBlocks[id].GetChild(1).GetComponent<Text>().color = new Color(0.21f, 0.21f, 0.21f);
        StartGameMechanic.setSensorBlocks[id].GetChild(0).GetComponent<Button>().image.color = Color.white;
        StartGameMechanic.setSensorBlocks[id].GetChild(2).GetComponent<Text>().color = new Color(0.21f, 0.21f, 0.21f);
    }

    private static IEnumerator createTruthTable(float wait)
    {
        yield return new WaitForSeconds(wait);
        CreateThuthTable();
    }

    public static void UpdateCountSensors(int id, int change)
    {
        id = id - SensorStartIndex;

        CountSensorBlocks[id] += change;
        StartGameMechanic.setSensorBlocks[id].GetChild(1).GetComponent<Text>().text = CountSensorBlocks[id].ToString();

        int count = 0;

        foreach (var number in CountSensorBlocks)
            count += number;

        OnUpdateSensorCount(count);
    }

    public delegate void SensorCountHandler(int count);

    public static event SensorCountHandler UpdateSensorCount;

    public static void OnUpdateSensorCount(int count)
    {
        if (UpdateSensorCount != null)
            UpdateSensorCount(count);
    }

    public delegate void TruthTableHandler(TruthTable truthTable);

    public static event TruthTableHandler TruthTableUpdate;

    public static void OnUpdateTruthTable(TruthTable truthTable)
    {
        if (TruthTableUpdate != null)
            TruthTableUpdate(truthTable);
    }

    public enum Wires
    {
        WireVertical = 5,
        WireLeftTop = 6,
        WireRigthTop = 7,
        WireRightDown = 8,
        WireLeftDown = 9,
        WireHorizontal = 10,
        WireTopVertical = 11,
        WireTopHorizontal = 12
    }

    public enum Logics
    {
        And = 1,
        Or = 2,
        Not = 3,
        Xor = 4
    }

    public static bool IsLogic()
    {
        if (CurrentBlock > 0 && CurrentBlock < 5)
            return true;
        else
            return false;
    }

    public static bool IsLogic(int number)
    {
        if (number > 0 && number < 5)
            return true;
        else
            return false;
    }

    public static bool IsWire()
    {
        if (CurrentBlock >= 5 && CurrentBlock <= 12)
            return true;
        else
            return false;
    }

    public static bool IsWire(int number)
    {
        if (number >= 5 && number <= 12)
            return true;
        else
            return false;
    }

    public static bool IsDoubleWire()
    {
        if (CurrentBlock == (int)Wires.WireTopHorizontal ||
            CurrentBlock == (int)Wires.WireTopVertical)
                return true;
        else
            return false;
    }

    public static bool IsDoubleWire(int number)
    {
        if (number == (int)Wires.WireTopHorizontal ||
            number == (int)Wires.WireTopVertical)
            return true;
        else
            return false;
    }

    public static bool IsSensor()
    {
        if (CurrentBlock >= 20 && CurrentBlock < 30)
            return true;
        else
            return false;
    }

    public static bool IsSensor(int number)
    {
        if (number >= 20 && number < 30)
            return true;
        else
            return false;
    }

    public static bool IsNull()
    {
        if (CurrentBlock == 0)
            return true;
        else
            return false;
    }

    public static bool IsNull(int number)
    {
        if (number == 0)
            return true;
        else
            return false;
    }

    public static bool IsMechanic()
    {
        if (CurrentBlock < 0)
            return true;
        else
            return false;
    }

    public static bool IsMechanic(int number)
    {
        if (number < 0)
            return true;
        else
            return false;
    }

    public static int SetDirection(int block)
    {
        if (block == (int)Wires.WireLeftDown ||
            block == (int)Wires.WireRightDown ||
            block == (int)Wires.WireVertical ||
            block == (int)Logics.Not ||
            IsMechanic(block))
        {
            return (int)Direction.Down;
        }

        if (block == (int)Wires.WireLeftTop)
        {
            return (int)Direction.Right;
        }

        if (block == (int)Wires.WireRigthTop)
        {
            return (int)Direction.Left;
        }

        if (block == (int)Wires.WireHorizontal ||
            block == (int)Logics.And ||
            block == (int)Logics.Or ||
            block == (int)Logics.Xor)
        {
            return (int)Direction.HorizontalBoth;
        }

        return 0;
    }

    public enum Direction
    {
        Top = 1,
        Down = 2,
        Right = 3,
        Left = 4,
        HorizontalBoth = 5
    }

    public static TileBinaryTree CheckSolution(TileBinaryTree root, Coordinate tileCoordinate, int direction)
    {
        switch (direction)
        {
            case (int)Direction.Down:
                tileCoordinate.Y++;

                if (tileCoordinate.Y >= StartGameMechanic.mapTilesValue.GetLength(1))
                    return new TileBinaryTree(0);
                break;

            case (int)Direction.Top:
                tileCoordinate.Y--;

                if (tileCoordinate.Y < 1)
                    return new TileBinaryTree(0);
                break;

            case (int)Direction.Right:
                tileCoordinate.X++;

                if (tileCoordinate.X >= StartGameMechanic.mapTilesValue.GetLength(0))
                    return new TileBinaryTree(0);
                break;

            case (int)Direction.Left:
                tileCoordinate.X--;

                if (tileCoordinate.X < 1)
                    return new TileBinaryTree(0);
                break;
        }

        int currentValue = StartGameMechanic.mapTilesValue[tileCoordinate.X, tileCoordinate.Y];
                
        if (IsNull(currentValue))
        {
            return new TileBinaryTree(0);
        }

        if (IsSensor(currentValue))
        {
            return new TileBinaryTree(currentValue);
        }

        if (IsLogic(currentValue))
        {
            if (direction != (int)Direction.Down)
                return new TileBinaryTree(0);

            TileBinaryTree new_tree = new TileBinaryTree(currentValue);

            if (currentValue == (int)Logics.Not)
            {
                new_tree.right = CheckSolution(new_tree.right, tileCoordinate, (int)Direction.Down);
            }
            else
            {
                new_tree.right = CheckSolution(new_tree.right, tileCoordinate, (int)Direction.Right);
                new_tree.left = CheckSolution(new_tree.right, tileCoordinate, (int)Direction.Left);
            }

            return new_tree;
        }

        if (IsWire(currentValue))
        {
            if (IsDoubleWire(currentValue))
            {
                if (direction == (int)Direction.Down)
                    return CheckSolution(root, tileCoordinate, (int)Direction.Down);
                else if (direction == (int)Direction.Top)
                    return CheckSolution(root, tileCoordinate, (int)Direction.Top);
                else if (direction == (int)Direction.Left)
                    return CheckSolution(root, tileCoordinate, (int)Direction.Left);
                else if (direction == (int)Direction.Right)
                    return CheckSolution(root, tileCoordinate, (int)Direction.Right);
            }
            else
            {
                #region Пока в разработке
                //switch (currentValue)
                //{
                //    case (int)Wires.WireHorizontal:
                //        if (direction == (int)Direction.Left)
                //            return CheckSolution(root, new Coordinate(tileCoordinate.X - 1, tileCoordinate.Y), (int)Direction.Left);
                //        else if (direction == (int)Direction.Right)
                //            return CheckSolution(root, new Coordinate(tileCoordinate.X + 1, tileCoordinate.Y), (int)Direction.Right);
                //        else
                //            return null;
                //        break;

                //    case (int)Wires.WireVertical:
                //        if (direction == (int)Direction.Down)
                //            return CheckSolution(root, new Coordinate(tileCoordinate.X, tileCoordinate.Y + 1), (int)Direction.Down);
                //        else
                //            return null;
                //        break;

                //    case (int)Wires.WireLeftDown:
                //        if (direction == (int)Direction.Down)
                //            return CheckSolution(root, new Coordinate(tileCoordinate.X, tileCoordinate.Y + 1), (int)Direction.Down);
                //        else
                //            return null;
                //        break;

                //    case (int)Wires.WireLeftTop:
                //        if (direction == (int)Direction.Right)
                //            return CheckSolution(root, new Coordinate(tileCoordinate.X + 1, tileCoordinate.Y), (int)Direction.Right);
                //        else
                //            return null;
                //        break;

                //    case (int)Wires.WireRightDown:
                //        if (direction == (int)Direction.Down)
                //            return CheckSolution(root, new Coordinate(tileCoordinate.X, tileCoordinate.Y + 1), (int)Direction.Down);
                //        else
                //            return null;
                //        break;

                //    case (int)Wires.WireRigthTop:
                //        if (direction == (int)Direction.Left)
                //            return CheckSolution(root, new Coordinate(tileCoordinate.X - 1, tileCoordinate.Y), (int)Direction.Left);
                //        else
                //            return null;
                //        break;
                //}
                #endregion

                switch (direction)
                {
                    case (int)Direction.Down:
                        if (currentValue == (int)Wires.WireVertical)
                            return CheckSolution(root, tileCoordinate, (int)Direction.Down);
                        else if (currentValue == (int)Wires.WireLeftTop)
                            return CheckSolution(root, tileCoordinate, (int)Direction.Right);
                        else if (currentValue == (int)Wires.WireRigthTop)
                            return CheckSolution(root, tileCoordinate, (int)Direction.Left);
                        else
                            return new TileBinaryTree(0);

                    case (int)Direction.Right:
                        if (currentValue == (int)Wires.WireHorizontal)
                            return CheckSolution(root, tileCoordinate, (int)Direction.Right);
                        else if (currentValue == (int)Wires.WireRightDown)
                            return CheckSolution(root, tileCoordinate, (int)Direction.Down);
                        else if (currentValue == (int)Wires.WireRigthTop)
                            return CheckSolution(root, tileCoordinate, (int)Direction.Top);
                        else
                            return new TileBinaryTree(0); ;
                        
                    case (int)Direction.Left:
                        if (currentValue == (int)Wires.WireHorizontal)
                            return CheckSolution(root, tileCoordinate, (int)Direction.Left);
                        else if (currentValue == (int)Wires.WireLeftDown)
                            return CheckSolution(root, tileCoordinate, (int)Direction.Down);
                        else if (currentValue == (int)Wires.WireLeftTop)
                            return CheckSolution(root, tileCoordinate, (int)Direction.Top);
                        else
                            return new TileBinaryTree(0);

                    case (int)Direction.Top:
                        if (currentValue == (int)Wires.WireVertical)
                            return CheckSolution(root, tileCoordinate, (int)Direction.Top);
                        else if (currentValue == (int)Wires.WireLeftDown)
                            return CheckSolution(root, tileCoordinate, (int)Direction.Right);
                        else if (currentValue == (int)Wires.WireRightDown)
                            return CheckSolution(root, tileCoordinate, (int)Direction.Left);
                        else
                            return new TileBinaryTree(0);
                }
            }
        }

        return new TileBinaryTree(0);
    }

    public static void ClearTruthTable()
    {
        Transform currentLine = SolutionPanel.GetChild(0);

        for (int j = 1; j < currentLine.childCount; j++)
        {
            Destroy(currentLine.GetChild(j).gameObject);
        }

        currentLine = SolutionPanel.GetChild(1);

        for (int j = 1; j < currentLine.childCount; j++)
        {
            Destroy(currentLine.GetChild(j).gameObject);
        }

        for (int i = 2; i < SolutionPanel.childCount; i++)
            Destroy(SolutionPanel.GetChild(i).gameObject);

        SolutionPanel.GetChild(0).GetChild(0).GetComponent<Text>().text = "F";
        SolutionPanel.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = "0";
    }

    public static void CreateThuthTableWait()
    {
        ClearTruthTable();

        thisGameObject.GetComponent<VariablesMechanic>().StartCoroutine(createTruthTable(0.05f));

        Thread.Sleep(100);
    }

    public static void CreateThuthTable()
    {
        solutionTree.right = CheckSolution(solutionTree, StartGameMechanic.mehanicBlockCoodinate, (int)Direction.Down);

        Dictionary<int, char> blockChars = new Dictionary<int, char>();

        for (int i = 0; i < CountSensors; i++)
            blockChars[20 + i] = StartGameMechanic.Alphabet[i];

        List<int> findBlocks = solutionTree.GetBlocks(new List<int>());

        List<Dictionary<string, bool>> blockConditions = new List<Dictionary<string, bool>>();

        List<string> findBlocksChars = new List<string>();

        foreach (var number in findBlocks)
        {
            int repeatCount = findBlocksChars.Where(str => str.Contains(blockChars[number] + "")).Count();

            if (repeatCount != 0)
                findBlocksChars.Add(blockChars[number] + repeatCount.ToString());
            else
                findBlocksChars.Add(blockChars[number] + "");
        }

        foreach (var item in Combinations(findBlocks.Count, new bool[] { false, true })) 
        {
            int counter = 0;
            Dictionary<string, bool> conditions = new Dictionary<string, bool>();

            foreach (var condition in item)
            {
                conditions[findBlocksChars[counter]] = condition;
                counter++;
            }

            blockConditions.Add(conditions);
        }

        int numberChar = 0;

        solutionTree.RenamedData(findBlocksChars, ref numberChar);

        Transform currentString = null;
        Transform currentColumn = null;

        if (findBlocksChars.Count == 0)
        {
            SolutionPanel.GetChild(0).GetChild(0).GetComponent<Text>().text = "F";
            SolutionPanel.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = "0";
        }
        else
        {
            SolutionPanel.GetChild(0).GetChild(0).GetComponent<Text>().text = findBlocksChars[0];
            currentString = SolutionPanel.GetChild(0).GetChild(0);

            for (int i = 1; i < findBlocksChars.Count; i++)
            {
                currentString = Instantiate(currentString, new Vector3(0, 0, 0),
                        Quaternion.identity, SolutionPanel.GetChild(0));

                currentString.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(currentString.GetComponent<RectTransform>().anchoredPosition3D.x,
                    currentString.GetComponent<RectTransform>().anchoredPosition3D.y, 0);

                currentString.GetComponent<Text>().text = findBlocksChars[i];
            }

            currentString = Instantiate(currentString, new Vector3(0, 0, 0),
                        Quaternion.identity, SolutionPanel.GetChild(0));

            currentString.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(currentString.GetComponent<RectTransform>().anchoredPosition3D.x,
                    currentString.GetComponent<RectTransform>().anchoredPosition3D.y, 0);

            currentString.GetComponent<Text>().text = "F";

            bool firstAddText = true;

            int columnNumber;
            int stringNumber = 0;

            foreach (var item in blockConditions)
            {
                if (firstAddText)
                {
                    stringNumber = 1;

                    currentString = SolutionPanel.GetChild(stringNumber);

                    foreach (var condition in item.Values)
                    {
                        if (firstAddText)
                        {
                            currentColumn = currentString.GetChild(0);
                            firstAddText = false;
                        }
                        else
                        {
                            currentColumn = Instantiate(currentColumn, new Vector3(0, 0, 0), 
                                Quaternion.identity, currentString);

                            currentColumn.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(currentColumn.GetComponent<RectTransform>().anchoredPosition3D.x,
                                currentColumn.GetComponent<RectTransform>().anchoredPosition3D.y, 0);
                        }

                        if (condition)
                        {
                            currentColumn.GetChild(0).GetComponent<Text>().text = "1";
                        }
                        else
                        {
                            currentColumn.GetChild(0).GetComponent<Text>().text = "0";
                        }
                    }

                    currentColumn = Instantiate(currentColumn, new Vector3(0, 0, 0),
                                Quaternion.identity, currentString);

                    currentColumn.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(currentColumn.GetComponent<RectTransform>().anchoredPosition3D.x,
                                currentColumn.GetComponent<RectTransform>().anchoredPosition3D.y, 0);

                    if (solutionTree.CheckCondition(item))
                    {
                        currentColumn.GetChild(0).GetComponent<Text>().text = "1";
                        item["F"] = true;
                    }
                    else
                    {
                        currentColumn.GetChild(0).GetComponent<Text>().text = "0";
                        item["F"] = false;
                    }

                    currentColumn.GetComponent<Image>().color = ResultColor;
                }
                else
                {
                    currentString = Instantiate(currentString, new Vector3(0, 0, 0),
                                Quaternion.identity, SolutionPanel);

                    currentString.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(currentString.GetComponent<RectTransform>().anchoredPosition3D.x,
                        currentString.GetComponent<RectTransform>().anchoredPosition3D.y, 0);

                    columnNumber = 0;

                    foreach (var condition in item.Values)
                    {
                        currentColumn = currentString.GetChild(columnNumber);
                        columnNumber++;

                        if (condition)
                        {
                            currentColumn.GetChild(0).GetComponent<Text>().text = "1";
                        }
                        else
                        {
                            currentColumn.GetChild(0).GetComponent<Text>().text = "0";
                        }
                    }

                    currentColumn = currentString.GetChild(columnNumber);

                    if (solutionTree.CheckCondition(item))
                    {
                        currentColumn.GetChild(0).GetComponent<Text>().text = "1";
                        item["F"] = true;
                    }
                    else
                    {
                        currentColumn.GetChild(0).GetComponent<Text>().text = "0";
                        item["F"] = false;
                    }

                    currentColumn.GetComponent<Image>().color = ResultColor;
                }

                stringNumber++;
            }
        }

        Loading.SetActive(false);

        TruthTable truthTable = new TruthTable();

        if (findBlocksChars.Count != 0)
        {
            if (blockConditions.Where(con => con["F"]).Count() <= blockConditions.Where(con => !con["F"]).Count())
            {
                truthTable.BlockConditions = blockConditions.Where(con => con["F"]).ToList();
                truthTable.CompareCondition = true;
            }
            else
            {
                truthTable.BlockConditions = blockConditions.Where(con => !con["F"]).ToList();
                truthTable.CompareCondition = false;
            }
            
            truthTable.BlockChars = findBlocksChars;
            truthTable.isNull = false;
        }
        else
        {
            truthTable.isNull = true;
        }

        OnUpdateTruthTable(truthTable);
    }

    public static List<T[]> Combinations<T>(int placesCount, T[] items)
    {
        int count = (int)Mathf.Pow(items.Length, placesCount);

        List<T[]> result = new List<T[]>(count);

        for (int i = 0; i < count; i++)
            result.Add(new T[placesCount]);

        int step = count;

        for (int column = 0; column < placesCount; column++)
        {
            step = step / items.Length;

            for (int stepNumber = 0; stepNumber < count / Mathf.Pow(items.Length, placesCount - column); stepNumber++)
            {
                for (int itemNumber = 0; itemNumber < items.Length; itemNumber++)
                {
                    for (int str = step * (itemNumber + stepNumber * items.Length); str < step * (itemNumber + stepNumber * items.Length + 1); str++)
                    {
                        result[str][column] = items[itemNumber];
                    }
                }
            }
        }

        return result;
    }

    public static void GetBlockCount()
    {

    }

    public void ClearMap()
    {
        startMechanic.GetComponent<StartGameMechanic>().ClearMap();

        int count = 0;

        foreach (var number in CountSensorBlocks)
            count += number;

        OnUpdateSensorCount(count);

        TileMapChanged(0, 0, -1);
    }
}
