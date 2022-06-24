using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VariablesDescribingBlocks : MonoBehaviour
{
    //public static string taskTitle = "";
    //public static string taskText = "";

    //public int minSizeMap;
    //public int maxSizeMap;
    //public static int sizeMap;

    //public static GameObject currentObject;

    //public Text mapSizeText;

    public GameObject sensorBlockList;

    //public GameObject mehanicBlockList;

    //public Image mehanicBlockImage;

    //public GameObject mehanicChoosePanel;

    //public InputField countBlockInput;

    //public Transform countBlock;

    //public Transform countBlockPanel;

    //public GameObject taskOptionsStage;

    //public GameObject createSolutionStage;

    //public Button nextButton;

    //public static Dictionary<int, Transform> setBlocksCount = new Dictionary<int, Transform>();

    private List<Block> blocks = Serialization.LoadBinaryFile<List<Block>>(ProjectPath.Blocks);

    //public static List<Block> mehanicBlocks = new List<Block>();

    private List<Transform> setSensorBlocks = new List<Transform>();
    //private List<Transform> setMehanicBlocks = new List<Transform>();

    //public static Block chooseMehanicBlock;

    //public int MaxBlockCount = 5;

    //private int SumCountBlock = 0;

    //public static InputField CurrentInputField;

    //private bool firstNextStage = true;

    //public static bool inputIsEnding;



    //private char setBlockNumber(char charToValidate)
    //{
    //    int count;
    //    int currentSum = SumCountBlock;

    //    if (!inputIsEnding)
    //        return '\0';

    //    if (CurrentInputField.text != "")
    //    {
    //        int lastNumber = int.Parse(CurrentInputField.text);

    //        currentSum -= lastNumber;

    //        if (int.TryParse(charToValidate.ToString(), out count))
    //        {
    //            if (currentSum + count > MaxBlockCount || count < 1)
    //            {
    //                charToValidate = '\0';
    //            }
    //            else
    //            {
    //                SumCountBlock = currentSum + count;

    //                int lastSum = currentSum + lastNumber;

    //                if (lastSum < MaxBlockCount && SumCountBlock == MaxBlockCount)
    //                    SwipChooseBlocks(false);
    //                else if (lastSum == MaxBlockCount && SumCountBlock < MaxBlockCount)
    //                    SwipChooseBlocks(true);
    //            }
    //        }
    //        else
    //            charToValidate = '\0';
    //    }
    //    else
    //    {
    //        SumCountBlock = 1;

    //        foreach (var number in setBlocksCount.Values)
    //        {
    //            string text = number.GetChild(0).GetComponent<InputField>().text;

    //            if (text != "")
    //                SumCountBlock += int.Parse(text);
    //        }

    //        if (SumCountBlock == MaxBlockCount)
    //            SwipChooseBlocks(false);
    //        else if (SumCountBlock < MaxBlockCount)
    //            SwipChooseBlocks(true);
    //    }

    //    inputIsEnding = false;

    //    return charToValidate;
    //}

    //public void DeactivateInputField(string newText)
    //{
    //    if (newText == "")
    //        CurrentInputField.text = "1";

    //    CurrentInputField.DeactivateInputField();

    //    CurrentInputField = null;
    //}

    //public static int GetBlockCount()
    //{
    //    int count = 0;

    //    foreach (var number in setBlocksCount.Values)
    //        count += int.Parse(number.GetChild(0).GetComponent<InputField>().text);

    //    return count;
    //}

    //public void SwipChooseBlocks(bool status)
    //{
    //    if (status)
    //    {
    //        for (int i = 0; i < setSensorBlocks.Count - 1; i += 2)
    //            setSensorBlocks[i].parent.GetComponent<Toggle>().interactable = true;
    //    }
    //    else
    //    {
    //        for (int i = 0; i < setSensorBlocks.Count - 1; i += 2)
    //            if (!setSensorBlocks[i].parent.GetComponent<Toggle>().isOn)
    //                setSensorBlocks[i].parent.GetComponent<Toggle>().interactable = false;
    //    }
    //}

    private void Start()
    {
        //currentObject = this.gameObject;

        //mapSizeText.text = minSizeMap.ToString();
        //sizeMap = minSizeMap;

        //countBlockInput.onValidateInput += delegate (string input, int charIndex, char addedChar) { return setBlockNumber(addedChar); };

        //countBlock.gameObject.SetActive(false);

        //mehanicChoosePanel.SetActive(false);

        //for (int i = 0; i < blocks.Count; i++)
        //    blocks[i].sprite = Texture.ByteToSprite(blocks[i].texture, 100, 100);

        //foreach (Block block in blocks)
        //{
        //    if (block.type != (byte)Block.Type.Механизм)
        //        sensorBlocks.Add(block);
        //    else
        //        mehanicBlocks.Add(block);
        //}

        foreach (var block in blocks)
        {
            block.sprite = Texture.ByteToSprite(block.texture, 100, 100);
        }

        #region Список датчиков
        int sensorListHigh = 0;

        List<Transform> panelSensorList = new List<Transform>();

        for (int i = 0; i < sensorBlockList.transform.childCount; i++)
            panelSensorList.Add(sensorBlockList.transform.GetChild(i));

        sensorListHigh = panelSensorList[0].childCount;

        for (int i = 0; i < sensorListHigh; i++)
        {
            setSensorBlocks.Add(panelSensorList[0].transform.GetChild(i));
        }

        int panelSensorCount = blocks.Count / sensorListHigh;

        if (blocks.Count % sensorListHigh != 0)
            panelSensorCount++;

        if (blocks.Count == 1)
        {
            setSensorBlocks[0].GetComponent<Image>().sprite = blocks[0].sprite;
            Destroy(setSensorBlocks[1].parent.gameObject);
            Destroy(setSensorBlocks[2].parent.gameObject);
            setSensorBlocks.RemoveAt(1);
            setSensorBlocks.RemoveAt(2);
        }
        else if (blocks.Count == 2)
        {
            setSensorBlocks[0].GetComponent<Image>().sprite = blocks[0].sprite;
            setSensorBlocks[1].GetComponent<Image>().sprite = blocks[1].sprite;
            Destroy(setSensorBlocks[2].parent.gameObject);
            setSensorBlocks.RemoveAt(2);
        }
        else
        {
            for (int i = 0; i < panelSensorCount - panelSensorList.Count; i++)
            {
                Transform panel = Instantiate(panelSensorList[0], new Vector2(0, 0),
                    Quaternion.identity, sensorBlockList.transform);

                for (int j = 0; j < sensorListHigh; j++)
                {
                    setSensorBlocks.Add(panel.transform.GetChild(j));
                }
            }

            if (blocks.Count % 3 == 1)
            {
                Destroy(setSensorBlocks[-1].parent.gameObject);
                Destroy(setSensorBlocks[-2].parent.gameObject);
                setSensorBlocks.RemoveAt(-1);
                setSensorBlocks.RemoveAt(-2);
            }
            else if (blocks.Count % 3 == 2)
            {
                Destroy(setSensorBlocks[-1].parent.gameObject);
                setSensorBlocks.RemoveAt(-1);
            }

            for (int i = 0; i < setSensorBlocks.Count; i++)
            {
                setSensorBlocks[i].GetComponent<Image>().sprite = blocks[i].sprite;

                setSensorBlocks[i].GetComponent<BlockListIndexDescribe>().index = i;
            }
        }
        #endregion

        #region Список механизмов
        //int mehanicListWidth = 0;

        //List<Transform> panelMehanicList = new List<Transform>();

        //panelMehanicList.Add(mehanicBlockList.transform.GetChild(0));

        //mehanicListWidth = panelMehanicList[0].childCount;

        //for (int i = 0; i < mehanicListWidth; i++)
        //    setMehanicBlocks.Add(panelMehanicList[0].transform.GetChild(i));

        //int panelMehanicCount = mehanicBlocks.Count / mehanicListWidth + mehanicBlocks.Count % mehanicListWidth;

        //if (mehanicBlocks.Count == 1)
        //{
        //    setMehanicBlocks[0].GetComponent<Image>().sprite = mehanicBlocks[0].sprite;
        //    Destroy(setMehanicBlocks[1].gameObject);
        //    setMehanicBlocks.RemoveAt(1);
        //}
        //else
        //{
        //    for (int i = 0; i < panelMehanicCount - panelMehanicList.Count; i++)
        //    {
        //        Transform panel = Instantiate(panelMehanicList[0], new Vector2(0, 0),
        //            Quaternion.identity, mehanicBlockList.transform);

        //        for (int j = 0; j < mehanicListWidth; j++)
        //            setMehanicBlocks.Add(panel.transform.GetChild(j));
        //    }

        //    for (int i = 0; i < setMehanicBlocks.Count; i++)
        //    {
        //        if (mehanicBlocks.Count == i)
        //        {
        //            Destroy(setMehanicBlocks[i].gameObject);
        //            setMehanicBlocks.RemoveAt(i);
        //            break;
        //        }

        //        setMehanicBlocks[i].GetComponent<Image>().sprite = mehanicBlocks[i].sprite;

        //        setMehanicBlocks[i].GetComponent<BlockListData>().index = i;
        //    }
        //}
        #endregion
    }

    //public void ResizeMap(int changeSize)
    //{
    //    if (sizeMap + changeSize < minSizeMap)
    //        return;

    //    if (sizeMap + changeSize > maxSizeMap)
    //        return;

    //    sizeMap += changeSize;

    //    mapSizeText.text = sizeMap.ToString();
    //}

    //public void UpdateChooseBlock(bool status, int index)
    //{
    //    if (status)
    //    {
    //        setBlocksCount[index] = Instantiate(countBlock, new Vector2(0, 0),
    //            Quaternion.identity, countBlockPanel);

    //        setBlocksCount[index].gameObject.SetActive(true);

    //        setBlocksCount[index].GetComponent<Image>().sprite = sensorBlocks[index].sprite;

    //        setBlocksCount[index].GetChild(0).GetComponent<InputField>().onValidateInput +=
    //            delegate (string input, int charIndex, char addedChar) { return setBlockNumber(addedChar); };

    //        SumCountBlock++;

    //        if (setBlocksCount.Count == MaxBlockCount || SumCountBlock == MaxBlockCount)
    //            SwipChooseBlocks(false);
    //    }
    //    else
    //    {
    //        SumCountBlock -= int.Parse(setBlocksCount[index].GetChild(0).GetComponent<InputField>().text);

    //        Destroy(setBlocksCount[index].gameObject);
    //        setBlocksCount.Remove(index);

    //        if (setBlocksCount.Count == 4 || SumCountBlock == 4)
    //            SwipChooseBlocks(true);
    //    }

    //    ActivateNextButton();
    //}

    //public void UpdateMehanicBlock(int index)
    //{
    //    mehanicBlockImage.sprite = mehanicBlocks[index].sprite;
    //    chooseMehanicBlock = mehanicBlocks[index];

    //    ActivateNextButton();
    //}

    //public void SetTitle(string title)
    //{
    //    taskTitle = title;

    //    ActivateNextButton();
    //}

    //public void SetText(string text)
    //{
    //    taskText = text;

    //    ActivateNextButton();
    //}

    //public void NextStage()
    //{
    //    taskOptionsStage.SetActive(false);
    //    createSolutionStage.SetActive(true);

    //    if (firstNextStage)
    //        firstNextStage = false;
    //    else
    //        StartCreateSolution.Restart();
    //}

    //public void ActivateNextButton()
    //{
    //    if ((chooseMehanicBlock != null) && (taskText.Length != 0) && (taskTitle.Length != 0) && (setBlocksCount.Count != 0))
    //    {
    //        nextButton.interactable = true;
    //    }
    //    else
    //    {
    //        nextButton.interactable = false;
    //    }
    //}
}
