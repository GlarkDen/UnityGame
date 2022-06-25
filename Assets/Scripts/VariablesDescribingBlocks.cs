using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VariablesDescribingBlocks : MonoBehaviour
{
    public GameObject sensorBlockList;

    public static List<Block> blocks = Serialization.LoadBinaryFile<List<Block>>(ProjectPath.Blocks);

    private List<Transform> setSensorBlocks = new List<Transform>();

    public void SetBlocks()
    {
        //blocks.RemoveAt(0);
        //blocks.RemoveAt(0);
        //blocks.RemoveAt(0);
        //blocks.RemoveAt(0);
        //blocks.RemoveAt(0);
        //blocks.RemoveAt(0);
        //blocks.RemoveAt(0);
        foreach (var block in blocks)
        {
            block.sprite = Texture.ByteToSprite(block.texture, 100, 100);
        }
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
        if (blocks.Count == 0)
        {
            Destroy(setSensorBlocks[0].parent.gameObject);
        }
        else if (blocks.Count == 1)
        {
            setSensorBlocks[0].GetComponent<Image>().sprite = blocks[0].sprite;

            Destroy(setSensorBlocks[2].gameObject);
            setSensorBlocks.RemoveAt(2);
            Destroy(setSensorBlocks[1].gameObject);
            setSensorBlocks.RemoveAt(1);
        }
        else if (blocks.Count == 2)
        {
            setSensorBlocks[0].GetComponent<Image>().sprite = blocks[0].sprite;
            setSensorBlocks[1].GetComponent<Image>().sprite = blocks[1].sprite;

            Destroy(setSensorBlocks[2].gameObject);
            setSensorBlocks.RemoveAt(2);
        }
        else
        {
            Debug.Log(panelSensorCount + " " + panelSensorList.Count);
            for (int i = 0; i < panelSensorCount - panelSensorList.Count; i++)
            {
                Transform panel = Instantiate(panelSensorList[0], new Vector2(0, 0),
                    Quaternion.identity, sensorBlockList.transform);

                for (int j = 0; j < sensorListHigh; j++)
                {
                    setSensorBlocks.Add(panel.transform.GetChild(j));
                }
            }
            int count = setSensorBlocks.Count;

            if (blocks.Count % 3 == 1)
            {
                Destroy(setSensorBlocks[count - 1].gameObject);
                Destroy(setSensorBlocks[count - 2].gameObject);
                setSensorBlocks.RemoveAt(count - 1);
                setSensorBlocks.RemoveAt(count - 2);
            }
            else if (blocks.Count % 3 == 2)
            {
                Destroy(setSensorBlocks[count - 1].gameObject);
                setSensorBlocks.RemoveAt(count - 1);
            }
            for (int i = 0; i < setSensorBlocks.Count; i++)
            {
                setSensorBlocks[i].GetComponent<Image>().sprite = blocks[i].sprite;
                setSensorBlocks[i].GetComponent<BlockListIndexDescribe>().index = i;
            }
        }
    }

    private void Start()
    {
        SetBlocks();
    }

    public void RemoveBlock(GameObject obj)
    {
        int index = obj.GetComponent<BlockListIndexDescribe>().index;
        blocks.RemoveAt(index);
        ClearContent();
        StartCoroutine(Wait());
    }

    private void ClearContent()
    {
        for (int i = 1; i < sensorBlockList.transform.childCount; i++)
        {
            Destroy(sensorBlockList.transform.GetChild(i).gameObject);
        }
        //for (int i = 0; i < 3; i++)
        //{
        //    Debug.Log(sensorBlockList.transform.GetChild(0).transform.GetChild(i));
        //    sensorBlockList.transform.GetChild(0).transform.GetChild(i).GetComponent<Image>().sprite = null;
        //}
        setSensorBlocks.Clear();
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.05f);
        SetBlocks();
    }
}
