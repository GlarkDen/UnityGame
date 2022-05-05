using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Texture : MonoBehaviour
{
    public static Sprite LoadSprite(string Name)
    {
        return Resources.Load<Sprite>("Textures/" + Name);
    }
}
