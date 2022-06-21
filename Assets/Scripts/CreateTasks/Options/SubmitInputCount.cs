using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SubmitInputCount : MonoBehaviour, IPointerUpHandler
{
    public void OnPointerUp(PointerEventData eventData)
    {
        if (VariablesCreateTasks.inputIsEnding)
        {
            VariablesCreateTasks.CurrentInputCount = this.gameObject;
            VariablesCreateTasks.inputIsEnding = false;
        }
    }
}
