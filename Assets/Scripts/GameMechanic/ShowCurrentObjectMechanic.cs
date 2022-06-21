using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShowCurrentObjectMechanic : MonoBehaviour
{
    private Vector3 lastPos = new Vector3();

    void Update()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (lastPos != pos)
        {
            transform.position = new Vector2(pos.x, pos.y);
            lastPos = pos;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {
            if (VariablesMechanic.CurrentBlock >= 5 && VariablesMechanic.CurrentBlock <= 10)
            {
                if (scroll > 0.0f)
                    VariablesMechanic.SetCurrentWire("forward");
                else
                    VariablesMechanic.SetCurrentWire("back");
            }
            else if (VariablesMechanic.CurrentBlock > 0 && VariablesMechanic.CurrentBlock < 5)
            {
                if (scroll > 0.0f)
                    VariablesMechanic.SetCurrentLogic("back");
                else
                    VariablesMechanic.SetCurrentLogic("forward");
            }
            else if (VariablesMechanic.CurrentBlock >= 20 && VariablesMechanic.CurrentBlock < 30)
            {
                if (scroll > 0.0f)
                    VariablesMechanic.SetCurrentSensor("back");
                else
                    VariablesMechanic.SetCurrentSensor("forward");
            }
        }

        if (Input.anyKeyDown)
        {
            if (VariablesMechanic.CurrentBlock >= 5 && VariablesMechanic.CurrentBlock <= 10)
            {
                if (Input.GetKeyDown(KeyCode.W))
                    VariablesMechanic.SetCurrentWire("W");
                else if (Input.GetKeyDown(KeyCode.A))
                    VariablesMechanic.SetCurrentWire("A");
                else if (Input.GetKeyDown(KeyCode.S))
                    VariablesMechanic.SetCurrentWire("S");
                else if (Input.GetKeyDown(KeyCode.D))
                    VariablesMechanic.SetCurrentWire("D");
            }
        }
    }
}
