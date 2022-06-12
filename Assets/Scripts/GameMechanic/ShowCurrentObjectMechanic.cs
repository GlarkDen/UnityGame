using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        if (Input.GetAxis("Mouse ScrollWheel") != 0.0f)
        {
            if (VariablesMechanic.CurrentBlock >= 5 && VariablesMechanic.CurrentBlock <= 10)
            {
                if (Input.GetAxis("Mouse ScrollWheel") > 0.0f)
                {
                    VariablesMechanic.SetCurrentWire("forward");
                    //transform.Rotate(0, 0, 90);
                }

                if (Input.GetAxis("Mouse ScrollWheel") < 0.0f)
                {
                    VariablesMechanic.SetCurrentWire("back");
                    //transform.Rotate(0, 0, -90);
                }
            }

            if (VariablesMechanic.CurrentBlock > 0 && VariablesMechanic.CurrentBlock < 5)
            {
                if (Input.GetAxis("Mouse ScrollWheel") > 0.0f)
                {
                    VariablesMechanic.SetCurrentLogic("back");
                    //transform.Rotate(0, 0, 90);
                }

                if (Input.GetAxis("Mouse ScrollWheel") < 0.0f)
                {
                    VariablesMechanic.SetCurrentLogic("forward");
                    //transform.Rotate(0, 0, -90);
                }
            }

            if (VariablesMechanic.CurrentBlock >= 20 && VariablesMechanic.CurrentBlock < 30)
            {
                if (Input.GetAxis("Mouse ScrollWheel") > 0.0f)
                {
                    VariablesMechanic.SetCurrentSensor("back");
                    //transform.Rotate(0, 0, 90);
                }

                if (Input.GetAxis("Mouse ScrollWheel") < 0.0f)
                {
                    VariablesMechanic.SetCurrentSensor("forward");
                    //transform.Rotate(0, 0, -90);
                }
            }
        }

        if (VariablesMechanic.CurrentBlock >= 5 && VariablesMechanic.CurrentBlock <= 10)
        {
            if (Input.GetKeyUp(KeyCode.W))
                VariablesMechanic.SetCurrentWire("W");

            if (Input.GetKeyUp(KeyCode.A))
                VariablesMechanic.SetCurrentWire("A");

            if (Input.GetKeyUp(KeyCode.S))
                VariablesMechanic.SetCurrentWire("S");

            if (Input.GetKeyUp(KeyCode.D))
                VariablesMechanic.SetCurrentWire("D");
        }
    }
}
