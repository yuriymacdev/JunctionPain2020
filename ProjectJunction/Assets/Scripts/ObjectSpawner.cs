using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject _asteroid;
    public float time_tillNextSpawn;
    float spawner_timer = 0.0f;
    float spawn_difficualty = 3.0f;
    Transform t;
    void Start()
    {
        t = GetComponent<Transform>();
        time_tillNextSpawn = Random.Range(0.5f, 5.0f + spawn_difficualty);
      
    }

    // Update is called once per frame
    void Update()
    {
        
        if (spawner_timer > time_tillNextSpawn)
        {
            float randomX = Random.Range(0, 5);
            float randomY = Random.Range(0, 5);

            Instantiate(_asteroid, new Vector3(transform.position.x,transform.position.y,transform.position.z),Quaternion.identity);
            spawner_timer = 0;
            time_tillNextSpawn = Random.Range(0.5f, 2.0f);
        }
        spawner_timer += 1 * Time.deltaTime;
    }
}
