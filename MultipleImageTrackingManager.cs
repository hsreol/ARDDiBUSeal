using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using Firebase.Database;
using System.Linq;
using Newtonsoft.Json;

public class MultipleImageTrackingManager : MonoBehaviour
{
    public GameObject Save_btn;
    public Image cameraFrame;

    public AudioSource getddibu;

    public GameObject[] objs;
    private Dictionary<string, GameObject> spawnedObjs = new Dictionary<string, GameObject>();

    public List<string> cookies;

    public string cur_cookie;
    public int cookie_count;

    FireBaseTest FireBase;

    public item brave_cookie;
    public item sea_fairy_origin;
    public item sea_fairy_pink;
    public item sea_fairy_black;
    public item sea_fairy_unique;

    private ARTrackedImageManager TrackedImageManager;

    public InputField inputField_code;
    public Canvas save_canvas;
    public AudioSource getSound;
    private void Awake()
    {
        TrackedImageManager = GetComponent<ARTrackedImageManager>();
        foreach (GameObject prefabs in objs)
        {
            GameObject clone = Instantiate(prefabs);
            spawnedObjs.Add(prefabs.name, clone);
            //clone.transform.rotation = Quaternion.Euler(0, 0, 0);
            clone.SetActive(false);
        }
    }

    private void OnEnable()
    {
        TrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void Start()
    {
        FireBase = FireBaseTest.Instance;
        //FireBase.reference = FirebaseDatabase.DefaultInstance.RootReference;
        FireBase.auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        LoadData();

        Save_btn.GetComponent<Button>().onClick.AddListener(SaveData);
    }

    private void OnDisable()
    {
        TrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach(var trackedImage in eventArgs.added)
        {
            UpdateImage(trackedImage);

            cameraFrame.gameObject.SetActive(true);

            Save_btn.GetComponent<Button>().enabled = true;
            Save_btn.GetComponent<Animation>().enabled = true;
            Save_btn.GetComponent<Image>().color = new Color(255, 255, 255, 255);
            getddibu.Play();
        }
        
        foreach(var trackedImage in eventArgs.updated)
        {
            cameraFrame.gameObject.SetActive(true);

            Save_btn.GetComponent<Button>().enabled = true;
            Save_btn.GetComponent<Animation>().enabled = true;
            Save_btn.GetComponent<Image>().color = new Color(255, 255, 255, 255);
            UpdateImage(trackedImage);
        }

        foreach(var trackedImage in eventArgs.removed)
        {
            cameraFrame.gameObject.SetActive(true);

            Save_btn.GetComponent<Button>().enabled = true;
            Save_btn.GetComponent<Animation>().enabled = true;
            Save_btn.GetComponent<Image>().color = new Color(255, 255, 255, 255);

            spawnedObjs[trackedImage.name].SetActive(false);
        }
    }

    void UpdateImage(ARTrackedImage trackedImage)
    {
        GameObject trackedObject = spawnedObjs[trackedImage.referenceImage.name];

        if (trackedImage.trackingState == TrackingState.Tracking)
        {
            cur_cookie = trackedImage.referenceImage.name;
            trackedObject.transform.position = trackedImage.transform.position;
            trackedObject.transform.rotation = trackedImage.transform.rotation;
            trackedObject.transform.Rotate(new Vector3(0, 180f, 0));
            //trackedObject.transform.Rotate(Vector3.right * 0.3f);
            trackedObject.SetActive(true);
        }
        else
        {
            trackedObject.SetActive(false);
        }
    }

    public void SaveData()
    {
        cameraFrame.gameObject.SetActive(false);

        Save_btn.GetComponent<Button>().enabled = false;
        Save_btn.GetComponent<Animation>().enabled = false;
        Save_btn.GetComponent<Image>().color = new Color(123, 123, 123, 123);
    }

    public void CookieRegist()
    {
        FireBase.reference.Child("code").Child(cur_cookie).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("실패");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (var codes in snapshot.Children)
                {
                    Debug.Log(codes.Key.ToString());

                    if (inputField_code.text == codes.Key.ToString() && codes.Child("use").Value.ToString() == "False")
                    {
                        cookies.Add(cur_cookie);
                        cookies = cookies.Distinct().ToList();
                        FireBase.reference.Child("users").Child(FireBase.auth.CurrentUser.UserId).Child("cookie_name").SetValueAsync(cookies);
                        FireBase.reference.Child("users").Child(FireBase.auth.CurrentUser.UserId).Child("cookie_num").SetValueAsync(cookies.Count);

                        FireBase.reference.Child("code").Child(cur_cookie).Child(inputField_code.text).Child("use").SetValueAsync(true);

                        if (cur_cookie == "용감한쿠키")
                        {
                            inventory.Instance.AddItem(brave_cookie);
                            closeChang();
                        }
                        else if (cur_cookie == "바다요정쿠키(오리지널)")
                        {
                            inventory.Instance.AddItem(sea_fairy_origin);
                            closeChang();
                        }
                        else if (cur_cookie == "바다요정쿠키(블랙)")
                        {
                            inventory.Instance.AddItem(sea_fairy_black);
                            closeChang();
                        }
                        else if (cur_cookie == "바다요정쿠키(핑크)")
                        {
                            inventory.Instance.AddItem(sea_fairy_pink);
                            closeChang();
                        }
                        else if (cur_cookie == "바다요정쿠키(유니크)")
                        {
                            inventory.Instance.AddItem(sea_fairy_unique);
                            closeChang();
                        }
                    }
                }
                inputField_code.text = "";
            }
        });
    }

    private void LoadData()
    {
        FireBase.reference.Child("users").Child(FireBase.auth.CurrentUser.UserId).Child("cookie_name").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("실패");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (var cookie in snapshot.Children)
                {
                    if (cookie.Value.ToString() == "용감한쿠키")
                    {
                        inventory.Instance.AddItem(brave_cookie);
                    }
                    else if(cookie.Value.ToString() == "바다요정쿠키(오리지널)")
                    {
                        inventory.Instance.AddItem(sea_fairy_origin);
                    }
                    else if (cookie.Value.ToString() == "바다요정쿠키(블랙)")
                    {
                        inventory.Instance.AddItem(sea_fairy_black);
                    }
                    else if (cookie.Value.ToString() == "바다요정쿠키(핑크)")
                    {
                        inventory.Instance.AddItem(sea_fairy_pink);
                    }
                    else if (cookie.Value.ToString() == "바다요정쿠키(유니크)")
                    {
                        inventory.Instance.AddItem(sea_fairy_unique);
                    }
                    cookies.Add(cookie.Value.ToString());
                }
            }
        });

        FireBase.reference.Child("users").Child(FireBase.auth.CurrentUser.UserId).Child("cookie_num").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("실패");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (var cookie in snapshot.Children)
                {
                    cookie_count = int.Parse(cookie.Key);
                }
            }
        });
    }

    public void closeChang()
    {
        inputField_code.text = "";
        save_canvas.enabled = false;
        getSound.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
