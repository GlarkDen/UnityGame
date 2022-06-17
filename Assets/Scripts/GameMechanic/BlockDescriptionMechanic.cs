using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BlockDescriptionMechanic : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform BlockDescription;
    private Coroutine waitShow;

    public void OnPointerEnter(PointerEventData eventData)
    {
        waitShow = StartCoroutine(ShowDescription(1));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopCoroutine(waitShow);
        if (BlockDescription.gameObject.activeSelf)
            BlockDescription.gameObject.SetActive(false);
    }

    private IEnumerator ShowDescription(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        BlockDescription.anchoredPosition =
            new Vector2(Input.mousePosition.x - (Screen.width + BlockDescription.sizeDelta.x) / 2,
            Input.mousePosition.y - (Screen.height - BlockDescription.sizeDelta.y) / 2);

        int blockNumber = gameObject.GetComponent<SensorBlockManagerMechanic>().Number;

        BlockDescription.GetChild(0).GetComponent<Text>().text = StartGameMechanic.sensorBlockList[blockNumber].description;

        BlockDescription.gameObject.SetActive(true);
    }
}
