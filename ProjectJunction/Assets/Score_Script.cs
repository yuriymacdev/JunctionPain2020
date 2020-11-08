using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UI;

public class Score_Script : MonoBehaviour
{
    // Start is called before the first frame update
    int player_score = 0;
    public GameController gc;

    Text text;

    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        player_score = gc.GetComponent<GameController>().GetScore();
        text.text = " : " + player_score;
    }
}
