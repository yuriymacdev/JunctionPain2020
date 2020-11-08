using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPickUp : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject gc;
    public AudioSource pickUpSource;
    public AudioClip pickUpSound;


    private void Start()
    {
        gc = GameObject.FindGameObjectWithTag("MainCamera");
        pickUpSource = GetComponent<AudioSource>();
       
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            // instantiate WOW explosion. 


            gc.GetComponent<GameController>().AddToScore(30);
            OnDestroyed();
        }
    }

    void OnDestroyed()
    {
        gameObject.SetActive(false);

        pickUpSource.PlayOneShot(pickUpSound,0.4f);
        if(!pickUpSource.isPlaying)
        {
            Destroy(gameObject, pickUpSound.length);
        }
        
;
    }
}
