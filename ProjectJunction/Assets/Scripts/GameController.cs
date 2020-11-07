using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    /// <summary>
    ///  So far this is just for closing the application.
    /// </summary>
    /// 
    public bool player_has_died = false;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if(player_has_died == true && Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.R)  || Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(0);
            
        }

    }
}
