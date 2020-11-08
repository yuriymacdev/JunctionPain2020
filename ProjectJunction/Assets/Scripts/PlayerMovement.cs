using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Junction 2020 : painless gaining */
public class PlayerMovement : MonoBehaviour
{
    // Input for player manipulation where you plug in the external class.
    public Camera cam;
    public Vector2 input_position;

   public float player_speed;
   
    
    /// Clamping Variables to keep the object on the screen.  
    public float left_limit;
    public float right_limit;
    public float top_limit;
    public float bottom_limit;
    // Tempory mouse input.
    Vector2 mouse_position;


    Transform player_transform;
   
    void Start()
    {
        player_transform = GetComponent<Transform>();
        top_limit = cam.pixelHeight - 64;
        right_limit = cam.pixelWidth - 64;
    }

    // Update is called once per frame
    void Update()
    {
        // Get Converted Screen Space position of mouse;
        Vector3 point = cam.ScreenToWorldPoint( new Vector3(Input.mousePosition.x,Input.mousePosition.y,Input.mousePosition.z));

        input_position = point;
      
        PlayerControl(input_position);
        ClampPlayerObjectToArea(left_limit,right_limit,top_limit,bottom_limit);
    }

    void PlayerControl(Vector2 input_position)
    {
        // Step is to control the player movement speed.
        float step = player_speed * Time.deltaTime;

        // move towards moves this game object towards a target object position. 
        transform.position = Vector3.MoveTowards(transform.position, input_position, step);
        
        
        /// Original move command. 
        // player_transform.position = new Vector3(input_position.x, input_position.y, 0.0f);

    }

    void ClampPlayerObjectToArea(float left_limit, float right_limit, float top_limit,float bottom_limit)
    {
        /// Get Screen space location.
        Vector3 point = cam.WorldToScreenPoint(transform.position);

                
        // Keep the object inside the game area. 
        // With the eye tracking method this can most likely be dissabled as the player wont be able to look out of the playing area.

        if (point.x < left_limit) { point = new Vector3(left_limit,point.y,point.z); }
        else if (point.x > right_limit) { point = new Vector3(right_limit,point.y,point.z); }

        if (point.y > top_limit) { point = new Vector3(point.x, top_limit , point.z); }
        else if (point.y < bottom_limit) { point = new Vector3(point.x, bottom_limit , point.z); }
      
        Vector3 r = cam.ScreenToWorldPoint(point);
        transform.position = r;

    }

    
 

}
