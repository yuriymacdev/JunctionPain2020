using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class OnPickUp : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject gc;
    public AudioSource pickUpSource;
    public AudioClip pickUpSound;
    bool collided = false;
    float deleteCounter = 1.0f;
    private void Start()
    {
        gc = GameObject.FindGameObjectWithTag("MainCamera");
        pickUpSource = GetComponent<AudioSource>();
        if (collided == true)
        {
            if(transform.position.x > 500 & transform.position.y > 500 && transform.position.z > 500)
            {
                deleteCounter -= 1 * Time.deltaTime;
            }
            if(deleteCounter <= 0)
            {

                Destroy(gameObject);
            }
           
           
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            // instantiate WOW explosion. 

            pickUpSource.PlayOneShot(pickUpSound, 1.0f);
            OnDestroyed();



        }

    }

    void OnDestroyed()
    {
        gameObject.transform.position = new Vector3(1000, 1000, 1000);
        gc.GetComponent<GameController>().AddToScore(30);
       collided = true;        
    
            

    }
}
