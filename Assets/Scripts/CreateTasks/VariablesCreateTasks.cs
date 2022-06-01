using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SFB;
using System.IO;

public class VariablesCreateTasks : MonoBehaviour
{
    int sizeMap = 1;

    public Text mapSizeText;

    public GameObject setBlockCount;

    private void Start()
    {
        mapSizeText.text = sizeMap.ToString();
    }

    public void ResizeMap(int changeSize)
    {
        if (sizeMap + changeSize <= 0)
            return;

        if (sizeMap + changeSize > 7)
            return;
            
        sizeMap += changeSize;

        mapSizeText.text = sizeMap.ToString();
    }
}
