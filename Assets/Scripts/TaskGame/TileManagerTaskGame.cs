using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TileManagerTaskGame : MonoBehaviour, IPointerClickHandler
{
    public int X;
    public int Y;
    public int Value;

    public void OnPointerClick(PointerEventData eventData)
    {
        Value = VariablesTaskGame.CurrentBlock;

        GetComponent<Image>().sprite = VariablesTaskGame.Sprites[Value];
    }
}
