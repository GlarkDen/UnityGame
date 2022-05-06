using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CurrentObjectTaskGame : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image showObject;

    private void Start()
    {
        VariablesTaskGame.ShowCurrentObject = showObject;
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
