using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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

    public Text SolutionText;

    public static Text solutionText;

    public static Image CurrentTile;

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
    public static TruthTable truthTable = new TruthTable();

    private void Start()
    {
        Combination();

        solutionText = SolutionText;

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

        for (int i = 0; i < CountSensors; i++)
            Sprites[20 + i] = StartGameMechanic.ActivateSensorBlocks[i].sprite;
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
        StartGameMechanic.setSensorBlocks[id].GetChild(1).GetComponent<Text>().color = Color.gray;
        StartGameMechanic.setSensorBlocks[id].GetChild(0).GetComponent<Button>().image.color = Color.white;
        StartGameMechanic.setSensorBlocks[id].GetChild(2).GetComponent<Text>().color = Color.gray;
    }

    public static void UpdateCountSensors(int id, int change)
    {
        id = id - SensorStartIndex;

        CountSensorBlocks[id] += change;
        StartGameMechanic.setSensorBlocks[id].GetChild(1).GetComponent<Text>().text = CountSensorBlocks[id].ToString();
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

        return new TileBinaryTree(0); ;
    }

    public static void CreateThuthTable()
    {
        solutionTree.right = CheckSolution(solutionTree, StartGameMechanic.mehanicBlockCoodinate, (int)Direction.Down);

        solutionText.text = "";

        solutionTree.MegaShow(solutionText, change_indent: "---");

        Dictionary<int, char> blockChars = new Dictionary<int, char>();

        for (int i = 0; i < CountSensors; i++)
            blockChars[20 + i] = StartGameMechanic.Alphabet[i];

        List<int> findBlocks = solutionTree.GetBlocks(new List<int>());

        Dictionary<int, bool> blockConditions = new Dictionary<int, bool>();

        foreach (int block in findBlocks)
            blockConditions[block] = true;

        Debug.Log(solutionTree.CheckCondition(blockConditions));

        
    }

    public void Combination()
    {
        string[] items = { "0", "1" };
        int n = 4;

        int count = (int)Mathf.Pow(items.Length, n);

        List<string[]> result = new List<string[]>(count);

        for (int i = 0; i < count; i++)
            result.Add(new string[4]);

        int step = count;

        for (int i = 0; i < n; i++)
        {
            step = step / items.Length;

            for (int h = 0; h < count / Mathf.Pow(items.Length, n - i); h++)
            {
                for (int j = 0; j < items.Length; j++)
                {
                    for (int k = step * (j + h * items.Length); k < step * (j + h * items.Length + 1); k++)
                    {
                        result[k][i] = items[j];
                    }
                }
            }
        }

        foreach (var str in result)
        {
            foreach (var num in str)
                Debug.Log(num + " ");
        }
    }
}
