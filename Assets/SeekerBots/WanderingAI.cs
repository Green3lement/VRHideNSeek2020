using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class WanderingAI : MonoBehaviour
{
    //private Animator anim;

    private GameObject player;
    public float wanderRadius;
    public float wanderTimer;
    Transform LastSeenLocation;

    private Transform target;
    public NavMeshAgent agent;
    private float timer;

    // Use this for initialization
    void Start()
    {
        //anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        //if(anim.GetBool("playerVisible"))
        //{
        //    agent.SetDestination(player.transform.position);
       // }
        if (timer >= wanderTimer)
        {
            Debug.Log("Hello: " + agent);
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);//, anim);
            agent.SetDestination(newPos);
            Debug.Log("Destination: " + newPos);
            timer = 0;
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)//, Animator anim)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        
        //anim.SetBool("Wandering", true);

        return navHit.position;
    }
}
