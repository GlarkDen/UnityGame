using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JumpScene : MonoBehaviour
{
    /// <summary>
    /// ������, �� ������� ������� ����� ����������� ������� � �����
    /// </summary>
    public Button button;

    /// <summary>
    /// �������� �����, � ������� �������������� �������
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
