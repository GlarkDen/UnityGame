using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VariablesTaskGame : MonoBehaviour
{
    public Image and;
    public Image or;
    public Image not;
    public Image xor;
    public Image wires;

    public Sprite WireVertical;
    public Sprite WireHorizontal;
    public Sprite WireTopVertical;
    public Sprite WireTopHorizontal;
    public Sprite WireRigthTop;
    public Sprite WireRightDown;
    public Sprite WireLeftTop;
    public Sprite WireLeftDown;

    public Image tile;

    public Image CurrentObject;

    public static Image ShowCurrentObject;
    public static int CurrentBlock;
    public static Sprite[] Sprites = new Sprite[13];
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
        Sprites[5] = WireVertical;

        Sprites[6] = WireHorizontal;
        Sprites[7] = WireTopVertical;
        Sprites[8] = WireTopHorizontal;
        Sprites[9] = WireRigthTop;
        Sprites[10] = WireRightDown;
        Sprites[11] = WireLeftTop;
        Sprites[12] = WireLeftDown;
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

    public static void SetCurrentWire(bool plus)
    {
        if (plus)
        {
            if (CurrentBlock < 12)
                CurrentBlock++;
            else
                CurrentBlock = 5;
        }
        else
        {
            if (CurrentBlock > 5)
                CurrentBlock--;
            else
                CurrentBlock = 12;
        }

        ShowCurrentObject.sprite = Sprites[CurrentBlock];
    }
}
