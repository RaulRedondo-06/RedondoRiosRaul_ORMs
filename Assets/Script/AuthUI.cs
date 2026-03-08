using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class AuthUI : MonoBehaviour
{
    public SQLiteDB db;

    public TMP_InputField userInput;
    public TMP_InputField passInput;

    public GameObject popup;
    public TMP_Text popupText;

    [SerializeField] private string mainSceneName = "MainScene";

    public void Register()
    {
        string msg = db.Register(userInput.text, passInput.text);
        ShowPopup(msg);
    }

    public void Login()
    {
        string msg = db.Login(userInput.text, passInput.text);

        if (msg == "OK")
        {
            SceneManager.LoadScene(mainSceneName);
        }
        else
        {
            ShowPopup(msg);
        }
    }

    public void ShowUsers()
    {
        string msg = db.GetUsersList();
        ShowPopup(msg);
    }

    public void DeleteUser()
    {
        string msg = db.DeleteUser(userInput.text);
        ShowPopup(msg);
    }

    public void DeleteAllUsers()
    {
        string msg = db.DeleteAllUsers();
        ShowPopup(msg);
    }

    private void ShowPopup(string msg)
    {
        popup.SetActive(true);
        popupText.text = msg;
    }

    public void ClosePopup()
    {
        popup.SetActive(false);
    }
}