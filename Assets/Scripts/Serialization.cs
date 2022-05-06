using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class Serialization
{
    private static string rootPath = Application.streamingAssetsPath;

    private static int codeKey = 157;

    public static void SaveJsonFile<T>(T obj, string path, bool encription = true)
    {
        string jsonObject = JsonUtility.ToJson(obj);

        if (encription)
            jsonObject = XORCript(jsonObject);
            
        File.WriteAllText(rootPath + "/" + path, jsonObject);
    }

    public static T LoadJsonFile<T>(string path, bool decription = true)
    {
        string jsonObject = File.ReadAllText(rootPath + "/" + path);

        if (decription)
            jsonObject = XORCript(jsonObject);

        T obj = JsonUtility.FromJson<T>(jsonObject);

        return obj;
    }

    public static void SaveBinaryFile<T>(T obj, string path, FileMode writeMode = FileMode.Create)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        FileStream writeStream = new FileStream(rootPath + "/" + path, writeMode);

        binaryFormatter.Serialize(writeStream, obj);

        writeStream.Close();
    }

    public static T LoadBinaryFile<T>(string path, FileMode readMode = FileMode.Open)
    {
        if (File.Exists(rootPath + "/" + path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream readStream = new FileStream(rootPath + "/" + path, readMode);

            return (T)binaryFormatter.Deserialize(readStream);
        }
        else
        {
            return default(T);
        }
    }

    private static string XORCript(string text)
    {
        string encodingString = "";

        for (int i = 0; i < text.Length; i++)
            encodingString += (char)(text[i] ^ codeKey);

        return encodingString;
    }

    public static byte[] ObjectToByteArray(Object obj)
    {
        BinaryFormatter bf = new BinaryFormatter();
        using (var ms = new MemoryStream())
        {
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }

    public static object ByteArrayToObject(byte[] arrBytes)
    {
        using (var memStream = new MemoryStream())
        {
            var binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            var obj = binForm.Deserialize(memStream);
            return obj;
        }
    }
}