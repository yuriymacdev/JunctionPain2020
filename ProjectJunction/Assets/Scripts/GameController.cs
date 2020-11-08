using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    int player_score = 0;

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
    public void AddToScore(int value)
    {
        player_score += value;
    }
    public int GetScore()
    {
        return player_score;
    }
}
