using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using Firebase.Database;
using Newtonsoft.Json;
using System.Linq;

public class ReadQRCode : MonoBehaviour
{
    public ARCameraManager CameraManager;
    public Button Save_btn;
    public Text Text;

    public List<string> cookies;
    FireBaseTest FireBase;
    void Start()
    {
        FireBase = FireBaseTest.Instance;
        FireBase.reference = FirebaseDatabase.DefaultInstance.RootReference;
        FireBase.auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        Save_btn.onClick.AddListener(SaveData);
    }

    // Update is called once per frame
    void Update()
    {
        if (CameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
        {
            using(image)
            {
                var conversionParams = new XRCpuImage.ConversionParams(image, TextureFormat.R8, XRCpuImage.Transformation.MirrorY);
                var dataSize = image.GetConvertedDataSize(conversionParams);
                var grayscalePixels = new byte[dataSize];

                unsafe
                {
                    fixed (void* ptr = grayscalePixels)
                    {
                        image.Convert(conversionParams, new System.IntPtr(ptr), dataSize);
                    }
                }

                IBarcodeReader barcodeReader = new BarcodeReader();
                var result = barcodeReader.Decode(grayscalePixels, image.width, image.height, RGBLuminanceSource.BitmapFormat.Gray8);

                if (result != null)
                {
                    Text.text = result.Text;
                    
                    cookies.Add(result.Text);
                }
            }
        }
    }

    public void SaveData()
    {
        Debug.Log("Å¬¸¯");

        cookies = cookies.Distinct().ToList();

        CookieRegist(cookies);
    }

    private void CookieRegist(List<string> cookies)
    {
        string jsondata = JsonConvert.SerializeObject(cookies);
                
        FireBase.reference.Child("users").Child(FireBase.auth.CurrentUser.UserId).Child("cookie_name").SetValueAsync(jsondata);
        FireBase.reference.Child("users").Child(FireBase.auth.CurrentUser.UserId).Child("cookie_num").SetValueAsync(cookies.Count);
    }
}
