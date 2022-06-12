using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockListData : MonoBehaviour
{
    public int index;

    public void ClickSensor(bool status)
    {
        VariablesCreateTasks.currentObject.GetComponent<VariablesCreateTasks>().UpdateChooseBlock(status, index);
    }
    public void ClickMehanic()
    {
        VariablesCreateTasks.currentObject.GetComponent<VariablesCreateTasks>().UpdateMehanicBlock(index);
    }
}
