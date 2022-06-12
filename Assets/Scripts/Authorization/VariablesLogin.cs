using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VariablesLogin : MonoBehaviour
{
    public Button buttonLogin;

    public Text login;
    public Text password;

    public Image imageMessage;
    public Text textMessage;

    private string loginText = "";
    private string passwordText = "";

    public static Account account;
    private Dictionary<string, Account> accounts;

    private string nameScene;


    private void Start()
    {
        accounts = Serialization.LoadBinaryFile<Dictionary<string, Account>>(ProjectPath.Accounts);

        buttonLogin.interactable = false;

        imageMessage.gameObject.SetActive(false);

        if (accounts == null)
        {
            accounts = new Dictionary<string, Account>();

            Account firstAdmin = new Account("Admin", "12345", "Учитель");
            accounts.Add(firstAdmin.login, firstAdmin);

            Serialization.SaveBinaryFile(accounts, ProjectPath.Accounts);
        }
    }


    public void SetLogin()
    {
        loginText = login.text;
        imageMessage.gameObject.SetActive(false);

        if (CheckText())
        {
            buttonLogin.interactable = true;
        }
    }

    public void SetPassword()
    {
        passwordText = password.text;
        imageMessage.gameObject.SetActive(false);

        if (CheckText())
        {
            buttonLogin.interactable = true;
        }
    }


    private bool CheckText()
    {
        if (loginText.Length == 0)
            return false;

        if (passwordText.Length == 0)
            return false;
        return true;
    }

    public void Login()
    {
        if (!accounts.ContainsKey(loginText))
        {
            Message("Неверный логин или пароль", Color.red);
        }
        else
        {
            account = accounts[loginText];

            if (passwordText != account.password)
            {
                Message("Неверный логин или пароль", Color.red);
            }
            else
            {
                if (account.status == "Учитель")
                {
                    nameScene = "AdministratorMainMenu";
                }
                else
                {
                    nameScene = "UserMainMenu";
                }
                SceneManager.LoadScene(nameScene);
            }
        }
    }

    private void Message(string message, Color color)
    {
        buttonLogin.interactable = false;

        textMessage.text = message;
        textMessage.color = color;

        imageMessage.gameObject.SetActive(true);
    }

    public void ResetScene()
    {
        loginText = "";
        passwordText = "";

        buttonLogin.interactable = false;
    }
}
