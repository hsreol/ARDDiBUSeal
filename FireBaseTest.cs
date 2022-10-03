using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FireBaseTest : MonoBehaviour
{
    public static FireBaseTest Instance;

    public Button Login_Btn;
    public Button Logout_Btn;
    public Text Name;

    public GameObject logo;
    public GameObject touchscreen;
    public RawImage rawImage;
    public Button button;
    public Canvas canvas;

    public FirebaseAuth auth;
    public FirebaseUser user;
    public DatabaseReference reference;

    public bool alreadylogin = false;

    // Start is called before the first frame update

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        InitializeFirebase();

        Login_Btn.onClick.AddListener(Login);
        Logout_Btn.onClick.AddListener(Logout);
    }

    void InitializeFirebase()
    {
        Debug.Log("���̾�̽� ���� ����");
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if(auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if(!signedIn && user != null)
            {
                Debug.Log("�α׾ƿ� �� " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                reference.Child("users").Child(user.UserId).Child("username").GetValueAsync().ContinueWith(task =>
                {
                    if (task.IsCanceled)
                    {
                        Debug.Log("�ε� ���");
                    }
                    else if (task.IsFaulted)
                    {
                        Debug.Log("�ε� ����");
                    }
                    else
                    {
                        if (!task.Result.Value.Equals(""))
                        {
                            alreadylogin = true;
                        }
                        else
                        {
                            Debug.Log("�α��� �� " + user.UserId);
                        }
                    }
                });
            }

        }
    }

    private void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }

    public void AlreadyLogin()
    {
        if (alreadylogin)
        {
            Login();
        }
        else
        {
            logo.SetActive(false);
            touchscreen.SetActive(false);
            rawImage.canvasRenderer.SetAlpha(0.3f);
            button.gameObject.SetActive(false);
            canvas.gameObject.SetActive(true);
        }
    }

    void Login()
    {
        if (Name.text == "" && !alreadylogin)
        {
            Debug.Log("����� �̸��� �Է����ּ���");
        }
        else
        {
            if (!alreadylogin)
            {
                auth.SignInAnonymouslyAsync().ContinueWith(task =>
                {
                    if (task.IsCanceled)
                    {
                        Debug.LogError("�͸� �α����� ��ҵǾ����ϴ�.");
                        return;
                    }
                    if (task.IsFaulted)
                    {
                        Debug.LogError("�͸� �α��ο� ���� �߻�" + task.Exception);
                        return;
                    }

                    user = task.Result;
                    Debug.LogFormat("�͸� �α��� ����: {0} ({1})", Name.text, user.UserId);

                    writeNewUser(user.UserId, Name.text);
                });
            }
            SceneManager.LoadScene("GameScene");
        }

    }

    void Logout()
    {
        deleteUser(auth.CurrentUser.UserId);
        if (user != null)
        {
            user.DeleteAsync().ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("DeleteAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("DeleteAsync encountered an error: " + task.Exception);
                    return;
                }

                Debug.Log("User deleted successfully.");
            });
        }
        FirebaseAuth.DefaultInstance.SignOut();
    }

    private void writeNewUser(string userID, string username)
    {
        UserData user = new UserData(userID, username);
        string json = JsonUtility.ToJson(user);

        reference.Child("users").Child(userID).SetRawJsonValueAsync(json);
    }

    private void deleteUser(string userID)
    {
        reference.Child("users").Child(userID).RemoveValueAsync();
    }

    public class UserData
    {
        public string userID;
        public string username;
        public string cookie_name;
        public int cookie_num;

        public UserData() { }

        public UserData(string userID, string username)
        {
            this.userID = userID;
            this.username = username;
        }
    }

}
