using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private List<Block> blocks = Serialization.LoadBinaryFile<List<Block>>(ProjectPath.Blocks);

    public static List<Block> sensorBlocks = new List<Block>();
    public static List<Block> mehanicBlocks = new List<Block>();

    private List<Transform> setSensorBlocks = new List<Transform>();
    private List<Transform> setMehanicBlocks = new List<Transform>();

    public static Block chooseMehanicBlock;

    private char setBlockNumber(char charToValidate)
    {
        int count;

        if (int.TryParse(charToValidate.ToString(), out count))
        {
            if (count > 5 || count < 1)
            {
                charToValidate = '\0';
            }
        }
        else
            charToValidate = '\0';

        return charToValidate;
    }

    private void Start()
    {
        currentObject = this.gameObject;

        mapSizeText.text = minSizeMap.ToString();
        sizeMap = minSizeMap;

        countBlockInput.onValidateInput += delegate (string input, int charIndex, char addedChar) { return setBlockNumber(addedChar); };

        countBlock.gameObject.SetActive(false);

        mehanicChoosePanel.SetActive(false);

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

            if (setBlocksCount.Count == 5)
                for (int i = 0; i < setSensorBlocks.Count - 1; i += 2)
                    if (!setSensorBlocks[i].parent.GetComponent<Toggle>().isOn)
                        setSensorBlocks[i].parent.GetComponent<Toggle>().interactable = false;
        }
        else
        {
            Destroy(setBlocksCount[index].gameObject);
            setBlocksCount.Remove(index);

            if (setBlocksCount.Count == 4)
                for (int i = 0; i < setSensorBlocks.Count - 1; i += 2)
                    setSensorBlocks[i].parent.GetComponent<Toggle>().interactable = true;
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
