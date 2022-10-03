using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class delay : MonoBehaviour
{
    public GameObject touchbtn;
    float num;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per fram
    void Update()
    {
        num += Time.deltaTime;
        if(num>=4.0f)
        {
            touchbtn.SetActive(true);
            this.gameObject.GetComponent<delay>().enabled=false;
        }
    }
}
