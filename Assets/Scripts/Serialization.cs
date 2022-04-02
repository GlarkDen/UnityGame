using UnityEngine;
using System.IO;

public static class Serialization
{
    private static string rootPath = Application.streamingAssetsPath;

    private static int codeKey = 157;

    public static bool SaveJsonFile<T>(T obj, string path, bool encription = true)
    {
        string jsonObject = JsonUtility.ToJson(obj);

        if (encription)
            jsonObject = XORCript(jsonObject);
            
        File.WriteAllText(rootPath + "/" + path, jsonObject);

        return true;
    }

    public static T LoadJsonFile<T>(string path, bool decription = true)
    {
        string jsonObject = File.ReadAllText(rootPath + "/" + path);

        if (decription)
            jsonObject = XORCript(jsonObject);

        T obj = JsonUtility.FromJson<T>(jsonObject);

        return obj;
    }

    private static string XORCript(string text)
    {
        string encodingString = "";

        for (int i = 0; i < text.Length; i++)
            encodingString += (char)(text[i] ^ codeKey);

        return encodingString;
    }
}

