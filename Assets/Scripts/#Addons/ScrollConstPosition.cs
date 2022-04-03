using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollConstPosition : MonoBehaviour
{
    public RectTransform Content;

    public void SavePosition()
    {
        Content.position = new Vector2(Content.rect.x, -Content.rect.height);
    }
}
