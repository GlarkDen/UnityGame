using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsCreateTasks : MonoBehaviour
{
    public void Menu()
    {
        SceneManager.LoadScene("AdministratorMainMenu");
    }
}
