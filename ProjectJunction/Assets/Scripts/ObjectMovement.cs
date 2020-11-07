using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    public float speed_forward = 0.0f;
    public float speed_Up_Down = 0.0f;
    public float max_speed_horrizontal = 10.0f;
    public float max_speed_verticle = 4.0f;

    public Camera cam;
    public Rigidbody _rigidbody;

    void Start()
    {
        speed_forward = Random.Range(0,max_speed_horrizontal);
        speed_Up_Down = Random.Range(0,max_speed_verticle);
        cam = Camera.main;
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 point = cam.WorldToScreenPoint(transform.position);
        MoveObject();
        if (point.x < 0)
        {
            DestroyOnLeavingScreen();
        }
        else if (point.x > 1000)
        {
            DestroyOnLeavingScreen();
        }
        if (point.y < 0)
        {
            DestroyOnLeavingScreen();
        }
       
        else if (point.y > 1000)
        {
            DestroyOnLeavingScreen();
        }
      
       
               
    }
    void MoveObject()
    {

        if(_rigidbody.velocity.x < max_speed_horrizontal && _rigidbody.velocity.y < max_speed_verticle )
        {
            _rigidbody.AddForce(new Vector3(-2.0f, 0.0f, 0.0f), ForceMode.Force);
        }
        
        
    }
    void DestroyOnLeavingScreen()
    {
      
        Destroy(this.gameObject);

    }
}
