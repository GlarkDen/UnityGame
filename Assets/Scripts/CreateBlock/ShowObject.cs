using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowObject : MonoBehaviour
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

        //if (Input.GetAxis("Mouse ScrollWheel") > 0.0f)
        //    transform.Rotate(0, 0, 90);

        //if (Input.GetAxis("Mouse ScrollWheel") < 0.0f)
        //    transform.Rotate(0, 0, -90);
    }
}
