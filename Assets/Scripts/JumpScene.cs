using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JumpScene : MonoBehaviour
{
    /// <summary>
    /// Кнопка, по нажатию которой будет происходить переход к сцене
    /// </summary>
    public Button button;

    /// <summary>
    /// Название сцены, к которой осуществляется переход
    /// </summary>
    public string nameScene;

    public void Scene()
    {
        SceneManager.LoadScene(nameScene);
    }

    public void Start()
    {
        button.onClick.AddListener(Scene);
    }
}
