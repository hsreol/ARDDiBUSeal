using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gotoAR : MonoBehaviour
{
   public void GoToAR()
    {
        SceneManager.LoadScene("ARScene");
    }
}
