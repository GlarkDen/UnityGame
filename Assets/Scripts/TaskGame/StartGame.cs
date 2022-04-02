using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    // solutionEfficiency

    void Start()
    {
        Settings set = new Settings();

        set.a = 5;
        set.b = "Ïנמגונךא";

        Serialization.SaveJsonFile(set, "Ôאיכ");

        Debug.Log("Checkung-1...");

        set = Serialization.LoadJsonFile<Settings>("Ôאיכ");

        Debug.Log(set.a);
        Debug.Log(set.b);

        Debug.Log("Checkung-2...");

        List<Settings> settings = new List<Settings>();

        Debug.Log("Checkung-3...");

        settings.Add(new Settings(2, "aaa"));

        Debug.Log("Checkung-4...");

        settings.Add(new Settings(5, "bbb"));
        settings.Add(new Settings(10, "ccc"));

        Debug.Log(settings.Count);

        Debug.Log("Checkung-5...");

        Serialization.SaveJsonFile(settings, "Ôאיכ-3", false);

        Debug.Log("Checkung-6...");

        List<Settings> set2 = Serialization.LoadJsonFile<List<Settings>>("Ôאיכ-3", false);

        Debug.Log("Checkung-7...");

        Debug.Log(set2);

        Debug.Log(set2.Count);

        Debug.Log("Checkung-8...");

        Dictionary<int, Settings> dict = new Dictionary<int, Settings>();
        dict[10] = new Settings(5, "bbb");
        dict[15] = new Settings(10, "ccc");

        Serialization.SaveJsonFile(dict, "Ôאיכ-4", false);
        dict = Serialization.LoadJsonFile<Dictionary<int, Settings>>("Ôאיכ-4", false);

        Debug.Log("Checkung-9...");

        Debug.Log(dict[10].a);
    }
}

public class Settings
{
    public int a;
    public string b;

    public Settings(int a, string b)
    {
        this.a = a;
        this.b = b;
    }

    public Settings()
    {

    }
}
