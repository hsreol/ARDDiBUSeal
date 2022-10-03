using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControler : MonoBehaviour
{
    public GameObject touchscreen;
    float a = 255;

    // Start is called before the first frame update
    void Start()
    {
        touchscreen.GetComponent<SpriteRenderer>().color = new Color(2, 255, 255, a);
    }

    // Update is called once per frame
    void Update()
    {

        if (a >= 255)
        {
            touchscreen.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, a);
            a -= Time.deltaTime*100;
        }
        else if(a<=10)
        {
            touchscreen.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, a);
            a += Time.deltaTime*100;
        }
    }
}
