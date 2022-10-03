using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class getCookieNum : MonoBehaviour
{
    

    [SerializeField]
    public List<string> brave_cookie_num;
    [SerializeField]
    public List<string> seafairy_origin_num;
    [SerializeField]
    public List<string> seafairy_pink_num;
    [SerializeField]
    public List<string> seafairy_black_num;
    [SerializeField]
    public List<string> seafairy_special_num;

    public item brave_cookie;
    public item sea_fairy_origin;
    public item sea_fairy_pink;
    public item sea_fairy_black;
    public item sea_fairy_special;

    public InputField inputField_cookienum;
    public Canvas save_canvas;
    public AudioSource getSound;

    public void inputBtClick()
    {
        for (int i=0; i < brave_cookie_num.Count; i++)
        {
            if(inputField_cookienum.text== brave_cookie_num[i])
            {
                inventory.Instance.AddItem(brave_cookie);
                brave_cookie_num.RemoveAt(i);
                closeChang();
                break;
            }
        }

        for (int i = 0; i < seafairy_origin_num.Count; i++)
        {
            if (inputField_cookienum.text == seafairy_origin_num[i])
            {
                inventory.Instance.AddItem(sea_fairy_origin);
                seafairy_origin_num.RemoveAt(i);
                closeChang();
                break;
            }
        }

        for (int i = 0; i < seafairy_pink_num.Count; i++)
        {
            if (inputField_cookienum.text == seafairy_pink_num[i])
            {
                inventory.Instance.AddItem(sea_fairy_pink);
                seafairy_pink_num.RemoveAt(i);
                closeChang();
                break;
            }
        }

        for (int i = 0; i < seafairy_black_num.Count; i++)
        {
            if (inputField_cookienum.text == seafairy_black_num[i])
            {
                inventory.Instance.AddItem(sea_fairy_black);
                seafairy_black_num.RemoveAt(i);
                closeChang();
                break;
            }
        }

        for (int i = 0; i < seafairy_special_num.Count; i++)
        {
            if (inputField_cookienum.text == seafairy_special_num[i])
            {
                inventory.Instance.AddItem(sea_fairy_special);
                seafairy_special_num.RemoveAt(i);
                closeChang();
                break;
            }
            
        }
    }

    public void closeChang()
    {
        inputField_cookienum.text = "";
        save_canvas.enabled = false;
        getSound.Play();
    }
}
