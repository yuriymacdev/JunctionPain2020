using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UI;

public class ReplayScript : MonoBehaviour
{
    public GameController gc;
    public Image image;
    public Text text;
    // Update is called once per frame
    void Update()
    {
     
        if(gc.GetComponent<GameController>().player_has_died == true)
        {
            image.enabled = true;
            text.enabled = true;
        }else
        {
            image.enabled = false;
            text.enabled = false;
        }
    }
}
