using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    //public float speed_forward = 0.0f;
    // public float speed_Up_Down = 0.0f;
    // public float max_speed_horrizontal = 10.0f;
    // public float max_speed_verticle = 4.0f;

    public Camera cam;


    void Start()
    {
        // speed_forward = Random.Range(0,max_speed_horrizontal);
        //  speed_Up_Down = Random.Range(0,max_speed_verticle);
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();


    }

    // Update is called once per frame
    void Update()
    {
        Vector3 point = cam.WorldToScreenPoint(transform.position);
        
        if (point.x < -50)
        {
            DestroyOnLeavingScreen();
        }
        else if (point.x > 2000)
        {
            DestroyOnLeavingScreen();
        }
        if (point.y < -50)
        {
            DestroyOnLeavingScreen();
        }

        else if (point.y > 2000)
        {
            DestroyOnLeavingScreen();
        }


    }

    void DestroyOnLeavingScreen()
    {

        Destroy(this.gameObject);

    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {

            Debug.Log("Hit Player");
            Destroy(gameObject);
            Destroy(collision.gameObject);
            Debug.Log("GAME OVER");
            cam.GetComponent<GameController>().player_has_died = true;
        }
    }
}
