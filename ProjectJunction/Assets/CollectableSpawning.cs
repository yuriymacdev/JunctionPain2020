using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSpawning : MonoBehaviour
{

    public Rigidbody pickUp1;
    public Rigidbody pickUp2;

    int collectable_controller = 0;
    float spawn_timer = 0.0f;
    public float max_spawn_time = 20.0f;
    public float lowest_velocity = 1.0f;
    public float Maximum_velocity = 3.0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(spawn_timer > max_spawn_time)
        {
            SpawnCollectables();
            spawn_timer = 0.0f;
            max_spawn_time = Random.Range(0.0f, 20.0f);
        }

        spawn_timer += 1 * Time.deltaTime;
    }


    void SpawnCollectables()
    {
       
        if (spawn_timer > max_spawn_time)
        {
            Rigidbody clone;
            /// Random X and Y position to spawn the object with.
            float randomX = Random.Range(-1.0f, 3.0f); float randomY = Random.Range(-3.5f, 3.5f);
            // Give the object a little spin.
            float randomRotationTorque = Random.Range(-25.0f, 25.0f);
            
            
             // Change this in the future to something less repetative.
            if (collectable_controller == 0)
            {
                // Instantiate object.
                clone = Instantiate(pickUp1, new Vector3(transform.position.x + randomX, transform.position.y + randomY, transform.position.z), Quaternion.identity);
                lowest_velocity = Random.Range(-Maximum_velocity, -1);
                clone.velocity = transform.TransformDirection(Vector3.right * lowest_velocity);
                clone.AddTorque(Vector3.forward * randomRotationTorque, ForceMode.Impulse);
                          

              
            }
            else if(collectable_controller == 1)
            {
                // Instantiate object.
                clone = Instantiate(pickUp2, new Vector3(transform.position.x + randomX, transform.position.y + randomY, transform.position.z), Quaternion.identity);
                lowest_velocity = Random.Range(-Maximum_velocity, -1);
                clone.velocity = transform.TransformDirection(Vector3.right * lowest_velocity);
                clone.AddTorque(Vector3.forward * randomRotationTorque, ForceMode.Impulse);
              
                                
            }

           







        }


    }
}
