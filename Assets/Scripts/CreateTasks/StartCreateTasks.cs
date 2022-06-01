using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StartCreateTasks : MonoBehaviour
{
    public GameObject chooseBlockList;

    public InputField countBlock;

    private List<Block> blocks = Serialization.LoadBinaryFile<List<Block>>(ProjectPath.Blocks);

    private List<Transform> setBlocks = new List<Transform>();

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
        countBlock.onValidateInput += delegate (string input, int charIndex, char addedChar) { return setBlockNumber(addedChar); };

        int blockListWidth = 0;

        List<Transform> panelBlockList = new List<Transform>();

        for (int i = 0; i < chooseBlockList.transform.childCount; i++)
            panelBlockList.Add(chooseBlockList.transform.GetChild(i));

        blockListWidth = panelBlockList[0].childCount;

        for (int i = 0; i < blockListWidth; i++)
        {
            Transform panelObject = panelBlockList[0].transform.GetChild(i);
            setBlocks.Add(panelObject.GetChild(0));
            setBlocks.Add(panelObject.GetChild(0).GetChild(0));
        }

        int panelBlockCount = blocks.Count / blockListWidth + blocks.Count % blockListWidth;

        if (blocks.Count == 1)
        {
            setBlocks[0].GetComponent<Image>().sprite = Texture.ByteToSprite(blocks[0].texture, 100, 100);
            Destroy(setBlocks[1]);
        }
        else
        {
            for (int i = 0; i < panelBlockCount - panelBlockList.Count; i++)
            {
                Transform panel = Instantiate(panelBlockList[0], new Vector2(0, 0),
                    Quaternion.identity, chooseBlockList.transform);

                for (int j = 0; j < blockListWidth; j++)
                {
                    Transform panelObject = panel.transform.GetChild(j);
                    setBlocks.Add(panelObject.GetChild(0));
                    setBlocks.Add(panelObject.GetChild(0).GetChild(0));
                }
            }

            for (int i = 0; i < setBlocks.Count; i += 2)
            {
                setBlocks[i].GetComponent<Image>().sprite = Texture.ByteToSprite(blocks[i / 2].texture, 100, 100);
                setBlocks[i + 1].GetComponent<Image>().sprite = Texture.ByteToSprite(blocks[i / 2].texture, 100, 100);

                if (blocks.Count - 1 == i)
                {
                    Destroy(setBlocks[i + 2]);
                    break;
                }
            }
        }
    }
}
