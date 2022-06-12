using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveToMouse : MonoBehaviour
{
    public void MoveObjToMouse(RectTransform obj)
    {
        obj.anchoredPosition = new Vector2(Input.mousePosition.x - (Screen.width - obj.sizeDelta.x) / 2, 
            Input.mousePosition.y - (Screen.height + obj.sizeDelta.y) / 2);
    }
}
