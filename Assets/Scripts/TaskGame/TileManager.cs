using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileManager : MonoBehaviour, IPointerClickHandler
{
    public int X;
    public int Y;
    public int Value;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("� ����� ����!");
        Debug.Log($"X:{X}; Y:{Y}");

        //// ����� �������� ������
        //StartGame.SetSprite(this.gameObject, StartGame.PlayerTurnBuild);

        //// ����� �������������� ������
        //StartGame.MapTilesValue[X, Y] = StartGame.PlayerTurnBuild;
    }
}
