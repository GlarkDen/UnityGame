using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SFB;

public class CreateBlocksVariables : MonoBehaviour
{
    public Image and;
    public Image or;
    public Image not;
    public Image xor;
    public Image wires;

    public Image test;

    public Text connectionTop;
    public Text connectionDown;
    public Text connectionLeft;
    public Text connectionRight;

    public Text type;

    public Image tile;

    public Image CurrentObject;

    public static Image ShowCurrentObject;
    public static int CurrentBlock;
    public static Sprite[] Sprites = new Sprite[6];
    public static byte[] Connections = new byte[4];
    public static byte Type;

    private void Start()
    {
        ShowCurrentObject = CurrentObject;

        ShowCurrentObject.sprite = tile.sprite;

        Sprites[0] = tile.sprite;
        Sprites[1] = and.sprite;
        Sprites[2] = or.sprite;
        Sprites[3] = not.sprite;
        Sprites[4] = xor.sprite;
        Sprites[5] = wires.sprite;
    }

    public void SetCurrentBlock(string name)
    {
        int id = 0;

        switch (name)
        {
            case "and":
                id = 1;
                break;

            case "or":
                id = 2;
                break;

            case "not":
                id = 3;
                break;

            case "xor":
                id = 4;
                break;

            case "wire":
                id = 5;
                break;
        }

        CurrentBlock = id;
        ShowCurrentObject.sprite = Sprites[id];
    }

    public void SetType(bool right)
    {
        if (right)
        {
            if (Type < Block.types.Length - 1)
                Type++;
            else
                Type = 0;
        }
        else
        {
            if (Type > 0)
                Type--;
            else
                Type = (byte)(Block.types.Length - 1);
        }

        type.text = Block.types[Type];
    }

    public void SetConnection(string position)
    {
        int id = 0;

        switch (position)
        {
            case "top":
                id = 0;
                break;

            case "down":
                id = 1;
                break;

            case "left":
                id = 2;
                break;

            case "right":
                id = 3;
                break;
        }

        if (Connections[id] < Block.connectionType.Length - 1)
            Connections[id]++;
        else 
            Connections[id] = 0;

        switch (position)
        {
            case "top":
                connectionTop.text = Block.connectionType[Connections[id]];
                break;

            case "down":
                connectionDown.text = Block.connectionType[Connections[id]];
                break;

            case "left":
                connectionLeft.text = Block.connectionType[Connections[id]];
                break;

            case "right":
                connectionRight.text = Block.connectionType[Connections[id]];
                break;
        }
    }

    public void LoadTexture()
    {
        var extensions = new[] {  //какие файлы вообще можно открыть
            new ExtensionFilter("Image Files", "png", "jpg", "jpeg" ),
            new ExtensionFilter("All Files", "*" ),
        };
        
        string pathNew = "";

        foreach (string path in StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, true))
        { 
            //открытие формы для загрузки файла
            Debug.Log(path);
            pathNew = path;
        }

        if (pathNew == "")
            return;

        WWW www = new WWW(pathNew);

        //yield return www;

        var texture = www.texture;
        test.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
    }
}
