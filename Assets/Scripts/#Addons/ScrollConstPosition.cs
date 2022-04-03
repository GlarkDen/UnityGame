using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollConstPosition : MonoBehaviour
{
    public RectTransform Content;

    public void SavePosition()
    {
        Content.localPosition = new Vector2(Content.localPosition.x, -Content.rect.height);
    }
}
