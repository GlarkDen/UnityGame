using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinishLogin : MonoBehaviour
{
    public void Scene()
    {
        SceneManager.LoadScene("AdministratorMainMenu");
    }

    public void Text()
    {
        Debug.Log(gameObject.name);
    }

    public Button buttonLogin;
    public Button buttonFinish;

    void Start()
    {
        buttonLogin.onClick.AddListener(Text);
        buttonFinish.onClick.AddListener(Scene);
    }
}
