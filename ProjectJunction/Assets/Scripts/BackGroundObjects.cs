using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundObjects : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody Object1;
    public Rigidbody Object2;
    public Rigidbody Object3;
    public Rigidbody Object4;

    float spawn_timer_1;
    float max_timer_1;
    float spawn_timer_2;
    float max_timer_2;
    float spawn_timer_3;
    float max_timer_3;
    float spawn_timer_4;
    float max_timer_4;

    float object_vel_1;
    float object_vel_2;
    float object_vel_3;
    float object_vel_4;


    void Start()
    {
        max_timer_1 = Random.Range(1.0f, 10.0f);
        max_timer_2 = Random.Range(1.0f, 10.0f);
        max_timer_3 = Random.Range(3.0f, 10.0f);
        max_timer_4 = Random.Range(5.0f, 10.0f);

    }
    void Update()
    {

        if(spawn_timer_1 > max_timer_1)
        {
            SpawnObjects(Object1, object_vel_1);
            spawn_timer_1 = 0.0f;
        }
        if (spawn_timer_2 > max_timer_3)
        {
            SpawnObjects(Object2, object_vel_2);
            spawn_timer_2 = 0.0f;
        }
        if (spawn_timer_3 > max_timer_3)
        {
            SpawnObjects(Object3, object_vel_3);
            spawn_timer_3 = 0.0f;
        }
        if (spawn_timer_4 > max_timer_4)
        {
            SpawnObjects(Object4, object_vel_4);
            spawn_timer_4 = 0.0f;
        }

        spawn_timer_1 += 1 * Time.deltaTime;
        spawn_timer_2 += 1 * Time.deltaTime;
        spawn_timer_3 += 1 * Time.deltaTime;
        spawn_timer_4 += 1 * Time.deltaTime;
    }
    void SpawnObjects(Rigidbody _object, float _object_vel)
    {
        
            /// Random X and Y position to spawn the object with.
            float randomX = Random.Range(-1.0f, 3.0f); float randomY = Random.Range(-3.5f, 3.5f);
            // Give the object a little spin.
            float randomRotationTorque = Random.Range(-25.0f, 25.0f);

            // Instantiate object.
            Rigidbody clone = Instantiate(_object, new Vector3(transform.position.x + randomX, transform.position.y + randomY, transform.position.z), Quaternion.identity);
            _object_vel = Random.Range(-10, -1);
            clone.velocity = transform.TransformDirection(Vector3.right * _object_vel);
            clone.AddTorque(Vector3.forward * randomRotationTorque, ForceMode.Impulse);

            /// Creating random scales to apply to the new object.
            float randomScaleX = Random.Range(1.0f, 2.0f), randomScaleY = Random.Range(1.0f, 2.0f), randomScaleZ = Random.Range(1.0f, 2.0f);

            clone.GetComponent<Transform>().localScale = new Vector3(randomScaleX, randomScaleY, randomScaleZ);
                
        
    }
   
}
