using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.SceneManagement;
using Firebase.Database;
using System.Linq;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTrackerManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> ObjectsToPlace;

    private int refImageCount;
    private Dictionary<string, GameObject> allObjects;

    private ARTrackedImageManager arTrackedImageManager;
    private IReferenceImageLibrary refLibrary;

    public Image cameraFrame;
    public GameObject save_bt;

    public AudioSource getddibu;

    public GameObject[] objs;

    public List<string> cookies;
    FireBaseTest FireBase;

    void Awake()
    {
        arTrackedImageManager = GetComponent<ARTrackedImageManager>();
    }


    private void OnEnable()
    {
        arTrackedImageManager.trackedImagesChanged += OnImageChanged;
    }

    private void OnDisable()
    {
        arTrackedImageManager.trackedImagesChanged -= OnImageChanged;
    }

    private void Start()
    {
        FireBase = FireBaseTest.Instance;
        //FireBase.reference = FirebaseDatabase.DefaultInstance.RootReference;
        FireBase.auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        LoadData();

        save_bt.GetComponent<Button>().onClick.AddListener(SaveData);

        refLibrary = arTrackedImageManager.referenceLibrary;
        refImageCount = refLibrary.count;
        LoadObjectDictionary();
    }

    void LoadObjectDictionary()
    {
        allObjects = new Dictionary<string, GameObject>();

        for (int i = 0; i < refImageCount; i++)
        {
            GameObject newOverlay = new GameObject();
            newOverlay = ObjectsToPlace[i];
            //check if the object is prefab and need to be instantiated
            if (ObjectsToPlace[i].gameObject.scene.rootCount == 0)
            {
                newOverlay = (GameObject)Instantiate(ObjectsToPlace[i], transform.localPosition, Quaternion.identity);
            }

            allObjects.Add(refLibrary[i].name, newOverlay);
            newOverlay.SetActive(false);
        }
    }


    void ActivateTrackedObject(string imageName)
    {
        Debug.Log("Tracked the target: " + imageName);
        
        // Give the initial image a reasonable default scale
        allObjects[imageName].transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
        allObjects[imageName].SetActive(true);
    }

    private void UpdateTrackedObject(ARTrackedImage trackedImage)
    {
        GameObject trackedObject = allObjects[trackedImage.referenceImage.name];

        //if tracked image tracking state is comparable to tracking
        if (trackedImage.trackingState == TrackingState.Tracking)
        {
            cookies.Add(trackedObject.name);
            
            trackedObject.transform.position = trackedImage.transform.position;
            trackedObject.transform.rotation = trackedImage.transform.rotation;

            trackedObject.SetActive(true);
        }
        else //if tracked image tracking state is limited or none 
        {
            //deactivate the image tracked ar object 
            trackedObject.SetActive(false);
        }
    }

    public void OnImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        // for each tracked image that has been added
        foreach (var addedImage in args.added)
        {
            ActivateTrackedObject(addedImage.referenceImage.name);
            cameraFrame.gameObject.SetActive(false);

            save_bt.GetComponent<Button>().enabled = true;
            save_bt.GetComponent<Animation>().enabled = true;
            save_bt.GetComponent<Image>().color = new Color(255, 255, 255, 255);
            getddibu.Play();
        }

        // for each tracked image that has been updated
        foreach (var updated in args.updated)
        {
            //throw tracked image to check tracking state
            UpdateTrackedObject(updated);
        }

        // for each tracked image that has been removed  
        foreach (var trackedImage in args.removed)
        {
            // destroy the AR object associated with the tracked image
            allObjects[trackedImage.name].SetActive(false);
            cameraFrame.gameObject.SetActive(true);

            save_bt.GetComponent<Button>().enabled = false;
            save_bt.GetComponent<Animation>().enabled = false;
            save_bt.GetComponent<Image>().color = new Color(123, 123, 123, 123);
        }
    }

    public void SaveData()
    {
        cookies = cookies.Distinct().ToList();

        CookieRegist(cookies);
    }

    private void CookieRegist(List<string> cookies)
    {
        //string jsondata = JsonConvert.SerializeObject(cookies);

        FireBase.reference.Child("users").Child(FireBase.auth.CurrentUser.UserId).Child("cookie_name").SetValueAsync(cookies);
        FireBase.reference.Child("users").Child(FireBase.auth.CurrentUser.UserId).Child("cookie_num").SetValueAsync(cookies.Count);

        save_bt.gameObject.SetActive(false);
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
                    Debug.Log(cookie.Value);
                    cookies.Add(cookie.Value.ToString());
                }
                //Debug.Log(cookies);
            }
        });
    }

}
