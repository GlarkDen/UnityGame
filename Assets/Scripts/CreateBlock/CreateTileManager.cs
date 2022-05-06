using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CreateTileManager : MonoBehaviour, IPointerClickHandler
{
    public int X;
    public int Y;
    public int Value;
    public byte Rotate;

    public void OnPointerClick(PointerEventData eventData)
    {
        Value = CreateBlocksVariables.CurrentBlock;

        GetComponent<Image>().sprite = CreateBlocksVariables.Sprites[Value];
    }
}
