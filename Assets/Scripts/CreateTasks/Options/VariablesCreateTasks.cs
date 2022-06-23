using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VariablesCreateTasks : MonoBehaviour
{
    public static string taskTitle = "";
    public static string taskText = "";

    public int minSizeMap;
    public int maxSizeMap;
    public static int sizeMap;

    public static GameObject currentObject;

    public Text mapSizeText;

    public GameObject sensorBlockList;

    public GameObject mehanicBlockList;

    public Image mehanicBlockImage;

    public GameObject mehanicChoosePanel;

    public InputField countBlockInput;

    public Transform countBlock;

    public Transform countBlockPanel;

    public GameObject taskOptionsStage;

    public GameObject createSolutionStage;

    public Button nextButton;

    public static Dictionary<int, Transform> setBlocksCount = new Dictionary<int, Transform>();

    public Button chooseMehanic;

    private List<Block> blocks = Serialization.LoadBinaryFile<List<Block>>(ProjectPath.Blocks);

    public static List<Block> sensorBlocks = new List<Block>();
    public static List<Block> mehanicBlocks = new List<Block>();

    private List<Transform> setSensorBlocks = new List<Transform>();
    private List<Transform> setMehanicBlocks = new List<Transform>();

    public static Block chooseMehanicBlock;

    public int MaxBlockCount = 5;

    private int SumCountBlock = 0;

    public static GameObject CurrentInputCount;

    private bool firstNextStage = true;

    public static bool inputIsEnding = true;

    private string saveCountText;

    private char setBlockNumber(char charToValidate)
    {
        charToValidate = '\0';
        return charToValidate;
    }

    public static int GetBlockCount()
    {
        int count = 0;

        foreach (var number in setBlocksCount.Values)
            count += int.Parse(number.GetChild(0).GetComponent<InputField>().text);

        return count;
    }

    public void SwipChooseBlocks(bool status)
    {
        if (status) 
        {
            for (int i = 0; i < setSensorBlocks.Count - 1; i += 2)
                setSensorBlocks[i].parent.GetComponent<Toggle>().interactable = true;
        }
        else
        {
            for (int i = 0; i < setSensorBlocks.Count - 1; i += 2)
                if (!setSensorBlocks[i].parent.GetComponent<Toggle>().isOn)
                    setSensorBlocks[i].parent.GetComponent<Toggle>().interactable = false;
        }
    }

    private void Start()
    {
        currentObject = this.gameObject;

        mapSizeText.text = minSizeMap.ToString();
        sizeMap = minSizeMap;

        countBlock.gameObject.SetActive(false);

        mehanicChoosePanel.SetActive(false);

        if (blocks == null || blocks.Count == 0)
        {
            sensorBlockList.transform.GetChild(0).gameObject.SetActive(false);
            chooseMehanic.interactable = false;
        }
        else
        {
            for (int i = 0; i < blocks.Count; i++)
                blocks[i].sprite = Texture.ByteToSprite(blocks[i].texture, 100, 100);

            foreach (Block block in blocks)
            {
                if (block.type != (byte)Block.Type.Механизм)
                    sensorBlocks.Add(block);
                else
                    mehanicBlocks.Add(block);
            }

            #region Список датчиков
            int sensorListWidth = 0;

            List<Transform> panelSensorList = new List<Transform>();

            for (int i = 0; i < sensorBlockList.transform.childCount; i++)
                panelSensorList.Add(sensorBlockList.transform.GetChild(i));

            sensorListWidth = panelSensorList[0].childCount;

            for (int i = 0; i < sensorListWidth; i++)
            {
                Transform panelObject = panelSensorList[0].transform.GetChild(i);
                setSensorBlocks.Add(panelObject.GetChild(0));
                setSensorBlocks.Add(panelObject.GetChild(0).GetChild(0));
            }

            int panelSensorCount = sensorBlocks.Count / sensorListWidth + sensorBlocks.Count % sensorListWidth;

            if (sensorBlocks.Count == 1)
            {
                setSensorBlocks[0].GetComponent<Image>().sprite = sensorBlocks[0].sprite;
                setSensorBlocks[1].GetComponent<Image>().sprite = sensorBlocks[0].sprite;
                Destroy(setSensorBlocks[2].parent.gameObject);
                setSensorBlocks.RemoveAt(2);
            }
            else
            {
                for (int i = 0; i < panelSensorCount - panelSensorList.Count; i++)
                {
                    Transform panel = Instantiate(panelSensorList[0], new Vector2(0, 0),
                        Quaternion.identity, sensorBlockList.transform);

                    for (int j = 0; j < sensorListWidth; j++)
                    {
                        Transform panelObject = panel.transform.GetChild(j);
                        setSensorBlocks.Add(panelObject.GetChild(0));
                        setSensorBlocks.Add(panelObject.GetChild(0).GetChild(0));
                    }
                }

                for (int i = 0; i < setSensorBlocks.Count; i += 2)
                {
                    if (sensorBlocks.Count == i / 2)
                    {
                        Destroy(setSensorBlocks[i].parent.gameObject);
                        setSensorBlocks.RemoveAt(i);
                        break;
                    }

                    setSensorBlocks[i].GetComponent<Image>().sprite = sensorBlocks[i / 2].sprite;
                    setSensorBlocks[i + 1].GetComponent<Image>().sprite = sensorBlocks[i / 2].sprite;

                    setSensorBlocks[i].GetComponentInParent<BlockListData>().index = i / 2;
                }
            }
            #endregion

            #region Список механизмов
            int mehanicListWidth = 0;

            List<Transform> panelMehanicList = new List<Transform>();

            panelMehanicList.Add(mehanicBlockList.transform.GetChild(0));

            mehanicListWidth = panelMehanicList[0].childCount;

            for (int i = 0; i < mehanicListWidth; i++)
                setMehanicBlocks.Add(panelMehanicList[0].transform.GetChild(i));

            int panelMehanicCount = mehanicBlocks.Count / mehanicListWidth + mehanicBlocks.Count % mehanicListWidth;

            if (mehanicBlocks.Count == 1)
            {
                setMehanicBlocks[0].GetComponent<Image>().sprite = mehanicBlocks[0].sprite;
                Destroy(setMehanicBlocks[1].gameObject);
                setMehanicBlocks.RemoveAt(1);
            }
            else
            {
                for (int i = 0; i < panelMehanicCount - panelMehanicList.Count; i++)
                {
                    Transform panel = Instantiate(panelMehanicList[0], new Vector2(0, 0),
                        Quaternion.identity, mehanicBlockList.transform);

                    for (int j = 0; j < mehanicListWidth; j++)
                        setMehanicBlocks.Add(panel.transform.GetChild(j));
                }

                for (int i = 0; i < setMehanicBlocks.Count; i++)
                {
                    if (mehanicBlocks.Count == i)
                    {
                        Destroy(setMehanicBlocks[i].gameObject);
                        setMehanicBlocks.RemoveAt(i);
                        break;
                    }

                    setMehanicBlocks[i].GetComponent<Image>().sprite = mehanicBlocks[i].sprite;

                    setMehanicBlocks[i].GetComponent<BlockListData>().index = i;
                }
            }
            #endregion
        }
    }

    public void InputBlockCount()
    {
        CurrentInputCount.GetComponent<Button>().interactable = false;

        SumCountBlock = 0;
        foreach (var number in setBlocksCount.Values)
            SumCountBlock += int.Parse(number.GetChild(0).GetChild(1).GetComponent<Text>().text);

        saveCountText = CurrentInputCount.transform.GetChild(1).GetComponent<Text>().text;
        CurrentInputCount.transform.GetChild(1).GetComponent<Text>().text = "";

        CurrentInputCount.transform.GetChild(0).gameObject.SetActive(true);
        StartCoroutine(inputNumber(CurrentInputCount.transform.GetChild(1).GetComponent<Text>()));
    }

    public void EndInput(bool result)
    {
        if (!result)
            CurrentInputCount.transform.GetChild(1).GetComponent<Text>().text = saveCountText;

        CurrentInputCount.transform.GetChild(0).gameObject.SetActive(false);
        CurrentInputCount.GetComponent<Button>().interactable = true;
        inputIsEnding = true;
    }

    public bool SetBlockCondition(int newCount)
    {
        int currentSum = SumCountBlock + newCount - int.Parse(saveCountText);

        if (newCount > 0 && currentSum <= MaxBlockCount)
        {
            if (SumCountBlock < MaxBlockCount && currentSum == MaxBlockCount)
                SwipChooseBlocks(false);
            else if (SumCountBlock == MaxBlockCount && currentSum < MaxBlockCount)
                SwipChooseBlocks(true);

            return true;
        }
        else
        {
            return false;
        }
    }

    public IEnumerator inputNumber(Text text)
    {
        while (true)
        {
            yield return new WaitForSeconds(0.001f);
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                EndInput(false);
                yield break;
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                EndInput(false);
                yield break;
            }
            else if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                EndInput(false);
                yield break;
            }
            else if (Input.anyKeyDown)
            {
                string key = Input.inputString;
                int keyCode;

                if (int.TryParse(key, out keyCode))
                {
                    if (SetBlockCondition(keyCode)) 
                    { 
                        text.text = key;
                        EndInput(true);
                        yield break;
                    }
                }
            }
        }
    }

    public static List<int> GetBlocksCount()
    {
        List<int> blocksCount = new List<int>();

        foreach (Transform value in setBlocksCount.Values)
            blocksCount.Add(int.Parse(value.GetChild(0).GetChild(1).GetComponent<Text>().text));

        return blocksCount;
    }

    public static List<Block> GetBlocksList()
    {
        List<Block> blocksList = new List<Block>();

        foreach (int key in setBlocksCount.Keys)
            blocksList.Add(sensorBlocks[key]);

        return blocksList;
    }

    public void ResizeMap(int changeSize)
    {
        if (sizeMap + changeSize < minSizeMap)
            return;

        if (sizeMap + changeSize > maxSizeMap)
            return;
            
        sizeMap += changeSize;

        mapSizeText.text = sizeMap.ToString();
    }

    public void UpdateChooseBlock(bool status, int index)
    {
        if (status)
        {
            setBlocksCount[index] = Instantiate(countBlock, new Vector2(0, 0),
                Quaternion.identity, countBlockPanel);

            setBlocksCount[index].gameObject.SetActive(true);

            setBlocksCount[index].GetComponent<Image>().sprite = sensorBlocks[index].sprite;

            setBlocksCount[index].GetChild(0).GetChild(0).gameObject.SetActive(false);

            setBlocksCount[index].GetChild(1).GetChild(0).GetComponent<Text>().text = StartGameMechanic.Alphabet[setBlocksCount.Count - 1].ToString();

            SumCountBlock++;

            if (setBlocksCount.Count == MaxBlockCount || SumCountBlock == MaxBlockCount)
                SwipChooseBlocks(false);
        }
        else
        {
            SumCountBlock -= int.Parse(setBlocksCount[index].GetChild(0).GetChild(1).GetComponent<Text>().text);

            int j = 0;
            for (int i = 1; i < countBlockPanel.childCount; i++)
            {
                if (countBlockPanel.GetChild(i).gameObject == setBlocksCount[index].gameObject)
                    continue;

                countBlockPanel.GetChild(i).GetChild(1).GetChild(0).GetComponent<Text>().text = StartGameMechanic.Alphabet[j].ToString();
                j++;
            }

            Destroy(setBlocksCount[index].gameObject);
            setBlocksCount.Remove(index);

            if (setBlocksCount.Count == 4 || SumCountBlock == 4)
                SwipChooseBlocks(true);
        }

        ActivateNextButton();
    }

    public void UpdateMehanicBlock(int index)
    {
        mehanicBlockImage.sprite = mehanicBlocks[index].sprite;
        chooseMehanicBlock = mehanicBlocks[index];

        ActivateNextButton();
    }

    public void SetTitle(string title)
    {
        taskTitle = title;

        ActivateNextButton();
    }

    public void SetText(string text)
    {
        taskText = text;

        ActivateNextButton();
    }

    public void NextStage()
    {
        taskOptionsStage.SetActive(false);
        createSolutionStage.SetActive(true);

        if (firstNextStage)
            firstNextStage = false;
        else
            StartCreateSolution.Restart();
    }

    public void ActivateNextButton()
    {
        if ((chooseMehanicBlock != null) && (taskText.Length != 0) && (taskTitle.Length != 0) && (setBlocksCount.Count != 0))
        {
            nextButton.interactable = true;
        }
        else
        {
            nextButton.interactable = false;
        }
    }
}
