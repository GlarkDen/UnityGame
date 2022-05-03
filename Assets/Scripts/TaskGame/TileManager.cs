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
        Debug.Log("Я живой тайл!");
        Debug.Log($"X:{X}; Y:{Y}");

        //// Смена текстуры клетки
        //StartGame.SetSprite(this.gameObject, StartGame.PlayerTurnBuild);

        //// Смена идентификатора клетки
        //StartGame.MapTilesValue[X, Y] = StartGame.PlayerTurnBuild;
    }
}
