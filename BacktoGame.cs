using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BacktoGame : MonoBehaviour
{

    public void gotoGame()
    {
        SceneManager.LoadScene("GameScene");
    }

}
