using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SubmitInputField : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        if (VariablesCreateTasks.CurrentInputField != null && 
            VariablesCreateTasks.CurrentInputField.gameObject == eventData.selectedObject)
        {
            VariablesCreateTasks.inputIsEnding = false;
        }
        else
        {
            VariablesCreateTasks.CurrentInputField = eventData.selectedObject.GetComponent<InputField>();
            VariablesCreateTasks.inputIsEnding = true;
        }
    }
}
