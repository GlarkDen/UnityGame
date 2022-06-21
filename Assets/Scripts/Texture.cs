using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Texture : MonoBehaviour
{
    public static Sprite LoadSprite(string Name)
    {
        return Resources.Load<Sprite>("Textures/" + Name);
    }

    public static byte[] SpriteToByte(Sprite sprite)
    {
        return sprite.texture.EncodeToPNG();
    }

    public static byte[] TextureToByte(Texture2D texture)
    {
        return texture.EncodeToPNG();
    }

    public static Sprite ByteToSprite(byte[] sprite, int width, int height)
    {
        Texture2D texture = new Texture2D(width, height);
        texture.LoadImage(sprite);

        return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0, 0));
    }
}
