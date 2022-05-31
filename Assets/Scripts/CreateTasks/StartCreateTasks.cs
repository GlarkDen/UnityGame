using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StartCreateTasks : MonoBehaviour
{
    public GameObject chooseBlockList;

    private List<Block> blocks = Serialization.LoadBinaryFile<List<Block>>(ProjectPath.Blocks);

    private List<Transform> setBlocks = new List<Transform>();

    private void Start()
    {
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

        

        for (int i = 0; i < setBlocks.Count; i += 2)
        {
            setBlocks[i].GetComponent<Image>().sprite = Texture.ByteToSprite(blocks[i].texture, 100, 100);
            setBlocks[i + 1].GetComponent<Image>().sprite = Texture.ByteToSprite(blocks[i].texture, 100, 100);
        }

        int panelBlockCount = (int)(blocks.Count / blockListWidth) + blocks.Count % blockListWidth;

        return;

        foreach (var blocksList in panelBlockList)
        {
            for (int i = 0; i < blockListWidth; i++)
                setBlocks.Add(blocksList.transform.GetChild(i));

            for (int i = 0; i < setBlocks.Count; i++)
                setBlocks[i].GetComponent<Image>().sprite = Texture.ByteToSprite(blocks[i].texture, 100, 100);
        }
        

    }
}
