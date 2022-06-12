using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VariablesRegistration : MonoBehaviour
{
    public Button buttonRegistration;
    public Button buttonEntrance;

    public Text login;
    public Text password;
    public Text passwordRepeat;

    public Image imageMessage;
    public Text textMessage;

    private string loginText = "";
    private string passwordText = "";
    private string passwordRepeatText = "";

    public static Account account;
    private Dictionary<string, Account> accounts;

    public InputField inputPassword;
    public InputField inputPasswordRepeat;


    private void Start()
    {
        accounts = Serialization.LoadBinaryFile<Dictionary<string, Account>>(ProjectPath.Accounts);

        buttonRegistration.interactable = false;

        buttonEntrance.gameObject.SetActive(false);

        imageMessage.gameObject.SetActive(false);

        if (accounts == null)
        {
            accounts = new Dictionary<string, Account>();
        }
    }


    public void SetLogin()
    {
        loginText = login.text;
        imageMessage.gameObject.SetActive(false);

        if (CheckText())
        {
            buttonRegistration.interactable = true;
        }
    }

    public void SetPassword()
    {
        passwordText = password.text;
        imageMessage.gameObject.SetActive(false);

        if (CheckText())
        {
            buttonRegistration.interactable = true;
        }
    }

    public void SetPasswordRepeat()
    {
        passwordRepeatText = passwordRepeat.text;
        imageMessage.gameObject.SetActive(false);

        if (CheckText())
        {
            buttonRegistration.interactable = true;
        }
    }


    private bool CheckText()
    {
        if (loginText.Length == 0)
            return false;

        if (passwordText.Length == 0)
            return false;

        if (passwordRepeatText.Length == 0)
            return false;

        return true;
    }

    public void SaveAccounts()
    {
        if (passwordText != passwordRepeatText)
        {
            ClearPassword();
            Message("Пароли не совпадают", Color.red);
        }
        else if (loginText.Length < 3)
        {
            Message("Слишком короткий логин\nНе менее 3 символов", Color.red);
        }
        else if (passwordText.Length < 5)
        {
            ClearPassword();
            Message("Слишком короткий пароль\nНе менее 5 символов", Color.red);
        }
        else if (accounts.ContainsKey(loginText))
        {
            Message("Пользователь с таким логином уже есть", Color.red);
        }
        else
        {
            account = new Account(loginText, passwordText, "Ученик");
            accounts.Add(loginText, account);
            Serialization.SaveBinaryFile(accounts, ProjectPath.Accounts);

            Message("Регистрация успешна", Color.green);

            buttonEntrance.gameObject.SetActive(true);
        }
    }

    private void Message(string message, Color color)
    {
        buttonRegistration.interactable = false;

        textMessage.text = message;
        textMessage.color = color;

        imageMessage.gameObject.SetActive(true);
    }

    private void ClearPassword()
    {
        inputPassword.text = "";
        inputPasswordRepeat.text = "";
    }

    public void ResetScene()
    {
        loginText = "";
        passwordText = "";
        passwordRepeatText = "";

        buttonRegistration.interactable = false;
    }
}
