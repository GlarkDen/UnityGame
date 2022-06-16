using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public Button button;

    // Закрытие игры
    public static void Exit()
    {
        Application.Quit();   
    }
}
