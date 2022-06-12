using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Registration : MonoBehaviour
{
    public Button buttonExit;

    public void Scene()
    {
        SceneManager.LoadScene("Login");
    }

    public void Text()
    {
        Debug.Log(gameObject.name);
    }

    void Start()
    {
        buttonExit.onClick.AddListener(Text);
        buttonExit.onClick.AddListener(Scene);
    }
}
