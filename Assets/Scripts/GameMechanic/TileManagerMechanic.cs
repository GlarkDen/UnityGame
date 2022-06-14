using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TileManagerMechanic : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    public int X;
    public int Y;
    public int Value;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (VariablesMechanic.IsNull())
            return;

        if ((Value == (int)VariablesMechanic.Wires.WireVertical) && 
            (VariablesMechanic.CurrentBlock == (int)VariablesMechanic.Wires.WireHorizontal))
        {
            Value = (int)VariablesMechanic.Wires.WireTopHorizontal;
            StartGameMechanic.mapTilesValue[X, Y] = Value;
            GetComponent<Image>().sprite = VariablesMechanic.Sprites[Value];
            return;
        }

        if ((Value == (int)VariablesMechanic.Wires.WireHorizontal) &&
            (VariablesMechanic.CurrentBlock == (int)VariablesMechanic.Wires.WireVertical))
        {
            Value = (int)VariablesMechanic.Wires.WireTopVertical;
            StartGameMechanic.mapTilesValue[X, Y] = Value;
            GetComponent<Image>().sprite = VariablesMechanic.Sprites[Value];
            return;
        }

        if (!VariablesMechanic.IsNull(Value))
            return;

        if (VariablesMechanic.CurrentBlock >= 20 && VariablesMechanic.CurrentBlock < 30)
        {
            if (VariablesMechanic.UpdateCountSensors())
            {
                Value = VariablesMechanic.CurrentBlock;
                StartGameMechanic.mapTilesValue[X, Y] = Value;
                GetComponent<Image>().sprite = VariablesMechanic.Sprites[Value];

                if (VariablesMechanic.CountSensorBlocks[Value - VariablesMechanic.SensorStartIndex] == 0)
                    VariablesMechanic.SetCurrentBlock("null");
            }
        }
        else
        {
            Value = VariablesMechanic.CurrentBlock;
            StartGameMechanic.mapTilesValue[X, Y] = Value;
            GetComponent<Image>().sprite = VariablesMechanic.Sprites[Value];
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        VariablesMechanic.CurrentTile = this.gameObject.GetComponent<Image>();
    }
}
