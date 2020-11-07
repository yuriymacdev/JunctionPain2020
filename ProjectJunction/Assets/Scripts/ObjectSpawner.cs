using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody _importObject;
    public float time_tillNextSpawn;
    float spawner_timer = 0.0f;
    float spawn_difficualty = 3.0f;
    public float InitialMaxSpeedRange;
    float initialVelocity = -1.0f;

    Transform t;
    void Start()
    {
        t = GetComponent<Transform>();
        time_tillNextSpawn = Random.Range(0.5f, 5.0f + spawn_difficualty);
        
    }

    // Update is called once per frame
    void Update()
    {

        SpawnAsteroids();


    }

    void SpawnAsteroids()
    {
        if (spawner_timer > time_tillNextSpawn)
        {
            /// Random X and Y position to spawn the object with.
            float randomX = Random.Range(-1.0f, 3.0f); float randomY = Random.Range(-3.5f, 3.5f);
            // Give the object a little spin.
            float randomRotationTorque = Random.Range(-25.0f, 25.0f);

            // Instantiate object.
            Rigidbody clone = Instantiate(_importObject, new Vector3(transform.position.x + randomX, transform.position.y + randomY, transform.position.z), Quaternion.identity);
            initialVelocity = Random.Range(-InitialMaxSpeedRange, -1);
            clone.velocity = transform.TransformDirection(Vector3.right * initialVelocity);
            clone.AddTorque(Vector3.forward * randomRotationTorque, ForceMode.Impulse);

            /// Creating random scales to apply to the new object.
            float randomScaleX = Random.Range(10.0f, 20.0f), randomScaleY = Random.Range(10.0f, 20.0f), randomScaleZ = Random.Range(10.0f, 20.0f);

            _importObject.GetComponent<Transform>().localScale = new Vector3(randomScaleX,randomScaleY,randomScaleZ);
            spawner_timer = 0;
            time_tillNextSpawn = Random.Range(0.1f, 4.0f);
        }
        spawner_timer += 1 * Time.deltaTime;



    }
}
