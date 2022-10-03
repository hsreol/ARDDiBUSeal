using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class logout : MonoBehaviour
{
    FireBaseTest FireBase;
    public FirebaseUser user;
    void Start()
    {
        FireBase = FireBaseTest.Instance;
        //FireBase.reference = FirebaseDatabase.DefaultInstance.RootReference;
        FireBase.auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        user = FireBase.auth.CurrentUser;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Logout()
    {
        deleteUser(FireBase.auth.CurrentUser.UserId);
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

        SceneManager.LoadScene("Init");
    }

    private void deleteUser(string userID)
    {
        FireBase.reference.Child("users").Child(userID).RemoveValueAsync();
    }
}
