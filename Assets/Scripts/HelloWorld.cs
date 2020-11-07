using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class HelloWorld : MonoBehaviour
{
    public Camera cam;
    Transform t;

    public float move_speed_X;
    float vel = 0; 
    public int tileSize = 64;

    public float screenBoundaryLeft;
    public float screenBoundaryRight;

    
    void Start()
    {
        t = GetComponent<Transform>();
        if(cam == null)
        {
            cam = GetComponent<Camera>();

        }
        screenBoundaryRight = cam.pixelWidth;
        Debug.Log("Hello Im alive !");
    }
   
    // Update is called once per frame
    void Update()
    {

         

        /// Move object left and right

        vel = 0;


        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {

            vel = -1.0f;         
       
        }
         
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            vel = 1.0f;

        }
        t.position += new Vector3(vel * move_speed_X * Time.deltaTime, vel * move_speed_X * Time.deltaTime);

        ClampToScreenX(t, tileSize);

       
    }

    void ClampToScreenX( Transform t, int size)
    {
        float currentScreenX = t.position.x;
        

        Vector3 point = cam.WorldToScreenPoint(new Vector3(currentScreenX, -4, 0));

        
       // Debug.Log("Object is : " + point.x +  point.y + point.z + "Pixels from the left");


        if(point.x < screenBoundaryLeft)
        {
            point.x = screenBoundaryLeft;
                       
        }
        else if (point.x > screenBoundaryRight)
        {
            point.x = screenBoundaryRight;
            
        }

        Vector3 r = cam.ScreenToWorldPoint(point);
        t.position = r;
    }

   
}
