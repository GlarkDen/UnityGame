using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SFB;
using System.IO;

public class VariablesCreateBlock : MonoBehaviour
{
    public Image blockImage;
    private bool setImage = false;

    public Button createBlock;

    public Text blockType;

    public Text blockTitle;

    public Text blockDescription;

    private List<Block> blocks;

    public static byte Type;

    private string blockTitleText = "";
    private string blockDescriptionText = "";

    private void Start()
    {
        blocks = Serialization.LoadBinaryFile<List<Block>>(ProjectPath.Blocks);

        createBlock.interactable = false;

        if (blocks == null) 
        {
            blocks = new List<Block>();
        }
    }

    public void SetType(bool right)
    {
        if (right)
        {
            if (Type < Block.types.Length - 2)
                Type++;
            else
                Type = 0;
        }
        else
        {
            if (Type > 0)
                Type--;
            else
                Type = (byte)(Block.types.Length - 2);
        }

        blockType.text = Block.types[Type];
    }

    public void LoadTexture()
    {
        //какие файлы вообще можно открыть
        ExtensionFilter[] extensions = new[] {
            new ExtensionFilter("Image Files", "png"),
            new ExtensionFilter("All Files", "*" ),
        };
        
        string path = "";

        //открытие формы для загрузки файла
        foreach (string choosePath in StandaloneFileBrowser.OpenFilePanel("Загрузка изображения", "", extensions, false))
        {
            path = choosePath;
        }

        if (path == "")
            return;

        byte[] byteTexture = File.ReadAllBytes(path);

        blockImage.sprite = Texture.ByteToSprite(byteTexture, 100, 100);

        setImage = true;

        if (blockDescriptionText != "" && blockTitleText != "")
            createBlock.interactable = true;
    }

    public void SetDescription()
    {
        blockDescriptionText = blockDescription.text;

        if (blockDescriptionText == "")
            createBlock.interactable = false;
        else if (blockTitleText.Length > 0 && setImage)
            createBlock.interactable = true;
    }

    public void SetTitle()
    {
        blockTitleText = blockTitle.text;

        if (blockTitleText == "")
            createBlock.interactable = false;
        else if (blockDescriptionText.Length > 0 && setImage)
            createBlock.interactable = true;
    }

    public void CreateBlock()
    {
        Block block = new Block();
        block.title = blockTitle.text;
        block.description = blockDescription.text;
        block.texture = Texture.SpriteToByte(blockImage.sprite);
        block.type = Type;

        blocks.Add(block);
    }

    public void SaveBlock()
    {
        Serialization.SaveBinaryFile(blocks, ProjectPath.Blocks);
    }

    public void ResetScene()
    {
        setImage = false;
        blockTitleText = "";
        blockDescriptionText = "";

        blockImage.sprite = null;

        createBlock.interactable = false;
    }
}
