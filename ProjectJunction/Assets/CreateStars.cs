using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateStars : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody star;
    public float spawn_rate;
    public float max_travel_speed;
    public float least_travel_speed;
    float timer;


    // Update is called once per frame
    void Update()
    {
        if (timer > spawn_rate)
        {
            Rigidbody clone;
            /// Random X and Y position to spawn the object with.
            float randomX = Random.Range(-10.0f, 10.0f); float randomY = Random.Range(-10f, 10f);
            // Give the object a little spin.
            float randomRotationTorque = Random.Range(-25.0f, 25.0f);


            // Change this in the future to something less repetative.

            // Instantiate object.
            clone = Instantiate(star, new Vector3(transform.position.x + randomX, transform.position.y + randomY, transform.position.z), Quaternion.identity);
            least_travel_speed = Random.Range(-max_travel_speed, -1);
            clone.velocity = transform.TransformDirection(Vector3.right * least_travel_speed);
            clone.AddTorque(Vector3.forward * randomRotationTorque, ForceMode.Impulse);
            timer = 0;



        }
        timer += 1 * Time.deltaTime;
    }
}