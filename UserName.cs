using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using UnityEngine.UI;

public class UserName : MonoBehaviour
{
    FireBaseTest FireBase;

    public Text Name;

    public string namedata;

    private void Start()
    {
        FireBase = FireBaseTest.Instance;
        //FireBase.reference = FirebaseDatabase.DefaultInstance.RootReference;
        FireBase.auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        LoadData(); 
    }

    private void Update()
    {
        Name.text = namedata;
    }

    private void LoadData()
    {
        FireBase.reference.Child("users").Child(FireBase.auth.CurrentUser.UserId).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("실패");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                foreach (var data in snapshot.Children)
                {
                    if(data.Key == "username")
                    {
                        namedata = data.Value.ToString();
                    }
                }
            }
            else
            {
                Debug.Log("이거 왜 안됨?");
            }
        });
    }
}
