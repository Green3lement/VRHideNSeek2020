using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioPlayer : MonoBehaviour
{
    public AudioSource boxNoise;

    // Start is called before the first frame update
    void Start()
    {
        boxNoise = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.CompareTag("SeekerBots"))
        {
            boxNoise.Play();
        }
    }
}
