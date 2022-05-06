using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShowCurrentObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image showObject;

    private void Start()
    {
        CreateBlocksVariables.ShowCurrentObject = showObject;
        showObject.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        showObject.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        showObject.gameObject.SetActive(false);
    }
}
