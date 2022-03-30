using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinishLogin : MonoBehaviour
{
    public string Button;

    public void Scene ()
    {
        SceneManager.LoadScene("AdministratorMainMenu");
    }

    public void Text()
    {
        Debug.Log(gameObject.name);
    }

    public void Text1()
    {
        Debug.Log("Text");
    }

    public Button buttonLogin;
    public Button buttonFinish;

    void Start()
    {
        buttonLogin.onClick.AddListener(Text);
        buttonLogin.onClick.AddListener(Text1);

        buttonFinish.onClick.AddListener(Scene);
    }
}
